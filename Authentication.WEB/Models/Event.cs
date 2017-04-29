using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace InsuredTraveling.Models
{
    public class Event
    {
        public int id { get; set; }

        public string CreatedBy { get; set; }

        public int PeopleAttending { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Location { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Organizer { get; set; }

        public string EventType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public DateTime PublishDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public TimeSpan EndTime { get; set; }

        public List<SelectListItem> EventTypes { get; set; }
        
    }
}