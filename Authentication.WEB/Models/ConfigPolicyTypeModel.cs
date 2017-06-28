using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class ConfigPolicyTypeModel
    {
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

        public bool? status { get; set; }
        public string NameSurename {get;set; }
        public string raiting { get; set; }
    }
}