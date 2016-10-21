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

            return View();
        }


        [HttpGet]
        [Route("GetUsers")]
        public JObject GetUsers(string name, string embg, string address, string email, string postal_code, string phone, string city, string passport)
        {
            name = name.ToLower();
            embg = embg.ToLower();
            address = address.ToLower();
            city = city.ToLower();
            phone = phone.ToLower();
            email = email.ToLower();
            passport = passport.ToLower();

            InsuredTravelingEntity db = new InsuredTravelingEntity();
            var data = db.aspnetusers.Where(x => x.EMBG.Contains(embg) &&
                                             x.FirstName.ToLower().Contains(name) &&
                                             x.Address.ToLower().Contains(address) &&
                                             x.Email.ToLower().Contains(email) &&
                                             (x.PostalCode.Contains(postal_code)) &&
                                             x.PhoneNumber.Contains(phone) &&
                                             x.EMBG.Contains(embg) &&
                                             x.City.ToLower().Contains(city) &&
                                             x.PassportNumber.Contains(passport)
                                            ).ToArray();

            var j = new JObject();
            var data1 = new JArray();
            foreach (var v in data)
            {
                var j1 = new JObject();
                j1.Add("Id", v.Id);
                j1.Add("FirstName", v.FirstName);
                j1.Add("Email", v.Email);
                j1.Add("PostalCode", v.PostalCode);
                j1.Add("PassportNumber", v.PassportNumber);
                j1.Add("PhoneNumber", v.PhoneNumber);
                j1.Add("Address", v.Address);
                j1.Add("EMBG", v.EMBG);
                j1.Add("City", v.City);
                data1.Add(j1);
            }
            j.Add("data", data1);
            return j;
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
                                   string operatorDateS)
        {
            DateTime startDate1 = String.IsNullOrEmpty(startDate) ? new DateTime() : Convert.ToDateTime(startDate);
            DateTime endDate1 = String.IsNullOrEmpty(endDate) ? new DateTime() : Convert.ToDateTime(endDate);
            DateTime dateI1 = String.IsNullOrEmpty(dateI) ? new DateTime() : Convert.ToDateTime(dateI);
            DateTime dateS2 = String.IsNullOrEmpty(dateS) ? new DateTime() : Convert.ToDateTime(dateS);

            var data = _ps.GetPoliciesByCountryAndType(TypePolicy, Country);

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

        public JObject GetFNOL()
        {
            var fnol = _fnls.GetAll();
            var data = new JObject();
            var data1 = new JArray();
            foreach (var v in fnol)
            {
                var user = _iss.GetInsuredData(_ps.GetPolicyHolderByPolicyID(v.PolicyId).ID);

                var j1 = new JObject();
                j1.Add("ID", v.ID);               
                j1.Add("PolicyNumber", _ps.GetPolicyNumberByPolicyId(v.PolicyId));
                j1.Add("InsuredName", user.Name + " " + user.Lastname);
                j1.Add("ClaimantPersonName", v.insured.Name+" " + v.insured.Lastname);
                j1.Add("Claimant_insured_relation", v.Relation_claimant_policy_holder);
                j1.Add("AllCosts", v.Total_cost);
                j1.Add("Date", v.additional_info.Datetime_accident);

                var HealthInsurance = _fnls.GetHealthAdditionalInfoByLossId(v.ID);
                if (HealthInsurance == null)
                {
                    j1.Add("HealthInsurance_Y_N", "Ne");
               }
                else
                {
                    j1.Add("HealthInsurance_Y_N", "Da");              
                }

                var LuggageInsurance = _fnls.GetLuggageAdditionalInfoByLossId(v.ID);
                if (LuggageInsurance == null)
                {
                    j1.Add("LuggageInsurance_Y_N", "Ne");
                }
                else
                {
                    j1.Add("LuggageInsurance_Y_N", "Da");
                }
             
                data1.Add(j1);
            }
            data.Add("data", data1);
            return data;

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
    }
}