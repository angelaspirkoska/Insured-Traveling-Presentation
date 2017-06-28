using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public interface IConfigPolicyService
    {
        int AddConfigPolicy(int idConfigPolicyType, string rating, DateTime StartDate, DateTime EndDate, bool isPaid);
        bool UpdateConfigPolicy(int idPolicy, string rating);
        IQueryable<SelectListItem> GetConfigPolicyList();
        List<config_policy> GetConfigPolicyByConfigType(int type);
        config_policy GetConfigByPolicyId(int policyId);

    }
}
