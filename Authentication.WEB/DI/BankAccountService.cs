using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public class BankAccountService : IBankAccountService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public List<bank_account_info> BankAccountsByInsuredId(int InsuredId)
        {
           return _db.bank_account_info.Where(x => x.Account_HolderID == InsuredId).ToList();            
        }

        public List<bank_prefix> GetAllPrefix()
        {
            
            return _db.bank_prefix.ToList();
        }
    }
}
