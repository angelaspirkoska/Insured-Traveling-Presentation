using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
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
        // GET: ReportLoss
        public ActionResult Index()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            InsuredTravelingEntity db = new InsuredTravelingEntity();
            var user = db.aspnetusers.Where(x => x.UserName == username).ToArray().First();

            ViewBag.Name = user.FirstName + " " + user.LastName;
            ViewBag.Address = user.Address;
            ViewBag.Phone = user.MobilePhoneNumber;
            ViewBag.EMBG = user.EMBG;
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(FNOL f)
        {
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
                
                var uri = new Uri("http://localhost:19655/api/mobile/ReportLoss");

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
    }
}