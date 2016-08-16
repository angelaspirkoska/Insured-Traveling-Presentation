using InsuredTraveling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace InsuredTraveling.Controllers
{
    [RoutePrefix("SignUp")]
    [AllowAnonymous]
    public class SignUpController : Controller
    {
        
        [HttpPost]
        public async Task<ActionResult> Index(User user)
        {
            ViewBag.Gender = Gender();

            if (ModelState.IsValid)
            {
                //var jsonData = Json(user, JsonRequestBehavior.AllowGet);
                //return (Json(new { success = true, responseText = jsonData }, JsonRequestBehavior.AllowGet));
                Uri uri = new Uri("http://localhost:19655/api/account/register");
                HttpClient client = new HttpClient();
                client.BaseAddress = uri;
                var jsonFormatter = new JsonMediaTypeFormatter();
                HttpContent content = new ObjectContent<User>(user, jsonFormatter);
                HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Successfully registered!";
                    Response.Redirect("/Login");
                }

            }
            ViewBag.Message = "Registration faild";
            return View();
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