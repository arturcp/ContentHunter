using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContentHunter.Web.Models.Util
{
    public class Enum
    {
        public enum ErrorCodes
        {
            NoErrors = 0,
            NotFound = 404,
            GeneralError = 500,
            NullInput = 1
        }

    }
}