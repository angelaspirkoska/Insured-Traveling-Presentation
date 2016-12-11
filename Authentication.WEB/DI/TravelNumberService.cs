using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class TravelNumberService : ITravelNumberService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public List<travel_number> GetAllTravelNumbers()
        {
            return _db.travel_number.ToList();
        }
    }
}