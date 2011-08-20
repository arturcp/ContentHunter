using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentHunter.Web.Models
{
    public interface IHunter
    {
        string Parse(string url);
    }
}