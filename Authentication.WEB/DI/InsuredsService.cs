﻿using System;
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
            var InsuredData = _db.insureds.Where(x => x.SSN == Insured.SSN).FirstOrDefault();
            if (InsuredData != null)
            {
                return InsuredData.ID;
            }
            _db.insureds.Add(Insured);
            _db.SaveChanges();
            return Insured.ID;
        }

        public type_insured GetInsuredType()
        {
            return _db.type_insured.Where(x => x.Name == "insured").FirstOrDefault();
        }

        public List<type_insured> GetAllInsuredTypes()
        {
            return _db.type_insured.ToList();
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
            var insured = _db.insureds.Where(x => x.SSN == Ssn).FirstOrDefault();
            if (insured != null)
            {
                return insured.ID;
            }
            
            return -1;
        }

        public List<insured> GetInsuredBySearchValues(string name, string lastname, string embg, string address, string email, string postal_code, string phone, string city, string passport, string createdBy)
        {
            return _db.insureds.Where(x => (x.Name.Contains(name) || String.IsNullOrEmpty(name)) &&
                                           (x.Lastname.Contains(lastname) || String.IsNullOrEmpty(lastname)) &&
                                           (x.SSN.Contains(embg) || String.IsNullOrEmpty(embg)) &&
                                           (x.Address.Contains(address) || String.IsNullOrEmpty(address)) &&
                                           (x.Email.Contains(email) || String.IsNullOrEmpty(email)) &&
                                           (x.Postal_Code.Contains(postal_code) || String.IsNullOrEmpty(postal_code)) &&
                                           (x.Phone_Number.Contains(phone) || String.IsNullOrEmpty(phone)) &&
                                           (x.City.Contains(city) || String.IsNullOrEmpty(city)) &&
                                           (x.Passport_Number_IdNumber.Contains(passport) || String.IsNullOrEmpty(passport)) &&
                                           (x.Created_By.Contains(createdBy))).ToList();
        }

        public insured GetInsuredBySsn(string Ssn)
        {
            var insured = _db.insureds.Where(x => x.SSN == Ssn).FirstOrDefault();
            return insured;
        }

        public insured GetInsuredBySsnAndCreatedBy(string Ssn, string userId)
        {
            var insured = _db.insureds.Where(x => x.SSN == Ssn && x.Created_By == userId). FirstOrDefault();
            return insured;
        }

        public insured GetBrokerManagerInsuredBySsnAndCreatedBy(string Ssn, string userId)
        {
            var user = _db.aspnetusers.FirstOrDefault(x => x.Id == userId);

            List<string> brokersUsers =
                _db.aspnetusers.Where(x => x.CreatedBy == user.Id).Select(x => x.Id).ToList();

            var insured = _db.insureds.FirstOrDefault(x => x.SSN == Ssn && brokersUsers.Contains(x.Created_By));
            return insured;
        }


        public void UpdateInsuredData(insured insured)
        {
           var insuredOld = _db.insureds.Where(x => x.ID == insured.ID && x.Created_By == insured.Modified_By).SingleOrDefault();
           insuredOld = insured;
            _db.SaveChanges();
        }

        public int GetInsuredIdBySsnAndCreatedBy(string Ssn, string userId)
        {
            var insured = _db.insureds.Where(x => x.SSN == Ssn && x.Created_By == userId).FirstOrDefault();
            if (insured != null)
            {
                return insured.ID;
            }

            return -1;
        }
    }
}