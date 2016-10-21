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

        policy_insured Create();
       
    }
}
