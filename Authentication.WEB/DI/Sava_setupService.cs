using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.Models;
using AutoMapper;

namespace InsuredTraveling.DI
{
    public class Sava_setupService : ISava_setupService
    {
        InsuredTravelingEntity2 _db = new InsuredTravelingEntity2();
        public void AddSavaOkSetup(Sava_AdminPanelModel ok)
        {
            sava_setup ok2 = _db.sava_setup.Create();
            ok2 = Mapper.Map<Sava_AdminPanelModel, sava_setup>(ok);
            _db.sava_setup.Add(ok2);
            _db.SaveChanges();
        }
        public void DeleteOkSetup(int id)
        {
            var o = _db.sava_setup.Where(x => x.id == id);
            if (o != null)
            {
                _db.sava_setup.Remove(o.ToArray().First());
                _db.SaveChanges();
            }

        }
        public List<sava_setup> GetAllSavaSetups()
        {
            return _db.sava_setup.ToList();
        }
        public sava_setup GetLast()
        {
            return _db.sava_setup.ToArray().Last();
        }

        public sava_setup GetActiveSavaSetup()
        {
            return _db.sava_setup.FirstOrDefault(x => x.Active);
        }
    }
}