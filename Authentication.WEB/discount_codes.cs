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
    
    public partial class discount_codes
    {
        public int ID { get; set; }
        public string Discount_Name { get; set; }
        public double Discount_Coef { get; set; }
        public System.DateTime Start_Date { get; set; }
        public System.DateTime End_Date { get; set; }
    }
}