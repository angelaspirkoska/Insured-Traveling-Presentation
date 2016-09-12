using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("Home")]
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home        
        public ActionResult Index()
        {
            if(System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false)
            {
                Response.Redirect("http://insuredtraveling.com/Login");  
            }
            return View();

        }
    }
}