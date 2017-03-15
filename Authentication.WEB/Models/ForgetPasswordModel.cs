using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class ForgetPasswordModel
    {
        public string ID { get; set; }

        [Required]
        [Display(Name ="New Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name ="Repeat New Password")]
        [Compare("Password", ErrorMessage ="The passwords must match")]
        public string NewPassword { get; set; }
    }
}