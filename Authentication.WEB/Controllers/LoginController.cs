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
                IDictionary<string, string> data1 = new Dictionary<string, string>();
                data1.Add("username", user.username);
                data1.Add("password", user.password);
                data1.Add("grant_type", user.grant_type);
                HttpContent content = new FormUrlEncodedContent(data1);
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var responseMessage = client.PostAsync(uri, content).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = await responseMessage.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(responseBody);
                    string token = data.access_token;
                    if (!String.IsNullOrEmpty(token))
                    {
                        var c = new HttpCookie("token") { ["t"] = (string.IsNullOrEmpty(token)) ? " " : token };
                        HttpContext.Response.Cookies.Add(c);
                        Response.Redirect("/home");
                    }
                    else
                    {
                        ModelState.AddModelError("loginErr", "Invalid username or password");
                    }
                }else
                {
                    ModelState.AddModelError("loginErr", "Something get wrong!");
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