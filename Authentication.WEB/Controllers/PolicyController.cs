using Authentication.WEB.Services;
using Newtonsoft.Json.Linq;
using System;
using Rotativa;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Web.Mvc;
using InsuredTraveling;
using System.Configuration;
using InsuredTraveling.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity;

namespace Authentication.WEB.Controllers
{
    public class PolicyController : Controller
    {
        // GET: Policy
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var type_policies = GetTypeOfPolicy();
            var countries = GetTypeOfCountry();
            var franchises = GetTypeOfFranchise();
            var additional_charges = GetTypeOfAdditionalCharges();

            await Task.WhenAll(type_policies, countries, franchises, additional_charges);

            ViewBag.TypeOfPolicy = type_policies.Result;
            ViewBag.Countries = countries.Result;
            ViewBag.Franchise = franchises.Result;
            ViewBag.additional_charges = additional_charges.Result;

            
            return View();
        }

        [HttpPost]
        public ActionResult Index(Policy p)
        {
            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> CreatePolicy(travel_policy policy)
        {
            ValidationService validationService = new ValidationService();
            RatingEngineService ratingEngineService = new RatingEngineService();

            InsuredTravelingEntity entityDB = new InsuredTravelingEntity();
            travel_policy polisaEntity = new travel_policy();

            Uri uri = new Uri(ConfigurationManager.AppSettings["webpage_url"] + "/api/premium/calculate");
            HttpClient client = new HttpClient();
            client.BaseAddress = uri;
            var mediaType = new MediaTypeHeaderValue("application/json");
            var jsonFormatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<travel_policy>(policy, jsonFormatter);

            HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
            string responseBody = await responseMessage.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(responseBody);
            int premium = data.PremiumAmount;

            bool valid = validationService.masterValidate(policy);
            double? vkupnaPremija = ratingEngineService.totalPremium(policy);
            policy.Total_Premium = vkupnaPremija;
            System.Web.HttpContext.Current.Session.Add("SessionId", policy.Policy_Number);

            if (valid)
            {

                string ID = (entityDB.travel_policy.OrderByDescending(p => p.ID).Select(r => r.Policy_Number).FirstOrDefault() + 1);
                string ID_Company = entityDB.travel_policy.OrderByDescending(p => p.ID).Select(r => r.Policy_Number).FirstOrDefault();
                int tempID;
                if (String.IsNullOrEmpty(ID_Company))
                {
                    tempID = 0;
                }
                else {
                    string ID_trim = ID_Company.Substring(0, 4);
                    string ID_trim2 = ID_Company.Substring(5);
                    tempID = int.Parse(ID_trim2) + 1;
                }

                polisaEntity.Policy_Number = ID;
                polisaEntity.Policy_TypeID = policy.policy_type.ID;
                polisaEntity.CountryID = policy.CountryID;
                polisaEntity.Start_Date = policy.Start_Date;
                polisaEntity.End_Date = policy.End_Date;
                polisaEntity.Valid_Days = policy.Valid_Days;
                polisaEntity.Travel_Insurance_TypeID = policy.Travel_Insurance_TypeID;
                polisaEntity.Travel_NumberID = policy.Travel_NumberID;
                polisaEntity.Travel_Insurance_TypeID = policy.Travel_Insurance_TypeID;

                polisaEntity.Exchange_RateID = policy.Exchange_RateID;



                entityDB.travel_policy.Add(polisaEntity);
                var result = entityDB.SaveChanges();
            }

            return Json(new { success = true, responseText = "Success." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPolicy()
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            // long id = Convert.ToInt64 (System.Web.HttpContext.Current.Session["SessionId"]);
            int id = 1;

            PaymentModel pat = new PaymentModel();
            pat.Pat = entities.travel_policy.Where(x => x.ID.Equals(id)).FirstOrDefault();
            return new ViewAsPdf("Print", pat);
        }

        private async Task<List<SelectListItem>> GetTypeOfPolicy()
        {
            InsuredTravelingEntity db = new InsuredTravelingEntity();
            var policy = db.policy_type.Select(p => new SelectListItem
            {
                Text = p.type,
                Value = p.ID.ToString()
            });
            return await policy.ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTypeOfCountry()
        {
            InsuredTravelingEntity db = new InsuredTravelingEntity();
            var country = db.countries.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ID.ToString()
            });
            return await country.ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTypeOfFranchise()
        {
            InsuredTravelingEntity db = new InsuredTravelingEntity();
            var franchise = db.retaining_risk.Select(p => new SelectListItem
            {
                Text = p.Franchise,
                Value = p.ID.ToString()
            });
            return await franchise.ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTypeOfAdditionalCharges()
        {
            InsuredTravelingEntity db = new InsuredTravelingEntity();
            var additional_charge = db.additional_charge.Select(p => new SelectListItem
            {
                Text = p.Doplatok,
                Value = p.ID.ToString()
            });
            return await additional_charge.ToListAsync();
        }
    }
}