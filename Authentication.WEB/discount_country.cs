
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
    
public partial class discount_country
{

    public int ID { get; set; }

    public Nullable<int> CountryID { get; set; }

    public Nullable<int> Policy_typeID { get; set; }

    public Nullable<double> Percentage { get; set; }

    public string Franchise { get; set; }

    public Nullable<double> Discount_franchise { get; set; }

    public System.DateTime Date_Created { get; set; }

    public string Created_By { get; set; }

    public Nullable<System.DateTime> Date_Modified { get; set; }

    public string Modified_By { get; set; }



    public virtual country country { get; set; }

    public virtual policy_type policy_type { get; set; }

    public virtual aspnetuser aspnetuser { get; set; }

}

}
