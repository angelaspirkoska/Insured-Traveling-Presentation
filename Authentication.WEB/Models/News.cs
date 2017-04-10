using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class News
    {
        public string Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Content { get; set; }
        
        public string InsuranceCompany { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string ImageLocation { get; set; }
    }
}
