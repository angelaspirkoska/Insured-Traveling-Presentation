using AutoMapper;
using System.Linq;
using InsuredTraveling.Models;
using System;

namespace InsuredTraveling.App_Start
{
    public class AutoMapperInitializer
    {
        public static void Initialize()
        {
            InsuredTravelingEntity db = new InsuredTravelingEntity();

            Mapper.CreateMap<Example, Example2>().AfterMap((src, dst) => {
                dst.Name2 = src.Name;
            });

            Mapper.CreateMap<FNOL, first_notice_of_loss>().AfterMap((src, dst) =>
            {
                dst.PolicyNumber = (int)src.policyNumber;
                dst.Insured_User = db.aspnetusers.Where(x => x.UserName == src.username).Select(x => x.Id).Single();
                dst.Insured_person_transaction_number = src.TransactionAccount;
                dst.Insured_person_deponent_bank = src.deponent;
                dst.Claimant_person_name = src.insuredName;
                dst.Claimant_person_embg = src.insuredEMBG;
                dst.Claimant_person_address = src.insuredAddress;
                dst.Claimant_person_number = src.insuredPhone;
                dst.Claimant_person_transaction_number = src.insuredTransactionAccount;
                dst.Claimant_person_deponent_bank = src.deponentInsured;
                dst.Claimant_insured_relation = src.relationship;
                dst.Land_trip = src.travelDestination;
                dst.Trip_startdate = ((DateTime)src.travelDateFrom).Date;
                dst.Trip_starttime = src.travelTimeFrom;
                dst.Trip_enddate = ((DateTime)src.travelDateTo).Date;
                dst.Trip_endtime = src.travelTimeTo;
                dst.Type_transport_trip = src.transportationType;
                dst.Additional_documents_handed = src.additionalDocumentsHanded;
                dst.DateTime = DateTime.Now;
                dst.AllCosts = src.valueExpenses;
                dst.LuggageInsurance_Y_N = src.LuggageInsurance;
                dst.HealthInsurance_Y_N = src.HealthInsurance;
                dst.Web_Mobile = src.WebMobile;
                dst.Short_Detailed = src.ShortDetailed;
                dst.PolicyType = src.PolicyType;
                dst.Message = src.message;
            });

            //Mapper.Initialize(csrcg =>
            //{
            //    csrcg.CreateMap<FNOL, first_notice_of_loss>().AfterMap((src, dst) =>
            //   {
            //       dst.PolicyNumber = (int)src.policyNumber;
            //       dst.Insured_User = db.aspnetusers.Where(x => x.UserName == src.username).Select(x => x.Id).Single();
            //       dst.Insured_person_transaction_number = src.TransactionAccount;
            //       dst.Insured_person_deponent_bank = src.deponent;
            //       dst.Claimant_person_name = src.insuredName;
            //       dst.Claimant_person_embg = src.insuredEMBG;
            //       dst.Claimant_person_address = src.insuredAddress;
            //       dst.Claimant_person_number = src.insuredPhone;
            //       dst.Claimant_person_transaction_number = src.insuredTransactionAccount;
            //       dst.Claimant_person_deponent_bank = src.deponentInsured;
            //       dst.Claimant_insured_relation = src.relationship;
            //       dst.Land_trip = src.travelDestination;
            //       dst.Trip_startdate = ((DateTime)src.travelDateFrom).Date;
            //       dst.Trip_starttime = src.travelTimeFrom;
            //       dst.Trip_enddate = ((DateTime)src.travelDateTo).Date;
            //       dst.Trip_endtime = src.travelTimeTo;
            //       dst.Type_transport_trip = src.transportationType;
            //       dst.Additional_documents_handed = src.additionalDocumentsHanded;
            //       dst.DateTime = DateTime.Now;
            //       dst.AllCosts = src.valueExpenses;
            //       dst.LuggageInsurance_Y_N = src.LuggageInsurance;
            //       dst.HealthInsurance_Y_N = src.HealthInsurance;
            //       dst.Web_Mobile = src.WebMobile;
            //       dst.Short_Detailed = src.ShortDetailed;
            //   });
            //});
        }
    }
}