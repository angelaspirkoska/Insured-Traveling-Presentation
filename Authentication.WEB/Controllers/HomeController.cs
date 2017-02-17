using InsuredTraveling.App_Start;
using System.Configuration;
using System.Web.Mvc;
using InsuredTraveling.Filters;
using System;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("Home")]
    [SessionExpire]
    public class HomeController : Controller
    {  
        public ActionResult Index()
        {
             if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)                
                Response.Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");                               

            return View();
        }
    }
}