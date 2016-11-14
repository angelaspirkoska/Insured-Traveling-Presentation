﻿using InsuredTraveling.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.Models;

namespace InsuredTraveling.Helpers
{
    public static class SaveInsuredHelper
    {
        public static int SaveInsured(IInsuredsService _iss, string FirstName, string LastName, string SSN,
            string Email, DateTime BirthDate, string PhoneNumber, string PassportNumber_ID, string Address,
            string City, string PostalCode, string CreatedById)
        {
            var insuredId = _iss.GetInsuredIdBySsn(SSN);
            //update na razlicni polinja?!?!?!
            if (insuredId != -1)
            {
              return insuredId;
            }

       
                var newInsured = _iss.Create();

                newInsured.Name = FirstName;
                newInsured.Lastname = LastName;
                newInsured.SSN = SSN;

                newInsured.Email = Email;
                newInsured.DateBirth = BirthDate;
                newInsured.Phone_Number = PhoneNumber;

                newInsured.Passport_Number_IdNumber = PassportNumber_ID;

                newInsured.City = City;
                newInsured.Postal_Code = PostalCode;
                newInsured.Address = Address;

                newInsured.Date_Created = DateTime.Now;
                newInsured.Created_By = CreatedById;
                    try
                    {
                insuredId = _iss.AddInsured(newInsured);
                    }
            finally { }
            return insuredId;
                   
            
        }
    }
} 