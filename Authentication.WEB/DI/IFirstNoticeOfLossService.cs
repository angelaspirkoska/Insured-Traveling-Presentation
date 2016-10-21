﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public interface IFirstNoticeOfLossService
    {
        List<first_notice_of_loss> GetAll();
        luggage_insurance_info isHealthInsurance(int lossId);
        first_notice_of_loss GetById(int id);
        first_notice_of_loss[] GetByInsuredUserId(string id);
        int Add(first_notice_of_loss FirstNoticeOfLoss);
        luggage_insurance_info GetLuggageAdditionalInfoByLossId(int LossID);
        health_insurance_info GetHealthAdditionalInfoByLossId(int LossID);
        bool IsHealthInsuranceByAdditionalInfoId(int Id);
        first_notice_of_loss Create();
        int AddDocument(document document);
        int AddInvoice(int documentId);
        int AddDocumentToFirstNoticeOfLoss(int documentId, int firstNoticeOFLossId);
        List<first_notice_of_loss> GetByPolicyId(int policy_Id);

    }
}
