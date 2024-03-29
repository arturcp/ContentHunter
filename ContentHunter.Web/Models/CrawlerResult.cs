﻿using ContentHunter.Web.Models.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContentHunter.Web.Models
{
    public class CrawlerResult
    {
        public CrawlerResult()
        {
            //ErrorCode = (short)Enum.ErrorCodes.NoErrors;
            Title = string.Empty;
            Content = string.Empty;
            //Message = string.Empty;
            //ErrorMessage = string.Empty;
        }

        public int Id { get; set; }
        
        [Required, StringLength(300)]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        /*[StringLength(45)]
        public string Message { get; set; }
        
        public short ErrorCode { get; set; }

        [StringLength(100)]
        public string ErrorMessage { get; set; }*/

        [Required, StringLength(200)]
        public string Url { get; set; }

        [StringLength(500)]
        public string Tags { get; set; }

        public string Data { get; set; }

        [NotMapped]
        public object CustomBag { get; set; }
    }
}