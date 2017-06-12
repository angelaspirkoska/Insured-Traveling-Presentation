using InsuredTraveling.App_Start;
using System.Configuration;
using System.Web.Mvc;
using InsuredTraveling.Filters;
using System;
using InsuredTraveling.Schedulers;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("Home")]
    [SessionExpireAttribute]
    public class HomeController : Controller
    {  
        public ActionResult Index()
        {
            return View();
        }
    }
}