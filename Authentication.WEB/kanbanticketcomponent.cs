
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
    
public partial class kanbanticketcomponent
{

    public int Id { get; set; }

    public Nullable<int> TicketId { get; set; }

    public Nullable<int> ComponentId { get; set; }

    public string Value { get; set; }

    public string Name { get; set; }



    public virtual kanbancomponent kanbancomponent { get; set; }

    public virtual kanbanticket kanbanticket { get; set; }

}

}
