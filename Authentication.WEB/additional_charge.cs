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
    
    public partial class additional_charge
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public additional_charge()
        {
            this.policy_additional_charge = new HashSet<policy_additional_charge>();
        }
    
        public int ID { get; set; }
        public string Doplatok { get; set; }
        public Nullable<double> Percentage { get; set; }
        public string Surcharge { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Modified_Date { get; set; }
        public string Modified_By { get; set; }
        public int Version { get; set; }
    
        public virtual aspnetuser aspnetuser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<policy_additional_charge> policy_additional_charge { get; set; }
        public virtual aspnetuser aspnetuser1 { get; set; }
    }
}
