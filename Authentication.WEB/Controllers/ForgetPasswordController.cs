using InsuredTraveling.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("ForgetPassword")]
    public class ForgetPasswordController : Controller
    {
        [HttpPost]
        // GET: ForgetPassword
        public async Task<ActionResult> Index(ForgetPasswordModel model)
        {
            ViewBag.Message = " ";
            if (ModelState.IsValid)
            {
                model.ID = Request.Params["ID"];
                AuthRepository _repo = new AuthRepository();
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
    }
}