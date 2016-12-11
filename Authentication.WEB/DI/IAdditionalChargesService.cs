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
        List<additional_charge> GetAdditionalChargesByPolicyId(int policyId);
        string GetAdditionalChargeName(int chargeId);
        additional_charge GetAdditionalChargeById(int chargeId);
        List<additional_charge> GetAllAdditionalCharge();
        additional_charge_name GetAdditionalChargeENData(int chargeId);
        additional_charge_name GetAdditionalChargeMKData(int chargeId);

    }

}
