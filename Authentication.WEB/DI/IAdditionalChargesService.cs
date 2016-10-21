using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public interface IAdditionalChargesService
    {
         IQueryable<SelectListItem> GetAll();

        int AddAdditionalChargesPolicy(policy_additional_charge policyAdditionalCharge);
        policy_additional_charge Create();
    }

}
