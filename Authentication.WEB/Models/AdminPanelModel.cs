using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class AdminPanelModel
    {
        public int ID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "AdminPanel_DiscountName", ResourceType = typeof(Resource))]
        public string Discount_Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "AdminPanel_DiscountCoefi", ResourceType = typeof(Resource))]
        public double Discount_Coef { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "AdminPanel_DiscountStartDate", ResourceType = typeof(Resource))]
        public DateTime Start_Date { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "AdminPanel_DiscountEndDate", ResourceType = typeof(Resource))]
        public DateTime End_Date { get; set; }


        public Sava_AdminPanelModel SavaAdmin { get; set; }
        //public string Name { get; set; }
        //public int Id { get; set; }
        //public string UserID { get; set; }
    }
}