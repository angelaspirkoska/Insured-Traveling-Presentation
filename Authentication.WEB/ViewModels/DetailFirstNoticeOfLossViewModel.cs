using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    public class DetailFirstNoticeOfLossViewModel
    {
        public string Policy_Number { get; set; }
        public int PolicyId { get; set; }
        public int ClaimantId { get; set; }
        public string RelationClaimantPolicyHolder { get; set; }
        public int PolicyHolder_Account_HolderID { get; set; }
        public int Claimant_Account_HolderID { get; set; }
        public string Destination { get; set; }
        public DateTime Depart_Date_Time { get; set; }
        public DateTime Arrival_Date_Time { get; set; }
        public string Transport_means { get; set; }
        public float Total_cost { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime Datetime_accident { get; set; }
        public string Accident_place { get; set; }
        public string HealthInsurance_Y_N { get; set; }
        public string LuggageInsurance_Y_N { get; set; }

        //attributes for luggage insurance
        public string Place_description { get; set; }
        public string Detail_description { get; set; }
        public string Report_place { get; set; }
        public string Floaters { get; set; }
        public string Floaters_value { get; set; }
        public TimeSpan? Luggage_checking_Time { get; set; }

        //attributes for health insurance
        public DateTime Datetime_doctor_visit { get; set; }
        public string Doctor_info { get; set; }
        public string Medical_case_description { get; set; }
        public bool Previous_medical_history { get; set; }
        public string Responsible_institution { get; set;}

    }
}