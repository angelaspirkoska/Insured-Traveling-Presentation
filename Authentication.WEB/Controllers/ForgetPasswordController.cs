using InsuredTraveling.Filters;
using InsuredTraveling.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{

    [RoutePrefix("ForgetPassword")]
    public class ForgetPasswordController : Controller
    {
        private AuthRepository _repo;

        public ForgetPasswordController()
        {
            _repo = new AuthRepository();
        }

        [HttpPost]
        public async Task<ActionResult> Index(ForgetPasswordModel model)
        {
            ViewBag.Message = " ";
            if (ModelState.IsValid)
            {
                model.ID = System.Web.HttpContext.Current.Request.QueryString["ID"].ToString();

                IdentityResult result = await _repo.PasswordChange(model);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password changed successfully!";
                    return View();
                }
                ViewBag.Message = "Not valid user";
                return View();
            }
            ViewBag.Message = " ";
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EnterUsernameOrMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnterUsernameOrMail(LoginUser u)
        {
            AuthRepository _repo = new AuthRepository();
            if (_repo.ValidUsernameOrMail(u.username))
            {
                ViewBag.Msg = "Check your mail to reset your password";
                return View();
            }
            ViewBag.Msg = "Not valid username or mail";
            return View();
        }
    }
}