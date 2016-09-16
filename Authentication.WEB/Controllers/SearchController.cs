using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    public class SearchController : Controller
    {
        private InsuredTravelingEntity _db;

        public SearchController()
        {
            _db = new InsuredTravelingEntity();
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
            foreach(var v in data)
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
        public JObject GetPolicies(string name, string embg, string land, string address, string TypePolycies, string agency, string startDate, string endDate, string dateI, string dateS, string operatorStartDate, string operatorEndDate, string operatorDateI, string operatorDateS)
        {
            name = name.ToLower();
            embg = embg.ToLower();
            address = address.ToLower();
            agency = agency.ToLower();
            DateTime startDate1 = String.IsNullOrEmpty(startDate) ? new DateTime() : Convert.ToDateTime(startDate);
            DateTime endDate1 = String.IsNullOrEmpty(endDate) ? new DateTime() : Convert.ToDateTime(endDate);
            DateTime dateI1 = String.IsNullOrEmpty(dateI) ? new DateTime() : Convert.ToDateTime(dateI);
            DateTime dateS2 = String.IsNullOrEmpty(dateS) ? new DateTime() : Convert.ToDateTime(dateS);

            if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(embg) || !String.IsNullOrEmpty(address) || !String.IsNullOrEmpty(land) || !String.IsNullOrEmpty(agency) || !String.IsNullOrEmpty(TypePolycies))
            {
                var data = _db.travel_policy.Where(x => x.Policy_TypeID.Equals(TypePolycies)
                                                       ).ToList();

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
                    j1.Add("Country", v.CountryID);
                    j1.Add("Policy_type", v.Policy_TypeID);
                    j1.Add("Zapocnuva_Na", v.Start_Date);
                    j1.Add("Zavrsuva_Na", v.End_Date);
                    j1.Add("Datum_Na_Izdavanje", v.Date_Created);

                    data1.Add(j1);
                }
                j.Add("data", data1);
                return j;

            }
            else if (!String.IsNullOrEmpty(startDate) || !String.IsNullOrEmpty(endDate) || !String.IsNullOrEmpty(dateI) || !String.IsNullOrEmpty(dateS))
            {
                var data = _db.travel_policy.ToList();
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
                    j1.Add("Policy_type", v.Policy_TypeID);
                    j1.Add("Zapocnuva_Na", v.Start_Date);
                    j1.Add("Zavrsuva_Na", v.End_Date);
                    j1.Add("Datum_Na_Izdavanje", v.Date_Created);
                    j1.Add("Datum_Na_Storniranje", v.Date_Cancellation);

                    data1.Add(j1);
                }
                j.Add("data", data1);
                return j;

            }
            else
            {
                var data = _db.travel_policy.ToArray();
                var j = new JObject();
                var data1 = new JArray();
                foreach (var v in data)
                {
                    var j1 = new JObject();
                    j1.Add("Polisa_Broj", v.Policy_Number);
                    j1.Add("Country", v.CountryID);
                    j1.Add("Policy_type", v.Policy_TypeID);
                    j1.Add("Zapocnuva_Na", v.Start_Date);
                    j1.Add("Zavrsuva_Na", v.End_Date);;
                    j1.Add("Datum_Na_Izdavanje", v.Date_Created);
                    j1.Add("Datum_Na_Storniranje", v.Date_Cancellation);

                    data1.Add(j1);
                }
                j.Add("data", data1);
                return j;
            }

        }

        public JObject GetFNOL()
        {
            var fnol = _db.first_notice_of_loss.ToArray();
            var data = new JObject();
            var data1 = new JArray();
            foreach (var v in fnol)
            {
                var user = _db.aspnetusers.Where(x => x.Id == v.Insured_User).ToArray().Last();
                var j1 = new JObject();
                j1.Add("ID", v.LossID);
                j1.Add("PolicyNumber", v.PolicyNumber);
                j1.Add("InsuredName", user.FirstName + " " + user.LastName);
                j1.Add("ClaimantPersonName", v.Claimant_person_name);
                j1.Add("Claimant_insured_relation", v.Claimant_insured_relation);
                j1.Add("AdditionalDocumentsHanded", v.Additional_documents_handed);
                j1.Add("AllCosts", v.AllCosts);
                j1.Add("Date", v.DateTime);
                j1.Add("HealthInsurance_Y/N", (v.HealthInsurance_Y_N == true)? "Da" : "Ne");
                j1.Add("LuggageInsurance_Y/N", (v.LuggageInsurance_Y_N == true)? "Da" : "Ne");
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
            var loss = _db.first_notice_of_loss.Where(x => x.LossID == lossID).ToArray().First();
            var user = _db.aspnetusers.Where(x => x.Id == loss.Insured_User).ToArray().Last();

            var jarray = new JArray();
            var j = new JObject();

            //Data of insured
            j.Add("InsuredName", user.FirstName + " " + user.LastName);
            j.Add("InsuredAddress", user.Address);
            j.Add("InsuredPhone", user.MobilePhoneNumber);
            j.Add("InsuredEmbg", user.EMBG);
            j.Add("InsuredTransaction", " ");
            j.Add("InsuredDeponentBank", " ");

            //Dats of user of insurance
            j.Add("ClaimantPersonName", loss.Claimant_person_name);
            j.Add("ClaimantPersonAddress", loss.Claimant_person_address);
            j.Add("ClaimantPersonPhone", loss.Claimant_person_number);
            j.Add("ClaimantPersonEMBG", loss.Claimant_person_embg);
            j.Add("ClaimantPersonTransaction", loss.Claimant_person_transaction_number);
            j.Add("ClaimantPersonDeponentBank", loss.Claimant_person_deponent_bank);
            j.Add("ClaimantInsuredRelation", loss.Claimant_insured_relation);

            //Data of trip
            j.Add("LandTrip", loss.Land_trip);
            j.Add("TypeTransport", loss.Type_transport_trip);
            j.Add("TripStartDate", ((DateTime)loss.Trip_startdate).Date + "-" + loss.Trip_starttime);
            j.Add("TripEndDate", ((DateTime)loss.Trip_enddate).Date + "-" + loss.Trip_endtime);

            //Data of costs
            j.Add("AdditionalDocumentsHanded", loss.Additional_documents_handed);
            j.Add("AllCosts", loss.AllCosts);
            j.Add("Date", loss.DateTime);
            j.Add("HealthInsurance_YN", (loss.HealthInsurance_Y_N == true) ? "Da" : "Ne");
            j.Add("LuggageInsurance_YN", (loss.LuggageInsurance_Y_N == true) ? "Da" : "Ne");

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

            }else if (loss.luggage_insurance != null)
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
            var policy = _db.policy_type.Select(p => new SelectListItem
            {
                Text = p.type,
                Value = p.ID.ToString()
            });
            return await policy.ToListAsync();
        }
    }
}