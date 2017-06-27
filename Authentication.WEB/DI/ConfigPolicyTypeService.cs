using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class ConfigPolicyTypeService : IConfigPolicyTypeService
    {
        InsuredTravelingEntity  _db = new InsuredTravelingEntity();

        public int AddConfigPolicyType(config_policy_type configPolicyType)
        {
            try
            {
                _db.config_policy_type.Add(configPolicyType);
                _db.SaveChanges();
                return configPolicyType.ID;
            }
            catch(Exception e)
            {
                return 0;
            }
        }

        public List<config_policy_type> GetAllActivePolicyTypes()
        {
            return _db.config_policy_type.Where(x => x.status == true).ToList();
        }

        public config_policy_type GetConfigPolicyTypeByID(int id)
        {
            return _db.config_policy_type.Where(x => x.ID == id).FirstOrDefault();
        }

        public List<config_policy_type> GetTypeByName(string TypeName)
        {
            List<config_policy_type> ListType = new List<config_policy_type>();
            if (TypeName == null || TypeName == " " || TypeName == "undefined")
            {
                return _db.config_policy_type.ToList();
            }
            else
            {
                return _db.config_policy_type.Where(x => x.policy_type_name == TypeName).ToList();
            }


        }
    }
}