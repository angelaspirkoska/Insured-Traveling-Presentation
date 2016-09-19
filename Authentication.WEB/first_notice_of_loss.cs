//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InsuredTraveling
{
    using System;
    using System.Collections.Generic;
    
    public partial class first_notice_of_loss
    {
        public int LossID { get; set; }
        public long PolicyNumber { get; set; }
        public string PolicyType { get; set; }
        public string Insured_User { get; set; }
        public string Insured_person_transaction_number { get; set; }
        public string Insured_person_deponent_bank { get; set; }
        public string Claimant_person_name { get; set; }
        public string Claimant_person_address { get; set; }
        public string Claimant_person_number { get; set; }
        public string Claimant_person_embg { get; set; }
        public string Claimant_person_transaction_number { get; set; }
        public string Claimant_person_deponent_bank { get; set; }
        public string Claimant_insured_relation { get; set; }
        public string Land_trip { get; set; }
        public Nullable<System.DateTime> Trip_startdate { get; set; }
        public Nullable<System.TimeSpan> Trip_starttime { get; set; }
        public Nullable<System.DateTime> Trip_enddate { get; set; }
        public Nullable<System.TimeSpan> Trip_endtime { get; set; }
        public string Type_transport_trip { get; set; }
        public string Additional_documents_handed { get; set; }
        public Nullable<int> AllCosts { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<bool> HealthInsurance_Y_N { get; set; }
        public Nullable<bool> LuggageInsurance_Y_N { get; set; }
        public Nullable<bool> Web_Mobile { get; set; }
        public Nullable<bool> Short_Detailed { get; set; }
        public string Message { get; set; }
    
        public virtual health_insurance health_insurance { get; set; }
        public virtual luggage_insurance luggage_insurance { get; set; }
    }
}
