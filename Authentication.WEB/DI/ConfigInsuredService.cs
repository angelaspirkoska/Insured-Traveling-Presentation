using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class ConfigInsuredService : IConfigInsuredService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int AddConfigInsured(string name, string surname, string SSN, string passport, DateTime dateBirth, string address)
        {
            try
            {
                var configInsured = new config_insureds();
                configInsured.Name = name;
                configInsured.Surname = surname;
                configInsured.SSN = SSN;
                configInsured.Passport = passport;
                configInsured.BirthDate = dateBirth;
                configInsured.Address = address;

                _db.config_insureds.Add(configInsured);
                _db.SaveChanges();
                return configInsured.ID;
            }
            catch(Exception e)
            {
                return 0;
            }
        }

        public config_insureds GetPolicyHolderByPolicyId(int policyId)
        {
            var insuredPolicy = _db.config_insured_policy.Where(x => x.IDPolicy == policyId && x.IdInsuredType == 1).FirstOrDefault();
            if(insuredPolicy != null)
            {
                return _db.config_insureds.Where(x => x.ID == insuredPolicy.IDInsured).FirstOrDefault();
            }
            return null;
        }

        public config_insureds GetInsuredByPolicyId(int policyId)
        {
            var insuredPolicy = _db.config_insured_policy.Where(x => x.IDPolicy == policyId && x.IdInsuredType == 2).FirstOrDefault();
            if (insuredPolicy != null)
            {
                return _db.config_insureds.Where(x => x.ID == insuredPolicy.IDInsured).FirstOrDefault();
            }
            return null;
        }
    }
}