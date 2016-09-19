using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    public class ReportLossController : Controller
    {
        public ActionResult Index()
        {
            ShowUserData();   
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(FNOL f)
        {
            ShowUserData();
            if (ModelState.IsValid)
            {
                if (f.insurance == "Health Insurance")
                {
                    f.HealthInsurance = true;
                    f.LuggageInsurance = false;
                    
                }
                else
                {
                    f.LuggageInsurance = true;
                    f.HealthInsurance = false;
                }
                f.ShortDetailed = false;
                
                var uri = new Uri(ConfigurationManager.AppSettings["webpage_url"] + "/api/mobile/ReportLoss");

                var client = new HttpClient { BaseAddress = uri };
                var jsonFormatter = new JsonMediaTypeFormatter();
                HttpContent content = new ObjectContent<FNOL>(f, jsonFormatter);
                HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
                string responseBody = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Successfully reported!";
                    return View();
                }
            }
            return View();
        }

        public void ShowUserData()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            InsuredTravelingEntity db = new InsuredTravelingEntity();
            var user = db.aspnetusers.Where(x => x.UserName == username).ToArray().First();

            ViewBag.Name = user.FirstName + " " + user.LastName;
            ViewBag.Address = user.Address;
            ViewBag.Phone = user.MobilePhoneNumber;
            ViewBag.EMBG = user.EMBG;
        }
    }
}