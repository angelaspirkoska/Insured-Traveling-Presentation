
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
    
public partial class kanbanpoollist
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public kanbanpoollist()
    {

        this.kanbantickets = new HashSet<kanbanticket>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public float OrderBy { get; set; }

    public int KanbanBoardId { get; set; }



    public virtual kanbanboard kanbanboard { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<kanbanticket> kanbantickets { get; set; }

}

}