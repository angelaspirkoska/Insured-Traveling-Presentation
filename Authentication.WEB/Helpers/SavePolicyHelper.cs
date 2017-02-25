using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.Models;
using InsuredTraveling.DI;
using InsuredTraveling.Filters;
using InsuredTraveling.ViewModels;

namespace InsuredTraveling.Helpers
{
    public static class SavePolicyHelper
    {
        public static int SavePolicy(Policy p, 
                                     IPolicyService _ps, 
                                     IUserService _us, 
                                     IInsuredsService _iss, 
                                     IPolicyInsuredService _pis, 
                                     IAdditionalChargesService _acs)
        {
            var policy = _ps.Create();
            var username = "";
            if (p.isMobile)
            {
                username = p.username;
                policy.Created_By = _us.GetUserIdByUsername(p.username);
            }
            else
            {
                username = System.Web.HttpContext.Current.User.Identity.Name;
                policy.Created_By = _us.GetUserIdByUsername(username);
            }

            policy.Date_Created = DateTime.UtcNow;
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
                _us.UpdateSsnById(policy.Created_By, p.SSN);

                var PolicyHolderId = SaveInsuredHelper.SaveInsured(_iss, p.Name, p.LastName, p.SSN, p.Email, p.BirthDate, p.PhoneNumber, p.PassportNumber_ID, p.Address, p.City, p.PostalCode, policy.Created_By);
                policy.Policy_HolderID = PolicyHolderId;

            }
            else if (r.IsUser("admin") || r.IsUser("broker"))
            {}

            if (!p.IsSamePolicyHolderInsured)
            {
                var ssn = "";
                if (r.IsUser("admin") || r.IsUser("broker"))
                {
                    ssn = p.PolicyHolderSSN;
                }
                else if (r.IsUser("end user"))
                {
                    ssn = _us.GetUserSsnByUsername(username);
                }

                var policyHolderId = _iss.GetInsuredIdBySsnAndCreatedBy(ssn, policy.Created_By);
                if (policyHolderId != -1)
                {
                    insured updateInsuredData = new insured();

                    policy.Policy_HolderID = policyHolderId;
                    updateInsuredData.ID = policyHolderId;
                    updateInsuredData.Name = p.PolicyHolderName;
                    updateInsuredData.Lastname = p.PolicyHolderLastName;
                    updateInsuredData.SSN = p.PolicyHolderSSN;

                    updateInsuredData.Email = p.PolicyHolderEmail;
                    updateInsuredData.DateBirth = p.PolicyHolderBirthDate ?? DateTime.UtcNow;
                    updateInsuredData.Phone_Number = p.PolicyHolderPhoneNumber;

                    updateInsuredData.Passport_Number_IdNumber = p.PolicyHolderPassportNumber_ID;

                    updateInsuredData.City = p.PolicyHolderCity;
                    updateInsuredData.Postal_Code = p.PolicyHolderPostalCode;
                    updateInsuredData.Address = p.PolicyHolderAddress;

                    updateInsuredData.Date_Modified = DateTime.Now;
                    updateInsuredData.Modified_By = policy.Created_By;

                    _iss.UpdateInsuredData(updateInsuredData);
                }
                else
                {
                    var newInsured = _iss.Create();

                    newInsured.Name = p.PolicyHolderName;
                    newInsured.Lastname = p.PolicyHolderLastName;
                    newInsured.SSN = p.PolicyHolderSSN;

                    newInsured.Email = p.PolicyHolderEmail;
                    newInsured.DateBirth = p.PolicyHolderBirthDate ?? DateTime.UtcNow;
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

            var policyID = _ps.AddPolicy(policy);
            //polisa da se kreira pa posle osigurenik!!!!
            var insuredId = _iss.GetInsuredIdBySsnAndCreatedBy(p.SSN, p.Created_By);
            if (insuredId != -1)
            {
                // da se update
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

            if (p.isMobile)
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
                if(p.additional_charges != null)
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
            }
            return policyID;

        }

        public static bool SavePolicyFromMobile(AddPolicyMobileViewModel addPolicyMobile,
                                                IPolicyService _ps,
                                                IUserService _us,
                                                IInsuredsService _iss,
                                                IPolicyInsuredService _pis,
                                                IAdditionalChargesService _acs)
        {
            try
            {
                //add policy details
                var username = addPolicyMobile.Username;

                var policy = _ps.Create();
                policy.Policy_Number = _ps.CreatePolicyNumber();
                policy.Exchange_RateID = addPolicyMobile.Exchange_RateID;
                policy.CountryID = addPolicyMobile.CountryID;
                policy.Policy_TypeID = addPolicyMobile.Policy_TypeID;
                policy.Retaining_RiskID = addPolicyMobile.Retaining_RiskID;
                policy.Start_Date = addPolicyMobile.Start_Date;
                policy.End_Date = addPolicyMobile.End_Date;
                policy.Valid_Days = addPolicyMobile.Valid_Days;
                policy.Travel_NumberID = addPolicyMobile.Travel_NumberID;
                policy.Travel_Insurance_TypeID = addPolicyMobile.Travel_Insurance_TypeID;
                policy.Total_Premium = addPolicyMobile.Total_Premium;
                policy.Created_By = addPolicyMobile.Created_By;
                policy.Date_Created = DateTime.UtcNow;
                policy.Payment_Status = false;

                //add policy holder
                var policyHolder = _iss.GetInsuredBySsnAndCreatedBy(addPolicyMobile.SSN, addPolicyMobile.Created_By);
                if (policyHolder == null)
                {
                    var policyHolderID = SaveInsuredHelper.SaveInsured(_iss, addPolicyMobile.Name, 
                                                                             addPolicyMobile.LastName, 
                                                                             addPolicyMobile.SSN, 
                                                                             addPolicyMobile.Email,
                                                                             addPolicyMobile.DateBirth, 
                                                                             addPolicyMobile.Phone_Number, 
                                                                             addPolicyMobile.Passport_Number_IdNumber, 
                                                                             addPolicyMobile.Address, 
                                                                             addPolicyMobile.City,
                                                                             addPolicyMobile.Postal_Code, 
                                                                             addPolicyMobile.Created_By);
                    policy.Policy_HolderID = policyHolderID;
                }
                else
                {
                    policy.Policy_HolderID = policyHolder.ID;
                }

                var policyID = _ps.AddPolicy(policy);

                if (addPolicyMobile.Insureds != null)
                {
                    foreach(var insured in addPolicyMobile.Insureds)
                    {
                        var addInsured = _iss.GetInsuredBySsnAndCreatedBy(insured.SSN, addPolicyMobile.Created_By);
                        var addInsuredID = SaveInsuredHelper.SaveInsured(_iss, insured.Name, 
                                                                               insured.Lastname, 
                                                                               insured.SSN, 
                                                                               insured.Email,
                                                                               insured.DateBirth, 
                                                                               insured.Phone_Number,
                                                                               insured.Passport_Number_IdNumber, 
                                                                               insured.Address, 
                                                                               insured.City, 
                                                                               insured.Postal_Code, 
                                                                               insured.Created_By);

                        var policyInsured = _pis.Create();
                        policyInsured.InsuredID = addInsuredID;
                        policyInsured.PolicyID = policyID;
                        _pis.Add(policyInsured);
                    }
                }

                if(addPolicyMobile.Additional_charges != null)
                {
                    foreach (var additionalChargeId in addPolicyMobile.Additional_charges)
                    {
                        var additionalChargeObject = _acs.GetAdditionalChargeById(additionalChargeId);
                        if (additionalChargeObject != null)
                        {
                            var addNewCharge = _acs.Create();
                            addNewCharge.PolicyID = policyID;
                            addNewCharge.Additional_ChargeID = additionalChargeObject.ID;
                            _acs.AddAdditionalChargesPolicy(addNewCharge);
                        }
                    }
                }
                return true;
                
            }
            catch (Exception e)
            {
                return false;
            }
           
        }
    }
}