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
    
    public partial class travel_policy_log
    {
        public int ID { get; set; }
        public string Policy_Number { get; set; }
        public int Policy_HolderID { get; set; }
        public int Exchange_RateID { get; set; }
        public int CountryID { get; set; }
        public int Policy_TypeID { get; set; }
        public int Retaining_RiskID { get; set; }
        public string Franchise_Age { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }
        public int Valid_Days { get; set; }
        public string Franchise_Days { get; set; }
        public int Travel_NumberID { get; set; }
        public int Travel_Insurance_TypeID { get; set; }
        public int Group_Members { get; set; }
        public double Group_Total_Premium { get; set; }
        public int Discount { get; set; }
        public double Total_Premium { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Date_Created { get; set; }
        public bool Modified { get; set; }
        public string Modified_By { get; set; }
        public System.DateTime Date_modified { get; set; }
        public string IPaddress { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public System.DateTime Date_Cancellation { get; set; }
        public bool Payment_Status { get; set; }
    }
}
