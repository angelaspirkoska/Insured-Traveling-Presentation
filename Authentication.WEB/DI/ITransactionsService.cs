using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
   public interface ITransactionsService
    {
        // kolku da go ima 
        string MakeOid();
        void AddTransaction(transaction Transaction);
    }
}
