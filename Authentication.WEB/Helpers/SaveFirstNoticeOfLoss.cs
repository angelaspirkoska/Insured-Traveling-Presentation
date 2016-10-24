using InsuredTraveling.DI;
using System.Web;
using InsuredTraveling.Models;
using System;
using System.Collections.Generic;


namespace InsuredTraveling.Helpers
{
    public static class SaveFirstNoticeOfLossHelper
    {
        public static bool SaveFirstNoticeOfLoss(IInsuredsService _iss, IUserService _us, IFirstNoticeOfLossService _fis,
            IBankAccountService _bas, IPolicyTypeService _pts, IAdditionalInfoService _ais,
            FirstNoticeOfLossReportViewModel firstNoticeOfLossViewModel, IEnumerable<HttpPostedFileBase> invoices,
            IEnumerable<HttpPostedFileBase> documentsHealth, IEnumerable<HttpPostedFileBase> documentsLuggage)
        {
            var result = true;
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

                    result = _ais.AddHealthInsuranceInfo(healthInsuranceInfo) > 0;

                }
                finally
                {

                }

            }

            else
            {
                additionalInfo.Accident_place = firstNoticeOfLossViewModel.AccidentPlaceLuggage;

                if (firstNoticeOfLossViewModel.AccidentDateTimeLuggage != null)
                {                  
                    var dateTime = firstNoticeOfLossViewModel.AccidentDateTimeLuggage.Value;
                    var timeSpan = firstNoticeOfLossViewModel.AccidentTimeLuggage.Value;
                    DateTime d = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
                    d.Add(timeSpan);
                    additionalInfo.Datetime_accident = d;
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
                finally
                {

                }
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
            firstNoticeOfLossEntity.Message = "";



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

            if(invoices != null)
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

            return FirstNoticeOfLossID > 0;

        }
    }
}