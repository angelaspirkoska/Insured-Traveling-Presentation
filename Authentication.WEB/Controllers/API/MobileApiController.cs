using InsuredTraveling.Models;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System;
using InsuredTraveling.DI;
using System.Text;

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
        private IBankAccountService _bas;

        

        public MobileApiController()
        {
           
        }
        public MobileApiController(IUserService us, IPolicyService ps, IFirstNoticeOfLossService fnls, IHealthInsurance his, ILuggageInsuranceService lis, IOkSetupService oss, IBankAccountService bas)
        {
            _ps = ps;
            _us = us;
            _fnls = fnls;
            _lis = lis;
            _oss = oss;
            _his = his;
            _bas = bas;

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
                    f.Add("LossID", f1.ID);

                    f.Add("policyNumber", f1.travel_policy.Policy_Number);
                }
                else
                {
                    var ClaimantBankAccount = _bas.BankAccountInfoById(f1.Claimant_bank_accountID);

                    var PolicyHolderBankAccount = _bas.BankAccountInfoById(f1.travel_policy.Policy_HolderID);
                    f.Add("LossID", f1.ID);
                    f.Add("policyNumber", f1.travel_policy.Policy_Number);

                    f.Add("insuredName", f1.insured.Name + " " + f1.insured.Lastname);
                    f.Add("insuredEMBG", f1.insured.SSN);
                    f.Add("insuredAddress", f1.insured.Address + " " + f1.insured.City);

                    f.Add("insuredTransactionAccount", ClaimantBankAccount.Account_Number);
                    f.Add("insuredPhone", f1.insured.Phone_Number);
                    f.Add("deponentInsured", ClaimantBankAccount.bank.Name);
                    f.Add("relationship", f1.Relation_claimant_policy_holder);

                    f.Add("deponent", PolicyHolderBankAccount.bank.Name);
                    f.Add("TransactionAccount", PolicyHolderBankAccount.Account_Number);

                    f.Add("travelDestination", f1.Destination);
                    f.Add("travelDateFrom", f1.Depart_Date_Time.Date);
                    f.Add("travelTimeFrom", f1.Depart_Date_Time.TimeOfDay);
                    f.Add("travelDateTo", f1.Arrival_Date_Time.Date);
                    f.Add("travelTimeTo", f1.Arrival_Date_Time.TimeOfDay);
                    f.Add("transportationType", f1.Transport_means);

                    f.Add("additionalDocumentsHanded", "");
                    f.Add("valueExpenses", f1.Total_cost);
                    var HealthInsurance = _fnls.GetHealthAdditionalInfoByLossId(f1.ID);
                    if (HealthInsurance == null)
                    {
                        f.Add("HealthInsurance_Y_N", "Ne");

                    }
                    else
                    {
                        f.Add("HealthInsurance_Y_N", "Da");
                        f.Add("Date_of_accsident", HealthInsurance.additional_info.Datetime_accident.Date + "-" + HealthInsurance.additional_info.Datetime_accident.TimeOfDay + "-" + HealthInsurance.additional_info.Accident_place);
                        f.Add("Doctor_data", HealthInsurance.Doctor_info + " " + HealthInsurance.Datetime_doctor_visit.ToString());
                        f.Add("Disease_description", HealthInsurance.Medical_case_description + " " + HealthInsurance.Previous_medical_history);

                        f.Add("Documents_proof", "");
                        f.Add("Additional_info", HealthInsurance.Responsible_institution);



                    }

                    var LuggageInsurance = _fnls.GetLuggageAdditionalInfoByLossId(f1.ID);
                    if (LuggageInsurance == null)
                    {
                        f.Add("LuggageInsurance_Y_N", "Ne");

                    }
                    else
                    {
                        f.Add("LuggageInsurance_Y_N", "Da");
                        f.Add("Date_of_loss", LuggageInsurance.additional_info.Datetime_accident.Date + "-" + LuggageInsurance.additional_info.Datetime_accident.TimeOfDay + "-" + LuggageInsurance.additional_info.Accident_place);
                        f.Add("Place_desc_of_loss", LuggageInsurance.Place_description);
                        f.Add("Detailed_description", LuggageInsurance.Detail_description);
                        f.Add("Place_reported", LuggageInsurance.Report_place);
                        f.Add("Desc_of_stolen_damaged_things", LuggageInsurance.Floaters + LuggageInsurance.Floaters_value.ToString());
                        f.Add("Documents_proof2", "");
                        f.Add("AirportArrivalTime", LuggageInsurance.additional_info.Datetime_accident.TimeOfDay);
                        f.Add("LuggageDropTime", LuggageInsurance.Luggage_checking_Time);
                    }


                    data3.Add(f);
                }
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
        public IHttpActionResult ReportLoss(FirstNoticeOfLossReportViewModel f)
        {
            if (f.ShortDetailed == true)
            {
                first_notice_of_loss f1 = new first_notice_of_loss();
                //f1.insured
                var user = _ps.GetPolicyHolderByPolicyID(f1.PolicyId);
                f1.travel_policy.Policy_HolderID = user.ID;

                f1.Message = f.Message;
                f1.travel_policy.Policy_Number = f.PolicyNumber.ToString();             
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
                var FirstNoticeOfLossNew = new first_notice_of_loss();// _fnls.Create();
                FirstNoticeOfLossNew.travel_policy.Policy_Number = f.PolicyNumber.ToString();              
                var user = _ps.GetPolicyHolderByPolicyID(_ps.GetPolicyIdByPolicyNumber(f.PolicyNumber.ToString()).ID);


                //PolicyHolderData
                FirstNoticeOfLossNew.travel_policy.Policy_HolderID = user.ID;
                
                FirstNoticeOfLossNew.Policy_holder_bank_account_info.Account_Number = f.PolicyHolderBankAccountNumber;              
                FirstNoticeOfLossNew.Policy_holder_bank_account_info.bank.Name = f.PolicyHolderBankName;
                string[] a = f.PolicyHolderName.Split(' ');
                StringBuilder Temp = new StringBuilder();
                int i;
                for (i = 0; i < a.Length-1; i++)
                {
                    Temp.Append(a[i] + " ");
                }
                FirstNoticeOfLossNew.insured.Name = Temp.ToString();
                FirstNoticeOfLossNew.insured.Lastname = a[a.Length-1];
                
                FirstNoticeOfLossNew.insured.SSN = f.PolicyHolderSsn;
                string[] b = f.PolicyHolderAdress.Split(' ');
                StringBuilder str = new StringBuilder();           
                for( i = 0; i<b.Length-1; i++ )
                {
                    str.Append(b[i] + " ");
                }
                FirstNoticeOfLossNew.insured.City = b[i];
                FirstNoticeOfLossNew.insured.Address = str.ToString();       
                FirstNoticeOfLossNew.insured.Phone_Number = f.PolicyHolderPhoneNumber;



                //Claimant data

                FirstNoticeOfLossNew.Claimant_bank_account_info.Account_Number = f.ClaimantBankAccountNumber;
                              
                FirstNoticeOfLossNew.Claimant_bank_account_info.bank.Name = f.ClaimantBankName;
                FirstNoticeOfLossNew.Relation_claimant_policy_holder = f.RelationClaimantPolicyHolder;
                FirstNoticeOfLossNew.Destination = f.Destination;
                FirstNoticeOfLossNew.Depart_Date_Time = f.DepartDateTime;
                //(int year, int month, int day, int hour, int minute, int second, int millisecond)

                FirstNoticeOfLossNew.Arrival_Date_Time = new DateTime(((DateTime)f.DepartDateTime).Year, ((DateTime)f.DepartDateTime).Month, ((DateTime)f.DepartDateTime).Day, f.DepartTime?.Hours ?? 0, f.DepartTime?.Minutes ?? 0, 0, 0);
               
                FirstNoticeOfLossNew.Arrival_Date_Time = new DateTime(((DateTime)f.ArrivalDateTime).Year, ((DateTime)f.ArrivalDateTime).Month, ((DateTime)f.ArrivalDateTime).Day, f.ArriveTime?.Hours ?? 0, f.ArriveTime?.Minutes ?? 0, 0, 0); 

                FirstNoticeOfLossNew.Transport_means = f.TransportMeans;
              //  FirstNoticeOfLossNew.documents_first_notice_of_loss = null;
                
                //ModifiedBy

                //FirstNoticeOfLossNew. = DateTime.Now;

                FirstNoticeOfLossNew.Total_cost = f.TotalCost;


                if (_fnls.IsHealthInsuranceByAdditionalInfoId(f.Id))
                {
                    FirstNoticeOfLossNew.additional_info.health_insurance_info =
                        _fnls.GetHealthAdditionalInfoByLossId(f.Id);
                }
                else
                {

                    FirstNoticeOfLossNew.additional_info.luggage_insurance_info =
                        _fnls.GetLuggageAdditionalInfoByLossId(f.Id);
                }

                FirstNoticeOfLossNew.Web_Mobile = f.WebMobile;
                FirstNoticeOfLossNew.Short_Detailed = f.ShortDetailed;



              

                if (f.IsHealthInsurance)
                {
                   
                    var healthInsuranceAdditionalInfo = _his.Create();
                    healthInsuranceAdditionalInfo.ID = FirstNoticeOfLossNew.ID;
                    healthInsuranceAdditionalInfo.Place_of_accsident = f.AccidentPlaceHealth;
                    healthInsuranceAdditionalInfo.Doctor_data = f.DoctorInfo+f.DoctorVisitDateTime.ToString();
                    healthInsuranceAdditionalInfo.Date_of_accsident = f.AccidentDateTimeHealth ?? new DateTime(0,0,0);
                    healthInsuranceAdditionalInfo.Disease_description = f.MedicalCaseDescription + ((f.PreviousMedicalHistory) ? "Имам претходно искуство со болеста" : "Немам претходно искуство со болеста") ;
                    //    h.Documents_proof = f.documentsHanded;
                    healthInsuranceAdditionalInfo.Additional_info = f.DetailDescription;
                    healthInsuranceAdditionalInfo.Time_of_accsident =f.AccidentTimeHealth ?? new TimeSpan(0,0,0);
                        try
                        {
                            _his.Add(healthInsuranceAdditionalInfo);
                        }
                        catch (Exception ex)
                        {
                            return InternalServerError(ex);
                        }


                }
                else
                {

                    var l = new luggage_insurance
                    {
                        ID = FirstNoticeOfLossNew.ID,
                        Date_of_loss = f.AccidentDateTimeLuggage ?? new DateTime(0, 0, 0),
                        Place_desc_of_loss = f.AccidentPlaceLuggage,
                        Place_reported = f.ReportPlace,
                        Detailed_description = f.DetailDescription,
                        Desc_of_stolen_damaged_things = f.Floaters + " " + f.FloatersValue,
                        AirportArrivalTime  = ((f.AccidentDateTimeLuggage.HasValue) ? new TimeSpan(f.AccidentDateTimeLuggage.Value.Hour, f.AccidentDateTimeLuggage.Value.Minute, f.AccidentDateTimeLuggage.Value.Second) : new TimeSpan(0, 0, 0)),                
                        LuggageDropTime = f.LugaggeCheckingTime ?? new TimeSpan(0, 0, 0)
                    };
                    //l.Documents_proof = f.documentsHanded2;

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
