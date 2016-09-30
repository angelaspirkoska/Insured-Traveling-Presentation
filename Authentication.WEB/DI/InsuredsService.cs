using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class InsuredsService : IInsuredsService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public void AddInsured(insured Insured)
        {
            _db.insureds.Add(Insured);
            _db.SaveChanges();
        }

        public insured GetInsuredData(int InsuredId)
        {
            insured InsuredData = _db.insureds.Where(x => x.ID == InsuredId).SingleOrDefault();
            return InsuredData;
        }
    }
}