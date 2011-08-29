using System.Net;
using System.IO;
using System.Collections.Generic;
using System;
using System.Reflection;
using ContentHunter.Web.Models;
using ContentHunter.Web.Models.Util;
using System.Linq;
using WebToolkit.Index;
using Lucene.Net.Documents;
using System.Data;

namespace ContentHunter.Web.Models.Engines
{
    public abstract class Crawler
    {
        public int HttpCode { get; set; }
        public Instruction Input { get; set; }

        protected List<string> CandidatesToRecursion { get; set; }

        public void AddCandidateLink(string link)
        {
            if (!CandidatesToRecursion.Contains(link))
                CandidatesToRecursion.Add(link);
        }

        public Crawler()
        {
            HttpCode = 200;
            CandidatesToRecursion = new List<string>();
        }

        public abstract List<CrawlerResult> ParseHtml();
        public abstract List<CrawlerResult> ParseRss();
        public abstract List<CrawlerResult> ParseXml();

        private ContentHunterDB db = new ContentHunterDB();
        
        public string GetContent(string url)
        {
            string content = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                HttpCode = 200;
                using (WebClient crawler = new WebClient())
                {
                    try
                    {
                        content = new StreamReader(crawler.OpenRead(url)).ReadToEnd();
                    }
                    catch (WebException error)
                    {
                        HttpWebResponse response = (HttpWebResponse)error.Response;
                        HttpCode = (int)response.StatusCode;
                    }
                }
            }
            else HttpCode = 404;
            return content;
        }

        private List<CrawlerResult> ExecuteByType(Models.Instruction.InputType type)
        {
            List<CrawlerResult> output = null;
            if (type == Models.Instruction.InputType.Html)
                output = this.ParseHtml();
            else if (type == Models.Instruction.InputType.Rss)
                output = this.ParseRss();
            else if (type == Models.Instruction.InputType.Xml)
                output = this.ParseXml();
            return output;
        }

        private List<CrawlerResult> Execute(Instruction instruction)
        {
            //cannot execute if it is not recurrent and it was already run
            List<CrawlerResult> outputs = new List<CrawlerResult>();

            if (instruction != null)
            {
                try
                {
                    if (instruction.IsOriginal)
                        instruction.StartedAt = DateTime.Now;

                    outputs = ExecuteByType((Instruction.InputType) instruction.Type);

                    List<CrawlerResult> toSave = new List<CrawlerResult>();
                    foreach (CrawlerResult result in outputs)
                    {
                        if (!ContentExists(result.Url))
                        {
                            db.CrawlerResults.Add(result);
                            toSave.Add(result);
                        }
                    }

                    db.SaveChanges();
                    Index(toSave);

                    //if (instruction.IsRecursive)
                    //{
                    //    Instruction recursiveInstruction = (Instruction)instruction.Clone();
                    //    recursiveInstruction.IsOriginal = false;
                    //    foreach (string link in CandidatesToRecursion)
                    //    {
                    //        recursiveInstruction.Url = link;
                    //        Execute(recursiveInstruction);
                    //    }
                    //}

                    if (instruction.IsOriginal)
                    {
                        instruction.FinishedAt = DateTime.Now;
                        db.Entry(instruction).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                catch (Exception error)
                {
                    //outputs.ErrorCode = (short)ContentHunter.Web.Models.Util.Enum.ErrorCodes.GeneralError;
                    //outputs.ErrorMessage = error.Message;
                }
            }
            //else outputs.ErrorCode = (short)ContentHunter.Web.Models.Util.Enum.ErrorCodes.NullInput;

            return outputs;
        }

        private void Index(List<CrawlerResult> results)
        {
            IndexManager indexManager = new IndexManager();
            List<Field> fields = null;
            foreach (CrawlerResult result in results)
            {
                if (result.Id > 0)
                {
                    fields = new List<Field>();
                    fields.Add(new Field("Id", result.Id.ToString(), Field.Store.YES, Field.Index.NO));
                    fields.Add(new Field("Url", result.Url, Field.Store.YES, Field.Index.NO));
                    fields.Add(new Field("Tags", result.Tags, Field.Store.YES, Field.Index.ANALYZED));
                    fields.Add(new Field("Categories", Input.Categories, Field.Store.YES, Field.Index.ANALYZED));
                    indexManager.Index(fields);
                }
            }
        }

        public bool ContentExists(string url)
        {
            List<CrawlerResult> list = (from r in db.CrawlerResults
                        where r.Url == url
                        select r).ToList<CrawlerResult>();

            return list.Count > 0;                 
        }

        public List<CrawlerResult> Execute()
        {
            return Execute(this.Input);
        }

        public string GetFriendlyName()
        {
            Type t = this.GetType();
            object[] list = t.GetCustomAttributes(typeof(ContentHunter.Web.Models.Util.FriendlyNameAttribute), false);

            if (list.Length > 0)
                return ((FriendlyNameAttribute)list[0]).Name;
            else
                return string.Empty;
        }

        public static List<Engine> GetEngines()
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Models/Engines");
            string[] files = Directory.GetFiles(path);
            List<Engine> list = new List<Engine>();

            foreach (string file in files)
            {
                Crawler crawler = (Crawler)Assembly.GetExecutingAssembly().CreateInstance(string.Format("ContentHunter.Web.Models.Engines.{0}", Path.GetFileNameWithoutExtension(file)));
                list.Add(new Engine() { ClassName = Path.GetFileNameWithoutExtension(file), FriendlyName = crawler.GetFriendlyName() });
            }

            return list;
        }
    }
}