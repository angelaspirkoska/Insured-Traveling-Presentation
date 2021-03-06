﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
   public interface IInsuredsService
   {
       insured GetBrokerManagerInsuredBySsnAndCreatedBy(string Ssn, string userId);
        int AddInsured(insured Insured);
        insured Create();
        insured GetInsuredData(int InsuredId);
        insured GetInsuredDataBySsn(string Ssn);
        int GetInsuredIdBySsn(string Ssn);
        type_insured GetInsuredType();
        List<type_insured> GetAllInsuredTypes();
        int GetInsuredIdBySsnAndCreatedBy(string Ssn, string userId);
        insured GetInsuredBySsn(string Ssn);
        insured GetInsuredBySsnAndCreatedBy(string Ssn, string userId);
        List<insured> GetInsuredBySearchValues(string name, string lastname, string embg, string address, string email, string postal_code, string phone, string city, string passport, string createdBy);

        void UpdateInsuredData(insured insured);


    }
}
