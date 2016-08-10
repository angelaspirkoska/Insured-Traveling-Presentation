using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        // GET: Home        
        public ActionResult Index()
        {
            if(System.Web.HttpContext.Current.User == null)
            {
                Response.Redirect("http://localhost:19655/Login");  
            }
            return View();

        }
    }
}