using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class SavaVoucherModel
    {
        public string voucher_code { get; set; }
        public string id_policyHolder { get; set; }
        public float points_used {get;set;}
        public string id_seller { get; set; }
        public DateTime timestamp { get; set; }
    }
}