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
    
    public partial class health_insurance_info
    {
        public int Additional_infoId { get; set; }
        public Nullable<System.DateTime> Datetime_doctor_visit { get; set; }
        public string Doctor_info { get; set; }
        public string Medical_case_description { get; set; }
        public Nullable<bool> Previous_medical_history { get; set; }
        public string Responsible_institution { get; set; }
    
        public virtual additional_info additional_info { get; set; }
    }
}