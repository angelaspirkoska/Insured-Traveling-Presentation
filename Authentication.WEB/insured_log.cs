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
    
    public partial class insured_log
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string SSN { get; set; }
        public System.DateTime DateBirth { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string City { get; set; }
        public string Postal_Code { get; set; }
        public string Address { get; set; }
        public string Passport_Number_IdNumber { get; set; }
        public int Type_InsuredID { get; set; }
        public System.DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Date_Modified { get; set; }
        public string Modified_By { get; set; }
        public string IPaddress { get; set; }
        public int Id_insured { get; set; }
    
        public virtual insured insured { get; set; }
    }
}
