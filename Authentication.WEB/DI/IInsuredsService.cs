using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
   public interface IInsuredsService
    {
       int AddInsured(insured Insured);

        insured Create();

        insured GetInsuredData(int InsuredId);

        insured GetInsuredDataBySsn(string Ssn);

        int GetInsuredIdBySsn(string Ssn);

    }
}
