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
    
    public partial class health_insurance
    {
        public int ID { get; set; }
        public System.DateTime Date_of_accsident { get; set; }
        public string Place_of_accsident { get; set; }
        public Nullable<System.DateTime> First_contact_with_doctor { get; set; }
        public System.TimeSpan Time_of_accsident { get; set; }
        public string Doctor_data { get; set; }
        public string Disease_description { get; set; }
        public string Documents_proof { get; set; }
        public string Additional_info { get; set; }
        public Nullable<int> LossID { get; set; }
    
        public virtual first_notice_of_loss first_notice_of_loss { get; set; }
    }
}
