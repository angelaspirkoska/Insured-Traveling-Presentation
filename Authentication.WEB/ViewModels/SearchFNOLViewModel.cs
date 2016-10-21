using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    public class SearchFNOLViewModel
    {
        public int ID { get; set; }
        public string PolicyNumber { get; set; }
        public string InsuredName { get; set; }
        public string ClaimantPersonName { get; set; }
        public string Claimant_insured_relation { get; set; }
        public string AdditionalDocumentsHanded { get; set; }
        public string AllCosts { get; set; }
        public string Date { get; set; }
        public string HealthInsurance { get; set; }
        public string LuggageInsurance { get; set; }
    }
}