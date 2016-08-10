using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace InsuredTraveling.Controllers
{
    [RoutePrefix("SignUp")]
    [AllowAnonymous]
    public class SignUpController : Controller
    {
        
        [HttpPost]
        public ActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                var jsonData = Json(user, JsonRequestBehavior.AllowGet);
                return (Json(new { success = true, responseText = jsonData }, JsonRequestBehavior.AllowGet));
            }
            return View();
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

    }
}