using InsuredTraveling.App_Start;
using System.Configuration;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("Home")]
    [Authorize(Roles ="")]
    public class HomeController : BaseController
    {
        // GET: Home        
        public ActionResult Index()
        {
            if(System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false)
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
            if (Url.IsLocalUrl(returnUrl))
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