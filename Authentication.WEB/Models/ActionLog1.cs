using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class ActionLog1
    {
        public long log_activityID { get; set; }
        public string username { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string ip_address { get; set; }
        public string date { get; set; }
        public string time { get; set; }

    }
}