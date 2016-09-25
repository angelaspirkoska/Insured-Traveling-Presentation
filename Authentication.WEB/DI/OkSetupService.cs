using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
    public class OkSetupService : IOkSetupService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public void AddOkSetup(ok_setup ok)
        {
            _db.ok_setup.Add(ok);
            _db.SaveChanges();
        }

        public void DeleteOkSetup(int id)
        {
            var o = _db.ok_setup.Where(x => x.id == id);
            if (o != null)
            {
                _db.ok_setup.Remove(o.ToArray().First());
                _db.SaveChanges();
            }

        }

        public List<ok_setup> GetAllOkSetups()
        {
            return _db.ok_setup.ToList();
        }

        public ok_setup GetLastByInsuranceCompany(string InsuranceCompany)
        {
           return _db.ok_setup.Where(x => x.InsuranceCompany == InsuranceCompany).ToArray().Last();
        }
    }
}