using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class ConfigPolicyModel
    {
        public int IDPolicy { get; set; }
        public int ID_config_policy_type { get; set; }
        public string raiting { get; set; }
        public bool IsPaid { get; set;}
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}