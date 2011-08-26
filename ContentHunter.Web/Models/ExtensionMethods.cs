using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ContentHunter.Web.Models
{
    public static class ExtensionMethods
    {
        public static List<T> FindByIds<T>(this DbSet<T> dbSet, string[] ids) where T: BaseModel //, new()
        {
            var list = (from f in dbSet
                      where ids.Contains<string>(f.Id.ToString())
                      select f).ToList<T>();
            return list;
        }
    }
}