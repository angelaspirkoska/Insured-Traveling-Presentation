using InsuredTraveling.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Linq;
using static InsuredTraveling.Models.AdminPanel;
using System.Configuration;
using System.Net.Http;
using InsuredTraveling.Filters;
using InsuredTraveling.DI;
using InsuredTraveling.App_Start;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;

namespace InsuredTraveling.Controllers
{
    [Authorize]
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        #region Parameters
        private readonly AuthRepository _authRepository;
        private readonly IUserService _userService;
        private ApplicationUserManager _userManager;
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }
        #endregion

        public AccountController(IUserService userService)
        {
            _userService = userService;
            _authRepository = new AuthRepository();
        }

        public AccountController()
        {
            _authRepository = new AuthRepository();
        }

        public AccountController(ApplicationUserManager userManager,
                        ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            _userManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public IHttpActionResult AddUserToRole(Roles r)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = _authRepository.AddUserToRole(r.UserID, r.Name);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [HttpPost]
        [Route("AddClient")]
        public IHttpActionResult AddClient(Client c)
        {
            if (_authRepository.AddClient(c) != -1)
                return Ok();
            return InternalServerError();
        }

        [HttpPost]
        [Route("AddRole")]
        public IHttpActionResult AddRole(Roles r)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = _authRepository.AddRole(r);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult results = _authRepository.FindUserByUsername(userModel.UserName).Result;
            var errors = results.Errors.ToList();

            if (errors[0] == "OK")
            {
                IdentityResult result = await _authRepository.CreateApplicationUser(userModel);
                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                    return errorResult;

                var id = _authRepository.GetUserID(userModel.UserName);
                return Ok("userId: " + id);
            }
            string errorString = "";
            foreach (var err in results.Errors)
            {
                errorString = errorString + " " + err.ToString();
            }
            return BadRequest(errorString);

        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("RegisterWeb")]
        public async Task<IHttpActionResult> RegisterWeb(User userModel)
        {
            if (userModel.Role != "Sava_Seller")
            {
                this.ModelState.Remove("PassportNumber");
            }
            if (!ModelState.IsValid && (userModel.Role == "Sava_Seller")||(!ModelState.IsValid && (userModel.Role != "Sava_Seller")))
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _authRepository.CreateApplictionUserWeb(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [HttpPost]
        [System.Web.Http.AllowAnonymous]
        [Route("ActivateAccount")]
        public IHttpActionResult ActivateAccount(UserDTO username)
        {
            _authRepository.ActivateAccount(username.username);
            return Ok();
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("FindUser")]
        public async Task<IHttpActionResult> FindUsername(UserDTO username)
        {
            if (!String.IsNullOrEmpty(username.username))
            {
                IdentityResult result = await _authRepository.FindUserByUsername(username.username);

                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }

            }
            return Ok();
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("FindSSN")]
        public async Task<IHttpActionResult> FindSSN(JObject ssn)
        {
            string StringSsn = ssn["ssn"].ToString();
            if (!String.IsNullOrEmpty(StringSsn))
            {
                var result = _userService.GetUserBySSN(StringSsn);

                if (result == null)
                {
                    return Ok();
                }

            }
            return Content(HttpStatusCode.BadRequest, "SSN Not valid");
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("GetUserID")]
        public IHttpActionResult GetUserID(UserDTO username)
        {
            if (username != null)
            {
                var id = _authRepository.GetUserID(username.username);
                var data = new JObject();
                data.Add("id", id);
                return Json(data);
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
                _authRepository.Dispose();
            }

            base.Dispose(disposing);
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("FillUser")]
        public async Task<IHttpActionResult> FillUserDetails(UserModel_Detail userModel)
        {
            IdentityResult result = await _authRepository.FillUserDetails(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("ForgetPassword")]
        public void ForgetPassword(UserDTO username)
        {
            if (username.username != null)
                _authRepository.SendEmailForForgetPasswordByUserName(username.username);
            else
                _authRepository.SendEmailForForgetPasswordByEmail(username.email);
        }

        [TwilioDownHandlingFilter]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("SendSmSCode")]
        public async Task<IHttpActionResult> SendSmsCode(UserDTO user)
        {
            var result = await _authRepository.SendSmsCode(user.username);
            IHttpActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("ConfirmSms")]
        public async Task<IHttpActionResult> ConfirmSms(UserDTO user)
        {
            var result = await _authRepository.ConfirmSmsCode(user.username, user.code);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                string resultError = " ";

                foreach (var r in result.Errors)
                {
                    resultError += r.ToString() + " ";
                }
                if (resultError.Trim(' ') == "OK")
                {
                    return null;
                }
                return BadRequest(resultError);
            }

            return null;
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("DeleteToken")]
        [AllowAnonymous]
        public IHttpActionResult DeleteToken()
        {

            if (HttpContext.Current.Request.Cookies["token"] != null)
            {
                var token = HttpContext.Current.Request.Cookies["token"];
                token.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Remove("token");
                HttpContext.Current.Response.Cookies.Clear();
                HttpContext.Current.Response.Cookies.Set(token);
            }

            if (HttpContext.Current.Request.Cookies["username"] != null)
            {
                var username = HttpContext.Current.Request.Cookies["username"];
                username.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Remove("username");
                HttpContext.Current.Response.Cookies.Clear();
                HttpContext.Current.Response.Cookies.Set(username);
            }

            if (HttpContext.Current.Request.Cookies["expires"] != null)
            {
                var expires = HttpContext.Current.Request.Cookies["expires"];
                expires.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Remove("expires");
                HttpContext.Current.Response.Cookies.Clear();
                HttpContext.Current.Response.Cookies.Set(expires);
            }

            return Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");
        }


        #region Helpers
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }
        #endregion
    }
}
