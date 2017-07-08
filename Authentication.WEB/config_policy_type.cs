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
    
    public partial class config_policy_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public config_policy_type()
        {
            this.excelconfigs = new HashSet<excelconfig>();
            this.config_policy = new HashSet<config_policy>();
        }
    
        public int ID { get; set; }
        public string policy_type_name { get; set; }
        public Nullable<System.DateTime> policy_effective_date { get; set; }
        public Nullable<System.DateTime> policy_expiry_date { get; set; }
        public Nullable<bool> status { get; set; }
        public Nullable<int> Version { get; set; }
        public Nullable<int> typeFrom { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<excelconfig> excelconfigs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<config_policy> config_policy { get; set; }
    }
}
