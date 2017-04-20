using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.DI;
using InsuredTraveling.Filters;
using InsuredTraveling.Models;
using System.Threading.Tasks;
using AutoMapper;

namespace InsuredTraveling.Controllers
{
    [SessionExpire]
    public class EventsController : Controller
    {
        private IUserService _us;
        private IEventsService _es;
        public EventsController(IUserService us, IEventsService es)
        {
            _us = us;
            _es = es;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.EventTypes = GetTypeOfEvent();
            return View();
        }

        [HttpPost]
        public ActionResult AddEvent(Event newEvent)
        {
            ViewBag.EventTypes = GetTypeOfEvent();
            if (ModelState.IsValid)
            {
                newEvent.CreatedBy = _us.GetUserIdByUsername(System.Web.HttpContext.Current.User.Identity.Name);
                var mappedEvent = Mapper.Map<Event, @event>(newEvent);
                try
                {
                    _es.AddEvent(mappedEvent);
                    ViewBag.Success = "Success";
                }
                catch
                {
                    ViewBag.Success = "Failure";
                }
                finally
                {

                }
            }
            return View("Index",newEvent);
        }

        private List<SelectListItem> GetTypeOfEvent()
        {
            List<SelectListItem> EventTypes = new List<SelectListItem>();
            var a = new SelectListItem
            {
                Text = "Normal",
                Value = "1"
            };
            var b = new SelectListItem
            {
                Text = "VIP",
                Value = "2"
            };
            var c = new SelectListItem
            {
                Text = "VIP Chat",
                Value = "3"
            };
            EventTypes.Add(a);
            EventTypes.Add(b);
            EventTypes.Add(c);
            return EventTypes;
        }
    }
}