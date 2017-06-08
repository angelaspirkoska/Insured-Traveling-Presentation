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
            sava_policy.createdBy = HttpContext.Current.User.Identity.Name;
            sp = Mapper.Map<SavaPolicyModel, sava_policy>(sava_policy);
            
            _db.sava_policy.Add(sp);
            _db.SaveChanges();
            
            //SumDiscountPoints(sava_policy.SSN_policyHolder,sava_policy.discount_points);
        }

        //Dodava poeni vo  aspnetusers 
        public void SumDiscountPoints(string policyHolder,float DiscountPoints, DateTime datePolicyCreated)
        {
            try
            {
                
                var tempUser = _db.aspnetusers.Where(x => x.EMBG.Equals(policyHolder)).FirstOrDefault();
                var datetime = new DateTime(2017, 6, 1).ToString("dd/MM/yyyy");
                if (datePolicyCreated >= tempUser.CreatedOn )
                {
                    if (tempUser.Points == null)
                    {
                        tempUser.Points = 0;
                        tempUser.Points += DiscountPoints;
                    }
                    else
                    {
                        tempUser.Points += DiscountPoints;
                    }

                    _db.aspnetusers.Attach(tempUser);
                    var entry = _db.Entry(tempUser);
                    entry.Property(e => e.Points).IsModified = true;
                    _db.SaveChanges();

                }
            }
            catch (Exception ex)
            {

            }
        }

        public float? GetUserPoints(string policyHolder)
        {
            var tempUser = _db.aspnetusers.Where(x => x.EMBG.Equals(policyHolder)).FirstOrDefault();
            return tempUser.Points;
        }
      

        public List<sava_policy> GetSavaPoliciesForList(string ssn, string policyNumber)
        {
           // int number = !String.IsNullOrEmpty(policyNumber) ? Convert.ToInt32(policyNumber) : 0;
            return
                _db.sava_policy.Where(
                    x =>
                        (String.IsNullOrEmpty(policyNumber) || x.policy_number ==  policyNumber)  &&
                        (String.IsNullOrEmpty(ssn) || x.SSN_policyHolder.Equals(ssn)) ).ToList();
        }

        public List<sava_policy> GetSavaPoliciesForInsuredList(string ssnInsured, string policyNumber)
        {
           // int number = !String.IsNullOrEmpty(policyNumber) ? Convert.ToInt32(policyNumber) : 0;
           
            return
                _db.sava_policy.Where(
                    x =>
                        (String.IsNullOrEmpty(policyNumber) || x.policy_number == policyNumber) &&
                        (String.IsNullOrEmpty(ssnInsured) || x.SSN_insured.Equals(ssnInsured))).ToList();
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

        public sava_policy GetSavaPolicyIdByPolicyNumber(string id)
        {
            
            return _db.sava_policy.Where(x => x.policy_number.Equals(id)).FirstOrDefault();
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