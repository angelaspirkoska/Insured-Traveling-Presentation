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
            this.kanbanticketassignedtoes = new HashSet<kanbanticketassignedto>();
            this.kanbanticketwatchers = new HashSet<kanbanticketwatcher>();
            this.kanbanticketcomponents = new HashSet<kanbanticketcomponent>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> TicketTypeId { get; set; }
        public string CreatedById { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime DeadlineDate { get; set; }
        public float OrderBy { get; set; }
        public int KanbanPoolListId { get; set; }
    
        public virtual aspnetuser aspnetuser { get; set; }
        public virtual kanbanpoollist kanbanpoollist { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kanbanticketassignedto> kanbanticketassignedtoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kanbanticketwatcher> kanbanticketwatchers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kanbanticketcomponent> kanbanticketcomponents { get; set; }
        public virtual kanbantickettype kanbantickettype { get; set; }
    }
}
