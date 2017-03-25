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
        List<travel_policy> GetBrokerManagerExpiringPolicies(string userId, DateTime dateFrom);
        List<travel_policy> GetBrokerManagerBrokersQuotesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country,
           string UserId, string PolicyNumber);
        List<travel_policy> GetBrokerManagerBrokersPoliciesByCountryAndTypeAndPolicyNumber(int? TypePolicy,
           int? Country, string UserId, string PolicyNumber);
        List<travel_policy> GetBrokersQuotes(string userId, DateTime dateFrom);
        List<travel_policy> GetBrokersExpiringPolicies(string userId, DateTime dateFrom);
        List<travel_policy> GetBrokersPolicies(string userId, DateTime dateFrom);
        int AddPolicy(travel_policy TravelPolicy);
        retaining_risk GetRetainingRisk(int id);
        policy_type GetPolicyType(int id);
        travel_policy GetPolicyIdByPolicyNumber(string id);
        travel_policy GetPolicyById(int id);
        string GetPolicyNumberByPolicyId(int id);
        DateTime GetStartDateByPolicyId(int PolicyID);
        DateTime GetEndDateByPolicyId(int PolicyID);
        travel_policy[] GetPolicyByUsernameId(string id);
        travel_policy[] GetPolicyNotPayedByUsernameId(string id);
        travel_policy Create();
        string CreatePolicyNumber();
        List<travel_policy> GetPolicyByTypePolicies(int TypePolicies);
        string GetCompanyID(string PolicyNumber);
        string GetPolicyHolderEmailByPolicyId(int PolicyId);
        void UpdatePaymentStatus(string PolicyNumber);
        List<travel_policy> GetAllPolicies();
        List<travel_policy> GetAllPoliciesByPolicyNumber(string Prefix);
        IQueryable<SelectListItem> GetAll();
        travel_policy GetPolicyClientsInfo(int PolicyID);
        insured GetPolicyHolderByPolicyID(int PolicyID);
        List<travel_policy> GetPoliciesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string UserId, string PolicyNumber);

        List<travel_policy> GetPoliciesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string PolicyNumber);
        List<travel_policy> GetQuotesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string UserId, string PolicyNumber);
        List<travel_policy> GetQuotesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string PolicyNumber);
        List<travel_policy> GetPoliciesByInsuredId(int insuredId);
        List<travel_policy> GetPoliciesByHolderId(int holderId);
        IQueryable<SelectListItem> GetPoliciesByUserId(string userID);
    }
}
