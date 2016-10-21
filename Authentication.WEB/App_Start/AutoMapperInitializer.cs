using AutoMapper;
using System.Linq;
using InsuredTraveling.Models;
using System;
using InsuredTraveling.DI;
using Autofac;
using InsuredTraveling.ViewModels;

namespace InsuredTraveling.App_Start
{
    public class AutoMapperInitializer
    {

        public static void Initialize()
        {
            InsuredTravelingEntity db = new InsuredTravelingEntity();

            Mapper.CreateMap<CreateClientModel , insured>().AfterMap((src, dst) =>
            {
                dst.Name = src.Name;
                dst.Lastname = src.LastName;
                dst.Email = src.Email;
                dst.DateBirth = src.DateBirth.Date;
                dst.Address = src.Address;
                dst.City = src.City;
                dst.SSN = src.SSN;
                dst.Postal_Code = src.Postal_Code;
                dst.Phone_Number = src.PhoneNumber;
                dst.Passport_Number_IdNumber = src.Passport_Number_IdNumber;
                dst.Created_By = db.aspnetusers.Where(x => x.UserName == System.Web.HttpContext.Current.User.Identity.Name).Single().Id;
                dst.Date_Created = DateTime.Now.Date;
                dst.type_insured = null;
                dst.aspnetuser = null;
                dst.aspnetuser1 = null;
                dst.Type_InsuredID = null;
                               
            });

            Mapper.CreateMap<Policy, travel_policy>().AfterMap((src, dst) =>
            {
                dst.Created_By = src.Created_By;
                dst.Date_Created = (src.Date_Created.HasValue) ? src.Date_Created.Value.Date: DateTime.Now;
                dst.CountryID = src.CountryID;
                dst.Policy_TypeID = src.Policy_TypeID;
                dst.Retaining_RiskID = src.Retaining_RiskID;
                dst.Exchange_RateID = (src.Exchange_RateID.HasValue) ? src.Exchange_RateID.Value : 1;
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
                dst.Web_Mobile = src.WebMobile;
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
                dst.Web_Mobile = src.WebMobile;
                dst.Message = src.Message;
                dst.Short_Detailed = src.ShortDetailed;
                
            });

            Mapper.CreateMap<InsuredTravelingEntity, PolicyInfoList>().AfterMap((src, dst) =>
            {
                dst.countries =(IQueryable <country>) src.countries;
                dst.franchises = (IQueryable<retaining_risk>)src.retaining_risk;
                dst.additional_charges =(IQueryable<additional_charge>) src.additional_charge;
                dst.policies =(IQueryable<policy_type>) src.policy_type;
            });

            Mapper.CreateMap<travel_policy, SearchPolicyViewModel>().AfterMap((src, dst) =>
            {
                dst.Polisa_Id = src.ID;
                dst.Polisa_Broj = src.Policy_Number;
                dst.Country = src.country.Name;
                dst.Policy_type = src.policy_type.type;
                dst.Zapocnuva_Na = src.Start_Date.Date.ToShortDateString();
                dst.Zavrsuva_Na = src.End_Date.Date.ToShortDateString();
                dst.Datum_Na_Izdavanje = src.Date_Created.Date.ToShortDateString();
                dst.Datum_Na_Storniranje = src.Date_Cancellation.HasValue ? src.Date_Cancellation.Value.Date.ToShortDateString().ToString() : "/";

            });

            Mapper.CreateMap<first_notice_of_loss, SearchFNOLViewModel>().AfterMap((src, dst) =>
            {
                dst.ID = src.ID;
                var policy = db.travel_policy.Where(x => x.ID == src.PolicyId).FirstOrDefault();
                var healthInsurance = db.health_insurance_info.SingleOrDefault(x => x.additional_info.ID == src.Additional_infoID);
                var luggageInsurance = db.luggage_insurance_info.SingleOrDefault(x => x.additional_info.ID == src.Additional_infoID);
                var user = policy != null ? policy.insured : null;
                dst.PolicyNumber = policy != null ? policy.Policy_Number : null;
                dst.InsuredName = user != null ? user.Name + " " + user.Lastname : null;
                dst.ClaimantPersonName = src.insured != null ? src.insured.Name + " " + src.insured.Lastname : null;
                dst.Claimant_insured_relation = src.Relation_claimant_policy_holder;
                dst.AllCosts = src.Total_cost.ToString();
                dst.Date = src.additional_info != null ? src.additional_info.Datetime_accident.Date.ToShortDateString().ToString() : null;
                dst.HealthInsurance = healthInsurance != null ? "Da" : "Ne";
                dst.LuggageInsurance = luggageInsurance != null ? "Da" : "Ne";
            });
        }
    }
}