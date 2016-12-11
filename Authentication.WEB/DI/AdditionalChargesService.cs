using InsuredTraveling.App_Start;
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
            return _db.policy_additional_charge.Where(x => x.PolicyID == policyId).Select(x => x.additional_charge).ToList();
        }

        public IQueryable<SelectListItem> GetAll()
        {
            var languageId = SiteLanguages.CurrentLanguageId();

            var additionalCharges = _db.additional_charge_name.Where(x => x.language_id == languageId).Select(p => new SelectListItem
            {
                Text = p.name,
                Value = p.additional_charge_id.ToString()
            });
            return additionalCharges;
        }

        public string GetAdditionalChargeName(int chargeId)
        {
            var languageId = SiteLanguages.CurrentLanguageId();
            var chargeName =  _db.additional_charge_name.Where(x => x.additional_charge_id == chargeId && x.language_id == languageId).FirstOrDefault();
            return chargeName != null ? chargeName.name : null;
        }

        public additional_charge GetAdditionalChargeById(int chargeId)
        {
            return _db.additional_charge.Where(x => x.ID == chargeId).FirstOrDefault();
        }

        public List<additional_charge> GetAllAdditionalCharge()
        {
            return _db.additional_charge.ToList();
        }

        public additional_charge_name GetAdditionalChargeENData(int chargeId)
        {
            var language = _db.languages.Where(x => x.CultureName == "en").FirstOrDefault();
            return _db.additional_charge_name.Where(x => x.additional_charge_id == chargeId && x.language_id == language.Id).FirstOrDefault();
        }

        public additional_charge_name GetAdditionalChargeMKData(int chargeId)
        {
            var language = _db.languages.Where(x=>x.CultureName == "mk").FirstOrDefault();
            return _db.additional_charge_name.Where(x => x.additional_charge_id == chargeId && x.language_id == language.Id).FirstOrDefault();
        }
    }
}
