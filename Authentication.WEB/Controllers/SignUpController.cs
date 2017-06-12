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
using System.Net.Mail;
using Authentication.WEB.Services;

namespace InsuredTraveling.Controllers
{
    [RoutePrefix("SignUp")]
    public class SignUpController : Controller
    {
        private IRolesService _rs;
        private IUserService _us;

        public SignUpController(IRolesService rs, IUserService us)
        {
            _rs = rs;
            _us = us;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Gender = Gender();
            // return View();
            return RedirectToAction("Index", "Home");
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
                    List < aspnetuser > sava_admins = _us.GetUsersByRoleName("Sava_admin");
                    foreach (var sava_admin in sava_admins)
                    {
                        try
                        {
                            var inlineLogo =
                                new LinkedResource(
                                    System.Web.HttpContext.Current.Server.MapPath(
                                        "~/Content/img/EmailHeaderWelcome1.png"));
                            inlineLogo.ContentId = Guid.NewGuid().ToString();

                            string body2 = string.Format(@"   
                            <div style='margin-left:20px'>
                            <p> <b>Welcome to My Sava </b> - the standalone platform for online sales of insurance policies.</p>                  
                            <br /> <br /> To inform you that the following user was registered at MySava : 
                            <br />" + "Username: " + user.UserName +
                                                         "<br /> " + "SSN: " + user.EMBG +
                                                         "<br /> " + "Email: " + user.Email +
                                                         "<br /> " + "Role: " + user.Role +
                                                         "<br /> <br />Thanks </div>", inlineLogo.ContentId);


                            MailService mailService2 = new MailService(sava_admin.Email, "signup@insuredtraveling.com");
                            mailService2.setSubject("My Sava - User registered on Sava");
                            mailService2.setBodyText(body2, true);

                            mailService2.sendMail();
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = ex;
                            return View();
                        }
                    }
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
            if (RoleAuthorize.IsUser("Broker manager"))
            {
                roles = GetBrokerManagerRoles();
            }
            if (RoleAuthorize.IsUser("Sava_admin"))
            {
                roles = GetAdminManagerRoles();
            }
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(User user/*, bool CaptchaValid*/)
        {
            ViewBag.Gender = Gender();
            var roles = Roles();

            if (RoleAuthorize.IsUser("Sava_admin"))
            {
                roles = GetAdminManagerRoles();
            }

            if (RoleAuthorize.IsUser("Broker manager"))
            {
                roles = GetBrokerManagerRoles();
            }
         
            ViewBag.Roles = roles;

            if ( user.Role != "Sava_Seller")
            {
                this.ModelState.Remove("PassportNumber");
            }

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
                }else
                {
                    ViewBag.Message = "Unsuccessful registered!";
                    return View();
                }

            }else
            {
                ViewBag.Message = "Unsuccessful registered!";
                return View();
            }
            
            
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
        private List<SelectListItem> GetAdminManagerRoles()
        {

            List<SelectListItem> roles = new List<SelectListItem>();

            roles.Add(new SelectListItem
            {
                Text = "Sava_Seller",
                Value = "Sava_Seller"
            });
            roles.Add(new SelectListItem
            {
                Text = "Sava_Sport_Vip",
                Value = "Sava_Sport_Vip"
            });

            roles.Add(new SelectListItem
            {
                Text = "Sava_Sport+",
                Value = "Sava_Sport+"
            });
            roles.Add(new SelectListItem
            {
                Text = "Sava_Support",
                Value = "Sava_Support"
            });
            roles.Add(new SelectListItem
            {
                Text = "Sava_normal",
                Value = "Sava_normal"
            });

            return roles;
        }


    }
}