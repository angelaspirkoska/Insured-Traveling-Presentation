using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
   public interface IPolicyInsuredService
    {
        List<insured> GetAllInsuredByPolicyId(int id);

        int Add(policy_insured policyInsured);
        List<insured> GetAllInsuredByPolicyIdAndInsuredCreatedBy(int id, string createdById);
        policy_insured Create();
       
    }
}
