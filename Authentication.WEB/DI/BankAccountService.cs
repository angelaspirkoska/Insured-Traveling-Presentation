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

        public int AddBank(bank bank)
        {
            _db.banks.Add(bank);
             _db.SaveChanges();
            return bank.ID;
        }

        public int AddBankAccountInfo(bank_account_info bankAccountInfo)
        {
            _db.bank_account_info.Add(bankAccountInfo);
             _db.SaveChanges();
            return bankAccountInfo.ID;
        }

        public bank_account_info BankAccountInfoById(int ID)
        {
            return _db.bank_account_info.Single(x => x.ID == ID);
        }

        public List<bank_account_info> BankAccountsByInsuredId(int insuredId)
        {
           return _db.bank_account_info.Where(x => x.Account_HolderID == insuredId).ToList();            
        }

        public bank_account_info BankAccountsInfoByIdandUser(int ID, int accountHolder)
        {
            return (bank_account_info)_db.bank_account_info.Where(x => x.Account_HolderID == ID).Where(x => x.ID == ID);
                
            
        }

        public bool checkBankAccountInfo(int id)
        {
            if (_db.first_notice_of_loss.Any(x => x.Claimant_bank_accountID == id) || _db.first_notice_of_loss.Any(x => x.Policy_holder_bank_accountID == id))
                return true;
            return false;
        }

        public bank_account_info Create()
        {
           return _db.bank_account_info.Create();
        }

        public bank CreateBank()
        {
            return _db.banks.Create();
            
        }

        public void deleteBankAccountInfo(int id)
        {
            var bankAccount = _db.bank_account_info.Where(x => x.ID == id).FirstOrDefault();
            _db.bank_account_info.Remove(bankAccount);
            _db.SaveChanges();
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
