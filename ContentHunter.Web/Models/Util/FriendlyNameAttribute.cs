using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentHunter.Web.Models.Util
{
    public class FriendlyNameAttribute : Attribute
    {
        public string Name { get; set; }

        public FriendlyNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}