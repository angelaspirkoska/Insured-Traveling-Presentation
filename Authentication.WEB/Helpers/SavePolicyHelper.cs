using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.Models;
using InsuredTraveling.DI;
using InsuredTraveling.Filters;

namespace InsuredTraveling.Helpers
{
    public static class SavePolicyHelper
    {
        public static int SavePolicy(Policy p, IPolicyService _ps, IUserService _us, IInsuredsService _iss, IPolicyInsuredService _pis, IAdditionalChargesService _acs)
        {
            var policy = _ps.Create();
            if ( p.isMobile)
            {
                policy.Created_By = _us.GetUserIdByUsername(p.username);              
            }
            else
            {
                policy.Created_By = _us.GetUserIdByUsername(System.Web.HttpContext.Current.User.Identity.Name);
            }
            
            policy.Date_Created = DateTime.Now;
            policy.Policy_Number = _ps.CreatePolicyNumber();
           
            policy.CountryID = p.CountryID;
            policy.Exchange_RateID = (p.Exchange_RateID.HasValue) ? p.Exchange_RateID.Value : 1;
            policy.Policy_TypeID = p.Policy_TypeID;
            policy.Retaining_RiskID = p.Retaining_RiskID;
            policy.Start_Date = p.Start_Date;
            policy.End_Date = p.End_Date;
            policy.Valid_Days = p.Valid_Days;
            policy.Travel_NumberID = p.Travel_NumberID;
            policy.Total_Premium = p.Total_Premium;
            policy.Payment_Status = false;
            policy.Travel_Insurance_TypeID = p.Travel_Insurance_TypeID;

            RoleAuthorize r = new RoleAuthorize();
                        
            if (p.IsSamePolicyHolderInsured && (p.isMobile || r.IsUser("end user"))) 
             {
                var policyHolderId = _iss.GetInsuredIdBySsn(p.SSN);
                if(policyHolderId != -1)
                {
                    policy.Policy_HolderID = policyHolderId;
                }
                else
                {
                    var newInsured = _iss.Create();

                    newInsured.Name = p.Name;
                    newInsured.Lastname = p.LastName;
                    newInsured.SSN = p.SSN;

                    newInsured.Email = p.Email;
                    newInsured.DateBirth = p.BirthDate;
                    newInsured.Phone_Number = p.PhoneNumber;

                    newInsured.Passport_Number_IdNumber = p.PassportNumber_ID;

                    newInsured.City = p.City;
                    newInsured.Postal_Code = p.PostalCode;
                    newInsured.Address = p.Address;

                    newInsured.Date_Created = DateTime.Now;
                    newInsured.Created_By = policy.Created_By;
                    try
                    {
                       var Id = _iss.AddInsured(newInsured);
                        policy.Policy_HolderID = Id;
                    }
                    finally { }
                }
            }

            if(p.isMobile && !p.IsSamePolicyHolderInsured)
            {
                var policyHolderId = _iss.GetInsuredIdBySsn(p.PolicyHolderSSN);
                if (policyHolderId != -1)
                {
                    policy.Policy_HolderID = policyHolderId;
                }
                else
                {
                    var newInsured = _iss.Create();

                    newInsured.Name = p.PolicyHolderName;
                    newInsured.Lastname = p.PolicyHolderLastName;
                    newInsured.SSN = p.PolicyHolderSSN;

                    newInsured.Email = p.PolicyHolderEmail;
                    newInsured.DateBirth = p.PolicyHolderBirthDate;
                    newInsured.Phone_Number = p.PolicyHolderPhoneNumber;

                    newInsured.Passport_Number_IdNumber = p.PolicyHolderPassportNumber_ID;

                    newInsured.City = p.PolicyHolderCity;
                    newInsured.Postal_Code = p.PolicyHolderPostalCode;
                    newInsured.Address = p.PolicyHolderAddress;

                    newInsured.Date_Created = DateTime.Now;
                    newInsured.Created_By = policy.Created_By;
                    try
                    {
                        var Id = _iss.AddInsured(newInsured);
                        policy.Policy_HolderID = Id;
                    }
                    finally { }
                }
            }
           

            
            var policyID =  _ps.AddPolicy(policy);
            


            //polisa da se kreira pa posle osigurenik!!!!

         
                var insuredId = _iss.GetInsuredIdBySsn(p.SSN);
                if (insuredId != -1)
                {
                var policyInsured = _pis.Create();
                policyInsured.InsuredID = insuredId;
                policyInsured.PolicyID = policyID;
                _pis.Add(policyInsured);                                  
                }
                else
                {
                    var newInsured = _iss.Create();
                newInsured.Date_Created = DateTime.Now;
                newInsured.Created_By = policy.Created_By;
                newInsured.Name = p.Name;
                    newInsured.Lastname = p.LastName;
                    newInsured.SSN = p.SSN;

                    newInsured.Email = p.Email;
                    newInsured.DateBirth = p.BirthDate;
                    newInsured.Phone_Number = p.PhoneNumber;

                    newInsured.Passport_Number_IdNumber = p.PassportNumber_ID;

                    newInsured.City = p.City;
                    newInsured.Postal_Code = p.PostalCode;
                    newInsured.Address = p.Address;
                    try
                    {
                      var insuredIdNew = _iss.AddInsured(newInsured);
                    policy_insured policyInsured = new policy_insured();
                    policyInsured.InsuredID = insuredIdNew;
                    policyInsured.PolicyID = policyID;
                    _pis.Add(policyInsured);
                }
                    finally { }
                }

          
            if(p.isMobile)
            {
                
                      if (p.AdditionalChargeId1 != 1)
                {
                    var addChargeNew = _acs.Create();
                    addChargeNew.PolicyID = policyID;
                    addChargeNew.Additional_ChargeID = p.AdditionalChargeId1;
                    _acs.AddAdditionalChargesPolicy(addChargeNew);
                }

                if (p.AdditionalChargeId2 != 1)
                {
                    var addChargeNew = _acs.Create();
                    addChargeNew.PolicyID = policyID;
                    addChargeNew.Additional_ChargeID = p.AdditionalChargeId2;
                    _acs.AddAdditionalChargesPolicy(addChargeNew);
                }

            }
            else
            {
                foreach (additional_charge additionalCharge in p.additional_charges)
                {
                    if (additionalCharge.ID != 1)
                    {
                        var addChargeNew = _acs.Create();
                        addChargeNew.PolicyID = policyID;
                        addChargeNew.Additional_ChargeID = additionalCharge.ID;
                        _acs.AddAdditionalChargesPolicy(addChargeNew);
                    }

                }
            }
          




            return policyID;

        }
    }
}