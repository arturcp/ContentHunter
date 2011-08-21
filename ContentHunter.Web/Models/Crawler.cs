using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System;

namespace ContentHunter.Web.Models
{
    public abstract class Crawler
    {
        public int HttpCode { get; set; }
        public Input Input { get; set; }

        protected List<string> CandidatesToRecursion { get; set; }

        public Crawler()
        {
            HttpCode = 200;
            CandidatesToRecursion = new List<string>();
        }
        
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

        public Output Execute()
        {
            Output output = new Output();

            if (this.Input != null)
            {
                if (this.Input.Type == Models.Input.InputType.Html)
                    output = this.ParseHtml();
                else if (this.Input.Type == Models.Input.InputType.Rss)
                    output = this.ParseRss();
                else output.ErrorMessage = "Method not found";
            }
            else output.ErrorMessage = "Input cannot be null";

            return output;
        }
        public abstract Output ParseHtml();
        public abstract Output ParseRss();
    }
}