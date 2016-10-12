using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.WEB.Models;

namespace InsuredTraveling.DI
{
    public class PolicyService : IPolicyService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public int AddPolicy(travel_policy TravelPolicy)
        {
            _db.travel_policy.Add(TravelPolicy);
            return _db.SaveChanges();
        }

        public string CreatePolicyNumber()
        {
            return _db.travel_policy.OrderByDescending(p => p.ID).Select(r => r.Policy_Number).FirstOrDefault() + 1;
        }

        public List<travel_policy> GetAllPolicies()
        {
            return _db.travel_policy.ToList();
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
            return _db.travel_policy.Where(x => x.aspnetuser.Id == id).ToArray();
        }

        public travel_policy GetPolicyClientsInfo(int PolicyID)
        {

            var policy = _db.travel_policy.Where(x => x.ID == PolicyID)
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
    }
}
