using System.ComponentModel.DataAnnotations;
using System.Web;

namespace InsuredTraveling.Models
{
    public class SalePoints
    {

        public int  id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string LocationName { get; set; }

        public bool insuranceDeal { get; set; }
        public bool damageReport { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Municipality { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Street { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "OnlyNumbers")]
        public int PostalCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        
        public string OpeningHours { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public string PhoneNumber { get; set; }
      


    }
}