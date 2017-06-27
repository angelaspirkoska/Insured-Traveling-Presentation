using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IConfigInsuredPolicyService
    {
        int AddConfigInsuredPolicy(int policyId, int insuredId, int type);
    }
}
