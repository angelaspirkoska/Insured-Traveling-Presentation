using AutoMapper;
using System.Linq;
using InsuredTraveling.Models;
using System;
using Autofac;
using InsuredTraveling.ViewModels;
using System.Globalization;
using System.Configuration;

namespace InsuredTraveling.App_Start
{
    public class AutoMapperInitializer
    {

        public static void Initialize()
        {
            var db = new InsuredTravelingEntity2();

            Mapper.CreateMap<aspnetuser, SearchRegisteredUser>().AfterMap((src, dst) =>
            {
                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && dateTime.Contains("yy") && !dateTime.Contains("yyyy") ? dateTime.Replace("yy", "yyyy") : dateTime;
                dst.Username = src.UserName;
                dst.FirstName = src.FirstName;
                dst.LastName = src.LastName;
                dst.Email = src.Email;
                var role = src.aspnetroles.FirstOrDefault();
                if (role != null) dst.RoleName = role.Name;
                if (src.CreatedOn != null) dst.CreatedOn = src.CreatedOn.Value.ToString(dateTimeFormat, new CultureInfo("en-US"));
                dst.ActiveInactive = src.Active == 1 ? "Active" : "Inactive";
                dst.ID = src.Id;
                dst.Points = !src.Points.HasValue ? "0" :src.Points.ToString();
            });

            Mapper.CreateMap<@event, Event>().AfterMap((src, dst) =>
            {

                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && dateTime.Contains("yy") && !dateTime.Contains("yyyy") ? dateTime.Replace("yy", "yyyy") : dateTime;

                dst.id = src.ID;
                dst.Title = src.Title;
                dst.EventType = src.Type == false ? "event" : "vip event";
                dst.Location = src.Location;
                dst.Organizer = src.Organizer;
                dst.Description = src.Description;
                dst.CreatedBy = db.aspnetusers.FirstOrDefault(x => x.Id == src.CreatedBy) != null ? db.aspnetusers.FirstOrDefault(x => x.Id == src.CreatedBy).UserName : null;
                dst.PublishDate = src.PublishDate.Value.Date;
                dst.PublishDateString = src.PublishDate != null ? src.PublishDate.Value.ToString(dateTimeFormat, CultureInfo.InvariantCulture) : null;
                dst.StartDate = src.StartDate.Date;
                dst.StartDateString = src.StartDate.ToString(dateTimeFormat, CultureInfo.InvariantCulture);
                dst.EndDate = src.EndDate.Date;
                dst.EndDateString = src.EndDate.ToString(dateTimeFormat, CultureInfo.InvariantCulture);
            });

            Mapper.CreateMap<Event, @event>().AfterMap((src, dst) =>
            {
                dst.Voucher = false;
                src.CreatedBy = dst.CreatedBy;
                src.PublishDate = DateTime.Now;
                src.Title = dst.Title;
                src.Description = dst.Description;
                src.Location = dst.Location;
                src.StartDate = dst.StartDate;
                src.EndDate = src.EndDate;
                src.Organizer = src.Organizer;
                if (src.EventType.Equals("1"))
                {
                    dst.Chat = false;
                    dst.Type = false; 
                }
                else if (src.EventType.Equals("2"))
                {
                    dst.Chat = false;
                    dst.Type = true;
                }
                else if (src.EventType.Equals("3"))
                {
                    dst.Chat = true;
                    dst.Type = true;
                }else
                {
                    dst.Chat = false;
                    dst.Type = false;
                }
                var dateTime = src.StartDate;
                var timeSpan = src.StartTime;
                DateTime d = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
                d = d.Add(timeSpan);
                dst.StartDate = d;

                 dateTime = src.EndDate;
                 timeSpan = src.EndTime;
                 d = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
                d = d.Add(timeSpan);
                dst.EndDate = d;
            });

            Mapper.CreateMap<Event_UserModel, event_users>().AfterMap((src, dst) =>
            {
                dst.EventID = src.EventID;
                dst.UserID = src.UserID;
            });

               Mapper.CreateMap<aspnetuser, User>().AfterMap((src, dst) =>
            {
                dst.UserName = src.UserName;
                dst.FirstName = src.FirstName;
                dst.LastName = src.LastName;
                dst.City = src.City;
                dst.Address = src.Address;
                dst.Municipality = src.Municipality;
                dst.MobilePhoneNumber = src.MobilePhoneNumber;
                dst.Email = src.Email;
                dst.DateOfBirth = src.DateOfBirth;
                dst.EMBG = src.EMBG;
                dst.Gender = src.Gender;
                dst.PassportNumber = src.PassportNumber;
                dst.PostalCode = src.PostalCode;
                var firstOrDefault = src.aspnetroles.FirstOrDefault();
                if (firstOrDefault != null) dst.Role = firstOrDefault.Name;
                dst.PhoneNumber = src.PhoneNumber;
                dst.Sum_premium = src.Sum_premium;
            });

            Mapper.CreateMap<CreateClientModel , insured>().AfterMap((src, dst) =>
            {
                dst.Name = src.Name;
                dst.Lastname = src.LastName;
                dst.Email = src.Email;
                dst.DateBirth = src.DateBirth;
                dst.Address = src.Address;
                dst.City = src.City;
                dst.SSN = src.SSN;
                dst.Postal_Code = src.Postal_Code;
                dst.Phone_Number = src.PhoneNumber;
                dst.Passport_Number_IdNumber = src.Passport_Number_IdNumber;
                dst.Created_By = System.Web.HttpContext.Current.User.Identity.Name;
                dst.Date_Created = DateTime.UtcNow;
                dst.Age = src.Age;
                dst.type_insured = null;
                dst.aspnetuser = null;
                dst.aspnetuser1 = null;            
            });

            Mapper.CreateMap<Ok_SetupModel, ok_setup>().AfterMap((src, dst) =>
            {
                dst.Sms_Code_Seconds = src.Sms_Code_Seconds;
                dst.NumberOfAttempts = src.NumberOfAttempts;
                dst.NumberOfNews = src.NumberOfNews;
                dst.NotificationTime = src.NotificationTime;
                dst.NumberOfLastMsg = src.NumberOfLastMsg;
                dst.InsuranceCompany = src.InsuranceCompany;
                dst.VersionNumber = src.VersionNumber;
                dst.Created_Date = src.Created_Date;
                dst.Created_By = src.Created_By;
          
                dst.SSNValidationActive = src.SSNValidationActive;
            });

       

            Mapper.CreateMap<SalePoints, sava_sale_points>().AfterMap((src, dst) =>
            {
                dst.longitude = src.Longitude;
                dst.latitude = src.Latitude;
                dst.insurance_deal = src.insuranceDeal ;
                dst.insurance_report = src.damageReport ;
                dst.location_name = src.LocationName;
                dst.municipality = src.Municipality;
                dst.street = src.Street;
                dst.postalcode = src.PostalCode.ToString();
                dst.opening_hours = src.OpeningHours;
                dst.phone_number = src.PhoneNumber;

            });
         
            Mapper.CreateMap<sava_sale_points, SalePoints>().AfterMap((src, dst) =>
            {
                dst.id = src.id;
                dst.Longitude = src.longitude;
                dst.insuranceDeal = src.insurance_deal;
                dst.LocationName = src.location_name;
                dst.Municipality = src.municipality;
                dst.Street = src.street;
                dst.OpeningHours = src.opening_hours;
                dst.PhoneNumber = src.phone_number;
                dst.PostalCode = int.Parse(src.postalcode);
                dst.damageReport = src.insurance_report;

             

            });

            //Save
            Mapper.CreateMap<News, news_all>().AfterMap((src, dst) =>
            {

                dst.ImageLocation = src.ImageLocation;
                dst.InsuranceCompany = src.InsuranceCompany;
                dst.isNotification = src.isNotification;
                dst.Title = src.Title;
                dst.Content = src.Content;
                dst.DataCreated = DateTime.Now.Date;
                

            });

            Mapper.CreateMap<news_all, News>().AfterMap((src, dst) =>
            {

                dst.ImageLocation = src.ImageLocation;
                dst.InsuranceCompany = src.InsuranceCompany;
                dst.isNotification = src.isNotification;
                dst.Title = src.Title;
                dst.Content = src.Content;
                


            });


            Mapper.CreateMap<DiscountModel, discount_codes>().AfterMap((src, dst) =>
            {
                dst.Discount_Name = src.Discount_Name;
                dst.Discount_Coef = src.Discount_Coef;
                dst.Start_Date = src.End_Date;
                dst.End_Date = src.End_Date;
           
            });
            Mapper.CreateMap<SavaAdPicturesModel, sava_ad_pictures>().AfterMap((src, dst) =>
            {
                dst.ImageLocation = src.ImageLocation;
                dst.Text = src.Text;
                dst.Title = src.Title;
                dst.DateCreated = src.DateCreated;

            });
            Mapper.CreateMap<PointsRequestModel, points_requests>().AfterMap((src, dst) =>
            {
                dst.id_user = src.id_user;
                dst.policy_id = src.policy_id;
                dst.ssn = src.ssn;
                dst.timestamp = src.DateCreated;

            }); 
            Mapper.CreateMap<points_requests, PointsRequestModel>().AfterMap((src, dst) =>
            {
                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && dateTime.Contains("yy") && !dateTime.Contains("yyyy") ? dateTime.Replace("yy", "yyyy") : dateTime;
                dst.id = dst.id;
                dst.flag = dst.flag;
                dst.id_user = src.id_user;
                dst.policy_id = src.policy_id;
                dst.ssn = src.ssn;
                dst.DateCreated = src.timestamp;
                dst.DateCreatedString = src.timestamp.ToString(dateTimeFormat, CultureInfo.InvariantCulture);
            });



            Mapper.CreateMap<Sava_AdminPanelModel, sava_setup>().AfterMap((src, dst) =>
            {
                dst.points_percentage = src.points_percentage;
                dst.vip_sum = src.vip_sum;
                dst.email_administrator = src.email_administrator;
                dst.timestamp = src.timestamp;
                dst.last_modify_by = src.last_modify_by;
                

            });

            Mapper.CreateMap<SavaVoucherModel, sava_voucher>().AfterMap((src, dst) =>
            {
                dst.voucher_code = src.voucher_code;
                dst.id_policyHolder = src.id_policyHolder;
                dst.points_used = src.points_used;
                dst.id_seller = src.id_seller;
                dst.timestamp = src.timestamp;


            });
           
            Mapper.CreateMap<sava_voucher, SavaVoucherModel>().AfterMap((src, dst) =>
            {
                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && dateTime.Contains("yy") && !dateTime.Contains("yyyy") ? dateTime.Replace("yy", "yyyy") : dateTime;

                dst.id = src.id;
                dst.voucher_code = src.voucher_code;
                dst.id_policyHolder = src.id_policyHolder;
                dst.points_used = src.points_used;
                dst.id_seller = src.id_seller;
                dst.timestamp = src.timestamp;
                dst.TimeStampString = src.timestamp.ToString(dateTimeFormat, CultureInfo.InvariantCulture);

            });

            Mapper.CreateMap<SavaPolicyModel, sava_policy>().AfterMap((src, dst) =>
            {
                dst.SSN_insured = src.SSN_insured;
                dst.SSN_policyHolder = src.SSN_policyHolder;
                dst.email_seller = src.email_seller;
                dst.discount_points = src.discount_points;
                dst.expiry_date = src.expiry_date;                
                dst.premium = src.premium;
                dst.date_created = src.date_created;
            });

            Mapper.CreateMap<QRCodeSavaPolicy, SavaPolicyModel>().AfterMap((src, dst) =>
            {
                dst.SSN_policyHolder = src.SSNHolder;
                dst.SSN_insured = src.SSNInsured;
                dst.policy_number = src.PolicyNumber;
                dst.email_seller = src.EmailSeller;
                dst.expiry_date = src.ExpireDate != null ? Convert.ToDateTime(src.ExpireDate) : DateTime.UtcNow;
                dst.premium = src.Premium != null ? Convert.ToInt32(src.Premium) : 0;
            });


            //Mapper.CreateMap<first_notice_of_loss, FirstNoticeOfLossEditViewModel>().AfterMap((src, dst) =>
            //{
            //    var policy = src.travel_policy;
            //    var user = policy != null ? policy.insured : null;
            //    var policy_holder_bank_account = src.Policy_holder_bank_account_info;
            //    var policy_holder_bank = src.Polidcy_holder_bank_account_info.bank;          
            //    var claimant = src.insured;
            //    var claimant_bank_account = src.Claimant_bank_account_info;
            //    var claimant_bank = src.Claimant_bank_account_info.bank;                               
            //    var additional_info = src.additional_info;
            //    var healthAdditionalInfo = src.additional_info.health_insurance_info;
            //    var luggageInsuranceInfo = src.additional_info.luggage_insurance_info;
            //    dst.PolicyId = src.PolicyId;
            //    dst.FNOLNumber = src.FNOL_Number;
            //    dst.PolicyHolderId = src.travel_policy.insured.ID;
            //    dst.PolicyNumber = policy != null ? Convert.ToInt32(policy.Policy_Number) : 0;
            //    dst.PolicyHolderName = user != null ? user.Name + " " + user.Lastname : null;
            //    dst.PolicyHolderAdress = user != null ? user.Address : null;
            //    dst.PolicyHolderPhoneNumber = user != null ? user.Phone_Number : null;
            //    dst.PolicyHolderSsn = user != null ? user.SSN : null;
            //    dst.PolicyHolderBankAccountNumber = user != null ? policy_holder_bank_account.Account_Number : null;
            //    dst.PolicyHolderBankName = policy_holder_bank != null ? policy_holder_bank.Name : null;
            //    dst.PolicyHolderBankAccountId = src.Policy_holder_bank_accountID.Value;
            //    dst.ClaimantId = src.ClaimantId;
            //    dst.ClaimantName = claimant != null ? claimant.Name + " " + claimant.Lastname : null;
            //    dst.ClaimantAdress = claimant != null ? claimant.Address : null;
            //    dst.ClaimantPhoneNumber = claimant != null ? claimant.Phone_Number : null;
            //    dst.ClaimantSsn = claimant != null ? claimant.SSN : null;
            //    dst.RelationClaimantPolicyHolder = src.Relation_claimant_policy_holder;
            //    dst.ClaimantBankAccountNumber = user != null ? claimant_bank_account.Account_Number : null;
            //    dst.ClaimantBankName = policy_holder_bank != null ? claimant_bank.Name : null;
            //    dst.ClaimantBankAccountId = src.Claimant_bank_accountID.Value;
            //    dst.Destination = src.Destination;
            //    dst.DepartDateTime = src.Depart_Date_Time.Date;
            //    dst.DepartTime = src.Depart_Date_Time.TimeOfDay;
            //    dst.TransportMeans = src.Transport_means;
            //    dst.ArrivalDateTime = src.Arrival_Date_Time.Date;
            //    dst.ArriveTime = src.Arrival_Date_Time.TimeOfDay;
            //    dst.IsHealthInsurance = healthAdditionalInfo != null ? true : false;
            //    dst.AccidentDateTimeHealth = additional_info != null ? (DateTime?)additional_info.Datetime_accident.Date : null;
            //    dst.AccidentTimeHealth = additional_info != null ? (TimeSpan?)additional_info.Datetime_accident.TimeOfDay : null;
            //    dst.AccidentPlaceHealth = additional_info != null ? additional_info.Accident_place : null;
            //    dst.DoctorVisitDateTime = healthAdditionalInfo != null ? healthAdditionalInfo.Datetime_doctor_visit : null;
            //    dst.DoctorInfo = healthAdditionalInfo != null ? healthAdditionalInfo.Doctor_info : null;
            //    dst.MedicalCaseDescription = healthAdditionalInfo != null ? healthAdditionalInfo.Medical_case_description : null;
            //    var periousMedicalHistory = healthAdditionalInfo != null ? healthAdditionalInfo.Previous_medical_history : null;
            //    dst.PreviousMedicalHistory = periousMedicalHistory != null ? Convert.ToBoolean(periousMedicalHistory) : false;
            //    dst.ResponsibleInstitution = healthAdditionalInfo != null ? healthAdditionalInfo.Responsible_institution : null;
            //    dst.AccidentDateTimeLuggage = additional_info != null ? (DateTime?)additional_info.Datetime_accident.Date : null;
            //    dst.AccidentPlaceLuggage = additional_info != null ? additional_info.Accident_place : null;
            //    dst.PlaceDescription = luggageInsuranceInfo != null ? luggageInsuranceInfo.Place_description : null;
            //    dst.DetailDescription = luggageInsuranceInfo != null ? luggageInsuranceInfo.Detail_description : null;
            //    dst.ReportPlace = luggageInsuranceInfo != null ? luggageInsuranceInfo.Report_place : null;
            //    dst.Floaters = luggageInsuranceInfo != null ? luggageInsuranceInfo.Floaters : null;
            //    dst.FloatersValue = luggageInsuranceInfo != null ? luggageInsuranceInfo.Floaters_value.ToString() : null;
            //    dst.AccidentTimeLuggage = additional_info != null ? (TimeSpan?)additional_info.Datetime_accident.TimeOfDay : null;
            //    dst.LugaggeCheckingTime = luggageInsuranceInfo != null ? (TimeSpan?)luggageInsuranceInfo.Luggage_checking_Time : null;
            //    dst.ModifiedBy = src.CreatedBy;
            //    dst.ModifiedDateTime = DateTime.Now;
            //    dst.TotalCost = src.Total_cost;
            //    dst.AdditionalInfoId = src.Additional_infoID;
            //});

            Mapper.CreateMap<Policy, travel_policy>().AfterMap((src, dst) =>
            {
                dst.Created_By = src.Created_By;
                dst.Date_Created = src.Date_Created.HasValue ? src.Date_Created.Value.Date: DateTime.Now;
                dst.CountryID = src.CountryID;
                dst.Policy_TypeID = src.Policy_TypeID;
                dst.Retaining_RiskID = src.Retaining_RiskID;
                dst.Exchange_RateID = src.Exchange_RateID.HasValue ? src.Exchange_RateID.Value : 1;
                dst.Start_Date = src.Start_Date;
                dst.End_Date = src.End_Date;
                dst.Group_Members = dst.Group_Members.HasValue ? dst.Group_Members.Value : 0;
                dst.Group_Total_Premium = dst.Group_Total_Premium.HasValue ? dst.Group_Total_Premium : 0;
                dst.Travel_NumberID = src.Travel_NumberID;
                dst.Travel_Insurance_TypeID = src.Travel_Insurance_TypeID;
                dst.Valid_Days = src.Valid_Days;
                dst.Total_Premium = src.Total_Premium.HasValue ? src.Total_Premium.Value : 0;
                dst.policy_additional_charge = null;
                dst.aspnetuser = null;
                dst.aspnetuser1 = null;
                dst.policy_insured = null;
                dst.travel_number = null;
                dst.travel_insurance_type = null;
                dst.retaining_risk = null;
                dst.country = null;
                dst.Policy_Number = src.Policy_Number;
                dst.policy_type = null;
                
            });

            Mapper.CreateMap<travel_policy, Policy>().AfterMap((src, dst) =>
            {
                dst.Policy_Number = src.Policy_Number;
                dst.PaymentStatys = src.Payment_Status == true ? 1 : 0;
                dst.Exchange_RateID = src.Exchange_RateID;
                dst.CountryID = src.CountryID;
                dst.Policy_TypeID = src.Policy_TypeID;
                dst.IsSamePolicyHolderInsured = src.Policy_HolderID == src.insured.ID;
                dst.Date_Created = src.Date_Created;
                dst.Created_By = src.Created_By;
                dst.Start_Date = src.Start_Date;
                dst.End_Date = src.End_Date;
                dst.Valid_Days = src.Valid_Days;
                dst.Travel_NumberID = src.Travel_NumberID;
                dst.Total_Premium = src.Total_Premium;
                dst.PolicyHolderId = src.Policy_HolderID;                
            });

            Mapper.CreateMap<FirstNoticeOfLossReportViewModel, first_notice_of_loss>().AfterMap((src, dst) =>
            {
                
                //dst.PolicyNumber = (int)src.policyNumber;
                //dst.Insured_User = db.aspnetusers.Where(x => x.UserName == src.username).Select(x => x.Id).First();
                //dst.Insured_person_transaction_number = src.TransactionAccount;
                //dst.Insured_person_deponent_bank = src.deponent;
                //dst.Claimant_person_name = src.insuredName;
                //dst.Claimant_person_embg = src.insuredEMBG;
                //dst.Claimant_person_address = src.insuredAddress;
                //dst.Claimant_person_number = src.insuredPhone;
                //dst.Claimant_person_transaction_number = src.insuredTransactionAccount;
                //dst.Claimant_person_deponent_bank = src.deponentInsured;
                //dst.Claimant_insured_relation = src.relationship;
                //dst.Land_trip = src.travelDestination;
                //dst.Trip_startdate = ((DateTime)src.travelDateFrom).Date;
                //dst.Trip_starttime = src.travelTimeFrom;
                //dst.Trip_enddate = ((DateTime)src.travelDateTo).Date;
                //dst.Trip_endtime = src.travelTimeTo;
                //dst.Type_transport_trip = src.transportationType;
                //dst.Additional_documents_handed = src.additionalDocumentsHanded;
                //dst.DateTime = DateTime.Now;
                //dst.AllCosts = src.valueExpenses;
                ////dst.LuggageInsurance_Y_N = src.LuggageInsurance;
                //dst.HealthInsurance_Y_N = src.HealthInsurance;
                dst.Web_Mobile = src.isMobile;
                dst.Short_Detailed = src.ShortDetailed;
                dst.health_insurance = null;
                dst.luggage_insurance = null;
            });

            Mapper.CreateMap<FirstNoticeOfLossReportViewModel, first_notice_of_loss>().AfterMap((src, dst) =>
            {

                dst.PolicyId = src.PolicyId;
                dst.ClaimantId = src.ClaimantId;
                dst.Relation_claimant_policy_holder = src.RelationClaimantPolicyHolder;
                dst.Policy_holder_bank_accountID = src.PolicyHolderForeignBankAccountId;
                dst.Claimant_bank_accountID = src.ClaimantForeignBankAccountId;
                dst.Destination = src.Destination;
                dst.Depart_Date_Time = src.DepartDateTime;
                dst.Arrival_Date_Time = src.ArrivalDateTime;
                dst.CreatedBy = src.CreatedBy;
                dst.CreatedDateTime = DateTime.Now;
                dst.Transport_means = src.TransportMeans;
                dst.Additional_infoID = src.AdditionalInfoId;
                dst.Total_cost = src.TotalCost;
                dst.Web_Mobile = src.isMobile;
                dst.ChatId = src.ChatId;
                dst.Short_Detailed = src.ShortDetailed;
                
            });

            //Mapper.CreateMap<first_notice_of_loss, FirstNoticeOfLossReportViewModel>().AfterMap((src, dst) =>
            //{
            //    var policy = src.travel_policy;
            //    var user = policy != null ? policy.insured : null;               
            //    var policy_holder_bank_account = src.Policy_holder_bank_account_info;
            //    var policy_holder_bank = src.Policy_holder_bank_account_info.bank;
            //    var claimant = src.insured;
            //    var claimant_bank_account = src.Claimant_bank_account_info;
            //    var claimant_bank = src.Claimant_bank_account_info.bank;               
            //    var additional_info = src.additional_info;
            //    var healthAdditionalInfo = src.additional_info.health_insurance_info;
            //    var luggageInsuranceInfo = src.additional_info.luggage_insurance_info;                           
            //    dst.PolicyId = src.PolicyId;
            //    dst.FNOLNumber = src.FNOL_Number;
            //    dst.PolicyNumber = policy != null ? Convert.ToInt32(policy.Policy_Number) : 0;
            //    dst.PolicyHolderId = src.travel_policy.insured.ID;
            //    dst.PolicyHolderName = user != null ? user.Name + " " + user.Lastname: null;
            //    dst.PolicyHolderAdress = user != null ? user.Address : null;
            //    dst.PolicyHolderPhoneNumber = user != null ? user.Phone_Number : null;
            //    dst.PolicyHolderSsn = user != null ? user.SSN : null;
            //    dst.PolicyHolderBankAccountNumber = user != null ? policy_holder_bank_account.Account_Number : null;
            //    dst.PolicyHolderBankName = policy_holder_bank != null ? policy_holder_bank.Name : null;
            //    dst.ClaimantId = src.ClaimantId;
            //    dst.ClaimantName = claimant != null ? claimant.Name + " " + claimant.Lastname : null;
            //    dst.ClaimantAdress = claimant != null ? claimant.Address : null;
            //    dst.ClaimantPhoneNumber = claimant != null ? claimant.Phone_Number : null;
            //    dst.ClaimantSsn = claimant != null ? claimant.SSN : null;
            //    dst.RelationClaimantPolicyHolder = src.Relation_claimant_policy_holder;
            //    dst.ClaimantBankAccountNumber = user != null ? claimant_bank_account.Account_Number : null;
            //    dst.ClaimantBankName = policy_holder_bank != null ? claimant_bank.Name : null;
            //    dst.Destination = src.Destination;
            //    dst.DepartDateTime = src.Depart_Date_Time;
            //    dst.DepartTime = src.Depart_Date_Time.TimeOfDay;
            //    dst.TransportMeans = src.Transport_means;
            //    dst.ArrivalDateTime = src.Arrival_Date_Time;
            //    dst.ArriveTime = src.Arrival_Date_Time.TimeOfDay;
            //    dst.IsHealthInsurance = healthAdditionalInfo != null ? true : false;
            //    dst.AccidentDateTimeHealth = additional_info != null ? (DateTime?)additional_info.Datetime_accident : null;
            //    dst.AccidentTimeHealth = additional_info != null ? (TimeSpan?)additional_info.Datetime_accident.TimeOfDay : null;
            //    dst.AccidentPlaceHealth = additional_info != null ? additional_info.Accident_place : null;
            //    dst.DoctorVisitDateTime = healthAdditionalInfo != null ? healthAdditionalInfo.Datetime_doctor_visit : null;
            //    dst.DoctorInfo = healthAdditionalInfo != null ? healthAdditionalInfo.Doctor_info : null;
            //    dst.MedicalCaseDescription = healthAdditionalInfo != null ? healthAdditionalInfo.Medical_case_description : null;
            //    var periousMedicalHistory = healthAdditionalInfo != null ? healthAdditionalInfo.Previous_medical_history : null;
            //    dst.PreviousMedicalHistory = periousMedicalHistory != null ? Convert.ToBoolean(periousMedicalHistory) : false;
            //    dst.ResponsibleInstitution = healthAdditionalInfo != null ? healthAdditionalInfo.Responsible_institution : null;
            //    dst.AccidentDateTimeLuggage = additional_info != null ? (DateTime?)additional_info.Datetime_accident : null;
            //    dst.AccidentPlaceLuggage = additional_info != null ? additional_info.Accident_place : null;
            //    dst.PlaceDescription = luggageInsuranceInfo != null ? luggageInsuranceInfo.Place_description : null;
            //    dst.DetailDescription = luggageInsuranceInfo != null ? luggageInsuranceInfo.Detail_description : null;
            //    dst.ReportPlace = luggageInsuranceInfo != null ? luggageInsuranceInfo.Report_place : null;
            //    dst.Floaters = luggageInsuranceInfo != null ? luggageInsuranceInfo.Floaters : null;
            //    dst.FloatersValue = luggageInsuranceInfo != null ? luggageInsuranceInfo.Floaters_value.ToString() : null;
            //    dst.AccidentTimeLuggage = additional_info != null ? (TimeSpan?)additional_info.Datetime_accident.TimeOfDay : null;
            //    dst.LugaggeCheckingTime = luggageInsuranceInfo != null ? (TimeSpan?)luggageInsuranceInfo.Luggage_checking_Time : null;
            //    dst.CreatedBy = src.CreatedBy;
            //    dst.CreatedDateTime = DateTime.Now;
            //    dst.TotalCost = src.Total_cost;

            //});

            //Mapper.CreateMap<first_notice_of_loss_archive, FirstNoticeOfLossReportViewModel>().AfterMap((src, dst) =>
            //{
            //    var policy = src.travel_policy;
            //    var user = policy != null ? policy.insured : null;
            //    var policy_holder_bank_account = src.Policy_holder_bank_account_info;
            //    var policy_holder_bank = src.Policy_Holder_BankAccountI;
            //    var claimant = src.insured;
            //    var claimant_bank_account = src.Claimant_bank_account_info;
            //    var claimant_bank = src.Claimant_bank_account_info.bank;
            //    var additional_info = src.additional_info;
            //    var healthAdditionalInfo = src.additional_info.health_insurance_info;
            //    var luggageInsuranceInfo = src.additional_info.luggage_insurance_info;
            //    dst.PolicyId = src.PolicyId;
            //    dst.FNOLNumber = src.FNOL_Number;
            //    dst.PolicyNumber = policy != null ? Convert.ToInt32(policy.Policy_Number) : 0;
            //    dst.PolicyHolderId = src.travel_policy.insured.ID;
            //    dst.PolicyHolderName = user != null ? user.Name + " " + user.Lastname : null;
            //    dst.PolicyHolderAdress = user != null ? user.Address : null;
            //    dst.PolicyHolderPhoneNumber = user != null ? user.Phone_Number : null;
            //    dst.PolicyHolderSsn = user != null ? user.SSN : null;
            //    dst.PolicyHolderBankAccountNumber = user != null ? policy_holder_bank_account.Account_Number : null;
            //    dst.PolicyHolderBankName = policy_holder_bank != null ? policy_holder_bank.Name : null;
            //    dst.ClaimantId = src.ClaimantId;
            //    dst.ClaimantName = claimant != null ? claimant.Name + " " + claimant.Lastname : null;
            //    dst.ClaimantAdress = claimant != null ? claimant.Address : null;
            //    dst.ClaimantPhoneNumber = claimant != null ? claimant.Phone_Number : null;
            //    dst.ClaimantSsn = claimant != null ? claimant.SSN : null;
            //    dst.RelationClaimantPolicyHolder = src.Relation;
            //    dst.ClaimantBankAccountNumber = user != null ? claimant_bank_account.Account_Number : null;
            //    dst.ClaimantBankName = policy_holder_bank != null ? claimant_bank.Name : null;
            //    dst.Destination = src.Destination;
            //    dst.DepartDateTime = src.Depart_Date_Time;
            //    dst.DepartTime = src.Depart_Date_Time.TimeOfDay;
            //    dst.TransportMeans = src.Transport_means;
            //    dst.ArrivalDateTime = src.Arrival_Date_Time;
            //    dst.ArriveTime = src.Arrival_Date_Time.TimeOfDay;
            //    dst.IsHealthInsurance = healthAdditionalInfo != null ? true : false;
            //    dst.AccidentDateTimeHealth = additional_info != null ? (DateTime?)additional_info.Datetime_accident : null;
            //    dst.AccidentTimeHealth = additional_info != null ? (TimeSpan?)additional_info.Datetime_accident.TimeOfDay : null;
            //    dst.AccidentPlaceHealth = additional_info != null ? additional_info.Accident_place : null;
            //    dst.DoctorVisitDateTime = healthAdditionalInfo != null ? healthAdditionalInfo.Datetime_doctor_visit : null;
            //    dst.DoctorInfo = healthAdditionalInfo != null ? healthAdditionalInfo.Doctor_info : null;
            //    dst.MedicalCaseDescription = healthAdditionalInfo != null ? healthAdditionalInfo.Medical_case_description : null;
            //    var periousMedicalHistory = healthAdditionalInfo != null ? healthAdditionalInfo.Previous_medical_history : null;
            //    dst.PreviousMedicalHistory = periousMedicalHistory != null ? Convert.ToBoolean(periousMedicalHistory) : false;
            //    dst.ResponsibleInstitution = healthAdditionalInfo != null ? healthAdditionalInfo.Responsible_institution : null;
            //    dst.AccidentDateTimeLuggage = additional_info != null ? (DateTime?)additional_info.Datetime_accident : null;
            //    dst.AccidentPlaceLuggage = additional_info != null ? additional_info.Accident_place : null;
            //    dst.PlaceDescription = luggageInsuranceInfo != null ? luggageInsuranceInfo.Place_description : null;
            //    dst.DetailDescription = luggageInsuranceInfo != null ? luggageInsuranceInfo.Detail_description : null;
            //    dst.ReportPlace = luggageInsuranceInfo != null ? luggageInsuranceInfo.Report_place : null;
            //    dst.Floaters = luggageInsuranceInfo != null ? luggageInsuranceInfo.Floaters : null;
            //    dst.FloatersValue = luggageInsuranceInfo != null ? luggageInsuranceInfo.Floaters_value.ToString() : null;
            //    dst.AccidentTimeLuggage = additional_info != null ? (TimeSpan?)additional_info.Datetime_accident.TimeOfDay : null;
            //    dst.LugaggeCheckingTime = luggageInsuranceInfo != null ? (TimeSpan?)luggageInsuranceInfo.Luggage_checking_Time : null;
            //    dst.CreatedBy = src.CreatedBy;
            //    dst.CreatedDateTime = DateTime.Now;
            //    dst.TotalCost = src.Total_cost;

            //});

            Mapper.CreateMap<InsuredTravelingEntity2, PolicyInfoList>().AfterMap((src, dst) =>
            {
                dst.countries =(IQueryable <country>) src.countries;
                dst.franchises = (IQueryable<retaining_risk>)src.retaining_risk;
                dst.additional_charges =(IQueryable<additional_charge>) src.additional_charge;
                dst.policies =(IQueryable<policy_type>) src.policy_type;
            });

            Mapper.CreateMap<travel_policy, SearchPolicyViewModel>().AfterMap((src, dst) =>
            {
                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && dateTime.Contains("yy") && !dateTime.Contains("yyyy") ? dateTime.Replace("yy", "yyyy") : dateTime;
                dst.CountryId = src.CountryID;    
                dst.InsuredName = src.policy_insured.Count() == 0 ? " " : src.policy_insured.FirstOrDefault().insured.Lastname + " " + src.policy_insured.FirstOrDefault().insured.Name;
                dst.Polisa_Id = src.ID;
                dst.Polisa_Broj = src.Policy_Number;
                dst.Policy_type = src.policy_type.type;
                dst.Zapocnuva_Na = src.Start_Date.ToString(dateTimeFormat, new CultureInfo("en-US"));
                dst.Zavrsuva_Na = src.End_Date.ToString(dateTimeFormat, new CultureInfo("en-US"));
                dst.Datum_Na_Izdavanje = src.Date_Created.ToString(dateTimeFormat, new CultureInfo("en-US"));
                dst.Datum_Na_Storniranje = src.Date_Cancellation.HasValue ? src.Date_Cancellation.Value.Date.ToShortDateString().ToString() : "/";
            });

            Mapper.CreateMap<sava_policy, SearchSavaPolicyModel>().AfterMap((src, dst) =>
            {
                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && dateTime.Contains("yy") && !dateTime.Contains("yyyy") ? dateTime.Replace("yy", "yyyy") : dateTime;
                dst.PolicyId = src.id.ToString();
                dst.PolicyNumber = src.policy_number.ToString();
                dst.SSNInsured = src.SSN_insured;
                dst.SSNHolder = src.SSN_policyHolder;
                dst.ExpireDate = src.expiry_date.HasValue? src.expiry_date.Value.ToString(dateTimeFormat, new CultureInfo("en-US")) : "/";
                dst.Premium = src.premium.ToString();
                dst.Points = src.discount_points.ToString();
                dst.EmailSeller = src.email_seller;
            });

            Mapper.CreateMap<first_notice_of_loss, SearchFNOLViewModel>().AfterMap((src, dst) =>
            {
                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && dateTime.Contains("yy") && !dateTime.Contains("yyyy") ? dateTime.Replace("yy", "yyyy") : dateTime;
                dst.ID = src.ID;
                var policy = src.travel_policy;
                var healthInsurance = src.additional_info.health_insurance_info;
                var luggageInsurance = src.additional_info.luggage_insurance_info;
                var user = policy != null ? policy.insured : null;
                dst.PolicyNumber = policy != null ? policy.Policy_Number : null;
                dst.FNOLNumber = src.FNOL_Number;
                dst.InsuredName = user != null ? user.Name + " " + user.Lastname : null;
                dst.ClaimantPersonName = src.insured != null ? src.insured.Name + " " + src.insured.Lastname : null;
                dst.Claimant_insured_relation = src.Relation_claimant_policy_holder;
                dst.AllCosts = src.Total_cost.ToString();
                dst.Date = src.additional_info != null ? src.additional_info.Datetime_accident.ToString(dateTimeFormat, new CultureInfo("en-US")) : null;
                dst.HealthInsurance = healthInsurance != null ? Resource.Yes : Resource.No;
                dst.LuggageInsurance = luggageInsurance != null ? Resource.Yes : Resource.No;
            });

            Mapper.CreateMap<first_notice_of_loss_archive, SearchFNOLViewModel>().AfterMap((src, dst) =>
            {
                var dateTime = ConfigurationManager.AppSettings["DateFormat"];
                var dateTimeFormat = dateTime != null && dateTime.Contains("yy") && !dateTime.Contains("yyyy") ? dateTime.Replace("yy", "yyyy") : dateTime;

                dst.ID = src.ID;
                var policyId = src.PolicyId;
                var policy = src.travel_policy;
                var healthInsurance = src.additional_info.health_insurance_info;
                var luggageInsurance = src.additional_info.luggage_insurance_info;
                var user = policy != null ? policy.insured : null;
                dst.PolicyNumber = policy != null ? policy.Policy_Number : null;
                dst.FNOLNumber = src.FNOL_Number;
                dst.InsuredName = user != null ? user.Name + " " + user.Lastname : null;
                dst.ClaimantPersonName = src.insured != null ? src.insured.Name + " " + src.insured.Lastname : null;
                dst.Claimant_insured_relation = src.Relation;
                dst.AllCosts = src.Total_cost.ToString();
                dst.Date = src.additional_info != null ? src.additional_info.Datetime_accident.ToString(dateTimeFormat, new CultureInfo("en-US")) : null;
                dst.HealthInsurance = healthInsurance != null ? Resource.Yes : Resource.No;
                dst.LuggageInsurance = luggageInsurance != null ? Resource.Yes : Resource.No;
            });

            Mapper.CreateMap<insured, SearchClientsViewModel>().AfterMap((src, dst) =>
            {
                dst.ID = src.ID;
                dst.Name = src.Name;
                dst.Lastname = src.Lastname;
                dst.SSN = src.SSN;
                dst.Address = src.Address;
                dst.Passport_Number_IdNumber = src.Passport_Number_IdNumber;
                dst.Phone_Number = src.Phone_Number;
                dst.Postal_Code = src.Postal_Code;
                dst.City = src.City;
                dst.Email = src.Email;
            });

            Mapper.CreateMap<travel_policy, PolicyAutoCompleteViewModel>().AfterMap((src, dst) =>
            {
                dst.Id = src.ID;
                dst.Name = src.Policy_Number;
            });

            Mapper.CreateMap<CalculatePremiumViewModel, Policy>().AfterMap((src, dst) =>
            {
                dst.CountryID = src.CountryID;
                dst.Group_Members = src.Group_Members;
                dst.Policy_TypeID = src.Policy_TypeID;
                dst.Retaining_RiskID = src.Retaining_RiskID;
                dst.Start_Date = src.Start_Date;
                dst.Valid_Days = src.Valid_Days;
                dst.Name = src.Policy_Holder != null? src.Policy_Holder.Name : "";
                dst.LastName = src.Policy_Holder != null ? src.Policy_Holder.Lastname : "";
                dst.SSN = src.Policy_Holder != null ? src.Policy_Holder.SSN : "";
                dst.insureds = src.Insureds;
                //dst.additional_charges = src.Additional_charges;
                foreach (var charge in src.additional_charges)
                {
                    var additionalCharge = db.additional_charge.Where(x => x.ID == charge).FirstOrDefault();
                    if (additionalCharge != null)
                        dst.additional_charges.Add(additionalCharge);
                }
            });
        }
    }
}