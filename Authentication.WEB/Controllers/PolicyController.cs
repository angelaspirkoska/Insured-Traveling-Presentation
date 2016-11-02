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
using InsuredTraveling.Filters;

namespace Authentication.WEB.Controllers
{
    [SessionExpire]
    public class PolicyController : Controller
    {


        private IPolicyService _ps;
        private IPolicyTypeService _pts;
        private ICountryService _cs;
        private IFranchiseService _fs;
        private IAdditionalChargesService _acs;
        private IUserService _us;
        private IPolicyInsuredService _pis;
        private IInsuredsService _iss;

        public PolicyController(IPolicyService ps, IPolicyTypeService pts, ICountryService cs, IFranchiseService fs,
            IAdditionalChargesService acs, IUserService us, IInsuredsService iss, IPolicyInsuredService pis)
        {
            _ps = ps;
            _pts = pts;
            _cs = cs;
            _fs = fs;
            _acs = acs;
            _us = us;
            _iss = iss;
            _pis = pis;
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
            ViewBag.Date = DateTime.Now.Year + String.Format("/{0:00}", DateTime.Now.Month) +String.Format("/{0:00}",DateTime.Now.Day);
            DateTime inTenDays = DateTime.Now.AddDays(9);
             ViewBag.DateAfterTenDays = inTenDays.Year + String.Format("/{0:00}", inTenDays.Month) + String.Format("/{0:00}", inTenDays.Day);
            //ViewBag.Date = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day;

            return View();
        }

        [HttpPost]
        public ActionResult Index(Policy p)
        {
            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> CreatePolicy(Policy policy)
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
            HttpContent content = new ObjectContent<Policy>(policy, jsonFormatter);

            HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
            string responseBody = await responseMessage.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(responseBody);
            int premium = data.PremiumAmount;

            bool valid = validationService.masterValidate(polisaEntity);
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
                polisaEntity.Policy_TypeID = policy.Policy_TypeID;
                polisaEntity.CountryID = policy.CountryID;
                polisaEntity.Start_Date = policy.Start_Date;
                polisaEntity.End_Date = policy.End_Date;
                polisaEntity.Valid_Days = policy.Valid_Days;
                polisaEntity.Travel_Insurance_TypeID = policy.Travel_Insurance_TypeID;
                polisaEntity.Travel_NumberID = policy.Travel_NumberID;
                polisaEntity.Travel_Insurance_TypeID = policy.Travel_Insurance_TypeID;
                polisaEntity.Exchange_RateID = (policy.Exchange_RateID.HasValue)? policy.Exchange_RateID.Value : 1;
                var result = _ps.AddPolicy(polisaEntity);
            }

            return Json(new { success = true, responseText = "Success." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPolicy(string id)
        {
            PaymentModel pat = new PaymentModel();
            pat.Pat = _ps.GetPolicyIdByPolicyNumber(id);
            pat.mainInsured = _pis.GetAllInsuredByPolicyId(pat.Pat.ID).First();
            //  model.additionalCharge1 = 
            var policy = _ps.GetPolicyById(pat.Pat.ID);
            var additionalCharges = _acs.GetAdditionalChargesByPolicyId(pat.Pat.ID);

            pat.additionalCharge1 = "Без доплаток";
            pat.additionalCharge2 = "Без доплаток";
            if (additionalCharges.Count >= 1 && additionalCharges[0] != null)
            {
                pat.additionalCharge1 = additionalCharges[0].Doplatok;
            }
            if (additionalCharges.Count >= 2 && additionalCharges[1] != null)
            {
                pat.additionalCharge2 = additionalCharges[1].Doplatok;
            }
            pat.Pat = _ps.GetPolicyIdByPolicyNumber(id);
            return new ViewAsPdf("Print", pat);
        }

        public ActionResult PolicyDetails(string id)
        {
            PaymentModel pat = new PaymentModel();
            pat.Pat = _ps.GetPolicyIdByPolicyNumber(id);
            pat.mainInsured = _pis.GetAllInsuredByPolicyId(pat.Pat.ID).First();
            //  model.additionalCharge1 = 
            var policy = _ps.GetPolicyById(pat.Pat.ID);
            var additionalCharges = _acs.GetAdditionalChargesByPolicyId(pat.Pat.ID);

            pat.additionalCharge1 = "Без доплаток";
            pat.additionalCharge2 = "Без доплаток";
            if (additionalCharges.Count >= 1 && additionalCharges[0] != null)
            {
                pat.additionalCharge1 = additionalCharges[0].Doplatok;
            }
            if (additionalCharges.Count >= 2 && additionalCharges[1] != null)
            {
                pat.additionalCharge2 = additionalCharges[1].Doplatok;
            }
            return View("Print", pat);
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


        [HttpGet]
        public async Task<JObject> GetExistentLoggedInUserData()
        {
            var Result = new JObject();
           


            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {

                string username = System.Web.HttpContext.Current.User.Identity.Name;

                //var loggedUserSsn = _us.GetUserSsnByUsername(username);
               
                var loggedUserSsn = _us.GetUserSsnByUsername(username);
                var loggedUserData = _iss.GetInsuredDataBySsn(loggedUserSsn);

                if(loggedUserData == null)
                {
                    Result.Add("response", "Not registered insured");
                    return Result;
                }

                JObject insuredData = new JObject();

                insuredData.Add("FirstName", loggedUserData.Name);
                insuredData.Add("Name", loggedUserData.Lastname);
                insuredData.Add("Address", loggedUserData.Address);
                insuredData.Add("City", loggedUserData.City);
                insuredData.Add("PostalCode", loggedUserData.Postal_Code);
                insuredData.Add("Ssn", loggedUserData.SSN);
               
                insuredData.Add("DateBirth", loggedUserData.DateBirth.Year + String.Format("-{0:00}-{0:00}", +loggedUserData.DateBirth.Month, loggedUserData.DateBirth.Day));
                insuredData.Add("PassportID", loggedUserData.Passport_Number_IdNumber);
                insuredData.Add("Email", loggedUserData.Email);
                insuredData.Add("PhoneNumber", loggedUserData.Phone_Number);


                Result.Add("InsuredData",insuredData);

            }
            else
            {
                Result.Add("response", "Not authenticated user");
            }

            return Result;           


        }




    }
}