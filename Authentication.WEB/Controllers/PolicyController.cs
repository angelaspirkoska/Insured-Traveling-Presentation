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
using InsuredTraveling.DI;

namespace Authentication.WEB.Controllers
{
    public class PolicyController : Controller
    {


        private IPolicyService _ps;
        private IPolicyTypeService _pts;
        private ICountryService _cs;
        private IFranchiseService _fs;
        private IAdditionalChargesService _acs;

        public PolicyController(IPolicyService ps, IPolicyTypeService pts, ICountryService cs, IFranchiseService fs,
            IAdditionalChargesService acs)
        {
            _ps = ps;
            _pts = pts;
            _cs = cs;
            _fs = fs;
            _acs = acs;
        }

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

                string PolicyNumber = _ps.CreatePolicyNumber();
                string ID_Company = _ps.GetCompanyID(policy.Policy_Number);
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

                polisaEntity.Policy_Number = PolicyNumber;
                polisaEntity.Policy_TypeID = policy.policy_type.ID;
                polisaEntity.CountryID = policy.CountryID;
                polisaEntity.Start_Date = policy.Start_Date;
                polisaEntity.End_Date = policy.End_Date;
                polisaEntity.Valid_Days = policy.Valid_Days;
                polisaEntity.Travel_Insurance_TypeID = policy.Travel_Insurance_TypeID;
                polisaEntity.Travel_NumberID = policy.Travel_NumberID;
                polisaEntity.Travel_Insurance_TypeID = policy.Travel_Insurance_TypeID;

                polisaEntity.Exchange_RateID = policy.Exchange_RateID;



                var result = _ps.AddPolicy(polisaEntity);
            }

            return Json(new { success = true, responseText = "Success." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPolicy()
        {
            int id = 2;

            PaymentModel pat = new PaymentModel();
            pat.Pat = _ps.GetPolicyById(id);
            return new ViewAsPdf("Print", pat);
        }

        private async Task<List<SelectListItem>> GetTypeOfPolicy()
        {           
            return await _pts.GetAll().ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTypeOfCountry()
        {           
            return await _cs.GetAll().ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTypeOfFranchise()
        {
      
            return await _fs.GetAll().ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTypeOfAdditionalCharges()
        {     
            return await _acs.GetAll().ToListAsync();
        }
    }
}