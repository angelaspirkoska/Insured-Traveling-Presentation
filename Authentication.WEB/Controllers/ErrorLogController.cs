using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.DI;
using InsuredTraveling.Filters;

namespace InsuredTraveling.Controllers
{
    [CustomActionFilter]
    public class ErrorLogController : Controller
    {
        // GET: ErrorLog
        public ActionResult Index()
        {
            return View();
        }
    }
}