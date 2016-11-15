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
        public static int SaveBankAccountInfo(IBankAccountService _bas, int clientId, string bankName, string bankAccountNumber )
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
                    bankAccount.BankID= _bas.AddBank(bank);
                }
                finally { }
            }

            bankAccount.Account_Number = bankAccountNumber;

            
                return _bas.AddBankAccountInfo(bankAccount);
        

           
            
        }
    }
} 