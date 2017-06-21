using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace InsuredTraveling.Helpers
{
    public static class ConvertDateTimeHelper
    {
        public static DateTime ConvertDateTime(string dateTimeToConvert)
        {
            try
            {
                DateTime dateTimeChanged = DateTime.Parse(dateTimeToConvert, new CultureInfo("en-US"));
                return dateTimeChanged;
            }
            catch(Exception e)
            {
                return DateTime.UtcNow;
            }  
        }
    }
}