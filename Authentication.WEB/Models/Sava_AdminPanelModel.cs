using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class Sava_AdminPanelModel
    {
        public int id { get; set; }
        public string email_administrator { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public float points_percentage { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Range(0,1000000 , ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "SavaAdminPanel_VipPoints", ResourceType = typeof(Resource) )]
        public float vip_sum { get; set; }
        public string last_modify_by {get;set;}
        public DateTime timestamp { get; set; }
    }
}