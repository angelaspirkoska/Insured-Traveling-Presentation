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
    
    public partial class bank_account_info
    {
        public int ID { get; set; }
        public int Account_HolderID { get; set; }
        public string Account_Number { get; set; }
        public int BankID { get; set; }
    
        public virtual insured insured { get; set; }
        public virtual bank bank { get; set; }
    }
}