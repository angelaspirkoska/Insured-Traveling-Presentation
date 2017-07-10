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
        private IErrorLogService _els;
        public ErrorLogController(IErrorLogService els)
        {
            _els = els;
        }
        [HttpGet]
        // GET: ErrorLog
        public ActionResult Index()
        {
            IQueryable <elmah_error> errors = _els.GetAllErrorLogs();
            return View(errors);
        }
    }
}
