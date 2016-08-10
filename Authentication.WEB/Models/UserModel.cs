using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace InsuredTraveling.Models
{
    public class UserModel 
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Insurance Company")]
        public string InsuranceCompany = "Sava";

        [Required]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Mobile Phone Number")]
        public string MobilePhoneNumber { get; set; }

        [Required]
        [Display(Name ="Gender")]
        public string Gender { get; set; }
    }
}