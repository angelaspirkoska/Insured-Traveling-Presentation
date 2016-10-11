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

        List<bank_prefix> GetAllPrefix();
        Task<List<bank_prefix>> GetAllPrefixAsync();
        bank_account_info BankAccountInfoById(int ID);

        bank_account_info Create();

        bank CreateBank();

        bank GetBank(string bankName);
    }
}
