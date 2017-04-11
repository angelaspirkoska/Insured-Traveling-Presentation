using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class Sava_AdminPanelModel
    {
        public int id { get; set; }
        public string email_administrator { get; set; }
        public float points_percentage { get; set; }
        public float vip_sum { get; set; }
        public string last_modify_by {get;set;}
        public DateTime timestamp { get; set; }
    }
}