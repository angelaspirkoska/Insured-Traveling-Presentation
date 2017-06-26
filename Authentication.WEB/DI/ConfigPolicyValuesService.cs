using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class ConfigPolicyValuesService : IConfigPolicyValuesService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public bool AddConfigPolicyValues(List<config_policy_values> values)
        {
            try
            {
                foreach (var value in values)
                {
                    _db.config_policy_values.Add(value);
                    _db.SaveChanges();
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
           
        }

    }
}