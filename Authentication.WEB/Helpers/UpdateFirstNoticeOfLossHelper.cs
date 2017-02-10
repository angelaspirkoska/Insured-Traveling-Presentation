using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.DI;
using InsuredTraveling.ViewModels;

namespace InsuredTraveling.Helpers
{
    public static class UpdateFirstNoticeOfLossHelper
    {
        public static void UpdateFirstNoticeOfLoss(FirstNoticeOfLossEditViewModel model, IFirstNoticeOfLossService _fnol, IBankAccountService _bas, IAdditionalInfoService _ais, IHealthInsuranceService _his, ILuggageInsuranceService _lis, IEnumerable<HttpPostedFileBase> invoices,
            IEnumerable<HttpPostedFileBase> documentsHealth, IEnumerable<HttpPostedFileBase> documentsLuggage)
        {
          
            var fnol = _fnol.GetById(model.Id);
            //bank accounts update    
            var isArchived = ArchiveFirstNoticeOfLossHelper.ArchiveFirstNoticeOfLoss(fnol, model.ModifiedBy, _fnol);
            if (!fnol.Claimant_bank_account_info.Account_Number.ToString().Equals( model.ClaimantBankAccountNumber)
                || !fnol.Claimant_bank_account_info.bank.Name.Equals(model.ClaimantBankName))
            {
               var bankAccountId = UpdateBankAccountInfoHelper.UpdateBankAccountInfo(fnol.Claimant_bank_accountID, model.ClaimantBankAccountNumber.Trim(), model.ClaimantBankName, fnol.ClaimantId, _bas);
                _fnol.UpdateClaimantBankAccountId(fnol.ID, bankAccountId);
            }

            if (!fnol.Policy_holder_bank_account_info.Account_Number.ToString().Equals(model.PolicyHolderBankAccountNumber)
               || !fnol.Policy_holder_bank_account_info.bank.Name.Equals(model.PolicyHolderBankName))
            {
                var bankAccountId = UpdateBankAccountInfoHelper.UpdateBankAccountInfo(fnol.Policy_holder_bank_accountID, model.PolicyHolderBankAccountNumber.Trim(), model.PolicyHolderBankName, fnol.travel_policy.Policy_HolderID, _bas);
                _fnol.UpdatePolicyHolderBankAccountId(fnol.ID, bankAccountId);
            }

            UpdateAdditionalInfoHelper.UpdateAdditionalInfo(model, _fnol, _ais, _lis, _his);

            //jos vkupna vrednost i dokumenti
            var newFnol = fnol;
            newFnol.Modified_Datetime = DateTime.Now;
            newFnol.ModifiedBy = model.ModifiedBy;
            newFnol.Destination = model.Destination;
            newFnol.Depart_Date_Time = model.DepartDateTime.Date;
            newFnol.Depart_Date_Time = model.DepartDateTime.Date + (model.DepartTime ?? new TimeSpan(0, 0, 0));
            //newFnol.Depart_Date_Time = new DateTime(model.DepartDateTime.Year,);

            newFnol.Arrival_Date_Time = model.ArrivalDateTime.Date;
            newFnol.Arrival_Date_Time = model.ArrivalDateTime.Date + (model.ArriveTime ?? new TimeSpan(0, 0, 0));
            newFnol.Transport_means = model.TransportMeans;
            newFnol.Relation_claimant_policy_holder = model.RelationClaimantPolicyHolder;
            newFnol.Total_cost = model.TotalCost;
            _fnol.Update(newFnol);

            if (invoices != null)
            {
                foreach (var file in invoices)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = @"~/DocumentsFirstNoticeOfLoss/Invoices/" + file.FileName;
                        file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));

                        var document = new document();
                        document.Name = file.FileName;
                        var documentID = _fnol.AddDocument(document);
                        _fnol.AddInvoice(documentID);
                        _fnol.AddDocumentToFirstNoticeOfLoss(documentID, fnol.ID);
                    }
                }
            }

            if (documentsHealth != null)
            {
                foreach (var file in documentsHealth)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = @"~/DocumentsFirstNoticeOfLoss/HealthInsurance/" + file.FileName;
                        file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));
                        var document = new document();
                        document.Name = file.FileName;
                        var documentID = _fnol.AddDocument(document);
                        _fnol.AddDocumentToFirstNoticeOfLoss(documentID, fnol.ID);
                    }
                }
            }

            if (documentsLuggage != null)
            {
                foreach (var file in documentsLuggage)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = @"~/DocumentsFirstNoticeOfLoss/LuggageInsurance/" + file.FileName;
                        file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));
                        var document = new document();
                        document.Name = file.FileName;
                        var documentID = _fnol.AddDocument(document);
                        _fnol.AddDocumentToFirstNoticeOfLoss(documentID, fnol.ID);
                    }
                }
            }

        }
    }
}