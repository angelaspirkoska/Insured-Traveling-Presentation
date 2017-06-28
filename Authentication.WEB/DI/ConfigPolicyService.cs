using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public class ConfigPolicyService : IConfigPolicyService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public int AddConfigPolicy(int idConfigPolicyType, string rating, DateTime StartDate, DateTime EndDate, bool isPaid)
        {
            try
            {
                var policy = new config_policy();
                policy.ID_Config_poliy_Type = idConfigPolicyType;
                policy.Rating = rating;
                policy.StartDate = StartDate;
                policy.EndDate = EndDate;
                policy.IsPaid = isPaid;

                _db.config_policy.Add(policy);
                _db.SaveChanges();
                return policy.IDPolicy;
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public bool UpdateConfigPolicy(int idPolicy, string rating)
        {
            try
            {
                var policy = _db.config_policy.Where(x => x.IDPolicy == idPolicy).FirstOrDefault();
                policy.Rating = rating;
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }


        }
        public IQueryable<SelectListItem> GetConfigPolicyList()
        {
            return _db.config_policy.Select(p => new SelectListItem
            {
                Text = p.IDPolicy.ToString(),
                Value = p.IDPolicy.ToString()
            });
          
        }
    }
}