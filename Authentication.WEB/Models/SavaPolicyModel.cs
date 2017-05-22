using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class SavaPolicyModel
    {
        public int id { get; set; }
        public string policy_number { get; set; }
        public string SSN_insured { get; set; }
        public string SSN_policyHolder { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yyyy}")]
        public DateTime date_created { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yyyy}")]
        public DateTime expiry_date { get; set; }
        public int premium { get; set; }
        public string email_seller { get; set; }
        public float discount_points { get; set; }

    }
}