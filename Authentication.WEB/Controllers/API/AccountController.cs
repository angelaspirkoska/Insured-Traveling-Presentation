using InsuredTraveling.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Linq;
using static InsuredTraveling.Models.AdminPanel;
using System.Configuration;
using AutoMapper;

namespace InsuredTraveling.Controllers
{
    [Authorize]
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo;

        public AccountController()
        {
            _repo = new AuthRepository();
        }
        
        //Za testiranje samo
        [HttpPost]
        public IHttpActionResult Check()
        {
            FirstNoticeOfLossReportViewModel f = new FirstNoticeOfLossReportViewModel();
            //f.PolicyType = "Comfort";
            //f.policyNumber = 123456;
            //f.username = "Daki123";
            //f.TransactionAccount = "jsJADKJASD";
            //f.deponent = "akhKSDds";
            //f.insuredAddress = "jsaksf";
            //f.insuredEMBG = "jsaksf";
            //f.insuredName = "jsaksf";
            //f.insuredPhone = "jsaksf";
            //f.insuredTransactionAccount = "jsaksf";
            //f.deponentInsured = "msalfdf";
            //f.relationship = "friend";
            //f.travelDestination = "gldfd";
            //f.message = "fnlgfsldfgk;dfxlgd;f";
            //f.additionalDocumentsHanded = "dajda";
            //f.travelTimeFrom = DateTime.Now.TimeOfDay;
            //f.travelTimeTo = DateTime.Now.TimeOfDay;
            //f.travelDateFrom = DateTime.Now;
            //f.travelDateTo = DateTime.Now;
            //f.message = "sjaDKa";
            //f.valueExpenses = 100;
            ////f.ShortDetailed = true;
            ////f.LuggageInsurance = true;
            ////f.HealthInsurance = false;
            ////f.WebMobile = false;
            //f.transportationType = "Car";

            

            var fnol = Mapper.Map<FirstNoticeOfLossReportViewModel, first_notice_of_loss>(f);
            return Ok(new { fnol = fnol});
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public IHttpActionResult AddUserToRole(Roles r)
        {
            IdentityResult result =_repo.AddUserToRole(r.UserID, r.Name);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [HttpPost]
        [Route("AddRole")]
        public IHttpActionResult AddRole(Roles r)
        {
            IdentityResult result =  _repo.AddRole(r);

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

            IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("RegisterWeb")]
        public async Task<IHttpActionResult> RegisterWeb(User userModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUserWeb(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [HttpPost]
        [Route("ActivateAccount")]
        public IHttpActionResult ActivateAccount(string username)
        {
             _repo.ActivateAccount(username);
            return Ok();
        }
        
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("FindUser")]
        public async Task<IHttpActionResult> FindUsername(Username username)
        {
            if (!String.IsNullOrEmpty(username.username))
            {
                IdentityResult result = await _repo.FindUserByUsername(username.username);

                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }

            }
            return Ok();
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("GetUserID")]
        public IHttpActionResult GetUserID(Username username)
        {
            if (username != null)
            {
                var id = _repo.GetUserID(username.username);
                var data = new JObject();
                data.Add("id", id);
                return Json(data);
            }
            return null;
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("FillUser")]
        public async Task<IHttpActionResult> FillUserDetails(UserModel_Detail userModel)
        {
            IdentityResult result = await _repo.FillUserDetails(userModel);

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
        public void ForgetPassword(Username username)
        {
            if (username.username !=null)
            {
                _repo.ForgetPassword(username.username);
            }
            else
            {
                _repo.ForgetPassword2(username.email);
            }
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("SendSmSCode")]
        public async Task<IHttpActionResult> SendSmsCode(Username user)
        {
            var result = await  _repo.SendSmsCode(user.username);

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
        public async Task<IHttpActionResult> ConfirmSms(Username user)
        {
            var result = await _repo.ConfirmSmsCode(user.username, user.code);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("DeleteToken")]
        public IHttpActionResult DeleteToken()
        {
            if (HttpContext.Current.Request.Cookies["token"] == null) return Redirect(ConfigurationManager.AppSettings["webpage_url"] +"/Login");
            var c = HttpContext.Current.Request.Cookies["token"];
            c.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Remove("token");
            HttpContext.Current.Response.Cookies.Clear();
            HttpContext.Current.Response.Cookies.Set(c);
            return Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");
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
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                string resultError = " ";

                foreach(var r in result.Errors)
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
    }
}
