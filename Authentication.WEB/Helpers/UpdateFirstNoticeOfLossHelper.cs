using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.DI;
using InsuredTraveling.ViewModels;
using InsuredTraveling.Models;

namespace InsuredTraveling.Helpers
{
    public static class UpdateFirstNoticeOfLossHelper
    {
        public static void UpdateFirstNoticeOfLoss(FirstNoticeOfLossEditViewModel model, IFirstNoticeOfLossService _fnol, IBankAccountService _bas)
        {
          

            var fnolPrevious = _fnol.GetById(model.Id);
            

            if (!fnolPrevious.Claimant_bank_account_info.Account_Number.ToString().Equals( model.ClaimantBankAccountNumber)
                || !fnolPrevious.Claimant_bank_account_info.bank.Name.Equals(model.ClaimantBankName))
            {
               var bankAccountId = UpdateBankAccountInfoHelper.UpdateBankAccountInfo(fnolPrevious.Claimant_bank_accountID, model.ClaimantBankAccountNumber, model.ClaimantBankName, fnolPrevious.ClaimantId, _bas);
                _fnol.UpdateClaimantBankAccountId(fnolPrevious.ID, bankAccountId);
            }

            if (!fnolPrevious.Policy_holder_bank_account_info.Account_Number.ToString().Equals(model.PolicyHolderBankAccountNumber)
               || !fnolPrevious.Policy_holder_bank_account_info.bank.Name.Equals(model.PolicyHolderBankName))
            {
                var bankAccountId = UpdateBankAccountInfoHelper.UpdateBankAccountInfo(fnolPrevious.Claimant_bank_accountID, model.ClaimantBankAccountNumber, model.ClaimantBankName, fnolPrevious.ClaimantId, _bas);
                _fnol.UpdatePolicyHolderBankAccountId(fnolPrevious.ID, bankAccountId);
            }

        }
    }
}