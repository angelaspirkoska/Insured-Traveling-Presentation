using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    public class ConfigurationPolicyViewModel
    {
        [Display(Name = "Config_Policy_Type", ResourceType = typeof(Resource))]
        public int PolicyTypeID { get; set; }
    }
}