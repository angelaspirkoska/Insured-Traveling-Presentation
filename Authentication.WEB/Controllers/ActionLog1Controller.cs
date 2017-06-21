using System;
using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
using InsuredTraveling.DI;

namespace InsuredTraveling.Controllers
    {
        public class ActionLog1Controller : Controller
        {
            private ILogActivityService _ls;

            public ActionLog1Controller(ILogActivityService ls)
            {
                _ls = ls;
            }
            [HttpGet]
            public ActionResult Index()
            {
                IQueryable<log_activities> logs = _ls.GetAllLogs();
                return View(logs);
            }
        }

       
    }
