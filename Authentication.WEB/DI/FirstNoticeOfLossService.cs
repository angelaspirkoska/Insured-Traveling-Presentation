using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;
using Microsoft.Ajax.Utilities;

namespace InsuredTraveling.DI
{
    public class FirstNoticeOfLossService : IFirstNoticeOfLossService
    {
        private InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public int Add(first_notice_of_loss FirstNoticeOfLoss)
        {
            _db.first_notice_of_loss.Add(FirstNoticeOfLoss);
             _db.SaveChanges();
            return FirstNoticeOfLoss.ID;
        }

        public bool IsHealthInsuranceByAdditionalInfoId(int id)
        {
            var healthAdditionalInfo = _db.health_insurance_info.Single(x => x.Additional_infoId == id);
            return healthAdditionalInfo != null;
        }

      
        public first_notice_of_loss Create()
        {
           return _db.first_notice_of_loss.Create();
        }

        public List<first_notice_of_loss> GetAll()
        {
           return _db.first_notice_of_loss.ToList();
        }

        public first_notice_of_loss GetById(int id)
        {
           return _db.first_notice_of_loss.Where(x => x.ID == id).ToArray().First();
        }

        public first_notice_of_loss[] GetByInsuredUserId(string id)
        {
            return _db.first_notice_of_loss.Where(x => x.CreatedBy == id).ToArray();
        }

        public health_insurance_info GetHealthAdditionalInfoByLossId(int lossId)
        {
            first_notice_of_loss fnolId = _db.first_notice_of_loss.Single(x => x.ID == lossId);

            return _db.health_insurance_info.Single(x => x.additional_info.ID == fnolId.Additional_infoID);

        }

        public luggage_insurance_info GetLuggageAdditionalInfoByLossId(int lossId)
        {
            first_notice_of_loss fnolId = _db.first_notice_of_loss.Single(x => x.ID == lossId);

            return _db.luggage_insurance_info.Single(x => x.additional_info.ID == fnolId.Additional_infoID);

        }

        public int AddDocument(document document)
        {
            _db.documents.Add(document);
            _db.SaveChanges();
            return document.ID;            
        }

        public int AddDocumentToFirstNoticeOfLoss(int documentId, int firstNoticeOFLossId)
        {
            var firstNoticeOFLossDocument = new documents_first_notice_of_loss();
            firstNoticeOFLossDocument.DocumentID = documentId;
            firstNoticeOFLossDocument.First_notice_of_lossID = firstNoticeOFLossId;
            _db.documents_first_notice_of_loss.Add(firstNoticeOFLossDocument);
            _db.SaveChanges();
            return firstNoticeOFLossDocument.ID;
        }

        public int AddInvoice(int documentId)
        {
            var invoice = new invoice();
            invoice.DocumentID = documentId;
            _db.invoices.Add(invoice);
            _db.SaveChanges();
            return invoice.DocumentID;
            
        }
    }
}
