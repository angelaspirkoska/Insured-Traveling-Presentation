
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
    
public partial class luggage_insurance
{

    public int ID { get; set; }

    public System.DateTime Date_of_loss { get; set; }

    public string Place_desc_of_loss { get; set; }

    public string Place_reported { get; set; }

    public string Detailed_description { get; set; }

    public string Desc_of_stolen_damaged_things { get; set; }

    public string Documents_proof { get; set; }

    public System.TimeSpan AirportArrivalTime { get; set; }

    public System.TimeSpan LuggageDropTime { get; set; }

    public Nullable<int> LossID { get; set; }



    public virtual first_notice_of_loss first_notice_of_loss { get; set; }

}

}
