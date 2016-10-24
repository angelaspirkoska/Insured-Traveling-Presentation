using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
   public class AdditionalChargesService : IAdditionalChargesService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int AddAdditionalChargesPolicy(policy_additional_charge policyAdditionalCharge)
        {
            _db.policy_additional_charge.Add(policyAdditionalCharge);
            _db.SaveChanges();
            return policyAdditionalCharge.ID;
        }

        public policy_additional_charge Create()
        {
            return _db.policy_additional_charge.Create();
        }

        public List<additional_charge> GetAdditionalChargesByPolicyId(int policyId)
        {
          return  _db.policy_additional_charge.Where(x => x.PolicyID == policyId).Select(x => x.additional_charge).ToList();
        }

        public IQueryable<SelectListItem> GetAll()
        {
            var AdditionalCharge = _db.additional_charge.Select(p => new SelectListItem
            {
                Text = p.Doplatok,
                Value = p.ID.ToString()
            });
            return AdditionalCharge;
        }
    }
}
