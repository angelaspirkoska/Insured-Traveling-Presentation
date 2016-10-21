﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;
using Authentication.WEB.Models;

namespace InsuredTraveling.DI
{
   public interface IPolicyService
    {
        int AddPolicy(travel_policy TravelPolicy);
        travel_policy GetPolicyIdByPolicyNumber(string id);
        travel_policy GetPolicyById(int id);
        string GetPolicyNumberByPolicyId(int id);
        DateTime GetStartDateByPolicyId(int PolicyID);
        DateTime GetEndDateByPolicyId(int PolicyID);
        travel_policy[] GetPolicyByUsernameId(string id);
        travel_policy Create();
        string CreatePolicyNumber();
        List<travel_policy> GetPolicyByTypePolicies(int TypePolicies);
        //ne e jasno zasto sluzi dolniot metod!!!!!
        string GetCompanyID(string PolicyNumber);
        List<travel_policy> GetAllPolicies();
        travel_policy GetPolicyClientsInfo(int PolicyID);
        insured GetPolicyHolderByPolicyID(int PolicyID);
        List<travel_policy> GetPoliciesByCountryAndType(int? TypePolicy, int? Country);
    }
}
