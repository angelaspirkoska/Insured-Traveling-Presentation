
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
    
public partial class travel_duration
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public travel_duration()
    {

        this.discount_days = new HashSet<discount_days>();

    }


    public int ID { get; set; }

    public string Days { get; set; }

    public System.DateTime Created_Date { get; set; }

    public string Created_By { get; set; }

    public string Modified_Date { get; set; }

    public string Modified_By { get; set; }

    public int Version { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<discount_days> discount_days { get; set; }

    public virtual aspnetuser aspnetuser { get; set; }

    public virtual aspnetuser aspnetuser1 { get; set; }

}

}
