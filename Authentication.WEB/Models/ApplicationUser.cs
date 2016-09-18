using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "EMBG")]
        public string EMBG { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Municipality")]
        public string Municipality { get; set; }

        [Required]
        [Display(Name = "Mobile Phone Number")]
        public string MobilePhoneNumber { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }

        [Display(Name = "Insurance Company")]
        public string InsuranceCompany { get; set; }

        [Required]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }         

        public string Gender { get; set; }
        public string ActivationCodeMail { get; set; }
        public string ActivationCodeSMS { get; set; }
        public string GroupID { get; set; }



    }
}
