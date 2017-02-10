using InsuredTraveling.App_Start;
using InsuredTraveling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace InsuredTraveling.Controllers
{
    public class LoginController : Controller
    {

        [HttpPost]
        public async Task<ActionResult> Index(LoginUser user, bool CaptchaValid)
        {

            if (!CaptchaValid)
            {
                ModelState.AddModelError("reCaptcha", "Invalid reCaptcha");
                return View(user);
            }
            if (ModelState.IsValid)
            {
                user.grant_type = "password";
                var uri = new Uri(ConfigurationManager.AppSettings["webpage_url"] + "/token");
                var client = new HttpClient {BaseAddress = uri};
                IDictionary<string, string> userData = new Dictionary<string, string>();
                userData.Add("username", user.username);
                userData.Add("password", user.password);
                userData.Add("grant_type", user.grant_type);
                userData.Add("client_id", "InsuredTravel");
                HttpContent content = new FormUrlEncodedContent(userData);
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var responseMessage = client.PostAsync(uri, content).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(responseBody);
                    string token = data.access_token;
                    string refresh_token = data.refresh_token;
                    if (!String.IsNullOrEmpty(token))
                    {
                        string encryptedToken = HttpUtility.UrlEncode(EncryptionHelper.Encrypt(token));
                        HttpCookie cookieToken = new HttpCookie("token", encryptedToken);
                        cookieToken.Expires = DateTime.Now.AddYears(1);
                        HttpContext.Response.Cookies.Add(cookieToken);

                        //string encryptedRefreshToken = HttpUtility.UrlEncode(EncryptionHelper.Encrypt(refresh_token));
                        //string decryptedRefreshToken = EncryptionHelper.Decrypt(HttpUtility.UrlEncode(encryptedRefreshToken));
                        HttpCookie cookieRefreshToken = new HttpCookie("refresh_token", refresh_token);
                        cookieRefreshToken.Expires = DateTime.Now.AddYears(1);
                        HttpContext.Response.Cookies.Add(cookieRefreshToken);

                        Response.Redirect("/home");
                    }
                    else
                    {
                        ModelState.AddModelError("loginErr", "Invalid username or password");
                    }
                }else
                {
                    ModelState.AddModelError("loginErr", "Something went wrong!");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User != null)
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    Response.Redirect("/home");
                }
            }
            return View();
        }

       
    }
}