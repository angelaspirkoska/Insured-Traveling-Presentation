
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
    
public partial class kanbanboard
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public kanbanboard()
    {

        this.kanbanpoollists = new HashSet<kanbanpoollist>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Color { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<kanbanpoollist> kanbanpoollists { get; set; }

}

}
