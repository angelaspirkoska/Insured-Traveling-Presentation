using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
        string GetPolicyHolderEmailByPolicyId(int PolicyId);
        void UpdatePaymentStatus(string PolicyNumber);
        List<travel_policy> GetAllPolicies();
        List<SelectListItem> GetAllPoliciesAsSelectList();
        IQueryable<SelectListItem> GetAll();
        travel_policy GetPolicyClientsInfo(int PolicyID);
        insured GetPolicyHolderByPolicyID(int PolicyID);
        List<travel_policy> GetPoliciesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string UserId, string PolicyNumber);

        List<travel_policy> GetPoliciesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string PolicyNumber);
        List<travel_policy> GetPoliciesByInsuredId(int insuredId);
        List<travel_policy> GetPoliciesByHolderId(int holderId);
        IQueryable<SelectListItem> GetPoliciesByUserId(string userID);
    }
}
