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
    
    public partial class aspnetuser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMBG { get; set; }
        public string Address { get; set; }
        public string Municipality { get; set; }
        public string InsuranceCompany { get; set; }
        public string ActivationCodeMail { get; set; }
        public string ActivationCodeSMS { get; set; }
        public bool IsValidMail { get; set; }
        public string GroupID { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string PassportNumber { get; set; }
        public string City { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
}
