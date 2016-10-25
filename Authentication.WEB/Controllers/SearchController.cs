using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using InsuredTraveling.DI;
using AutoMapper;
using InsuredTraveling.ViewModels;

namespace InsuredTraveling.Controllers
{
    public class SearchController : Controller
    {

        private IPolicyService _ps;
        private IFirstNoticeOfLossService _fnls;
        private IUserService _us;
        private IPolicyTypeService _pts;
        private IInsuredsService _iss;
        private IBankAccountService _bas;
        private ICountryService _countryService;

        public SearchController(IPolicyService ps, 
                                IFirstNoticeOfLossService fnls, 
                                IUserService us, 
                                IPolicyTypeService pts, 
                                IInsuredsService iss, 
                                IBankAccountService bas,
                                ICountryService countryService)
        {
            _ps = ps;
            _fnls = fnls;
            _us = us;
            _pts = pts;
            _iss = iss;
            _bas = bas;
            _countryService = countryService;

        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var type_policies = GetTypeOfPolicy();
            await Task.WhenAll(type_policies);
            ViewBag.TypeOfPolicy = type_policies.Result;

            var countries = GetAllCountries();
            await Task.WhenAll(countries);
            ViewBag.Countries = countries.Result;

            var policies = GetAllPolicies();
            await Task.WhenAll(policies);
            ViewBag.Policies = policies.Result;
            return View();
        }


        [HttpGet]
        [Route("GetUsers")]
        public JObject GetUsers(string name, string lastname, string embg, string address, string email, string postal_code, string phone, string city, string passport)
        {
            var data = _iss.GetInsuredBySearchValues(name, lastname, embg, address, email, postal_code, phone, city, passport);
            var searchModel = data.Select(Mapper.Map<insured, SearchClientsViewModel>).ToList();

            var JSONObject = new JObject();
            var array = JArray.FromObject(searchModel.ToArray());
            JSONObject.Add("data", array);
            return JSONObject;
        }

        [HttpGet]
        [Route("GetPolicies")]
        public JObject GetPolicies(string name, 
                                   string embg, 
                                   string land, 
                                   string address, 
                                   int? TypePolicy, 
                                   int? Country, 
                                   string agency, 
                                   string startDate, 
                                   string endDate, 
                                   string dateI, 
                                   string dateS, 
                                   string operatorStartDate, 
                                   string operatorEndDate, 
                                   string operatorDateI, 
                                   string operatorDateS, 
                                   string PolicyNumber)
        {
            DateTime startDate1 = String.IsNullOrEmpty(startDate) ? new DateTime() : Convert.ToDateTime(startDate);
            DateTime endDate1 = String.IsNullOrEmpty(endDate) ? new DateTime() : Convert.ToDateTime(endDate);
            DateTime dateI1 = String.IsNullOrEmpty(dateI) ? new DateTime() : Convert.ToDateTime(dateI);
            DateTime dateS2 = String.IsNullOrEmpty(dateS) ? new DateTime() : Convert.ToDateTime(dateS);

            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var logUser = _us.GetUserIdByUsername(username);

            var data = _ps.GetPoliciesByCountryAndTypeAndPolicyNumber(TypePolicy, Country, logUser, PolicyNumber);

            if (!String.IsNullOrEmpty(startDate))
            {
                switch (operatorStartDate)
                {
                    case "<": data = data.Where(x => x.Start_Date < startDate1).ToList(); break;
                    case "=": data = data.Where(x => x.Start_Date == startDate1).ToList(); break;
                    case ">": data = data.Where(x => x.Start_Date > startDate1).ToList(); break;
                    default: break;
                }
            }
            if (!String.IsNullOrEmpty(endDate))
            {
                switch (operatorEndDate)
                {
                    case "<": data = data.Where(x => x.End_Date < endDate1).ToList(); break;
                    case "=": data = data.Where(x => x.End_Date == endDate1).ToList(); break;
                    case ">": data = data.Where(x => x.End_Date > endDate1).ToList(); break;
                    default: break;
                }
            }
            if (!String.IsNullOrEmpty(dateI))
            {
                switch (operatorDateI)
                {
                    case "<": data = data.Where(x => x.Date_Created < dateI1).ToList(); break;
                    case "=": data = data.Where(x => x.Date_Created == dateI1).ToList(); break;
                    case ">": data = data.Where(x => x.Date_Created > dateI1).ToList(); break;
                    default: break;
                }
            }
            if (!String.IsNullOrEmpty(dateS))
            {
                switch (operatorDateS)
                {
                    case "<": data = data.Where(x => x.Date_Cancellation < dateS2).ToList(); break;
                    case "=": data = data.Where(x => x.Date_Cancellation == dateS2).ToList(); break;
                    case ">": data = data.Where(x => x.Date_Cancellation > dateS2).ToList(); break;
                    default: break;
                }
            }

            var JSONObject = new JObject();
            var dataJSON = new JArray();

            var searchModel = data.Select(Mapper.Map<travel_policy, SearchPolicyViewModel>).ToList();
            var array = JArray.FromObject(searchModel.ToArray());
            JSONObject.Add("data", array);
            return JSONObject;
        }

        public JObject GetFNOLByPolicyNumber(string number)
        {
            int id = !String.IsNullOrEmpty(number) ? Convert.ToInt32(number) : 0;
            var fnol = _fnls.GetByPolicyId(id);
            var JSONObject = new JObject();
            var dataJSON = new JArray();

            var searchModel = fnol.Select(Mapper.Map<first_notice_of_loss, SearchFNOLViewModel>).ToList();
            var array = JArray.FromObject(searchModel.ToArray());
            JSONObject.Add("data", array);
            return JSONObject;
        }

        public JObject GetFNOL(string PolicyNumber, string holderName, string holderLastName, string clientName, string clientLastName, string insuredName, string insuredLastName, string totalPrice, string healthInsurance, string luggageInsurance, string DateAdded, string operatorDateAdded)
        {
            var fnol = _fnls.GetFNOLBySearchValues(PolicyNumber, holderName, holderLastName, clientName, clientLastName, insuredName, insuredLastName, totalPrice, healthInsurance, luggageInsurance);
            if (!String.IsNullOrEmpty(DateAdded))
            {
                DateTime dateAdded = Convert.ToDateTime(DateAdded);
                switch (operatorDateAdded)
                {
                    case "<": fnol = fnol.Where(x => x.additional_info.Datetime_accident < dateAdded).ToList(); break;
                    case "=": fnol = fnol.Where(x => x.additional_info.Datetime_accident == dateAdded).ToList(); break;
                    case ">": fnol = fnol.Where(x => x.additional_info.Datetime_accident > dateAdded).ToList(); break;
                    default: break;
                }
            }
            var JSONObject = new JObject();
            var dataJSON = new JArray();

            var searchModel = fnol.Select(Mapper.Map<first_notice_of_loss, SearchFNOLViewModel>).Distinct().ToList();
            var array = JArray.FromObject(searchModel.ToArray());
            JSONObject.Add("data", array);
            return JSONObject;
        }

        public JObject CreatePoliciesByInsuredId(string insuredId)
        {
            int id = !String.IsNullOrEmpty(insuredId) ? Convert.ToInt32(insuredId) : 0;
            var policies = _ps.GetPoliciesByInsuredId(id);
            var JSONObject = new JObject();
            var dataJSON = new JArray();

            var searchModel = policies.Select(Mapper.Map<travel_policy, SearchPolicyViewModel>).ToList();
            var array = JArray.FromObject(searchModel.ToArray());
            JSONObject.Add("data", array);
            return JSONObject;
        }

        public JObject CreatePoliciesByHolderId(string holderId)
        {
            int id = !String.IsNullOrEmpty(holderId) ? Convert.ToInt32(holderId) : 0;
            var policies = _ps.GetPoliciesByHolderId(id);
            var JSONObject = new JObject();
            var dataJSON = new JArray();

            var searchModel = policies.Select(Mapper.Map<travel_policy, SearchPolicyViewModel>).ToList();
            var array = JArray.FromObject(searchModel.ToArray());
            JSONObject.Add("data", array);
            return JSONObject;
        }

        [HttpPost]
        [Route("FNOLDetails")]
        public JObject FNOLDetails(int lossID)
        {
            var data = new JObject();


            var loss = _fnls.GetById(lossID);

            var user = _ps.GetPolicyHolderByPolicyID(loss.PolicyId);

            var Claimant = _iss.GetInsuredData(loss.ClaimantId);
            var ClaimantBankAccount = _bas.BankAccountInfoById(loss.Claimant_bank_accountID);

            var jarray = new JArray();
            var j = new JObject();

            //Data of insured
            j.Add("InsuredName", user.Name + " " + user.Lastname);
            j.Add("InsuredAddress", user.Address);
            j.Add("InsuredPhone", user.Phone_Number);
            j.Add("InsuredEmbg", user.SSN);
            j.Add("InsuredTransaction", " ");
            j.Add("InsuredDeponentBank", " ");

            //Dats of user of insurance
            j.Add("ClaimantPersonName", Claimant.Name+" "+Claimant.Lastname);
            j.Add("ClaimantPersonAddress", Claimant.Address+ " " +Claimant.City);
            j.Add("ClaimantPersonPhone", Claimant.Phone_Number);
            j.Add("ClaimantPersonEMBG", Claimant.SSN);
            j.Add("ClaimantPersonTransaction", ClaimantBankAccount.Account_Number);
            j.Add("ClaimantPersonDeponentBank", ClaimantBankAccount.bank.Name);
            j.Add("ClaimantInsuredRelation", loss.Relation_claimant_policy_holder);

            //Data of trip
            j.Add("LandTrip", loss.Destination);
            j.Add("TypeTransport", loss.Transport_means);
            j.Add("TripStartDate", ((DateTime)loss.Depart_Date_Time).Date + "-" + loss.Depart_Date_Time.TimeOfDay);
            j.Add("TripEndDate", ((DateTime)loss.Arrival_Date_Time).Date + "-" + loss.Arrival_Date_Time.TimeOfDay);

            //Data of costs
            j.Add("AdditionalDocumentsHanded", "");
            j.Add("AllCosts", loss.Total_cost);
            
            j.Add("Date", loss.additional_info.Datetime_accident);

            var HealthInsurance =  _fnls.GetHealthAdditionalInfoByLossId(loss.ID);
            if (HealthInsurance == null)
            {
                j.Add("HealthInsurance_Y_N", "Ne");
                jarray.Add(j);
                data.Add("data", jarray);
            }
            else
            {
                j.Add("HealthInsurance_Y_N", "Da");
                j.Add("Date_of_accsident", HealthInsurance.additional_info.Datetime_accident.Date + "-" + HealthInsurance.additional_info.Datetime_accident.TimeOfDay + "-" + HealthInsurance.additional_info.Accident_place);
                j.Add("Doctor_data", HealthInsurance.Doctor_info + " " + HealthInsurance.Datetime_doctor_visit.ToString());
                j.Add("Disease_description", HealthInsurance.Medical_case_description + " " + HealthInsurance.Previous_medical_history);

                j.Add("Documents_proof", "");
                j.Add("Additional_info", HealthInsurance.Responsible_institution);

                jarray.Add(j);
                data.Add("data", jarray);
            }
            
            var LuggageInsurance = _fnls.GetLuggageAdditionalInfoByLossId(loss.ID);
            if (LuggageInsurance == null)
            {
                j.Add("LuggageInsurance_Y_N", "Ne");
                jarray.Add(j);
                data.Add("data", jarray);
            }
            else
            {
                j.Add("LuggageInsurance_Y_N", "Da");
                j.Add("Date_of_loss", LuggageInsurance.additional_info.Datetime_accident.Date + "-" + LuggageInsurance.additional_info.Datetime_accident.TimeOfDay + "-" + LuggageInsurance.additional_info.Accident_place);
                j.Add("Place_desc_of_loss", LuggageInsurance.Place_description);
                j.Add("Detailed_description", LuggageInsurance.Detail_description);
                j.Add("Place_reported", LuggageInsurance.Report_place);
                j.Add("Desc_of_stolen_damaged_things", LuggageInsurance.Floaters + LuggageInsurance.Floaters_value.ToString());
               // j.Add("Documents_proof2", luggage_insurance.Documents_proof);
                j.Add("AirportArrivalTime", LuggageInsurance.additional_info.Datetime_accident.TimeOfDay);
                j.Add("LuggageDropTime", LuggageInsurance.Luggage_checking_Time);

                jarray.Add(j);
                data.Add("data", jarray);
            }

          

            if (loss.health_insurance != null)
            {
                var health_insurance = loss.health_insurance;

                //Data of health insurance
                j.Add("Date_of_accsident", health_insurance.Date_of_accsident + "-" + health_insurance.Time_of_accsident + "-" + health_insurance.Place_of_accsident);
                j.Add("Doctor_data", health_insurance.Doctor_data);
                j.Add("Disease_description", health_insurance.Disease_description);
                j.Add("Documents_proof", health_insurance.Documents_proof);
                j.Add("Additional_info", health_insurance.Additional_info);

                jarray.Add(j);
                data.Add("data", jarray);

            }
            else if (loss.luggage_insurance != null)
            {
                var luggage_insurance = loss.luggage_insurance;

                //Data of luggage insurance
                j.Add("Date_of_loss", luggage_insurance.Date_of_loss);
                j.Add("Place_desc_of_loss", luggage_insurance.Place_desc_of_loss);
                j.Add("Detailed_description", luggage_insurance.Detailed_description);
                j.Add("Place_reported", luggage_insurance.Place_reported);
                j.Add("Desc_of_stolen_damaged_things", luggage_insurance.Desc_of_stolen_damaged_things);
                j.Add("Documents_proof2", luggage_insurance.Documents_proof);
                j.Add("AirportArrivalTime", luggage_insurance.AirportArrivalTime);
                j.Add("LuggageDropTime", luggage_insurance.LuggageDropTime);

                jarray.Add(j);
                data.Add("data", jarray);
            }


            return data;
        }

        private async Task<List<SelectListItem>> GetTypeOfPolicy()
        {
            var policy = _pts.GetAll();
            return await policy.ToListAsync();
        }

        private async Task<List<SelectListItem>> GetAllCountries()
        {
            var countries = _countryService.GetAll();
            return await countries.ToListAsync();
        }

        public async Task<List<SelectListItem>> GetAllPolicies()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var logUser = _us.GetUserIdByUsername(username);

            var policies = _ps.GetPoliciesByUserId(logUser);
            return await policies.ToListAsync();
        }
    }
}