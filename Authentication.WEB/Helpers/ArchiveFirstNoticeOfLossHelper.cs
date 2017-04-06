using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.DI;

namespace InsuredTraveling.Helpers
{
    public static class ArchiveFirstNoticeOfLossHelper
    {
        public static bool ArchiveFirstNoticeOfLoss(first_notice_of_loss firstNoticeOfLoss, 
                                                    string modifiedBy, 
                                                    IFirstNoticeOfLossService _fnol,
                                                    IFirstNoticeOfLossArchiveService _firstNoticeLossArchive)
        {
            first_notice_of_loss_archive firstNoticeOfLossArchive = new first_notice_of_loss_archive();

            firstNoticeOfLossArchive.fnol_ID = firstNoticeOfLoss.ID;
            firstNoticeOfLossArchive.ModifiedBy = modifiedBy;
            firstNoticeOfLossArchive.Modified_Datetime = DateTime.Now;
            firstNoticeOfLossArchive.CreatedBy = firstNoticeOfLoss.CreatedBy;
            firstNoticeOfLossArchive.Created_Datetime = firstNoticeOfLoss.CreatedDateTime;
            firstNoticeOfLossArchive.ChatID = firstNoticeOfLoss.ChatId;
            firstNoticeOfLossArchive.Short_Detailed = firstNoticeOfLoss.Short_Detailed;
            firstNoticeOfLossArchive.Web_Mobile = firstNoticeOfLoss.Web_Mobile;
            firstNoticeOfLossArchive.Total_cost = firstNoticeOfLoss.Total_cost;
            firstNoticeOfLossArchive.Transport_means = firstNoticeOfLoss.Transport_means;
            firstNoticeOfLossArchive.Arrival_Date_Time = firstNoticeOfLoss.Arrival_Date_Time;
            firstNoticeOfLossArchive.Depart_Date_Time = firstNoticeOfLoss.Depart_Date_Time;
            firstNoticeOfLossArchive.Destination = firstNoticeOfLoss.Destination;
            firstNoticeOfLossArchive.PolicyId = firstNoticeOfLoss.PolicyId;
            firstNoticeOfLossArchive.Relation = firstNoticeOfLoss.Relation_claimant_policy_holder;
            firstNoticeOfLossArchive.Policy_Holder_Name = firstNoticeOfLoss.travel_policy.insured.Name;
            firstNoticeOfLossArchive.Policy_Holder_Last_Name = firstNoticeOfLoss.travel_policy.insured.Lastname;
            firstNoticeOfLossArchive.Policy_HolderId = firstNoticeOfLoss.travel_policy.insured.ID;
            firstNoticeOfLossArchive.Policy_Holder_Address = firstNoticeOfLoss.travel_policy.insured.Address + " " + firstNoticeOfLoss.travel_policy.insured.City +" "+ firstNoticeOfLoss.travel_policy.insured.Postal_Code;
            firstNoticeOfLossArchive.Policy_Holder_Phone = firstNoticeOfLoss.travel_policy.insured.Phone_Number;
            firstNoticeOfLossArchive.Policy_Holder_Ssn = firstNoticeOfLoss.travel_policy.insured.SSN;
            firstNoticeOfLossArchive.Policy_Holder_BankAccountId = firstNoticeOfLoss.Policy_holder_bank_accountID;
            firstNoticeOfLossArchive.ClaimantId = firstNoticeOfLoss.insured.ID;
            firstNoticeOfLossArchive.Claimant_BankAccountId = firstNoticeOfLoss.Claimant_bank_accountID;
            firstNoticeOfLossArchive.Claimant_Address = firstNoticeOfLoss.insured.Address + " " + firstNoticeOfLoss.insured.City + " " + firstNoticeOfLoss.insured.Postal_Code;
            firstNoticeOfLossArchive.Claimant_Last_Name = firstNoticeOfLoss.insured.Lastname;
            firstNoticeOfLossArchive.Claimant_Name = firstNoticeOfLoss.insured.Name;
            firstNoticeOfLossArchive.Claimant_Phone = firstNoticeOfLoss.insured.Phone_Number;
            firstNoticeOfLossArchive.Claimant_Ssn = firstNoticeOfLoss.insured.SSN;
            firstNoticeOfLossArchive.Additional_infoId = firstNoticeOfLoss.Additional_infoID;
            firstNoticeOfLossArchive.Additional_info_datetime = firstNoticeOfLoss.additional_info.Datetime_accident;
            firstNoticeOfLossArchive.Additional_info_accident_place = firstNoticeOfLoss.additional_info.Accident_place;
            firstNoticeOfLossArchive.FNOL_Number = firstNoticeOfLoss.FNOL_Number;

            if(firstNoticeOfLoss.additional_info.luggage_insurance_info!=null)
            {
                firstNoticeOfLossArchive.luggage_place_description = firstNoticeOfLoss.additional_info.luggage_insurance_info.Place_description;
                firstNoticeOfLossArchive.luggage_detail_description = firstNoticeOfLoss.additional_info.luggage_insurance_info.Detail_description;
                firstNoticeOfLossArchive.luggage_report_place = firstNoticeOfLoss.additional_info.luggage_insurance_info.Report_place;
                firstNoticeOfLossArchive.luggage_floaters = firstNoticeOfLoss.additional_info.luggage_insurance_info.Floaters;
                firstNoticeOfLossArchive.luggage_floaters_value = firstNoticeOfLoss.additional_info.luggage_insurance_info.Floaters_value;
                firstNoticeOfLossArchive.luggage_checking_time = firstNoticeOfLoss.additional_info.luggage_insurance_info.Luggage_checking_Time;
            } else if(firstNoticeOfLoss.additional_info.health_insurance_info != null)
            {
                firstNoticeOfLossArchive.health_datetime_doctor_visit = firstNoticeOfLoss.additional_info.health_insurance_info.Datetime_doctor_visit;
                firstNoticeOfLossArchive.health_doctor_info = firstNoticeOfLoss.additional_info.health_insurance_info.Doctor_info;
                firstNoticeOfLossArchive.health_medical_case_description = firstNoticeOfLoss.additional_info.health_insurance_info.Medical_case_description;
                firstNoticeOfLossArchive.health_responsible_institution = firstNoticeOfLoss.additional_info.health_insurance_info.Responsible_institution;
                firstNoticeOfLossArchive.health_previous_medical_history = firstNoticeOfLoss.additional_info.health_insurance_info.Previous_medical_history;
            }

            var result = _firstNoticeLossArchive.Archive(firstNoticeOfLossArchive);

            if (result > 0)
            {
                return true;
            }
            else return false;
        }
    }
}