using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using ContentHunter.Web.Models.Engines;

namespace ContentHunter.Web.Models
{
    public class Instruction
    {
        public enum InputType {Rss, Html, Xml}

        public int Id { get; set; }
        public string Url { get; set; }
        public InputType Type { get; set; }
        public string Engine { get; set; }
        public bool IsRecursive { get; set; }

        private Crawler GetEngine()
        {
            Crawler crawler = (Crawler)Assembly.GetExecutingAssembly().CreateInstance(string.Format("ContentHunter.Web.Models.Engines.{0}", Engine));
            crawler.Input = this;
            return crawler;
        }

        public CrawlerResult Execute()
        {
            return GetEngine().Execute();
        }

    }
}