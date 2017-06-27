using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.DI
{
    public class ConfigInsuredService : IConfigInsuredService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int AddConfigInsured(string name, string surname, string SSN, string passport, DateTime dateBirth, string address)
        {
            try
            {
                var configInsured = new config_insureds();
                configInsured.Name = name;
                configInsured.Surname = surname;
                configInsured.SSN = SSN;
                configInsured.Passport = passport;
                configInsured.BirthDate = dateBirth;
                configInsured.Address = address;

                _db.config_insureds.Add(configInsured);
                _db.SaveChanges();
                return configInsured.ID;
            }
            catch(Exception e)
            {
                return 0;
            }
        }
    }
}