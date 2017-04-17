using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    public class SearchSavaPolicyModel
    {
        public string PolicyId { get; set; }
        public string  PolicyNumber { get; set; }
        public string SSNInsured { get; set; }
        public string SSNHolder { get; set; }
        public string ExpireDate { get; set; }
        public string Premium { get; set; }
        public string EmailSeller { get; set; }
        public string Points { get; set; }
    }
}