using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IHealthInsurance
    {
        health_insurance Create();

        void Add(health_insurance HealthInsurance);
    }
}
