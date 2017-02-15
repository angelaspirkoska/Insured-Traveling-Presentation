using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFoundPage()
        {
            return View();
        }

        public ActionResult Forbiden()
        {
            return View();
        }

        public ActionResult Conflict()
        {
            return View();
        }
        public ActionResult InternalServerError()
        {
            return View();
        }

        public ActionResult Anauthorized()
        {
            return View();
        }

        public ActionResult MethodNotAllowed()
        {
            return View();
        }

        public ActionResult NotAcceptable()
        {
            return View();
        }

        public ActionResult RequestTimeout()
        {
            return View();
        }

        public ActionResult UnsupportedMediaType()
        {
            return View();
        }

        public ActionResult ServiceUnavailable()
        {
            return View();
        }

        public ActionResult Mail()
        {
            return View();
        }
    }
}