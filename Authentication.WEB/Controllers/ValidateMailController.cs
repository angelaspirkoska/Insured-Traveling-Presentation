using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{

    [RoutePrefix("ValidateMail")]
    public class ValidateMailController : Controller
    {
        public ActionResult Index(string ID)
        {
            AuthRepository _repo = new AuthRepository();
            bool result = _repo.ValidateMail(ID);
            if (result)
            {
                ViewBag.Message = "You have successfully verified yor account.";
            }
            else
            {
                ViewBag.Message = "The verification of your account failed, are you sure you are the one that created the account on the OptimalInsured.com";
            }
            return View();
        }
    }
}