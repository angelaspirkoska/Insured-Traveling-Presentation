
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
    
public partial class first_notice_of_loss_archive
{

    public int ID { get; set; }

    public int fnol_ID { get; set; }

    public System.DateTime Created_Datetime { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<int> ChatID { get; set; }

    public Nullable<bool> Short_Detailed { get; set; }

    public Nullable<bool> Web_Mobile { get; set; }

    public float Total_cost { get; set; }

    public string Transport_means { get; set; }

    public System.DateTime Arrival_Date_Time { get; set; }

    public System.DateTime Depart_Date_Time { get; set; }

    public string Destination { get; set; }

    public int PolicyId { get; set; }

    public string Relation { get; set; }

    public string Policy_Holder_Name { get; set; }

    public string Policy_Holder_Last_Name { get; set; }

    public int Policy_HolderId { get; set; }

    public string Policy_Holder_Address { get; set; }

    public string Policy_Holder_Phone { get; set; }

    public string Policy_Holder_Ssn { get; set; }

    public int Policy_Holder_BankAccountId { get; set; }

    public int ClaimantId { get; set; }

    public string Claimant_Name { get; set; }

    public string Claimant_Last_Name { get; set; }

    public string Claimant_Address { get; set; }

    public string Claimant_Phone { get; set; }

    public int Claimant_BankAccountId { get; set; }

    public int Additional_infoId { get; set; }

    public System.DateTime Additional_info_datetime { get; set; }

    public string Additional_info_accident_place { get; set; }

    public string luggage_place_description { get; set; }

    public string luggage_detail_description { get; set; }

    public string luggage_report_place { get; set; }

    public string luggage_floaters { get; set; }

    public Nullable<float> luggage_floaters_value { get; set; }

    public Nullable<System.DateTime> health_datetime_doctor_visit { get; set; }

    public string health_doctor_info { get; set; }

    public string health_medical_case_description { get; set; }

    public string health_responsible_institution { get; set; }

    public Nullable<bool> health_previous_medical_history { get; set; }

    public string ModifiedBy { get; set; }

    public System.DateTime Modified_Datetime { get; set; }

    public string Claimant_Ssn { get; set; }

    public Nullable<System.TimeSpan> luggage_checking_time { get; set; }

    public string FNOL_Number { get; set; }



    public virtual first_notice_of_loss first_notice_of_loss { get; set; }

    public virtual additional_info additional_info { get; set; }

    public virtual travel_policy travel_policy { get; set; }

    public virtual insured insured { get; set; }

    public virtual bank_account_info Policy_holder_bank_account_info { get; set; }

    public virtual bank_account_info Claimant_bank_account_info { get; set; }

    public virtual aspnetuser aspnetuser { get; set; }

}

}
