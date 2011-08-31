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

        protected List<string> VisitedLinks { get; set; }

        public void AddCandidateLink(ContextResult contextResult, string link)
        {
            if (!VisitedLinks.Contains(link))
                contextResult.CandidatesToRecursion.Add(link);
        }

        public Crawler()
        {
            HttpCode = 200;
            VisitedLinks = new List<string>();
        }

        public abstract ContextResult ParseHtml(Instruction instruction);
        public abstract ContextResult ParseRss(Instruction instruction);
        public abstract ContextResult ParseXml(Instruction instruction);

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

        private ContextResult ExecuteByType(Instruction instruction)
        {
            this.VisitedLinks.Add(instruction.Url);
            Models.Instruction.InputType type = (Models.Instruction.InputType)instruction.Type;
            ContextResult output = null;
            if (type == Models.Instruction.InputType.Html)
                output = this.ParseHtml(instruction);
            else if (type == Models.Instruction.InputType.Rss)
                output = this.ParseRss(instruction);
            else if (type == Models.Instruction.InputType.Xml)
                output = this.ParseXml(instruction);
            return output;
        }

        private ContextResult Execute(Instruction instruction)
        {
            //cannot execute if it is not recurrent and it was already run
            ContextResult context = new ContextResult();

            if (instruction != null)
            {
                //try
                //{
                if (instruction.IsOriginal)
                {
                    instruction.StartedAt = DateTime.Now;
                    instruction.State = true;
                }


                context = ExecuteByType(instruction);

                List<CrawlerResult> toSave = new List<CrawlerResult>();
                foreach (var result in context.Results)
                {
                    if (!ContentExists(result.Url))
                    {
                        db.CrawlerResults.Add(result);
                        toSave.Add(result);
                    }
                }

                if (toSave.Count > 0)
                {
                    db.SaveChanges();
                    Index(toSave);
                }

                if (instruction.IsRecursive)
                {
                    Instruction recursiveInstruction = (Instruction)instruction.Clone();
                    recursiveInstruction.IsOriginal = false;
                    foreach (string link in context.CandidatesToRecursion)
                    {
                        recursiveInstruction.Url = link;
                        Execute(recursiveInstruction);
                    }
                }

                if (instruction.IsOriginal)
                {
                    instruction.FinishedAt = DateTime.Now;
                    instruction.State = false;
                    db.Entry(instruction).State = EntityState.Modified;
                    db.SaveChanges();
                }
                // }
                // catch (Exception error)
                // {
                //outputs.ErrorCode = (short)ContentHunter.Web.Models.Util.Enum.ErrorCodes.GeneralError;
                //outputs.ErrorMessage = error.Message;
                //  }
            }
            //else outputs.ErrorCode = (short)ContentHunter.Web.Models.Util.Enum.ErrorCodes.NullInput;

            return context;
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

        public ContextResult Execute()
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