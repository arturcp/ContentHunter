using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using ContentHunter.Web.Models.Engines;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContentHunter.Web.Models
{
    public class Instruction: ICloneable
    {
        public Instruction()
        {
            Type = GetType(InputType.Html);
            IsOriginal = true;
            State = false;
        }

        public enum InputType : short {Rss, Html, Xml}

        public static short GetType(InputType type)
        {
            return (short)type;
        }

        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Url { get; set; }
        
        [Required]
        public short Type { get; set; }

        [Required, StringLength(45)]
        public string Engine { get; set; }
        
        [Required]
        public bool IsRecursive { get; set; }
        
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        
        [Required]
        public bool IsRecurrent { get; set; }
        
        //used on lucene index
        [StringLength(500)]
        public string Categories { get; set; }

        public bool State { get; set; }

        public int FrequencyValue { get; set; }

        //hour, day, month and year
        public int FrequencyUnit { get; set; }

        public DateTime? ScheduledTo { get; set; }

        //used to recursive crawler, do not persist on database
        [NotMapped]
        public bool IsOriginal { get; set; }

        private Crawler GetEngine()
        {
            Crawler crawler = (Crawler)Assembly.GetExecutingAssembly().CreateInstance(string.Format("ContentHunter.Web.Models.Engines.{0}", Engine));
            crawler.Input = this;
            return crawler;
        }

        public ContextResult Execute()
        {
            return GetEngine().Execute();
        }

        public void Unschedule()
        {
            FrequencyUnit = 0;
            FrequencyValue = 0;
            ScheduledTo = null;
        }

        public string GetExecutionPlan()
        {
            string result = "Not set";
            if (FrequencyValue > 0 && FrequencyUnit > 0)
                result = string.Format("Each {0} {1}{2}", FrequencyValue, Enum.GetName(typeof(FrequencyUnits), FrequencyUnit).ToLower(), FrequencyUnit > 0 ? "" : "s");
            return result;
        }

        public enum FrequencyUnits
        {
            Never = 0,
            Hour = 1,
            Day = 2,
            Month = 3
        }


        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}