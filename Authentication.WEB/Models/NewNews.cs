using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class NewNews
    {
        public string Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "NewsMaximumLength")]
        public string Title { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        
        public string Content { get; set; }
        public bool isNotification { get; set; }

        public string InsuranceCompany { get; set; }
   
        public string ImageLocation { get; set; }

        public List<News> ListNews {get;set;}
        
        public HttpPostedFileBase Image { get; set; }
    }
}