using InsuredTraveling.App_Start;
using System.Configuration;
using System.Web.Mvc;
using InsuredTraveling.Filters;
using System;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("Home")]
    //[Authorize(Roles ="")]
    [SessionExpire]
    public class HomeController : Controller
    {  
        public ActionResult Index()
        {
            if(!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");  
            }
            return View();
        }
        
        public ActionResult ChangeLanguage(string lang)
        {
            new SiteLanguages().SetLanguage(lang);
            return RedirectToLocal(Request.UrlReferrer.AbsoluteUri);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}