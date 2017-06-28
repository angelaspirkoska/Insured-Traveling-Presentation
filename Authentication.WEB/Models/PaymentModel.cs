using InsuredTraveling;
using System;

namespace InsuredTraveling.Models
{
    public class PaymentModel
    {
        public travel_policy Pat { get; set; }
        public string PolicyNumber { get; set; }
        public insured mainInsured { get; set; }
        public string additionalCharge1 { get; set; }
        public string additionalCharge2 { get; set; }
        public string retaining_risk_mk { get; set; }
        public string retaining_risk { get; set; }
        public string clientId { get; set; }
        public string amount { get; set; }
        public string oid { get; set; }
        public string okUrl { get; set; }
        public string failUrl { get; set; }

        public string taksit { get; set; }
        public string rnd { get; set; }
        public string hashstr { get; set; }
        public string currency { get; set; }
        public string storetype { get; set; }
        public string refreshtime { get; set; }

        public string transactionType { get; set; }
        public string storekey { get; set; }
        public byte[] hashbytes { get; set; }
        public byte[] inputbytes { get; set; }
        public string instalmentCount { get; set; }
        public String hash { get; set; }
        public string Password { get; internal set; }
        public string Username { get; internal set; }

        #region New Paremeters
        public string ConfigPolicyType { get; set; }
        public int IdPolicy { get; set; }
        public string PolicyHolderNameLastName { get; set; }
        public string PolicyHolderAddress { get; set; }
        public string PolicyHolderPassport { get; set; }
        public string PolicyHolderSSN { get; set; }
        public DateTime PolicyHolderDateBirth { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public decimal Premium { get; set; }
        public string InsuredNameLastName { get; set; }
        public string InsuredAddress { get; set; }
        public string InsuredPassport { get; set; }
        public string InsuredSSN { get; set; }
        public DateTime InsuredDateBirth { get; set; }
        #endregion
    }
}
