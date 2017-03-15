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
using InsuredTraveling.Helpers;
using AutoMapper;

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
        private RoleAuthorize _roleAuthorize;

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
            _roleAuthorize = new RoleAuthorize();
        }

        // GET: Policy
        [HttpGet]
        [SessionExpire]
        public async Task<ActionResult> Index()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                Response.Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");

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

        public async System.Threading.Tasks.Task<ActionResult> CreatePolicy(Policy policy)
        {
            ValidationService validationService = new ValidationService();
            RatingEngineService ratingEngineService = new RatingEngineService();      
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
                polisaEntity.Exchange_RateID = (policy.Exchange_RateID.HasValue)? policy.Exchange_RateID.Value : 1;
                var result = _ps.AddPolicy(polisaEntity);
            }

            return Json(new { success = true, responseText = "Success." }, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<ActionResult> CreateQuote(Policy policy)
        {
            var result = SavePolicyHelper.SavePolicy(policy, _ps, _us, _iss, _pis, _acs);
            var quoteNumber= _ps.GetPolicyById(result).Policy_Number;
            if (result != 0)
            {
                return Json(new { success = true, responseText = quoteNumber, numberQuote = quoteNumber }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, responseText = "Fail." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RedirectPrintOffer(Policy policy)
        {
            policy.isMobile = false;
            var PolicyId = SavePolicyHelper.SavePolicy(policy, _ps, _us, _iss, _pis, _acs);

            var pNumber = _ps.GetPolicyNumberByPolicyId(PolicyId);
            return RedirectToAction("PolicyDetails", new { policyNumber = pNumber });
        }

        public ActionResult PrintPolicy(string id)
        {
            PaymentModel pat = new PaymentModel();
            pat.Pat = _ps.GetPolicyIdByPolicyNumber(id);
            
            pat.mainInsured = _pis.GetAllInsuredByPolicyIdAndInsuredCreatedBy(pat.Pat.ID, pat.Pat.Created_By).First();
            var policy = _ps.GetPolicyById(pat.Pat.ID);
            var additionalCharges = _acs.GetAdditionalChargesByPolicyId(pat.Pat.ID);

            pat.additionalCharge1 = InsuredTraveling.Resource.WithNoAddOn;
            pat.additionalCharge2 = InsuredTraveling.Resource.WithNoAddOn;
            if (additionalCharges.Count >= 1 && additionalCharges[0] != null)
            {
                pat.additionalCharge1 = _acs.GetAdditionalChargeName(additionalCharges[0].ID);
            }
            if (additionalCharges.Count >= 2 && additionalCharges[1] != null)
            {
                pat.additionalCharge2 = _acs.GetAdditionalChargeName(additionalCharges[1].ID);
            }
            pat.Pat = _ps.GetPolicyIdByPolicyNumber(id);
            return new ViewAsPdf("Print", pat);
        }

        public ActionResult PolicyDetails(string policyNumber)
        {
            PaymentModel pat = new PaymentModel();
            pat.Pat = _ps.GetPolicyIdByPolicyNumber(policyNumber);
            pat.mainInsured = _pis.GetAllInsuredByPolicyId(pat.Pat.ID).FirstOrDefault();

            var policy = _ps.GetPolicyById(pat.Pat.ID);
            var additionalCharges = _acs.GetAdditionalChargesByPolicyId(pat.Pat.ID);

            pat.additionalCharge1 = InsuredTraveling.Resource.WithNoAddOn;
            pat.additionalCharge2 = InsuredTraveling.Resource.WithNoAddOn;

            if (additionalCharges.Count >= 1 && additionalCharges[0] != null)
            {
                pat.additionalCharge1 = _acs.GetAdditionalChargeName(additionalCharges[0].ID);
            }
            if (additionalCharges.Count >= 2 && additionalCharges[1] != null)
            {
                pat.additionalCharge2 = _acs.GetAdditionalChargeName(additionalCharges[1].ID);
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
        [SessionExpire]
        public async Task<JObject> GetExistentLoggedInUserData()
        {
            var Result = new JObject();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string username = System.Web.HttpContext.Current.User.Identity.Name;
                var loggedUserSsn = _us.GetUserSsnByUsername(username);
                //var loggedUserData = _iss.GetInsuredDataBySsn(loggedUserSsn);
                var loggedUserData = _iss.GetInsuredBySsnAndCreatedBy(loggedUserSsn, _us.GetUserIdByUsername(username));
                JObject insuredData = new JObject();
                if (loggedUserData == null)
                {
                    var loggedUser = _us.GetUserDataByUsername(username);
                    insuredData.Add("FirstName", loggedUser.FirstName);
                    insuredData.Add("Name", loggedUser.LastName);
                    insuredData.Add("Address", loggedUser.Address);
                    insuredData.Add("City", loggedUser.City);
                    insuredData.Add("PostalCode", loggedUser.PostalCode);
                    insuredData.Add("Ssn", loggedUser.EMBG);

                    insuredData.Add("DateBirth", loggedUser.DateOfBirth.Value + String.Format("-{0:00}-{0:00}",  loggedUser.DateOfBirth.Value, loggedUser.DateOfBirth.Value));
                    insuredData.Add("PassportID", loggedUser.PassportNumber);
                    insuredData.Add("Email", loggedUser.Email);
                    insuredData.Add("PhoneNumber", loggedUser.MobilePhoneNumber);

                    Result.Add("InsuredData", insuredData);

                    Result.Add("response", "Not registered insured");
                    return Result;
                }
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

        public JObject GetExistentInsuredUserData(string ssn)
        {
            var Result = new JObject();
            insured InsuredUser = null;
            if (_roleAuthorize.IsUser("Admin"))
            {
                InsuredUser = _iss.GetInsuredBySsn(ssn);

            }else if(_roleAuthorize.IsUser("Broker"))
            {
                InsuredUser = _iss.GetInsuredBySsnAndCreatedBy(ssn, _us.GetUserIdByUsername(System.Web.HttpContext.Current.User.Identity.Name));
            }
            JObject insuredData = new JObject();

            if(InsuredUser != null)
            {
                insuredData.Add("FirstName", InsuredUser.Name);
                insuredData.Add("Name", InsuredUser.Lastname);
                insuredData.Add("Address", InsuredUser.Address);
                insuredData.Add("City", InsuredUser.City);
                insuredData.Add("PostalCode", InsuredUser.Postal_Code);
                insuredData.Add("Ssn", InsuredUser.SSN);

                insuredData.Add("DateBirth", InsuredUser.DateBirth.Year + String.Format("-{0:00}-{0:00}", +InsuredUser.DateBirth.Month, InsuredUser.DateBirth.Day));
                insuredData.Add("PassportID", InsuredUser.Passport_Number_IdNumber);
                insuredData.Add("Email", InsuredUser.Email);
                insuredData.Add("PhoneNumber", InsuredUser.Phone_Number);

                Result.Add("InsuredData", insuredData);
                return Result;
            }else
            {
                Result.Add("response", "User with that SSN not found");
                return Result;
            }
        }
    }
}