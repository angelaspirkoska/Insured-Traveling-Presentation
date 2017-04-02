using Authentication.WEB.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;


namespace InsuredTraveling.Models
{
    public class DiscountModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "AdminPanel_DiscountName", ResourceType = typeof(Resource))]
        public string Discount_Name { get; set; }
        [Required]
        [Display(Name = "AdminPanel_DiscountCoefi", ResourceType = typeof(Resource))]
        public double Discount_Coef { get; set; }
        [Required]
        [Display(Name = "AdminPanel_DiscountStartDate", ResourceType = typeof(Resource))]
        public DateTime Start_Date { get; set; }
        [Required]
        [Display(Name = "AdminPanel_DiscountEndDate", ResourceType = typeof(Resource))]
        public DateTime End_Date { get; set;  }
    
    }
}