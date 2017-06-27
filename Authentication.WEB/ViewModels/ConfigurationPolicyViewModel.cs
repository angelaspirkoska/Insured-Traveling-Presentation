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

        [Required]      
        [Display(Name = "Policy_HolderName", ResourceType = typeof(Resource))]
        public string PolicyHolderName { get; set;}
        [Required]
        [Display(Name = "Policy_HolderLastName", ResourceType = typeof(Resource))]
        public string PolicyHolderLastName { get; set; }
        [Required]
        [Display(Name = "Policy_HolderSSN", ResourceType = typeof(Resource))]
        public string PolicyHolderSSN { get; set; }
        [Required]
        [Display(Name = "User_PassportNumber", ResourceType = typeof(Resource))]
        public string PolicyHolderPassportNumber_ID { get; set; }
        [Required]
        [Display(Name = "Policy_HolderBirthDay", ResourceType = typeof(Resource))]
        public DateTime? PolicyHolderBirthDate { get; set; }
        [Required]
        [Display(Name = "Policy_InsuredAddress", ResourceType = typeof(Resource))]
        public string PolicyHolderAddress { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yyyy}")]
        [Display(Name = "Policy_PolicyEffectiveDate", ResourceType = typeof(Resource))]
        public DateTime? Start_Date { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yyyy}")]
        [Display(Name = "Policy_PolicyExpiryDate", ResourceType = typeof(Resource))]
        public DateTime? End_Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredName", ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredLastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredAddress", ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredBirthDate", ResourceType = typeof(Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yyyy}")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredSSN", ResourceType = typeof(Resource))]
        public string SSN { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredPassport", ResourceType = typeof(Resource))]
        public string PassportNumber_ID { get; set; }
    }
}