
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
    
public partial class kanbanticketassignedto
{

    public int Id { get; set; }

    public int KanbanTicketId { get; set; }

    public string AssignedToId { get; set; }

    public System.DateTime AssignedDateTime { get; set; }

    public bool Active { get; set; }



    public virtual aspnetuser aspnetuser { get; set; }

    public virtual kanbanticket kanbanticket { get; set; }

}

}
