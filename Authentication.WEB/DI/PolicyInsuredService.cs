using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public class PolicyInsuredService : IPolicyInsuredService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public List<insured> GetAllInsuredByPolicyId(int id)
        {
            var insuredsID = _db.policy_insured.Where(x => x.PolicyID == id).Select(x => x.insured).ToList();
           
            return insuredsID;
        }
      
    }
}
