
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
    
public partial class countries_name
{

    public int ID { get; set; }

    public int countries_id { get; set; }

    public int language_id { get; set; }

    public string name { get; set; }



    public virtual country country { get; set; }

    public virtual language language { get; set; }

}

}
