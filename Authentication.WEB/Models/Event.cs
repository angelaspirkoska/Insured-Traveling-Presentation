﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Models
{
    public class Event
    {
        public int id { get; set; }
        public string CreatedBy { get; set; }
        //public List<SearchRegisteredUser> ListUsers { get; set; }
        public int PeopleAttending { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Organizer { get; set; }
        public string EventType { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PublishDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<SelectListItem> EventTypes { get; set; }
        
    }
}