using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IConfigInsuredService
    {
        int  AddConfigInsured(string name, string surname, string SSN, string passport, DateTime dateBirth, string address);
        config_insureds GetPolicyHolderByPolicyId(int policyId);
        config_insureds GetInsuredByPolicyId(int policyId);
    }
}
