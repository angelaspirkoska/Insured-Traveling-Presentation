using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class Event
    {
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
        public string EventType { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PublishDate { get; set; }
    }
}