using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class UserModel_Detail
    {
        public string username { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "EMBG")]
        public string EMBG { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Municipality")]
        public string Municipality { get; set; }
    }
}