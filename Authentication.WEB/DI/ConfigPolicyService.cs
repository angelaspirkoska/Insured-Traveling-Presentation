using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class ConfigPolicyService :IConfigPolicyService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public int  AddConfigPolicy(config_policy policy)
        {
            try
            {
                _db.config_policy.Add(policy);
                _db.SaveChanges();
                return policy.IDPolicy;
            }
            catch(Exception e)
            {
                return 0;
            }
          
        }


    }
}