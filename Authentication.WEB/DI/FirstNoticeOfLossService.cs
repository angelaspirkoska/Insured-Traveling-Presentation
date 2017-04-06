using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;
using Microsoft.Ajax.Utilities;
using System.Globalization;
using System.Web;

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
            var healthAdditionalInfo = _db.health_insurance_info.SingleOrDefault(x => x.Additional_infoId == id);
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
        public List<first_notice_of_loss> GetFNOLBySearchValues(string PolicyNumber, string FNOLNumber, string holderName, string holderLastName, string clientName, string clientLastName, string insuredName, string insuredLastName, string totalPrice, string healthInsurance, string luggageInsurance)
        {
            float totalPricefloat = 0;
            int policyNum = !String.IsNullOrEmpty(PolicyNumber) ? Convert.ToInt32(PolicyNumber) : 0;
            if (!String.IsNullOrEmpty(totalPrice))
            {
                float.TryParse(totalPrice, out totalPricefloat);
            }
            if (healthInsurance == "true")
            {
                return _db.first_notice_of_loss.Where(x =>
                                   (x.travel_policy.ID == policyNum || String.IsNullOrEmpty(PolicyNumber)) &&
                                   (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                   (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                   (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                   (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                   (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                   (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                   (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                   (x.additional_info.health_insurance_info != null)).ToList();
            }
            if(luggageInsurance == "true")
            {
                return _db.first_notice_of_loss.Where(x =>
                                  (x.travel_policy.ID == policyNum || String.IsNullOrEmpty(PolicyNumber)) &&
                                  (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                  (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                  (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                  (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                  (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                  (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                  (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                  (x.additional_info.luggage_insurance_info != null)).ToList();
            }
            else
            {
                return _db.first_notice_of_loss.Where(x =>
                                (x.travel_policy.ID == policyNum || String.IsNullOrEmpty(PolicyNumber)) &&
                                (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName))).ToList();
            }
           

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
            return _db.health_insurance_info.SingleOrDefault(x => x.additional_info.ID == fnolId.Additional_infoID);
        }

        public luggage_insurance_info GetLuggageAdditionalInfoByLossId(int lossId)
        {
            first_notice_of_loss fnolId = _db.first_notice_of_loss.Single(x => x.ID == lossId);
            if (fnolId == null)
                return null;
            return _db.luggage_insurance_info.Where(x => x.additional_info.ID == fnolId.Additional_infoID).SingleOrDefault();
        }

        public luggage_insurance_info isHealthInsurance(int lossId)
        {
            first_notice_of_loss fnolId = _db.first_notice_of_loss.Single(x => x.ID == lossId);
            if (fnolId == null)
                return null;
            luggage_insurance_info test = _db.luggage_insurance_info.Where(x => x.additional_info.ID == fnolId.Additional_infoID).SingleOrDefault();
            return test;
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
        public List<first_notice_of_loss> GetByPolicyId(int policy_Id)
        {
            return _db.first_notice_of_loss.Where(x => x.PolicyId == policy_Id).ToList();
        }
        public List<string> GetInvoiceDocumentName(int lossID)
        {
            var allDoc = new List<string>();
            var documents = _db.documents_first_notice_of_loss.Where(x => x.First_notice_of_lossID == lossID).ToList();
            if (documents.Count() > 0)
            {
                foreach (var doc in documents)
                {
                    var file = _db.documents.Where(x => x.ID == doc.DocumentID).FirstOrDefault();
                    var invoices = _db.invoices.Where(x => x.DocumentID == file.ID).FirstOrDefault();
                    if (invoices != null)
                    {
                        allDoc.Add(file.Name);
                    }

                }
            }
            return allDoc;
        }

        public List<string> GetHealthLuggageDocumentName(int lossID)
        {
            var allDoc = new List<string>();
            var documents = _db.documents_first_notice_of_loss.Where(x => x.First_notice_of_lossID == lossID).ToList();
            if (documents.Count() > 0)
            {
                foreach (var doc in documents)
                {
                    var file = _db.documents.Where(x => x.ID == doc.DocumentID ).FirstOrDefault();
                    var invoices = _db.invoices.Where(x => x.DocumentID == file.ID).FirstOrDefault();
                    if (file != null && invoices == null)
                    {
                        allDoc.Add(file.Name);
                    }
                }
            }
            return allDoc;
        }

        public List<first_notice_of_loss> GetFNOLBySearchValues(string username, string PolicyNumber, string FNOLNumber, string holderName, string holderLastName, string clientName, string clientLastName, string insuredName, string insuredLastName, string totalPrice, string healthInsurance, string luggageInsurance)
        {
            string userID = _db.aspnetusers.Where(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();
            float totalPricefloat = 0;
            if (!String.IsNullOrEmpty(totalPrice))
            {
                float.TryParse(totalPrice, out totalPricefloat);
            }
            if (healthInsurance == "true")
            {
                return _db.first_notice_of_loss.Where(x =>
                                   (x.travel_policy.Policy_Number == PolicyNumber || String.IsNullOrEmpty(PolicyNumber)) &&
                                   (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                   (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                   (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                   (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                   (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                   (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                   (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                   (x.additional_info.health_insurance_info != null) && (x.CreatedBy == userID)).ToList();
            }
            if (luggageInsurance == "true")
            {
                return _db.first_notice_of_loss.Where(x =>
                                  (x.travel_policy.Policy_Number == PolicyNumber || String.IsNullOrEmpty(PolicyNumber)) &&
                                  (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                  (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                  (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                  (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                  (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                  (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                  (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                  (x.additional_info.luggage_insurance_info != null) && (x.CreatedBy == userID)).ToList();
            }
            else
            {
                return _db.first_notice_of_loss.Where(x =>
                               (x.travel_policy.Policy_Number == PolicyNumber || String.IsNullOrEmpty(PolicyNumber)) &&
                                (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                (x.CreatedBy == userID)).ToList();
            }

        }

        public List<first_notice_of_loss> GetBrokersFnols(string userId, DateTime dateFrom)
        {
            if (userId == "")
                return null;
            return _db.first_notice_of_loss.Where(x => x.travel_policy.Created_By == userId && x.CreatedDateTime >= dateFrom).ToList();
        }

        public List<first_notice_of_loss> GetBrokeManagerFnols(string userId, DateTime dateFrom)
        {
            if (userId == "")
                return null;
            List<string> brokersUsers =
                _db.aspnetusers.Where(x => x.CreatedBy == userId).Select(x => x.Id).ToList();
            return _db.first_notice_of_loss.Where(x => (x.travel_policy.Created_By == userId || brokersUsers.Contains(x.travel_policy.Created_By)) && 
                                                        x.CreatedDateTime >= dateFrom).ToList();

        }

        public List<first_notice_of_loss> GetFNOLForBrokerBySearchValues(string username, string PolicyNumber, string FNOLNumber, string holderName, string holderLastName, string clientName, string clientLastName, string insuredName, string insuredLastName, string totalPrice, string healthInsurance, string luggageInsurance)
        {
            string userID = _db.aspnetusers.Where(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();
            float totalPricefloat = 0;
            if (!String.IsNullOrEmpty(totalPrice))
            {
                float.TryParse(totalPrice, out totalPricefloat);
            }
            if (healthInsurance == "true")
            {
                return _db.first_notice_of_loss.Where(x =>
                                   (x.travel_policy.Policy_Number == PolicyNumber || String.IsNullOrEmpty(PolicyNumber)) &&
                                   (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                   (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                   (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                   (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                   (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                   (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                   (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                   (x.additional_info.health_insurance_info != null) && (x.CreatedBy == userID || x.travel_policy.Created_By == userID)).ToList();
            }
            if (luggageInsurance == "true")
            {
                return _db.first_notice_of_loss.Where(x =>
                                  (x.travel_policy.Policy_Number == PolicyNumber || String.IsNullOrEmpty(PolicyNumber)) &&
                                  (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                  (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                  (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                  (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                  (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                  (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                  (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                  (x.additional_info.luggage_insurance_info != null) && (x.CreatedBy == userID || x.travel_policy.Created_By == userID)).ToList();
            }
            else
            {
                return _db.first_notice_of_loss.Where(x =>
                               (x.travel_policy.Policy_Number == PolicyNumber || String.IsNullOrEmpty(PolicyNumber)) &&
                                (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                (x.CreatedBy == userID || x.travel_policy.Created_By == userID)).ToList();
            }

        }


        //Get BrokerManager's fnols and the fnols created by user he created
        public List<first_notice_of_loss> GetFNOLForBrokerManagerBySearchValues(string username, string PolicyNumber, string FNOLNumber, string holderName, string holderLastName, string clientName, string clientLastName, string insuredName, string insuredLastName, string totalPrice, string healthInsurance, string luggageInsurance)
        {
            string userID = _db.aspnetusers.Where(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();
            List<string> brokersUsers =
            _db.aspnetusers.Where(x => x.CreatedBy == userID).Select(x => x.Id).ToList();
            float totalPricefloat = 0;

            if (!String.IsNullOrEmpty(totalPrice))
            {
                float.TryParse(totalPrice, out totalPricefloat);
            }

            if (healthInsurance == "true")
            {
                return _db.first_notice_of_loss.Where(x =>
                                   (x.travel_policy.Policy_Number == PolicyNumber || String.IsNullOrEmpty(PolicyNumber)) &&
                                   (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                   (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                   (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                   (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                   (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                   (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                   (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                   (x.additional_info.health_insurance_info != null) && (x.CreatedBy == userID || x.travel_policy.Created_By == userID || brokersUsers.Contains(x.travel_policy.Created_By) )).ToList();
            }
            if (luggageInsurance == "true")
            {
                return _db.first_notice_of_loss.Where(x =>
                                  (x.travel_policy.Policy_Number == PolicyNumber || String.IsNullOrEmpty(PolicyNumber)) &&
                                  (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                  (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                  (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                  (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                  (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                  (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                  (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                  (x.additional_info.luggage_insurance_info != null) && (x.CreatedBy == userID || x.travel_policy.Created_By == userID) || brokersUsers.Contains(x.CreatedBy)).ToList();
            }
            else
            {
                return _db.first_notice_of_loss.Where(x =>
                               (x.travel_policy.Policy_Number == PolicyNumber || String.IsNullOrEmpty(PolicyNumber)) &&
                                (x.FNOL_Number == FNOLNumber || String.IsNullOrEmpty(FNOLNumber)) &&
                                (x.insured.Name.Contains(insuredName) || String.IsNullOrEmpty(insuredName)) &&
                                (x.insured.Lastname.Contains(insuredLastName) || String.IsNullOrEmpty(insuredLastName)) &&
                                (x.travel_policy.insured.Name.Contains(holderName) || String.IsNullOrEmpty(holderName)) &&
                                (x.travel_policy.insured.Lastname.Contains(holderName) || String.IsNullOrEmpty(holderLastName)) &&
                                (x.travel_policy.policy_insured.Select(y => y.insured.Name).Contains(clientName) || String.IsNullOrEmpty(clientName)) &&
                                (x.travel_policy.policy_insured.Select(y => y.insured.Lastname).Contains(clientLastName) || String.IsNullOrEmpty(clientLastName)) &&
                                (x.CreatedBy == userID || x.travel_policy.Created_By == userID) || brokersUsers.Contains(x.travel_policy.Created_By)).ToList();
            }

        }

        public void UpdateClaimantBankAccountId(int fnolId, int bankAccountId)
        {
            var fnol = _db.first_notice_of_loss.Where(x => x.ID == fnolId).SingleOrDefault();
            fnol.Claimant_bank_accountID = bankAccountId;
            _db.SaveChanges();
        }

        public void UpdatePolicyHolderBankAccountId(int fnolId, int bankAccountId)
        {
            var fnol = _db.first_notice_of_loss.Where(x => x.ID == fnolId).SingleOrDefault();
            fnol.Policy_holder_bank_accountID = bankAccountId;
            _db.SaveChanges();
        }

        public void Update(first_notice_of_loss newFnol)
        {
            var fnol = _db.first_notice_of_loss.Where(x => x.ID == newFnol.ID).SingleOrDefault();
           if(fnol!=null)
            {
                fnol = newFnol;
            }
            _db.SaveChanges();
        }



        public string CreateFNOLNumber()
        {
            return (Int64.Parse(_db.first_notice_of_loss.OrderByDescending(f => f.FNOL_Number).Select(r => r.FNOL_Number).FirstOrDefault()) + 1).ToString();
        }

        public int LastDocumentId()
        {
            var lastDocumentId = _db.documents.OrderByDescending(x => x.ID).FirstOrDefault();
            if(lastDocumentId == null)
            {
                return 0;
            }
            else
            {
                return lastDocumentId.ID;
            }
        }

       
    }
}
