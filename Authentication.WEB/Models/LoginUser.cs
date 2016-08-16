using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class LoginUser
    {
        [Required]
        [Display(Name = "Username:")]
        public string username { get; set; }

        [Required]
        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string grant_type { get; set; }
    }
}