using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class ConfigPolicyTypeModel
    {
        [Display(Name = "AdminPanel_Configuration_PolicyName", ResourceType = typeof(Resource))]
        public string name { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

        public bool? status { get; set; }
        public string NameSurename {get;set; }
        public string raiting { get; set; }
        public int id { get; set; }
        public int version { get; set; }
        public int typeFrom { get; set; }
    }
}