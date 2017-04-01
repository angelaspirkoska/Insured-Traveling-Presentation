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
using InsuredTraveling.Filters;
using System.Configuration;
using InsuredTraveling.App_Start;
using InsuredTraveling.Models;
using Microsoft.Office.Interop.Word;

namespace InsuredTraveling.Controllers
{
    [SessionExpire]
    public class SearchController : Controller
    {

        private IPolicyService _ps;
        private IFirstNoticeOfLossService _fnls;
        private IUserService _us;
        private IPolicyTypeService _pts;
        private IInsuredsService _iss;
        private IBankAccountService _bas;
        private ICountryService _countryService;
        private IChatService _ics;
        private IPolicySearchService _policySearchService;
        private RoleAuthorize _roleAuthorize;
        private IFirstNoticeOfLossArchiveService _firstNoticeLossArchiveService;
        private IRolesService _rs;

        public SearchController(IPolicyService ps, 
                                IFirstNoticeOfLossService fnls, 
                                IUserService us, 
                                IPolicyTypeService pts, 
                                IInsuredsService iss, 
                                IBankAccountService bas,
                                ICountryService countryService,
                                IChatService ics,
                                IPolicySearchService policySearchService,
                                IFirstNoticeOfLossArchiveService firstNoticeLossArchiveService,
                                IRolesService rs)
        {
            _ps = ps;
            _fnls = fnls;
            _us = us;
            _pts = pts;
            _iss = iss;
            _bas = bas;
            _countryService = countryService;
            _ics = ics;
            _rs = rs;
            _policySearchService = policySearchService;
            _roleAuthorize = new RoleAuthorize();
            _firstNoticeLossArchiveService = firstNoticeLossArchiveService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                Response.Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");
            var type_policies = GetTypeOfPolicy();
            ViewBag.TypeOfPolicy = type_policies.Result;

            var countries = GetAllCountries();
            ViewBag.Countries = countries.Result;

            var policies = GetAllPolicies();
            ViewBag.Policies = policies.Result;
            var roles = _rs.GetAll().ToList();
            foreach(var role in roles)
            {
                if (role.Selected)
                {
                    role.Selected = false;
                }
            }
            ViewBag.Roles = roles;
            return View();
        }

        [HttpGet]
        [Route("GetUsers")]
        public JObject GetUsers(string name, string lastname, string embg, string address, string email, string postal_code, string phone, string city, string passport)
        {
            List<SearchClientsViewModel> searchModel = new List<SearchClientsViewModel>();
            if (_roleAuthorize.IsUser("broker"))
            {
                var data = _iss.GetInsuredBySearchValues(name, lastname, embg, address, email, postal_code, phone, city, passport, _us.GetUserIdByUsername(System.Web.HttpContext.Current.User.Identity.Name));
                searchModel = data.Select(Mapper.Map<insured, SearchClientsViewModel>).ToList();
            }
            else
            {
                var data = _iss.GetInsuredBySearchValues(name, lastname, embg, address, email, postal_code, phone, city, passport, "");
                searchModel = data.Select(Mapper.Map<insured, SearchClientsViewModel>).ToList();
            }

            var JSONObject = new JObject();
            var array = JArray.FromObject(searchModel.ToArray());
            JSONObject.Add("data", array);
            return JSONObject;
        }

        [HttpGet]
        
        [Route("GetChats")]
        public JObject GetChats(string username, string chatId, bool all, bool active, bool discarded, bool noticed)
        {
            JObject result = new JObject();
            List<chat_requests> chats = new List<chat_requests>();
            JArray chatJArray = new JArray();
            bool isAdmin = false;

            if (_roleAuthorize.IsUser("Admin") || _roleAuthorize.IsUser("Claims adjuster"))
            {
                isAdmin = true;
                chats = _ics.GetChatsAdmin(System.Web.HttpContext.Current.User.Identity.Name);
                if (!String.IsNullOrEmpty(username))
                {
                    chats = chats.Where(x => x.Requested_by.Equals(username)).ToList();
                }
            }
            else if (_roleAuthorize.IsUser("End user"))
            {
                chats = _ics.GetChatsEndUser(System.Web.HttpContext.Current.User.Identity.Name);
                if (!String.IsNullOrEmpty(username))
                {
                    chats = chats.Where(x => x.Accepted_by.Equals(username)).ToList();
                }
            }

            if(!String.IsNullOrEmpty(chatId))
            {
                chats = chats.Where(x => x.ID == Int32.Parse(chatId)).ToList();
            }

            if(!all)
            {
                if (active)
                {
                    chats = chats.Where(x => x.fnol_created == false && x.discarded == false && x.Accepted == true).ToList();
                }
                else if (discarded)
                {
                    chats = chats.Where(x => x.fnol_created == false && x.discarded == true && x.Accepted == true).ToList();
                }
                else if (noticed)
                {
                    chats = chats.Where(x => x.fnol_created == true && x.discarded == false && x.Accepted == true).ToList();
                }
            }
           
            foreach(chat_requests chat in chats)
            {
                JObject temp = new JObject();
                temp.Add("chatId", chat.ID.ToString());

                if (isAdmin)
                {
                    temp.Add("chatWith", chat.Requested_by);
                }
                else
                {
                    temp.Add("chatWith", chat.Accepted_by);
                }
                temp.Add("noticed", chat.fnol_created);
                temp.Add("discarded", chat.discarded);
                temp.Add("isAdmin", isAdmin);
                chatJArray.Add(temp);
            }

            result.Add("data",chatJArray);
           
            return result;
        }


        [HttpGet]
        [Route("IsAdmin")]
        public JObject isAdmin()
        {
            RoleAuthorize r = new RoleAuthorize();
            JObject response = new JObject();
            if(r.IsUser("Admin"))
            {
                response.Add("isAdmin", true);
            }
            else
            {
                response.Add("isAdmin", false);
            }
            return response;
        }

        [HttpGet]
        [Route("IsBroker")]
        public JObject isBroker()
        {
            RoleAuthorize r = new RoleAuthorize();
            JObject response = new JObject();
            if (r.IsUser("Broker"))
            {
                response.Add("isBroker", true);
            }
            else
            {
                response.Add("isBroker", false);
            }
            return response;
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

            List<travel_policy> data = new List<travel_policy>();

            if (_roleAuthorize.IsUser("Admin") || _roleAuthorize.IsUser("Claims adjuster"))
            {
                data = _ps.GetPoliciesByCountryAndTypeAndPolicyNumber(TypePolicy, Country, PolicyNumber);
            }

            else if(_roleAuthorize.IsUser("End user") || _roleAuthorize.IsUser("Broker"))
            {
                data = _ps.GetPoliciesByCountryAndTypeAndPolicyNumber(TypePolicy, Country, logUser, PolicyNumber);
            }
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

            var languageId = SiteLanguages.CurrentLanguageId();
            var searchModel = data.Select(Mapper.Map<travel_policy, SearchPolicyViewModel>).ToList();
            searchModel = _policySearchService.GetCountriesName(searchModel, languageId);

            var array = JArray.FromObject(searchModel.ToArray());
            JSONObject.Add("data", array);
            return JSONObject;
        }
        [HttpGet]
        [Route("GetQuotes")]
        public JObject GetQuotes(
                                  int? TypePolicy,
                                  int? Country,
                                  string startDate,
                                  string endDate,
                                  string operatorStartDate,
                                  string operatorEndDate,
                                  string PolicyNumber)
        {

            DateTime startDate1 = String.IsNullOrEmpty(startDate) ? new DateTime() : Convert.ToDateTime(startDate);
            DateTime endDate1 = String.IsNullOrEmpty(endDate) ? new DateTime() : Convert.ToDateTime(endDate);

            string username = System.Web.HttpContext.Current.User.Identity.Name;
            var logUser = _us.GetUserIdByUsername(username);

            List<travel_policy> data = new List<travel_policy>();

            if (_roleAuthorize.IsUser("Admin") || _roleAuthorize.IsUser("Claims adjuster"))
            {
                data = _ps.GetQuotesByCountryAndTypeAndPolicyNumber(TypePolicy, Country, PolicyNumber);
            }

            else if (_roleAuthorize.IsUser("End user") || _roleAuthorize.IsUser("Broker"))
            {
                data = _ps.GetQuotesByCountryAndTypeAndPolicyNumber(TypePolicy, Country, logUser, PolicyNumber);
            }
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

            var JSONObject = new JObject();
            var dataJSON = new JArray();

            var languageId = SiteLanguages.CurrentLanguageId();
            var searchModel = data.Select(Mapper.Map<travel_policy, SearchPolicyViewModel>).ToList();
            searchModel = _policySearchService.GetCountriesName(searchModel, languageId);
          
            var array = JArray.FromObject(searchModel.ToArray());
            JSONObject.Add("data", array);
            return JSONObject;
        }

        [HttpGet]
        [Route("GetRegisteredUsers")]
        public JObject GetRegisteredUsers(string registerDate, string operatorRegisterDate, string roleName, string status)
        {
            List<aspnetuser> data = new List<aspnetuser>();
            DateTime registerDateValue = String.IsNullOrEmpty(registerDate) ? new DateTime() : Convert.ToDateTime(registerDate);

            data = _us.GetUsersByRoleName(roleName);

            if (!string.IsNullOrEmpty(registerDate))
            {
                switch (operatorRegisterDate)
                {
                    case "<": data = data.Where(x => x.CreatedOn < registerDateValue).ToList(); break;
                    case "=": data = data.Where(x => x.CreatedOn == registerDateValue).ToList(); break;
                    case ">": data = data.Where(x => x.CreatedOn > registerDateValue).ToList(); break;
                    default: break;
                }
            }

            if (!string.IsNullOrEmpty(status))
            {
                switch (status)
                {
                    case "0": data = data.Where(x => x.Active == 1).ToList(); break;
                    case "1": data = data.Where(x => x.Active == 0).ToList(); break;
                    case "2":
                        break;
                    default: break;
                }
            }

            var jsonObject = new JObject();
            var searchModel = data.Select(Mapper.Map<aspnetuser, SearchRegisteredUser>).ToList();
            var array = JArray.FromObject(searchModel.ToArray());
            jsonObject.Add("data", array);
            return jsonObject;
        }

        [HttpGet]
        [Route("ChangeUserStatus")]
        public JObject ChangeUserStatus(string username)
        {
            bool result = _us.ChangeStatus(username);
            JObject resultJson = new JObject();
            if (result)
            {
                resultJson.Add("message", "OK");
            }
            else
            {
                resultJson.Add("message", "NOK");
            }

            return resultJson;
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

        public JObject GetFNOL(string PolicyNumber, 
                               string FNOLNumber,
                               string holderName, 
                               string holderLastName, 
                               string clientName, 
                               string clientLastName, 
                               string insuredName, 
                               string insuredLastName, 
                               string totalPrice, 
                               string healthInsurance, 
                               string luggageInsurance, 
                               string DateAdded, 
                               string operatorDateAdded, 
                               string operatorTotalCost)
        {

            List<first_notice_of_loss> fnol = new List<first_notice_of_loss>();

            if (_roleAuthorize.IsUser("Admin") || _roleAuthorize.IsUser("Claims adjuster"))
            {
                fnol = _fnls.GetFNOLBySearchValues(PolicyNumber, FNOLNumber, holderName, holderLastName, clientName, clientLastName, insuredName, insuredLastName, totalPrice, healthInsurance, luggageInsurance);
            }
            else if (_roleAuthorize.IsUser("End user"))
            {
                fnol = _fnls.GetFNOLBySearchValues(System.Web.HttpContext.Current.User.Identity.Name,PolicyNumber, FNOLNumber, holderName, holderLastName, clientName, clientLastName, insuredName, insuredLastName, totalPrice, healthInsurance, luggageInsurance);
            }
            else if (_roleAuthorize.IsUser("Broker"))
            {
                fnol = _fnls.GetFNOLForBrokerBySearchValues(System.Web.HttpContext.Current.User.Identity.Name, PolicyNumber, FNOLNumber, holderName, holderLastName, clientName, clientLastName, insuredName, insuredLastName, totalPrice, healthInsurance, luggageInsurance);
            }

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
            if (!String.IsNullOrEmpty(totalPrice))
            {
                float price = 0;
                float.TryParse(totalPrice, out price);
                switch (operatorTotalCost)
                {
                    case "<": fnol = fnol.Where(x => x.Total_cost < price).ToList(); break;
                    case "=": fnol = fnol.Where(x => x.Total_cost == price).ToList(); break;
                    case ">": fnol = fnol.Where(x => x.Total_cost > price).ToList(); break;
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

        public JObject GetArchivedFNOL(string fnolID)
        {
            int id = !String.IsNullOrEmpty(fnolID) ? Convert.ToInt32(fnolID) : 0;
            var fnolArchive = _firstNoticeLossArchiveService.GetFNOLArchiveByFNOLId(id);
            var JSONObject = new JObject();

            var searchModel = fnolArchive.Select(Mapper.Map<first_notice_of_loss_archive, SearchFNOLViewModel>).ToList();
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

            if (policies != null)
            {
                var languageId = SiteLanguages.CurrentLanguageId();
                var searchModel = policies.Select(Mapper.Map<travel_policy, SearchPolicyViewModel>).ToList();
                searchModel = _policySearchService.GetCountriesName(searchModel, languageId);

                var array = JArray.FromObject(searchModel.ToArray());
                JSONObject.Add("data", array);
                return JSONObject;
            }
            JSONObject.Add("data", dataJSON);
            return JSONObject;
        }

        public JObject CreatePoliciesByHolderId(string holderId)
        {
            int id = !String.IsNullOrEmpty(holderId) ? Convert.ToInt32(holderId) : 0;
            var policies = _ps.GetPoliciesByHolderId(id);
            var JSONObject = new JObject();
            var dataJSON = new JArray();

            if(policies != null)
            {
                var languageId = SiteLanguages.CurrentLanguageId();
                var searchModel = policies.Select(Mapper.Map<travel_policy, SearchPolicyViewModel>).ToList();
                searchModel = _policySearchService.GetCountriesName(searchModel, languageId);

                var array = JArray.FromObject(searchModel.ToArray());
                JSONObject.Add("data", array);
                return JSONObject;
            }
            JSONObject.Add("data", dataJSON);
            return JSONObject;

        }

        [HttpPost]
        public JsonResult ShowPolicies(string Prefix)
        {
            if (_roleAuthorize.IsUser("End user"))
            {
                var policies = _us.GetPoliciesByUsernameToList(System.Web.HttpContext.Current.User.Identity.Name, Prefix);
                var policiesAutoComplete = policies.Select(Mapper.Map<travel_policy, PolicyAutoCompleteViewModel>).ToList();
                return Json(policiesAutoComplete, JsonRequestBehavior.AllowGet);

            }
            else if (_roleAuthorize.IsUser("Admin"))
            {
                var policies = _ps.GetAllPoliciesByPolicyNumber(Prefix);
                var policiesAutoComplete = policies.Select(Mapper.Map<travel_policy, PolicyAutoCompleteViewModel>).ToList();
                return Json(policiesAutoComplete, JsonRequestBehavior.AllowGet);
            }

            return null;
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

        [HttpGet]
        [Route("EditUser")]
        public ActionResult EditUser(string id)
        {
            var genderList = Gender();
            var roles = _rs.GetAll().ToList();
            aspnetuser userEdit = _us.GetUserById(id);

            User userEditModel = Mapper.Map<aspnetuser, User>(userEdit);

            foreach (var role in  roles)
            {
                if (role.Selected)
                    role.Selected = false;
                if (role.Text == userEditModel.Role)
                    role.Selected = true;
            }

            foreach (var gender in genderList)
            {
                if (gender.Text == userEditModel.Gender)
                    gender.Selected = true;
            }

            ViewBag.Roles = roles;
            ViewBag.Gender = genderList;

            
            return View(userEditModel);
        }

        [HttpPost]
        [Route("EditUser")]
        public ActionResult EditUser(User editedUser)
        {
            var genderList = Gender();
            var roles = _rs.GetAll().ToList();

            foreach (var role in roles)
            {
                if (role.Selected)
                    role.Selected = false;
                if (role.Text == editedUser.Role)
                    role.Selected = true;
            }

            foreach (var gender in genderList)
            {
                if (gender.Text == editedUser.Gender)
                    gender.Selected = true;
            }

            ViewBag.Roles = roles;
            ViewBag.Gender = genderList;

            int result = _us.UpdateUser(editedUser);

            if (result == -1)
            {
                ViewBag.Message = "NOK";
                return View(editedUser);
            }
            else
            {
                ViewBag.Message = "OK";
                return View(editedUser);
            }
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

        public List<SearchPolicyViewModel> DefinePolicyCountries(List<SearchPolicyViewModel> policies)
        {
          
            return policies;
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