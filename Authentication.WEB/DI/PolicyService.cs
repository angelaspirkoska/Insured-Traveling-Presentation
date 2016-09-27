using System;
using System.Collections.Generic;
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
            _db.SaveChanges();
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

        

        public travel_policy GetPolicyById(int id)
        {
            return _db.travel_policy.Where(x => x.Policy_Number.Equals(id)).FirstOrDefault();
        }


        public List<travel_policy> GetPolicyByTypePolicies(int TypePolicies)
        {
           return  _db.travel_policy.Where(x => x.Policy_TypeID.Equals(TypePolicies)).ToList();

        }

        public travel_policy[] GetPolicyByUsernameId(string id)
        {
           return _db.travel_policy.Where(x => x.aspnetuser.Id == id).ToArray();
        }

       
    }
}
