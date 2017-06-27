using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public interface IConfigPolicyTypeService
    {
        int AddConfigPolicyType(config_policy_type configPolicyType);
        List<config_policy_type> GetAllActivePolicyTypes();
        config_policy_type GetConfigPolicyTypeByID(int id);
        List<config_policy_type> GetTypeByName(string TypeName);
    }
}