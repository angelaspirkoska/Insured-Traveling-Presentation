using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.WEB.Models;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public class PolicyService : IPolicyService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public int AddPolicy(travel_policy TravelPolicy)
        {
            TravelPolicy.Payment_Status = false;
            _db.travel_policy.Add(TravelPolicy);
            _db.SaveChanges();
            return TravelPolicy.ID;
        }

        public retaining_risk GetRetainingRisk(int id)
        {
            retaining_risk r = _db.retaining_risk.Where(x=> x.ID == id).FirstOrDefault();
            return r;
        }
        public policy_type GetPolicyType(int id)
        {
            policy_type pt = _db.policy_type.Where(x => x.ID == id).FirstOrDefault();
            return pt;
        }
        public void UpdatePaymentStatus(string PolicyNumber)
        {
            var Policy = _db.travel_policy.Where(x => x.Policy_Number == PolicyNumber).SingleOrDefault();
            Policy.Payment_Status = true;
            _db.SaveChanges();
        }

        public string CreatePolicyNumber()
        {
            return (Int64.Parse(_db.travel_policy.OrderByDescending(p => p.ID).Select(r => r.Policy_Number).FirstOrDefault()) + 1).ToString();
        }


        public List<travel_policy> GetAllPolicies()
        {
            return _db.travel_policy.Where(x=> x.Payment_Status == true).ToList();
        }

        public List<travel_policy> GetAllPoliciesByPolicyNumber(string Prefix)
        {
            return _db.travel_policy.Where(x => x.Payment_Status == true && x.Policy_Number.Contains(Prefix)).ToList();
        }

        public IQueryable<SelectListItem> GetAll()
        {
            var policies = _db.travel_policy.Select(p => new SelectListItem
            {
                Text = p.Policy_Number,
                Value = p.ID.ToString()
            });
            return policies;
        }

        public string GetCompanyID(string PolicyNumber)
        {

            return _db.travel_policy.OrderByDescending(p => p.ID).Select(r => r.Policy_Number).FirstOrDefault();
        }

        public DateTime GetEndDateByPolicyId(int PolicyID)
        {
            return _db.travel_policy.Where(x => x.ID == PolicyID).Select(x => x.End_Date).Single();
        }

        public travel_policy GetPolicyIdByPolicyNumber(string id)
        {
            return _db.travel_policy.Where(x => x.Policy_Number.Equals(id)).FirstOrDefault();
        }


        public List<travel_policy> GetPolicyByTypePolicies(int TypePolicies)
        {
            return _db.travel_policy.Where(x => x.Policy_TypeID.Equals(TypePolicies)).ToList();

        }

        public travel_policy[] GetPolicyByUsernameId(string id)
        {
            return _db.travel_policy.Where(x => x.aspnetuser.Id == id && x.Payment_Status == true).ToArray();
        }

        public travel_policy[] GetPolicyNotPayedByUsernameId(string id)
        {
            return _db.travel_policy.Where(x => x.aspnetuser.Id == id && x.Payment_Status == false).ToArray();
        }

        public travel_policy GetPolicyClientsInfo(int PolicyID)
        {

            var policy = _db.travel_policy.Where(x => x.ID == PolicyID && x.Payment_Status == true)
                .Include(x => x.insured)
                .Include(x => x.policy_insured)
                .Include(x => x.insured.bank_account_info)
                .SingleOrDefault();


            return policy;

        }
        public insured GetPolicyHolderByPolicyID(int PolicyID)
        {

            insured PolicyHolder = _db.travel_policy.Where(x => x.ID == PolicyID).Select(x => x.insured).Single();

            return PolicyHolder;

        }

        public DateTime GetStartDateByPolicyId(int PolicyID)
        {
            return _db.travel_policy.Where(x => x.ID == PolicyID).Select(x => x.Start_Date).Single();
        }

        public string GetPolicyNumberByPolicyId(int id)
        {
            return _db.travel_policy.Where(x => x.ID == id).Select(x => x.Policy_Number).Single();
        }

        public travel_policy Create()
        {
            return _db.travel_policy.Create();                             
         }

        public travel_policy GetPolicyById(int id)
        {
            return _db.travel_policy.Where(x => x.ID == id).SingleOrDefault();
        }

        public List<travel_policy> GetBrokersExpiringPolicies(string userId, DateTime dateFrom)
        {
            if (userId == "")
                return null;
            return _db.travel_policy.Where(x => x.Created_By == userId && x.End_Date < dateFrom && x.End_Date > DateTime.Now).ToList();
        }

        public List<travel_policy> GetBrokersPolicies(string userId, DateTime dateFrom)
        {
            if (userId == "")
                return null;
            return _db.travel_policy.Where(x => x.Created_By == userId && x.Date_Created > dateFrom && x.Payment_Status == true).ToList();
        }

        public List<travel_policy> GetPoliciesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string UserId, string PolicyNumber)
        {
            if(UserId != "")
            {
                string ssn = " ";
                insured insured;
                var user = _db.aspnetusers.Where(x => x.Id == UserId).FirstOrDefault();

                if (user == null)
                    return null;
                ssn = user.EMBG;
                insured = _db.insureds.Where(x => x.SSN == ssn).FirstOrDefault();
                if (insured == null)
                    return null;
                return _db.travel_policy.Where(x => (x.Created_By == UserId || x.insured.SSN == ssn || x.policy_insured.Where(k => k.InsuredID == insured.ID).FirstOrDefault() != null) && (PolicyNumber == "" || x.Policy_Number.Contains(PolicyNumber)) && (TypePolicy == null || x.Policy_TypeID == TypePolicy.Value) &&
                                        (Country == null || x.CountryID == Country.Value) && x.Payment_Status == true).ToList();
            }
            return _db.travel_policy.Where(x => (x.Created_By == UserId) && (PolicyNumber == "" || x.Policy_Number.Contains(PolicyNumber)) && (TypePolicy == null || x.Policy_TypeID == TypePolicy.Value) &&
                                       (Country == null || x.CountryID == Country.Value) && x.Payment_Status == true).ToList();
        }
        public List<travel_policy> GetQuotesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string UserId, string PolicyNumber)
        {
            return _db.travel_policy.Where(x => (x.Created_By == UserId) && (x.Payment_Status == false) && (PolicyNumber == "" || x.Policy_Number.Contains(PolicyNumber)) && (TypePolicy == null || x.Policy_TypeID == TypePolicy.Value) &&
                                    (Country == null || x.CountryID == Country.Value)).ToList();
        }
        public List<travel_policy> GetPoliciesByInsuredId(int insuredId)
        {
            var allPolicies = _db.policy_insured.Where(x => x.InsuredID == insuredId).Select(x => x.PolicyID).ToList();
            return _db.travel_policy.Where(x => (allPolicies.Contains(x.ID)) && x.Payment_Status == true).ToList();
        }

        public List<travel_policy> GetPoliciesByHolderId(int holderId)
        {
            return _db.travel_policy.Where(x => x.Policy_HolderID == holderId && x.Payment_Status == true).ToList();
        }

        public IQueryable<SelectListItem> GetPoliciesByUserId(string userID)
        {
            return _db.travel_policy.Where(x => x.Created_By == userID && x.Payment_Status == true).Select(p => new SelectListItem
            {
                Text = p.Policy_Number,
                Value = p.ID.ToString()
            });
        }

        public List<travel_policy> GetPoliciesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string PolicyNumber)
        {
            return _db.travel_policy.Where(x =>  (PolicyNumber == "" || x.Policy_Number.Contains(PolicyNumber)) && (TypePolicy == null || x.Policy_TypeID == TypePolicy.Value) &&
                                    (Country == null || x.CountryID == Country.Value)).ToList();
        }

        public List<travel_policy> GetQuotesByCountryAndTypeAndPolicyNumber(int? TypePolicy, int? Country, string PolicyNumber)
        {
            return _db.travel_policy.Where(x => (PolicyNumber == "" || x.Policy_Number.Contains(PolicyNumber)) && (x.Payment_Status == false) && (TypePolicy == null || x.Policy_TypeID == TypePolicy.Value) &&
                                    (Country == null || x.CountryID == Country.Value)).ToList();
        }

        public string GetPolicyHolderEmailByPolicyId(int PolicyId)
        {
            return _db.travel_policy.Where(x => x.ID == PolicyId).Select(x => x.insured.Email).SingleOrDefault();
        }
    }
}
