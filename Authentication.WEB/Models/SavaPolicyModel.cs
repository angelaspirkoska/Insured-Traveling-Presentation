using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class SavaPolicyModel
    {
        public int id { get; set; }
        public int policy_number { get; set; }
        public string SSN_insured { get; set; }
        public string SSN_policyHolder { get; set; }
        public DateTime expiry_date { get; set; }
        public int premium { get; set; }
        public string email_seller { get; set; }
        public float discount_points { get; set; }

    }
}