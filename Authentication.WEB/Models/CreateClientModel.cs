using System;
using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class CreateClientModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Client_Name", ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Client_LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [EMBGValidateAdvanced(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_EMBG_Val_Advanced")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "OnlyNumbers")]
        [Display(Name = "Client_SSN", ResourceType = typeof(Resource))]
        public string SSN { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Client_DateBirth", ResourceType = typeof(Resource))]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Client_Age", ResourceType = typeof(Resource))]
        public int Age { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailNotValid")]
        [Display(Name = "Client_Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "OnlyNumbers")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Client_Phone", ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Client_City", ResourceType = typeof(Resource))]
        public string City { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "OnlyNumbers")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Client_PostalCode", ResourceType = typeof(Resource))]
        public string Postal_Code { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Client_Address", ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Client_Passport", ResourceType = typeof(Resource))]
        public string Passport_Number_IdNumber { get; set; }

        public int Type_InsuredID { get; set; }
        public DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public DateTime Date_Modified { get; set; }
        public string Modified_By { get; set; }
    }
}
