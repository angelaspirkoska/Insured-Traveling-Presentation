using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class ActionLog1
    {
        public int log_activityID { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string ip_address { get; set; }
        public DateTime datetime { get; set; }

    }
}