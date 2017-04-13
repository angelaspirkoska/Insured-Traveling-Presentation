using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public interface ISavaPoliciesService
    {
        List<sava_policy> GetSavaPoliciesForUser(string ssn);
        void AddSavaPolicyList(List<SavaPolicyModel> savaListPolicy);
        void AddSavaPolicy(SavaPolicyModel sava_policy);
    }
}