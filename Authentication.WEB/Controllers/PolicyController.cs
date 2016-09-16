using Authentication.WEB.Models;
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

namespace Authentication.WEB.Controllers
{
    public class PolicyController : Controller
    {
        // GET: Policy
        [HttpGet]
        public ActionResult Index()
        {
            PolicyInfoList info = new PolicyInfoList();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            info.countries = entities.countries;
            info.franchises = entities.retaining_risk;
            info.policies = entities.policy_type;
            info.doplatokList = entities.additional_charge;

            return View(info);
        }

        public async System.Threading.Tasks.Task<ActionResult> CreatePolicy(travel_policy policy)
        {
            ValidationService validationService = new ValidationService();
            RatingEngineService ratingEngineService = new RatingEngineService();

            InsuredTravelingEntity entityDB = new InsuredTravelingEntity();
            travel_policy polisaEntity = new travel_policy();

            Uri uri = new Uri("http://localhost:19655/api/premium/calculate");
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
            int id = 2;

            PaymentModel pat = new PaymentModel();
            pat.Pat = entities.travel_policy.Where(x => x.Policy_Number.Equals(id)).FirstOrDefault();
            return new ViewAsPdf("Print", pat);
        }
    }
}