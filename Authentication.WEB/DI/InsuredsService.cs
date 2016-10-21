using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class InsuredsService : IInsuredsService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int AddInsured(insured Insured)
        {
            var InsuredData = _db.insureds.Where(x => x.SSN == Insured.SSN).SingleOrDefault();
            if (InsuredData != null)
            {
                return InsuredData.ID;
            }
            _db.insureds.Add(Insured);
            _db.SaveChanges();
            return Insured.ID;
        }

        public insured Create()
        {
            return _db.insureds.Create();
        }

        public insured GetInsuredData(int InsuredId)
        {
            insured InsuredData = _db.insureds.Where(x => x.ID == InsuredId).SingleOrDefault();
            return InsuredData;
        }

        public insured GetInsuredDataBySsn(string Ssn)
        {
            var InsuredData = _db.insureds.Where(x => x.SSN == Ssn).SingleOrDefault();
            return InsuredData;
        }

        public int GetInsuredIdBySsn(string Ssn)
        {
            var insured = _db.insureds.Where(x => x.SSN == Ssn).SingleOrDefault();
            if (insured != null)
            {
                return insured.ID;
            }
            
            return -1;
        }
    }
}