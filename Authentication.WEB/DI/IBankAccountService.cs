using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IBankAccountService
    {
        List<bank_account_info> BankAccountsByInsuredId(int InsuredId);

        int AddBank(bank bank);
        int AddBankAccountInfo(bank_account_info bankAccountInfo);
        List<bank_prefix> GetAllPrefix();
        Task<List<bank_prefix>> GetAllPrefixAsync();
        bank_account_info BankAccountInfoById(int ID);
        bank_account_info BankAccountsInfoByIdandUser(int ID, int accountHolder);
        bank_account_info Create();

        //checks if the bank account is used in other fnol's
        bool checkBankAccountInfo(int id);

        bank_account_info GetBankAccountInfo(int clientId, string bankAccountNumber, string bankAccountName);
        void deleteBankAccountInfo(int id);

        bank CreateBank();

        bank GetBank(string bankName);

        List<bank> GetAllBanks();
    }
}
