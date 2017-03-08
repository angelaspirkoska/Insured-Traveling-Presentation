using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "User_UserName" ,ResourceType = typeof(Resource))]
        public override string UserName { get; set; }

        [Required]
        [Display(Name = "User_FirstName" , ResourceType = typeof(Resource))]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "User_LastName" , ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "User_Email", ResourceType = typeof(Resource))]
        public override string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "User_Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "User_ConfirmPassword" , ResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "EMBG must be numeric")]
        [Display(Name = "User_EMBG", ResourceType = typeof(Resource))]
        public string EMBG { get; set; }

        [Required]
        [Display(Name = "User_Address", ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Display(Name = "User_Municipality",ResourceType =typeof(Resource))]
        public string Municipality { get; set; }

        [Display(Name = "User_InsuranceCompany", ResourceType = typeof(Resource))]
        public string InsuranceCompany = "Sava";

        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
        [Display(Name = "User_PhoneNumber", ResourceType = typeof(Resource))]
        public override string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile number must be numeric")]
        [Display(Name = "User_MobilePhoneNumber", ResourceType = typeof(Resource))]
        public string MobilePhoneNumber { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Postal code must be numeric")]
        [Display(Name = "User_PostalCode", ResourceType = typeof(Resource))]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "User_PassportNumber", ResourceType = typeof(Resource))]
        public string PassportNumber { get; set; }

        [Display(Name = "User_DateOfBirth", ResourceType = typeof(Resource))]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "User_Gender", ResourceType = typeof(Resource))]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "User_City", ResourceType = typeof(Resource))]
        public string City { get; set; }
    }

    public class SmsCodeVerify
    {
        public string username { get; set; }

        [Required]
        [Display(Name = "We have sent you a message. Enter the code you received to verify your mobile phone number:")]
        public string SMSCode { get; set; }
    }
}