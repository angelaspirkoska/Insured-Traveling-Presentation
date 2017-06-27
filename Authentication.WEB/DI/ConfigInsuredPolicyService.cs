using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class ConfigInsuredPolicyService :IConfigInsuredPolicyService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int AddConfigInsuredPolicy(int policyId, int insuredId, int type)
        {
            try
            {
                var insuredPolicy = new config_insured_policy();
                insuredPolicy.IDPolicy = policyId;
                insuredPolicy.IDInsured = insuredId;
                insuredPolicy.IdInsuredType = type;
                _db.config_insured_policy.Add(insuredPolicy);
                _db.SaveChanges();
                return insuredPolicy.ID;
            }
            catch(Exception e)
            {
                return 0;
            }
        }
    }
}