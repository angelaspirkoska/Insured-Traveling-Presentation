using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class LoginUser
    {
        [Required]
        [Display(Name = "Login_UserName", ResourceType = typeof(Resource))]
        public string username { get; set; }

        [Required]
        [Display(Name = "Login_Password", ResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string grant_type { get; set; }
    }
}