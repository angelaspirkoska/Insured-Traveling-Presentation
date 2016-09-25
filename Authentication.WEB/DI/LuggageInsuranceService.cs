using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public class LuggageInsuranceService : ILuggageInsuranceService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public void Add(luggage_insurance LuggageInsurance)
        {
            _db.luggage_insurance.Add(LuggageInsurance);
            _db.SaveChanges();       
        }

        public luggage_insurance GetById(int id)
        {
            return _db.luggage_insurance.Where(x => x.ID == id).Single();
        }
    }
}
