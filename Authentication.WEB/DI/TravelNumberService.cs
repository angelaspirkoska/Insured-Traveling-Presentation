using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class TravelNumberService : ITravelNumberService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();

        public List<travel_number> GetAllTravelNumbers()
        {
            return _db.travel_number.ToList();
        }
    }
}