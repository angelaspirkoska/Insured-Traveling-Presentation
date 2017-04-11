using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public class HealthInsuranceService : IHealthInsuranceService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();

        public void Add(health_insurance HealthInsurance)
        {
             _db.health_insurance.Add(HealthInsurance);
            _db.SaveChanges();
        }

        public health_insurance Create()
        {
           return _db.health_insurance.Create();
        }
    }
}
