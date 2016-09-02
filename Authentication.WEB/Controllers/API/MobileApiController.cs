using InsuredTraveling.Models;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System;

namespace InsuredTraveling.Controllers.API
{
    [RoutePrefix("api/mobile")]
    public class MobileApiController : ApiController
    {
        private InsuredTravelingEntity db;

        public MobileApiController()
        {
            db = new InsuredTravelingEntity();
        }
        [Route("RetrieveUserInfo")]
        public JObject RetrieveUserInformation(Username username)
        {
            
            JObject data = new JObject();
            aspnetuser user = db.aspnetusers.Where(x => x.UserName == username.username).ToArray().First();
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
            u.Add("Gender",user.Gender);
            u.Add("City", user.City);
            u.Add("Address", user.Address);
            u.Add("Email", user.Email);
            
            data.Add("user", u);

            JArray data1 = new JArray();
            patnicko [] policy = db.patnickoes.Where(x => x.Polisa_Broj > 3).ToArray();
            
            foreach(patnicko p1 in policy)
            {
                var p = new JObject();
                p.Add("policyNumber", p1.Polisa_Broj);
                p.Add("embg", p1.EMBG);
                p.Add("insuredAddress", p1.Adresa);
                p.Add("insuredPassport", p1.Broj_Pasos);
                p.Add("nameContractor", p1.Ime_I_Prezime);
                p.Add("nameInsured", p1.Osigurenik1_Ime_I_Prezime);
                p.Add("ValidFrom", p1.Zapocnuva_Na);
                p.Add("ValidUntil", p1.Zavrsuva_Na);
                p.Add("insuredDays", p1.Vazi_Denovi);
                p.Add("franchiseTravel", p1.Fransiza);
                p.Add("basicPremium", p1.Osnovna_Premija);
                p.Add("insuredBday", "1994.04.04");
                p.Add("additional", " ");
                p.Add("discount", p1.Popust_Fransiza);
                p.Add("packetTravel", "Optimum");
                p.Add("totalPremium", p1.Vkupna_Premija);

                data1.Add(p);
            }

            data.Add("policy",data1);

            JArray data2 = new JArray();
            patnicko[] policy2 = db.patnickoes.Where(x => x.Polisa_Broj <= 3).ToArray();

            foreach (patnicko p1 in policy2)
            {
                var p = new JObject();
                p.Add("policyNumber", p1.Polisa_Broj);
                p.Add("embg", p1.EMBG);
                p.Add("insuredAddress", p1.Adresa);
                p.Add("insuredPassport", p1.Broj_Pasos);
                p.Add("nameContractor", p1.Ime_I_Prezime);
                p.Add("nameInsured", p1.Osigurenik1_Ime_I_Prezime);
                p.Add("ValidFrom", p1.Zapocnuva_Na);
                p.Add("ValidUntil", p1.Zavrsuva_Na);
                p.Add("insuredDays", p1.Vazi_Denovi);
                p.Add("franchiseTravel", p1.Fransiza);
                p.Add("basicPremium", p1.Osnovna_Premija);
                p.Add("insuredBday", "1994.04.04");
                p.Add("additional", " ");
                p.Add("discount", p1.Popust_Fransiza);
                p.Add("packetTravel", "Optimum");
                p.Add("totalPremium", p1.Vkupna_Premija);

                data2.Add(p);
            }

            data.Add("quote",data2);

            return data;
        }

        [HttpPost]
        [Route("ok_setup")]
        public IHttpActionResult OK_Setup(OK_SETUP ok_s)
        {
            if (ok_s == null) return BadRequest();
            if (String.IsNullOrEmpty(ok_s.InsuranceCompany)) return BadRequest();
            var data = db.ok_setup.Where(x => x.InsuranceCompany == ok_s.InsuranceCompany).ToArray().Last();

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
        public IHttpActionResult ReportLoss(FNOL f)
        {
            if (f.ShortDetailed == true)
            {
                first_notice_of_loss f1 = new first_notice_of_loss();
                f1.Insured_User = db.aspnetusers.Where(x => x.UserName == f.username).Select(x => x.Id).First();
                f1.Message = f.message;
                f1.PolicyNumber = f.policyNumber;
                f1.Web_Mobile = f.WebMobile;

                db.first_notice_of_loss.Add(f1);
                try
                {
                    db.SaveChanges();
                }catch(Exception ex)
                {
                    return InternalServerError(ex);
                }
                return Ok();
            }
            else
            {
                var f1 = db.first_notice_of_loss.Create();
                f1.PolicyNumber = f.policyNumber;
                f1.Insured_User = db.aspnetusers.Where(x => x.UserName == f.username).Select(x => x.Id).First();
                f1.Claimant_person_name = f.insuredName;
                f1.Claimant_person_embg = f.insuredEMBG;
                f1.Claimant_person_address = f.insuredAddress;
                f1.Claimant_person_number = f.insuredPhone;
                f1.Claimant_person_transaction_number = f.insuredTransactionAccount;
                f1.Claimant_person_deponent_bank = f.deponentInsured;
                f1.Claimant_insured_relation = f.relationship;
                f1.Land_trip = f.travelDestination;
                f1.Trip_startdate = f.travelDateFrom.Date;
                f1.Trip_starttime = f.travelTimeFrom;
                f1.Trip_enddate = f.travelDateTo.Date;
                f1.Trip_endtime = f.travelTimeTo;
                f1.Type_transport_trip = f.transportationType;
                f1.Additional_documents_handed = f.additionalDocumentsHanded;
                f1.DateTime = DateTime.Now;
                f1.AllCosts = f.valueExpenses;
                f1.LuggageInsurance_Y_N = f.LuggageInsurance;
                f1.HealthInsurance_Y_N = f.HealthInsurance;
                f1.Web_Mobile = f.WebMobile;
                f1.Short_Detailed = f.ShortDetailed;

                db.first_notice_of_loss.Add(f1);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
                

                if (f1.HealthInsurance_Y_N==true)
                {

                    var h = db.health_insurance.Create();
                    h.ID = f1.LossID;
                    h.Place_of_accsident = f.placeLoss;
                    h.Doctor_data = f.DoctorInfo;
                    h.Date_of_accsident = f.lossDate;
                    h.Disease_description = f.illnessInfo;
                    h.Documents_proof = f.documentsHanded;
                    h.Additional_info = f.additionalInfo;

                    db.health_insurance.Add(h);
                }
                else if (f1.LuggageInsurance_Y_N == true)
                {
                    luggage_insurance l = new luggage_insurance();
                    l.ID = f1.LossID;
                    l.Date_of_loss = f.baggageLossDate;
                    l.Place_desc_of_loss = f.placeBaggageLoss;
                    l.Place_reported = f.placeReported;
                    l.Detailed_description = f.detailedDescription;
                    l.Desc_of_stolen_damaged_things = f.descriptionLostStolenThings;
                    l.Documents_proof = f.documentsHanded2;
                    l.AirportArrivalTime = f.airportArrivalTime;
                    l.LuggageDropTime = f.baggageDropTime;

                    db.luggage_insurance.Add(l);

                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }

                return Ok();
            }
        }
    }
}
