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
    
    public partial class config_policy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public config_policy()
        {
            this.config_policy_values = new HashSet<config_policy_values>();
            this.config_insured_policy = new HashSet<config_insured_policy>();
        }
    
        public int IDPolicy { get; set; }
        public int ID_Config_poliy_Type { get; set; }
        public string Rating { get; set; }
        public bool IsPaid { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<config_policy_values> config_policy_values { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<config_insured_policy> config_insured_policy { get; set; }
        public virtual config_policy_type config_policy_type { get; set; }
    }
}
