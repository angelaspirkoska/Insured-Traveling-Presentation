using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.Models;
using AutoMapper;

namespace InsuredTraveling.DI
{
    public class OkSetupService : IOkSetupService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public void AddOkSetup(Ok_SetupModel ok)
        {
            ok_setup ok2 = _db.ok_setup.Create();
            ok2 = Mapper.Map<Ok_SetupModel,ok_setup>(ok);
            _db.ok_setup.Add(ok2);
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

        public ok_setup GetLast()
        {
            return _db.ok_setup.ToArray().Last();
        }
    }
}