using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Helpers
{
    public static class StringHelper
    {
        public static string Trunc(this string str, int maxLength)
        {
            return str.Substring(0, Math.Min(str.Length, maxLength)) +
                (str.Length > maxLength ? " ... " : "");
        }
    }
}
