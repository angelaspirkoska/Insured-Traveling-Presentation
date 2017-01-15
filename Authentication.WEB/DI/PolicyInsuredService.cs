﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public class PolicyInsuredService : IPolicyInsuredService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int Add(policy_insured policyInsured)
        {
            _db.policy_insured.Add(policyInsured);
            _db.SaveChanges();
            return policyInsured.ID;
        }

        public policy_insured Create()
        {
           return _db.policy_insured.Create();
        }

        public List<insured> GetAllInsuredByPolicyId(int id)
        {
            var insureds = _db.policy_insured.Where(x => x.PolicyID == id).ToList();
            if(insureds != null)
            {
                return insureds.Select(x => x.insured).ToList();
            }
            return null;
        }
        public List<insured> GetAllInsuredByPolicyIdAndInsuredCreatedBy(int id, string createdById)
        {
            var insuredsID = _db.policy_insured.Where(x => x.PolicyID == id && x.insured.Created_By == createdById).Select(x => x.insured).ToList();

            return insuredsID;
        }

    }
}
