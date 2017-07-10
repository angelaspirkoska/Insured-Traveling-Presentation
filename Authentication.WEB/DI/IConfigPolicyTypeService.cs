using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
    public interface IConfigPolicyTypeService
    {
        int AddConfigPolicyType(config_policy_type configPolicyType);
        List<config_policy_type> GetAllActivePolicyTypes();
        config_policy_type GetConfigPolicyTypeByID(int id);
        config_policy_type GetConfigPolicyTypeByTypeFromID(int id);
        List<config_policy_type> GetTypeByName(string TypeName);
        IQueryable<SelectListItem> GetAllActivePolicyTypesDropdown();
        List<config_policy_type> GetAllPolicies();

        int EditConfigPolicyType(ConfigPolicyTypeModel editedPolicy);
        int AddNewPolicyTypeVersion(config_policy_type selectedPolicy);
       

    }
}