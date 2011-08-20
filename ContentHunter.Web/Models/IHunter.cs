using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentHunter.Web.Models
{
    public interface IHunter
    {
        string Parse(string url);
        //List<string> CandidatesToRecursion(string url);
        //string ParseHtml(string url);
        //string ParseRss(string url);
    }
}