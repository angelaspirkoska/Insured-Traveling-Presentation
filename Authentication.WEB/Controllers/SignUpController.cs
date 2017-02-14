using InsuredTraveling.Filters;
using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace InsuredTraveling.Controllers
{
    [SmarteraspDownHandlingFilter]
    [RoutePrefix("SignUp")]
    [AllowAnonymous]
    public class SignUpController : Controller
    {
       
        [HttpPost]
        public async Task<ActionResult> Index(User user, bool CaptchaValid)
        {
            ViewBag.Gender = Gender();

            if (ModelState.IsValid && CaptchaValid)
            {
                Uri uri = new Uri(ConfigurationManager.AppSettings["webpage_apiurl"] + "/api/account/RegisterWeb");
                HttpClient client = new HttpClient();
                client.BaseAddress = uri;
                var jsonFormatter = new JsonMediaTypeFormatter();
                HttpContent content = new ObjectContent<User>(user, jsonFormatter);
                HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Successfully registered!";
                    return View();
                }

            }
            ViewBag.Message = "Registration failed";
            return View();
        }

        [HttpGet]
        [Route("MobilePhoneVerification")]
        public ActionResult MobilePhoneVerification(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        [HttpPost]
        [Route("MobilePhoneVerification")]
        public async Task<ActionResult> MobilePhoneVerification(SmsCodeVerify code)
        {
            if (ModelState.IsValid)
            {
                AuthRepository _repo = new AuthRepository();
                var result = await _repo.ConfirmSmsCode(code.username, code.SMSCode);
                if (result.Succeeded)
                {
                    ViewBag.CodeMsg = "OK";
                    return View(code);
                }
                ViewBag.CodeMsg = "NOK";
                return View(code);
            }
            return View(code);
        }


        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Gender = Gender();
            return View();
        }

        private List<SelectListItem> Gender()
        {
            List<SelectListItem> data = new List<SelectListItem>();
            data.Add(new SelectListItem
            {
                Text = "Female",
                Value = "Female"
            });
            data.Add(new SelectListItem
            {
                Text = "Male",
                Value = "Male"
            });
            data.Add(new SelectListItem
            {
                Text = "Other",
                Value = "Other"
            });
            return data;
        }

    }
}