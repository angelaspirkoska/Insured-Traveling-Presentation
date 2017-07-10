using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class ErrorLog
    {
        public System.Guid error_id { get; set; }
        public string application { get; set; }
        public string host { get; set; }
        public string type { get; set; }
        public string source { get; set; }
        public string message { get; set; }
        public string username { get; set; }
        public int status_code { get; set; }
        public DateTime datetime { get; set; }
        public int sequence { get; set; }
        public string xml { get; set; }
    }
}