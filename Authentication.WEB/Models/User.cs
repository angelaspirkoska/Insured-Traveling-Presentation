using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "Username")]
        public override string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        public override string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "EMBG")]
        public string EMBG { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Municipality")]
        public string Municipality { get; set; }

        [Display(Name = "Insurance Company")]
        public string InsuranceCompany = "Sava";

        [Required]
        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Mobile Phone Number")]
        public string MobilePhoneNumber { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }

        [Display(Name = "Date of birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }


    }
}