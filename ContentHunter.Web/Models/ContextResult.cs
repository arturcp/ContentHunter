using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentHunter.Web.Models
{
    public class ContextResult
    {
        public ContextResult()
        {
            Results = new List<CrawlerResult>();
            CandidatesToRecursion = new List<string>();
        }

        public List<CrawlerResult> Results{ get; set; }
        public List<string> CandidatesToRecursion { get; set; }

    }
}