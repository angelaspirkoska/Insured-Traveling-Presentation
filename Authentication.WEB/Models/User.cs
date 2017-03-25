using Authentication.WEB.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace InsuredTraveling.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "User_UserName", ResourceType = typeof(Resource))]
        public override string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "User_FirstName", ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "User_LastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_Email")]
        [Display(Name = "User_Email", ResourceType = typeof(Resource))]
        public override string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_PasswordLen", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "User_Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "User_ConfirmPassword", ResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_PasswordNotMatch")]
        public string ConfirmPassword { get; set; }

       
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [EMBGValidateAdvanced(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_EMBG_Val_Advanced")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_EMBG_Numeric")]
        [Display(Name = "User_EMBG", ResourceType = typeof(Resource))]
        public string EMBG { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "User_Address", ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "User_Municipality", ResourceType = typeof(Resource))]
        public string Municipality { get; set; }

        [Display(Name = "User_InsuranceCompany", ResourceType = typeof(Resource))]
        public string InsuranceCompany = "Sava";

        [RegularExpression("^\\+[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_Phone_Numeric")]
        [Display(Name = "User_PhoneNumber", ResourceType = typeof(Resource))]
        public override string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [RegularExpression("^\\+[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_Mobile_Numeric")]
        [Display(Name = "User_MobilePhoneNumber", ResourceType = typeof(Resource))]
        public string MobilePhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Error_PostalCode_Numeric")]
        [Display(Name = "User_PostalCode", ResourceType = typeof(Resource))]
        public string PostalCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "User_PassportNumber", ResourceType = typeof(Resource))]
        public string PassportNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "User_DateOfBirth", ResourceType = typeof(Resource))]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "User_Gender", ResourceType = typeof(Resource))]
        public string Gender { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "User_City", ResourceType = typeof(Resource))]
        public string City { get; set; }


        [Display(Name = "User_Role", ResourceType = typeof(Resource))]
        public string Role { get; set; }

    }

    public class SmsCodeVerify
    {
        public string username { get; set; }

        [Required]
        [Display(Name = "We have sent you a message. Enter the code you received to verify your mobile phone number:")]
        public string SMSCode { get; set; }
    }


    //CUSTOM EMBG VALIDATION CLASS USED FOR validateEMBG_Advanced
    public class EMBGValidateAdvanced : ValidationAttribute
    {
    
        public EMBGValidateAdvanced()
        {
           // this._embg = _embg;
            
        }


        string _embg { get;  set; }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationService validationService = new ValidationService();
            if (value != null)
            {
                Uri uri = new Uri(ConfigurationManager.AppSettings["webpage_apiurl"] + "/api/OkSetup/CheckSSN");
                HttpClient client = new HttpClient();
                client.BaseAddress = uri;
                HttpResponseMessage responseMessage = client.GetAsync(uri).Result;
                string responseBody = responseMessage.Content.ToString();
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return null;
                }
                bool valid = validationService.validateSSN_Advanced(value.ToString());

                if (!valid)
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return null;
        }
      
    }
}
