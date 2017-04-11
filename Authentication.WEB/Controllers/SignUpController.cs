using InsuredTraveling.Filters;
using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Mvc;
using InsuredTraveling.DI;
using System.Linq;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("SignUp")]
    [AllowAnonymous]
    public class SignUpController : Controller
    {
        private IRolesService _rs;
        private IUserService _us;
        private RoleAuthorize _roleAuthorize;
        public SignUpController(IRolesService rs, IUserService us)
        {
            _rs = rs;
            _us = us;
            _roleAuthorize = new RoleAuthorize();
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Gender = Gender();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(User user/*, bool CaptchaValid*/)
        {
            ViewBag.Gender = Gender();

            //if(!CaptchaValid)
            //{
            //    ModelState.AddModelError("reCaptcha", "recaptchaError");
            //    return View(user);
            //}

            if (ModelState.IsValid /*&& CaptchaValid*/)
            {
                user.Role = "Sava_normal";
                user.CreatedBy = _us.GetUserIdByUsername(System.Web.HttpContext.Current.User.Identity.Name);
                Uri uri = new Uri(ConfigurationManager.AppSettings["webpage_apiurl"] + "/api/account/RegisterWeb");
                HttpClient client = new HttpClient();
                client.BaseAddress = uri;
                var jsonFormatter = new JsonMediaTypeFormatter();
                HttpContent content = new ObjectContent<User>(user, jsonFormatter);
                HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.IsSuccessStatusCode)
                {
                    ViewBag.Message = "You are successfully registered!";
                    return View();
                }

            }
            ViewBag.Message = "Registration failed";
            return View();
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            ViewBag.Gender = Gender();
            var roles = Roles();
            if (_roleAuthorize.IsUser("Broker manager"))
            {
                roles = GetBrokerManagerRoles();
            }
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(User user/*, bool CaptchaValid*/)
        {
            ViewBag.Gender = Gender();
            var roles = Roles();
            if (_roleAuthorize.IsUser("Broker manager"))
            {
                roles = GetBrokerManagerRoles();
            }
            ViewBag.Roles = roles;

            if (ModelState.IsValid /*&& CaptchaValid*/)
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

        private List<SelectListItem> Gender()
        {
            List<SelectListItem> data = new List<SelectListItem>();
            data.Add(new SelectListItem
            {
                Text = InsuredTraveling.Resource.Female,
                Value = "Female"
            });
            data.Add(new SelectListItem
            {
                Text = InsuredTraveling.Resource.Male,
                Value = "Male"
            });
            data.Add(new SelectListItem
            {
                Text = InsuredTraveling.Resource.Other,
                Value = "Other"
            });
            return data;
        }

        private List<SelectListItem> Roles()
        {
            return _rs.GetAll().ToList();
        }

        private List<SelectListItem> GetBrokerManagerRoles()
        {

            List<SelectListItem> roles = new List<SelectListItem>();
            roles.Add(new SelectListItem
            {
                Text = "End user",
                Value = "End user"
            });

            roles.Add(new SelectListItem
            {
                Text = "Broker",
                Value = "Broker"
            });
          
            return roles;
         }

    }
}