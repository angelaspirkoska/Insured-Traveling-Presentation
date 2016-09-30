using System;
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

        travel_policy GetPolicyById(string id);

        travel_policy[] GetPolicyByUsernameId(string id);

        string CreatePolicyNumber();

        List<travel_policy> GetPolicyByTypePolicies(int TypePolicies);

        //ne e jasno zasto sluzi dolniot metod!!!!!
        string GetCompanyID(string PolicyNumber);

         List<travel_policy> GetAllPolicies();

        insured GetPolicyHolderByPolicyID(int PolicyID);

    }
}
