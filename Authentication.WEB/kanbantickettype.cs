
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
    
public partial class kanbantickettype
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public kanbantickettype()
    {

        this.kanbantickettypecomponents = new HashSet<kanbantickettypecomponent>();

        this.kanbantickets = new HashSet<kanbanticket>();

    }


    public int ID { get; set; }

    public string Name { get; set; }

    public string Color { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<kanbantickettypecomponent> kanbantickettypecomponents { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<kanbanticket> kanbantickets { get; set; }

}

}
