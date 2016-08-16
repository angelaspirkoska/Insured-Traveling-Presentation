using System.ComponentModel.DataAnnotations;

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