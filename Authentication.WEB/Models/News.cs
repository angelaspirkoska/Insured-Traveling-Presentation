using System.ComponentModel.DataAnnotations;
using System.Web;

namespace InsuredTraveling.Models
{
    public class News
    {
        public string Id { get; set; }
      
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NewsMaximumLength")]
        public string Title { get; set; }
        
        [StringLength(120, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NewsMaximumLength")]
        public string Content { get; set; }
        public bool isNotification { get; set; }
        
        public string InsuranceCompany { get; set; }
       
        public string ImageLocation { get; set; }
        public HttpPostedFileBase Image{ get; set; }
    }
}
