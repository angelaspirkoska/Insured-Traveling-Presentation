using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public class BankAccountService : IBankAccountService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public bank_account_info BankAccountInfoById(int ID)
        {
            return _db.bank_account_info.Single(x => x.Account_HolderID == ID);
        }

        public List<bank_account_info> BankAccountsByInsuredId(int insuredId)
        {
           return _db.bank_account_info.Where(x => x.Account_HolderID == insuredId).ToList();            
        }

        public bank_account_info Create()
        {
           return _db.bank_account_info.Create();
        }

        public bank CreateBank()
        {
            return _db.banks.Create();
            
        }

        public List<bank_prefix> GetAllPrefix()
        {
            
            return _db.bank_prefix.ToList();
        }

        public Task<List<bank_prefix>> GetAllPrefixAsync()
        {
            return _db.bank_prefix.ToListAsync();
        }

        public bank GetBank(string bankName)
        {
            return _db.banks.Single(x => x.Name.Equals(bankName));
           
        }

    
    }
}
