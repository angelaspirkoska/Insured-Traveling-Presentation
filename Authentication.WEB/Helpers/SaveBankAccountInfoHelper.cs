using InsuredTraveling.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.Models;

namespace InsuredTraveling.Helpers
{
    public static class SaveBankAccountInfoHelper
    {
        public static bank_account_info SaveBankAccountInfo(IBankAccountService _bas, int clientId, string bankName, string bankAccountNumber )
        {
            var bank = _bas.GetBank(bankName);
            var bankAccount = _bas.Create();
            bankAccount.Account_HolderID = clientId;

            if (bank != null)
            {
                bankAccount.BankID = bank.ID;
            }
            else {

                bank = _bas.CreateBank();
                bank.Name = bankName;
                try
                {
                    _bas.AddBank(bank);
                }
                finally { }
            }

            bankAccount.Account_Number = bankAccountNumber;
         
            try
            {
                _bas.AddBankAccountInfo(bankAccount);
            }
            finally { }

            return bankAccount;
            
        }
    }
}