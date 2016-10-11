using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using InsuredTraveling.DI;

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

        public SearchController(IPolicyService ps, IFirstNoticeOfLossService fnls, IUserService us, IPolicyTypeService pts, IInsuredsService iss, IBankAccountService bas)
        {
            _ps = ps;
            _fnls = fnls;
            _us = us;
            _pts = pts;
            _iss = iss;
            _bas = bas;

        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var type_policies = GetTypeOfPolicy();
            await Task.WhenAll(type_policies);
            ViewBag.TypeOfPolicy = type_policies.Result;
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
        public JObject GetPolicies(string name, string embg, string land, string address, int? TypePolycies, string agency, string startDate, string endDate, string dateI, string dateS, string operatorStartDate, string operatorEndDate, string operatorDateI, string operatorDateS)
        {
            name = name.ToLower();
            embg = embg.ToLower();
            address = address.ToLower();
            agency = agency.ToLower();
            DateTime startDate1 = String.IsNullOrEmpty(startDate) ? new DateTime() : Convert.ToDateTime(startDate);
            DateTime endDate1 = String.IsNullOrEmpty(endDate) ? new DateTime() : Convert.ToDateTime(endDate);
            DateTime dateI1 = String.IsNullOrEmpty(dateI) ? new DateTime() : Convert.ToDateTime(dateI);
            DateTime dateS2 = String.IsNullOrEmpty(dateS) ? new DateTime() : Convert.ToDateTime(dateS);

            if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(embg) || !String.IsNullOrEmpty(address) || !String.IsNullOrEmpty(land) || !String.IsNullOrEmpty(agency) || TypePolycies.HasValue)
            {
                var data = (TypePolycies.HasValue) ? _ps.GetPolicyByTypePolicies(TypePolycies.Value) : _ps.GetAllPolicies();

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

                var j = new JObject();
                var data1 = new JArray();
                foreach (var v in data)
                {
                    var j1 = new JObject();
                    j1.Add("Polisa_Broj", v.Policy_Number);
                    j1.Add("Country", v.country.Name);
                    j1.Add("Policy_type", v.policy_type.type);
                    j1.Add("Zapocnuva_Na", v.Start_Date.Date.ToShortDateString());
                    j1.Add("Zavrsuva_Na", v.End_Date.Date.ToShortDateString());
                    j1.Add("Datum_Na_Izdavanje", v.Date_Created.Date.ToShortDateString());
                    j1.Add("Datum_Na_Storniranje", v.Date_Cancellation.HasValue ? v.Date_Cancellation.Value.Date.ToShortDateString().ToString(): "/");

                    data1.Add(j1);
                }
                j.Add("data", data1);
                return j;

            }
            else if (!String.IsNullOrEmpty(startDate) || !String.IsNullOrEmpty(endDate) || !String.IsNullOrEmpty(dateI) || !String.IsNullOrEmpty(dateS))
            {
                var data = _ps.GetAllPolicies();
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
                var j = new JObject();
                var data1 = new JArray();
                foreach (var v in data)
                {
                    var j1 = new JObject();
                    j1.Add("Polisa_Broj", v.Policy_Number);
                    j1.Add("Country", v.country.Name);
                    j1.Add("Policy_type", v.policy_type.type);
                    j1.Add("Zapocnuva_Na", v.Start_Date.Date.ToShortDateString());
                    j1.Add("Zavrsuva_Na", v.End_Date.Date.ToShortDateString());
                    j1.Add("Datum_Na_Izdavanje", v.Date_Created.Date.ToShortDateString());
                    j1.Add("Datum_Na_Storniranje", v.Date_Cancellation.HasValue ? v.Date_Cancellation.Value.Date.ToShortDateString().ToString() : "/");

                    data1.Add(j1);
                }
                j.Add("data", data1);
                return j;

            }
            else
            {
                var data = _ps.GetAllPolicies();
                var j = new JObject();
                var data1 = new JArray();
                foreach (var v in data)
                {
                    var j1 = new JObject();
                    j1.Add("Polisa_Broj", v.Policy_Number);
                    j1.Add("Country", v.country.Name);
                    j1.Add("Policy_type", v.policy_type.type);
                    j1.Add("Zapocnuva_Na", v.Start_Date.Date.ToShortDateString());
                    j1.Add("Zavrsuva_Na", v.End_Date.Date.ToShortDateString());
                    j1.Add("Datum_Na_Izdavanje", v.Date_Created.Date.ToShortDateString());
                    j1.Add("Datum_Na_Storniranje", v.Date_Cancellation.HasValue ? v.Date_Cancellation.Value.Date.ToShortDateString().ToString() : "/");

                    data1.Add(j1);
                }
                j.Add("data", data1);
                return j;
            }

        }

        public JObject GetFNOL()
        {
            var fnol = _fnls.GetAll();
            var data = new JObject();
            var data1 = new JArray();
            foreach (var v in fnol)
            {
                //v.insureduser
               
                var user = _iss.GetInsuredData(_ps.GetPolicyHolderByPolicyID(v.PolicyId).ID);

                var j1 = new JObject();
                j1.Add("ID", v.ID);               
                j1.Add("PolicyNumber", _ps.GetPolicyNumberByPolicyId(v.PolicyId));
                j1.Add("InsuredName", user.Name + " " + user.Lastname);
                j1.Add("ClaimantPersonName", v.insured.Name+" " + v.insured.Lastname);
                j1.Add("Claimant_insured_relation", v.Relation_claimant_policy_holder);
               // j1.Add("AdditionalDocumentsHanded", v.Additional_documents_handed);
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
    }
}