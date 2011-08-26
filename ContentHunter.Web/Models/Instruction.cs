using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using ContentHunter.Web.Models.Engines;
using System.ComponentModel.DataAnnotations;

namespace ContentHunter.Web.Models
{
    public class Instruction: BaseModel, ICloneable
    {
        public Instruction()
        {
            Type = GetType(InputType.Html);
            IsOriginal = true;
        }

        public enum InputType : short {Rss, Html, Xml}

        public static short GetType(InputType type)
        {
            return (short)type;
        }

        public int Id { get; set; }
        public string Url { get; set; }
        public short Type { get; set; }
        public string Engine { get; set; }
        public bool IsRecursive { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public bool IsRecurrent { get; set; }
        
        //used on lucene index
        public string Category { get; set; }

        //used to recursive crawler, do not persist on database
        [NotMapped]
        public bool IsOriginal { get; set; }

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


        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}