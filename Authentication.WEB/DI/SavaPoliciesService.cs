using AutoMapper;
using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class SavaPoliciesService : ISavaPoliciesService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();

        public List<sava_policy> GetSavaPoliciesForUser(string ssn)
        {
            return _db.sava_policy.Where(x => x.SSN_insured == ssn || x.SSN_policyHolder == ssn).ToList();
        }

        public void AddSavaPolicyList(List<SavaPolicyModel> savaListPolicy)
        {
            foreach (SavaPolicyModel policy in savaListPolicy)
            {
                AddSavaPolicy(policy);
            }
        }
        public void AddSavaPolicy(SavaPolicyModel sava_policy)
        {
            sava_policy sp = _db.sava_policy.Create();
            sp = Mapper.Map<SavaPolicyModel, sava_policy>(sava_policy);
            
            _db.sava_policy.Add(sp);
            _db.SaveChanges();
        }

        public List<sava_policy> GetSavaPoliciesForList(string ssn, string policyNumber)
        {
            int number = !String.IsNullOrEmpty(policyNumber) ? Convert.ToInt32(policyNumber) : 0;
            return
                _db.sava_policy.Where(
                    x =>
                        (String.IsNullOrEmpty(policyNumber) || x.policy_number.Equals(number)) &&
                        (String.IsNullOrEmpty(ssn) || x.SSN_policyHolder.Equals(ssn))).ToList();
        }

        public List<sava_policy> GetSavaPoliciesAdminForList(string policyNumber, string ssnInsured, string ssnHolder)
        {
            int number = !String.IsNullOrEmpty(policyNumber) ? Convert.ToInt32(policyNumber) : 0;
            return
               _db.sava_policy.Where(
                   x =>
                       (String.IsNullOrEmpty(policyNumber) || x.policy_number.Equals(number)) &&
                       (String.IsNullOrEmpty(ssnInsured) || x.SSN_insured.Equals(ssnInsured)) &&
                       (String.IsNullOrEmpty(ssnHolder)) || x.SSN_policyHolder.Equals(ssnHolder)).ToList();
        }

        public int SaveSavaPolicy(sava_policy policy)
        {
            try
            {
                _db.sava_policy.Add(policy);
                _db.SaveChanges();
                return policy.id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

    }
}