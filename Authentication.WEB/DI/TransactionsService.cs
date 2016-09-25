using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public class TransactionsService : ITransactionsService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public void AddTransaction(transaction Transaction)
        {
            _db.transactions.Add(Transaction);
            _db.SaveChanges();
        }

        public string MakeOid()
        {
            return (_db.transactions.OrderByDescending(p => p.OrderID).Select(r => r.OrderID).First() + 1).ToString();
        }

    }
}
