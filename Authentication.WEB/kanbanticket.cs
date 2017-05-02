
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
    
public partial class kanbanticket
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public kanbanticket()
    {

        this.kanbanticketcomponents = new HashSet<kanbanticketcomponent>();

        this.kanbantimekeepers = new HashSet<kanbantimekeeper>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public float OrderBy { get; set; }

    public int CreatedBy { get; set; }

    public int AssignedTo { get; set; }

    public int KanbanPoolListId { get; set; }

    public Nullable<int> TicketTypeId { get; set; }



    public virtual kanbanpoollist kanbanpoollist { get; set; }

    public virtual user user { get; set; }

    public virtual user user1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<kanbanticketcomponent> kanbanticketcomponents { get; set; }

    public virtual kanbantickettype kanbantickettype { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<kanbantimekeeper> kanbantimekeepers { get; set; }

}

}
