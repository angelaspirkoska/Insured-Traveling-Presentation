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
    
    public partial class discount_days
    {
        public int ID { get; set; }
        public int Policy_typeID { get; set; }
        public Nullable<int> Travel_durationID { get; set; }
        public Nullable<double> Discount { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public string Modified_Date { get; set; }
        public string Modified_By { get; set; }
        public int Version { get; set; }
    
        public virtual policy_type policy_type { get; set; }
        public virtual travel_duration travel_duration { get; set; }
        public virtual aspnetuser aspnetuser { get; set; }
        public virtual aspnetuser aspnetuser1 { get; set; }
    }
}
