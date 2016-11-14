using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.DI;

namespace InsuredTraveling.Helpers
{
    public static class UpdateBankAccountInfoHelper
    {
        public static int UpdateBankAccountInfo(int oldBankAccountId, string newBankAccountNumber, string newBankName, int clientId, IBankAccountService _bas)
        {

            if(!_bas.checkBankAccountInfo(oldBankAccountId))
            {
                _bas.deleteBankAccountInfo(oldBankAccountId);
            }
            var id = SaveBankAccountInfoHelper.SaveBankAccountInfo(_bas, clientId, newBankName, newBankAccountNumber);

            return id;
        }

    }
}