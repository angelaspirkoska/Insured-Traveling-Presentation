using InsuredTraveling.DI;
using System.Web;
using InsuredTraveling.Models;
using System;
using System.Collections.Generic;
using InsuredTraveling.ViewModels;
using System.Linq;

namespace InsuredTraveling.Helpers
{
    public static class SaveFirstNoticeOfLossHelper
    {
        public static int SaveFirstNoticeOfLoss(IInsuredsService _iss, IUserService _us, IFirstNoticeOfLossService _fis,
            IBankAccountService _bas, IPolicyTypeService _pts, IAdditionalInfoService _ais,
            FirstNoticeOfLossReportViewModel firstNoticeOfLossViewModel, IEnumerable<HttpPostedFileBase> invoices,
            IEnumerable<HttpPostedFileBase> documentsHealth, IEnumerable<HttpPostedFileBase> documentsLuggage)
        {
            var result = -1;
            var additionalInfo = _ais.Create();
            if (firstNoticeOfLossViewModel.IsHealthInsurance)
            {
                additionalInfo.Accident_place = firstNoticeOfLossViewModel.AccidentPlaceHealth;
                if (firstNoticeOfLossViewModel.AccidentDateTimeHealth != null)
                {
                    var dateTime = firstNoticeOfLossViewModel.AccidentDateTimeHealth.Value;
                    var timeSpan = firstNoticeOfLossViewModel.AccidentTimeHealth.Value;
                    DateTime d = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
                    d.Add(timeSpan);
                    additionalInfo.Datetime_accident = d;
                }
                var healthInsuranceInfo = new health_insurance_info
                {
                    Additional_infoId = _ais.Add(additionalInfo),
                    additional_info = additionalInfo,
                    Datetime_doctor_visit = firstNoticeOfLossViewModel.DoctorVisitDateTime,
                    Doctor_info = firstNoticeOfLossViewModel.DoctorInfo,
                    Medical_case_description = firstNoticeOfLossViewModel.MedicalCaseDescription,
                    Previous_medical_history = firstNoticeOfLossViewModel.PreviousMedicalHistory,
                    Responsible_institution = firstNoticeOfLossViewModel.ResponsibleInstitution
                };

                try
                {
                    _ais.AddHealthInsuranceInfo(healthInsuranceInfo);

                }
                finally { }

            }
            else
            {
                additionalInfo.Accident_place = firstNoticeOfLossViewModel.AccidentPlaceLuggage;
                if (firstNoticeOfLossViewModel.AccidentDateTimeLuggage != null)
                {
                    var dateTime = firstNoticeOfLossViewModel.AccidentDateTimeLuggage.Value;
                    var timeSpan = firstNoticeOfLossViewModel.AccidentTimeLuggage.Value;
                    DateTime d = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
                    additionalInfo.Datetime_accident = d + timeSpan;
                }
                var luggageInsuranceInfo = new luggage_insurance_info
                {
                    Additional_infoId = _ais.Add(additionalInfo),
                    additional_info = additionalInfo,
                    Place_description = firstNoticeOfLossViewModel.PlaceDescription,
                    Detail_description = firstNoticeOfLossViewModel.DetailDescription,
                    Report_place = firstNoticeOfLossViewModel.ReportPlace,
                    Floaters = firstNoticeOfLossViewModel.Floaters,
                    Floaters_value = float.Parse(firstNoticeOfLossViewModel.FloatersValue),
                    Luggage_checking_Time = firstNoticeOfLossViewModel.LugaggeCheckingTime ?? new TimeSpan(0, 0, 0)
                };

                try
                {
                    _ais.AddLuggageInsuranceInfo(luggageInsuranceInfo);
                }
                finally { }
            }
            var firstNoticeOfLossEntity = _fis.Create();
            firstNoticeOfLossEntity.PolicyId = firstNoticeOfLossViewModel.PolicyId;
            firstNoticeOfLossEntity.ClaimantId = firstNoticeOfLossViewModel.ClaimantId;
            firstNoticeOfLossEntity.Relation_claimant_policy_holder = firstNoticeOfLossViewModel.RelationClaimantPolicyHolder;
            firstNoticeOfLossEntity.Destination = firstNoticeOfLossViewModel.Destination;
            firstNoticeOfLossEntity.Depart_Date_Time = firstNoticeOfLossViewModel.DepartDateTime;
            firstNoticeOfLossEntity.Arrival_Date_Time = firstNoticeOfLossViewModel.ArrivalDateTime;
            firstNoticeOfLossEntity.Transport_means = firstNoticeOfLossViewModel.TransportMeans;
            firstNoticeOfLossEntity.Total_cost = firstNoticeOfLossViewModel.TotalCost;
            firstNoticeOfLossEntity.CreatedDateTime = DateTime.Now;
            firstNoticeOfLossEntity.FNOL_Number = _fis.CreateFNOLNumber();

            string username;
            if (firstNoticeOfLossViewModel.isMobile)
            {
                username = firstNoticeOfLossViewModel.username;
            }
            else
            {
                username = System.Web.HttpContext.Current.User.Identity.Name;
            }
            firstNoticeOfLossEntity.CreatedBy = _us.GetUserIdByUsername(username);
            //firstNoticeOfLossEntity.Message = "";
            firstNoticeOfLossEntity.Additional_infoID = additionalInfo.ID;
            firstNoticeOfLossEntity.PolicyId = firstNoticeOfLossViewModel.PolicyId;
            firstNoticeOfLossEntity.ClaimantId = firstNoticeOfLossEntity.ClaimantId;
            firstNoticeOfLossEntity.Relation_claimant_policy_holder = firstNoticeOfLossEntity.Relation_claimant_policy_holder;

            if (!firstNoticeOfLossViewModel.isMobile && firstNoticeOfLossViewModel.ClaimantExistentBankAccount)
            {
                firstNoticeOfLossEntity.Claimant_bank_accountID = firstNoticeOfLossViewModel.ClaimantForeignBankAccountId;
            }
            else
            {
                var bankAccountId = SaveBankAccountInfoHelper.SaveBankAccountInfo(_bas, firstNoticeOfLossViewModel.ClaimantId,
                     firstNoticeOfLossViewModel.ClaimantBankName,
                     firstNoticeOfLossViewModel.ClaimantBankAccountNumber);

                firstNoticeOfLossEntity.Claimant_bank_accountID = bankAccountId;
            }

            if (firstNoticeOfLossViewModel.PolicyHolderExistentBankAccount)
            {
                firstNoticeOfLossEntity.Policy_holder_bank_accountID = firstNoticeOfLossViewModel.PolicyHolderForeignBankAccountId;
            }
            else
            {
                var bankAccountId = SaveBankAccountInfoHelper.SaveBankAccountInfo(_bas, firstNoticeOfLossViewModel.PolicyHolderId,
                     firstNoticeOfLossViewModel.PolicyHolderBankName,
                     firstNoticeOfLossViewModel.PolicyHolderBankAccountNumber);

                firstNoticeOfLossEntity.Policy_holder_bank_accountID = bankAccountId;
            }

            int FirstNoticeOfLossID = 0;
            try
            {
                FirstNoticeOfLossID = _fis.Add(firstNoticeOfLossEntity);

            }

            finally { }

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
                        var documentID = _fis.AddDocument(document);
                        _fis.AddInvoice(documentID);
                        _fis.AddDocumentToFirstNoticeOfLoss(documentID, FirstNoticeOfLossID);
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
                        var documentID = _fis.AddDocument(document);
                        _fis.AddDocumentToFirstNoticeOfLoss(documentID, FirstNoticeOfLossID);
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
                        var documentID = _fis.AddDocument(document);
                        _fis.AddDocumentToFirstNoticeOfLoss(documentID, FirstNoticeOfLossID);
                    }
                }
            }
            result = FirstNoticeOfLossID;
            return result;

        }

        public static int  SaveDetailFirstNoticeOdLoss(DetailFirstNoticeOfLossViewModel addDetailLoss,
                                                       travel_policy policy,
                                                       IFirstNoticeOfLossService _fis,
                                                       IAdditionalInfoService _ais,
                                                       IBankAccountService _bas)
        {
            try
            {
                var loss = _fis.Create();
                loss.PolicyId = policy.ID;
                loss.ClaimantId = addDetailLoss.Claimant_ID;
                loss.Relation_claimant_policy_holder = addDetailLoss.RelationClaimantPolicyHolder;
                loss.Destination = addDetailLoss.Destination;
                loss.Depart_Date_Time = addDetailLoss.Depart_Date_Time;
                loss.Arrival_Date_Time = addDetailLoss.Arrival_Date_Time;
                loss.Transport_means = addDetailLoss.Transport_means;
                loss.Total_cost = addDetailLoss.Total_cost;
                loss.CreatedDateTime = DateTime.UtcNow;
                loss.FNOL_Number = _fis.CreateFNOLNumber();

                //loss.Message = "";
                loss.CreatedBy = addDetailLoss.CreatedBy;
               
                //additional info
                var additionalInfo = _ais.Create();
                additionalInfo.Accident_place = addDetailLoss.Accident_place;
                additionalInfo.Datetime_accident = addDetailLoss.Datetime_accident;
                var additionalInfoID = _ais.Add(additionalInfo);
                loss.Additional_infoID = additionalInfoID;

                //bank accounts for policy holder
                var holderBankAccountsExist = _bas.CheckIfBankAccountExist(addDetailLoss.Policy_HolderID, addDetailLoss.PolicyHolder_BankAccount, addDetailLoss.PolicyHolder_BankID);
                if(!holderBankAccountsExist)
                {
                    var bankAccount = new bank_account_info();
                    bankAccount.Account_HolderID = addDetailLoss.Policy_HolderID;
                    bankAccount.BankID = addDetailLoss.PolicyHolder_BankID;
                    bankAccount.Account_Number = addDetailLoss.PolicyHolder_BankAccount;
                    var policyHolerBankAccount = _bas.AddBankAccountInfo(bankAccount);
                    loss.Policy_holder_bank_accountID = policyHolerBankAccount;
                }
                else
                {
                    var policyHolderBankAccount = _bas.GetBankAccountInfo(addDetailLoss.Policy_HolderID, addDetailLoss.PolicyHolder_BankAccount, addDetailLoss.PolicyHolder_BankID);
                    loss.Policy_holder_bank_accountID = policyHolderBankAccount.ID;
                }

                //bank accounts for claimant
                var claimantBankAccountsExist = _bas.CheckIfBankAccountExist(addDetailLoss.Claimant_ID, addDetailLoss.Claimant_BankAccount, addDetailLoss.Claimant_BankID);
                if (!claimantBankAccountsExist)
                {
                    var bankAccount = new bank_account_info();
                    bankAccount.Account_HolderID = addDetailLoss.Claimant_ID;
                    bankAccount.BankID = addDetailLoss.Claimant_BankID;
                    bankAccount.Account_Number = addDetailLoss.Claimant_BankAccount;
                    var claimantBankAccount = _bas.AddBankAccountInfo(bankAccount);
                    loss.Claimant_bank_accountID = claimantBankAccount;
                }
                else
                {
                    var claimantBankAccount = _bas.GetBankAccountInfo(addDetailLoss.Claimant_ID, addDetailLoss.Claimant_BankAccount, addDetailLoss.Claimant_BankID);
                    loss.Claimant_bank_accountID = claimantBankAccount.ID;
                }

                var lossID = _fis.Add(loss);

                if (addDetailLoss.HealthInsurance_Y_N.Equals("Y"))
                {
                    var healthInsuranceInfo = new health_insurance_info
                    {
                        Additional_infoId = additionalInfoID,
                        additional_info = additionalInfo,
                        Datetime_doctor_visit = addDetailLoss.Datetime_doctor_visit,
                        Doctor_info = addDetailLoss.Doctor_info,
                        Medical_case_description = addDetailLoss.Medical_case_description,
                        Previous_medical_history = addDetailLoss.Previous_medical_history,
                        Responsible_institution = addDetailLoss.Responsible_institution
                    };

                    _ais.AddHealthInsuranceInfo(healthInsuranceInfo);
                }
                else if (addDetailLoss.LuggageInsurance_Y_N.Equals("Y"))
                {
                    float floaterValue = 0;
                    float.TryParse(addDetailLoss.Floaters_value, out floaterValue);
                    var luggageInsuranceInfo = new luggage_insurance_info
                    {
                        Additional_infoId = additionalInfoID,
                        additional_info = additionalInfo,
                        Place_description = addDetailLoss.Place_description,
                        Detail_description = addDetailLoss.Detail_description,
                        Report_place = addDetailLoss.Report_place,
                        Floaters = addDetailLoss.Floaters,
                        Floaters_value = floaterValue,
                        Luggage_checking_Time = addDetailLoss.Luggage_checking_Time ?? new TimeSpan(0, 0, 0)
                    };

                    _ais.AddLuggageInsuranceInfo(luggageInsuranceInfo);
                  
                }
                return lossID;
            }
            catch (Exception e)
            {
                return -1;

            }
        }
    }
}

    