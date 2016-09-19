using System.Configuration;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("Home")]
    [Authorize(Roles ="")]
    public class HomeController : Controller
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
    }
}