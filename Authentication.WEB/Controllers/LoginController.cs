using InsuredTraveling.App_Start;
using InsuredTraveling.Filters;
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
        public async Task<ActionResult> Index(LoginUser user)
        {
            if (ModelState.IsValid)
            {
                var uri = new Uri(ConfigurationManager.AppSettings["webpage_url"] + "/token");
                var client = new HttpClient {BaseAddress = uri};
                IDictionary<string, string> userData = new Dictionary<string, string>();
                userData.Add("username", user.username);
                userData.Add("password", user.password);
                userData.Add("grant_type", "password");
                HttpContent content = new FormUrlEncodedContent(userData);
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var responseMessage = await client.PostAsync(uri, content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(responseBody);
                    string token = data.access_token;
                    if (!String.IsNullOrEmpty(token))
                    {
                        string encryptedToken = HttpUtility.UrlEncode(EncryptionHelper.Encrypt(token));
                        HttpCookie cookieToken = new HttpCookie("token", encryptedToken);
                        HttpContext.Response.Cookies.Set(cookieToken);

                        HttpCookie cookieUserName = new HttpCookie("username", user.username);
                        HttpContext.Response.Cookies.Set(cookieUserName);

                        HttpCookie cookieExpires = new HttpCookie("expires", DateTime.UtcNow.AddHours(5).ToString());
                        HttpContext.Response.Cookies.Set(cookieExpires);

                        Response.Redirect("/home");
                    }
                    else
                    {
                        ModelState.AddModelError("loginErr", "usernameOrPasswordError");
                    }
                }
                else
                {
                    ModelState.AddModelError("loginErr", "usernameOrPasswordError");
                }
            }
            else
            {
                ModelState.AddModelError("loginErr", "usernameOrPasswordEmpty");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            if(RoleAuthorize.IsUserLoggedIn())
            {
                Response.Redirect("/home");
            }
            return View();
        }

       
    }
}