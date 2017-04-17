using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class QRCodeSavaPolicy
    {
        public string PolicyNumber { get; set; }
        public string SellerNameLastName { get; set; }
        public string SSNInsured { get; set; }
        public string SSNHolder { get; set; }
        public string ExpireDate { get; set; }
        public string Premium { get; set; }
        public string EmailSeller { get; set; }

    }
}