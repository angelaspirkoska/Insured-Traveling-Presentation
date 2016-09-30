using InsuredTraveling.Models;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System;
using InsuredTraveling.DI;

namespace InsuredTraveling.Controllers.API
{
    [RoutePrefix("api/mobile")]
    public class MobileApiController : ApiController
    {
        private IPolicyService _ps;
        private IUserService _us;
        private IFirstNoticeOfLossService _fnls;
        private ILuggageInsuranceService _lis;
        private IOkSetupService _oss;
        private IHealthInsurance _his;


        

        public MobileApiController()
        {
           
        }
        public MobileApiController(IUserService us, IPolicyService ps, IFirstNoticeOfLossService fnls, IHealthInsurance his, ILuggageInsuranceService lis, IOkSetupService oss)
        {
            _ps = ps;
            _us = us;
            _fnls = fnls;
            _lis = lis;
            _oss = oss;
            _his = his;

        }
        [Route("RetrieveUserInfo")]
        public JObject RetrieveUserInformation(Username username)
        {

            JObject data = new JObject();

            //User data
            aspnetuser user = _us.GetUserDataByUsername(username.username);
            // aspnetuser user = db.aspnetusers.Where(x => x.UserName == username.username).ToArray().First();

            var u = new JObject();
            u.Add("FirstName", user.FirstName);
            u.Add("LastName", user.LastName);
            u.Add("Municipality", user.Municipality);
            u.Add("MobilePhoneNumber", user.MobilePhoneNumber);
            u.Add("PassportNumber", user.PassportNumber);
            u.Add("PostalCode", user.PostalCode);
            u.Add("PhoneNumber", user.PhoneNumber);
            u.Add("EMBG", user.EMBG);
            u.Add("DateOfBirth", user.DateOfBirth);
            u.Add("Gender", user.Gender);
            u.Add("City", user.City);
            u.Add("Address", user.Address);
            u.Add("Email", user.Email);


            data.Add("user", u);


            //User's policies
            JArray data1 = new JArray();

            // travel_policy [] policy = db.travel_policy.Where(x => x.aspnetuser.Id == user.Id).ToArray();
            travel_policy[] policy = _ps.GetPolicyByUsernameId(user.Id);
            foreach (travel_policy p1 in policy)
            {
                var p = new JObject();
                p.Add("policyNumber", p1.Policy_Number);
                p.Add("ValidFrom", p1.Start_Date);
                p.Add("ValidUntil", p1.End_Date);
                p.Add("insuredDays", p1.Valid_Days);
                p.Add("franchiseTravel", p1.Retaining_RiskID);
                //p.Add("basicPremium", p1.Osnovna_Premija);
                //p.Add("insuredBday", "1994.04.04");
                //p.Add("additional", " ");               
                //p.Add("packetTravel", "Optimum");
                p.Add("totalPremium", p1.Total_Premium);

                data1.Add(p);
            }

            data.Add("policy", data1);



            //User's quotes
            JArray data2 = new JArray();
            travel_policy[] policy2 = _ps.GetPolicyByUsernameId(user.Id);


            foreach (travel_policy p1 in policy2)
            {
                var p = new JObject();
                p.Add("policyNumber", p1.Policy_Number);
                //p.Add("insuredAddress", p1.Adresa);
                //p.Add("insuredPassport", p1.Broj_Pasos);
                //p.Add("nameContractor", p1.Ime_I_Prezime);
                // var i = db.insureds.Where(x => x.PolicyID == p1.Polisa_Broj).Single();
                // p.Add("nameInsured", i.Name + " " + i.Lastname);
                p.Add("ValidFrom", p1.Start_Date);
                p.Add("ValidUntil", p1.End_Date);
                p.Add("insuredDays", p1.Valid_Days);
                p.Add("franchiseTravel", p1.Retaining_RiskID);
                // p.Add("basicPremium", p1.Osnovna_Premija);
                // p.Add("insuredBday", "1994.04.04");
                // p.Add("additional", " ");
                // p.Add("discount", p1.Popust_Fransiza);
                // p.Add("packetTravel", "Optimum");
                p.Add("totalPremium", p1.Total_Premium);

                data2.Add(p);
            }

            data.Add("quote", data2);


            //User's reports of loss
            JArray data3 = new JArray();

            first_notice_of_loss[] fnol = _fnls.GetByInsuredUserId(user.Id);

            foreach (first_notice_of_loss f1 in fnol)
            {
                var f = new JObject();

                if (f1.Short_Detailed == true)
                {
                    f.Add("message", f1.Message);
                    f.Add("LossID", f1.LossID);
                    f.Add("policyNumber", f1.PolicyNumber);
                }
                else
                {

                    f.Add("LossID", f1.LossID);
                    f.Add("policyNumber", f1.PolicyNumber);

                    f.Add("insuredName", f1.Claimant_person_name);
                    f.Add("insuredEMBG", f1.Claimant_person_embg);
                    f.Add("insuredAddress", f1.Claimant_person_address);
                    f.Add("insuredTransactionAccount", f1.Claimant_person_transaction_number);
                    f.Add("insuredPhone", f1.Claimant_person_number);
                    f.Add("deponentInsured", f1.Claimant_person_deponent_bank);
                    f.Add("relationship", f1.Claimant_insured_relation);

                    f.Add("deponent", f1.Insured_person_deponent_bank);
                    f.Add("TransactionAccount", f1.Insured_person_transaction_number);

                    f.Add("travelDestination", f1.Land_trip);
                    f.Add("travelDateFrom", f1.Trip_startdate);
                    f.Add("travelTimeFrom", f1.Trip_starttime);
                    f.Add("travelDateTo", f1.Trip_enddate);
                    f.Add("travelTimeTo", f1.Trip_endtime);
                    f.Add("transportationType", f1.Type_transport_trip);

                    f.Add("additionalDocumentsHanded", f1.Additional_documents_handed);
                    f.Add("valueExpenses", f1.AllCosts);
                    f.Add("HealthInsurance", f1.HealthInsurance_Y_N);
                    f.Add("LuggageInsurance", f1.LuggageInsurance_Y_N);

                    if (f1.HealthInsurance_Y_N == true)
                    {
                        var h = f1.health_insurance;

                        if (h != null)
                        {
                            f.Add("lossDate", h.Date_of_accsident);
                            f.Add("lossTime", h.Time_of_accsident);
                            f.Add("placeLoss", h.Place_of_accsident);
                            f.Add("DoctorInfo", h.Doctor_data);
                            f.Add("illnessInfo", h.Disease_description);
                            f.Add("documentsHanded", h.Documents_proof);
                            f.Add("additionalInfo", h.Additional_info);
                        }
                    }
                    else if (f1.LuggageInsurance_Y_N == true)
                    {

                        var l = _lis.GetById(f1.LossID);

                        f.Add("baggageLossDate", l.Date_of_loss);
                        f.Add("placeBaggageLoss", l.Place_desc_of_loss);
                        f.Add("placeReported", l.Place_reported);
                        f.Add("descriptionLostStolenThings", l.Desc_of_stolen_damaged_things);
                        f.Add("detailedDescription", l.Detailed_description);
                        f.Add("documentsHanded2", l.Documents_proof);
                        f.Add("airportArrivalTime", l.AirportArrivalTime);
                        f.Add("baggageDropTime", l.LuggageDropTime);
                    }
                }

                data3.Add(f);
            }

            data.Add("loss", data3);


            return data;
        }

        [HttpPost]
        [Route("ok_setup")]
        public IHttpActionResult OK_Setup(OK_SETUP ok_s)
        {
            if (ok_s == null) return BadRequest();
            if (String.IsNullOrEmpty(ok_s.InsuranceCompany)) return BadRequest();

            var data = _oss.GetLastByInsuranceCompany(ok_s.InsuranceCompany);
            if (ok_s.Version == null)
            {
                return Json(data);
            }
            else if (ok_s.Version == data.VersionNumber)
            {
                return Ok();
            }
            return Json(data);
        }

        [HttpPost]
        [Route("ReportLoss")]
        public IHttpActionResult ReportLoss(FirstNoticeOfLoss f)
        {
            if (f.ShortDetailed == true)
            {
                first_notice_of_loss f1 = new first_notice_of_loss();
                f1.Insured_User = _us.GetUserIdByUsername(f.username);
                f1.Message = f.message;
                f1.PolicyNumber = (int)f.policyNumber;
                f1.Web_Mobile = f.WebMobile;

                try
                {
                    _fnls.Add(f1);

                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                return Ok();
            }
            else
            {

                var FirstNoticeOfLossNew = _fnls.Create();
                FirstNoticeOfLossNew.PolicyNumber = (int)f.policyNumber;
                FirstNoticeOfLossNew.Insured_User = _us.GetUserIdByUsername(f.username);
                FirstNoticeOfLossNew.Insured_person_transaction_number = f.TransactionAccount;
                FirstNoticeOfLossNew.Insured_person_deponent_bank = f.deponent;
                FirstNoticeOfLossNew.Claimant_person_name = f.insuredName;
                FirstNoticeOfLossNew.Claimant_person_embg = f.insuredEMBG;
                FirstNoticeOfLossNew.Claimant_person_address = f.insuredAddress;
                FirstNoticeOfLossNew.Claimant_person_number = f.insuredPhone;
                FirstNoticeOfLossNew.Claimant_person_transaction_number = f.insuredTransactionAccount;
                FirstNoticeOfLossNew.Claimant_person_deponent_bank = f.deponentInsured;
                FirstNoticeOfLossNew.Claimant_insured_relation = f.relationship;
                FirstNoticeOfLossNew.Land_trip = f.travelDestination;
                FirstNoticeOfLossNew.Trip_startdate = ((DateTime)f.travelDateFrom).Date;
                FirstNoticeOfLossNew.Trip_starttime = f.travelTimeFrom;
                FirstNoticeOfLossNew.Trip_enddate = ((DateTime)f.travelDateTo).Date;
                FirstNoticeOfLossNew.Trip_endtime = f.travelTimeTo;
                FirstNoticeOfLossNew.Type_transport_trip = f.transportationType;
                FirstNoticeOfLossNew.Additional_documents_handed = f.additionalDocumentsHanded;
                FirstNoticeOfLossNew.DateTime = DateTime.Now;
                FirstNoticeOfLossNew.AllCosts = f.valueExpenses;
                FirstNoticeOfLossNew.LuggageInsurance_Y_N = f.LuggageInsurance;
                FirstNoticeOfLossNew.HealthInsurance_Y_N = f.HealthInsurance;
                FirstNoticeOfLossNew.Web_Mobile = f.WebMobile;
                FirstNoticeOfLossNew.Short_Detailed = f.ShortDetailed;


                try
                {
                    _fnls.Add(FirstNoticeOfLossNew);

                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }


                if (FirstNoticeOfLossNew.HealthInsurance_Y_N == true)
                {
                    var h = _his.Create();
                    h.ID = FirstNoticeOfLossNew.LossID;
                    h.Place_of_accsident = f.placeLoss;
                    h.Doctor_data = f.DoctorInfo;
                    h.Date_of_accsident = (DateTime)f.lossDate;
                    h.Disease_description = f.illnessInfo;
                    h.Documents_proof = f.documentsHanded;
                    h.Additional_info = f.additionalInfo;
                    h.Time_of_accsident = (TimeSpan)f.lossTime;
                    try
                    {
                        _his.Add(h);
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }


                }
                else if (FirstNoticeOfLossNew.LuggageInsurance_Y_N == true)
                {
                    luggage_insurance l = new luggage_insurance();
                    l.ID = FirstNoticeOfLossNew.LossID;
                    l.Date_of_loss = (DateTime)f.baggageLossDate;
                    l.Place_desc_of_loss = f.placeBaggageLoss;
                    l.Place_reported = f.placeReported;
                    l.Detailed_description = f.detailedDescription;
                    l.Desc_of_stolen_damaged_things = f.descriptionLostStolenThings;
                    l.Documents_proof = f.documentsHanded2;
                    l.AirportArrivalTime = (TimeSpan)f.airportArrivalTime;
                    l.LuggageDropTime = (TimeSpan)f.baggageDropTime;

                    try
                    {
                        _lis.Add(l);
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError(ex);
                    }



                }



                return Ok();
            }
        }
    }
}
