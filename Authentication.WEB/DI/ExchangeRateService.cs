using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class ExchangeRateService : IExchangeRateService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
        public List<exchange_rate> GetAllExchangeRates()
        {
            return _db.exchange_rate.ToList();
        }
    }
}