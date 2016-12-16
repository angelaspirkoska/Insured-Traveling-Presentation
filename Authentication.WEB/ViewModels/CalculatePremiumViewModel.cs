using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    public class CalculatePremiumViewModel
    {
        public CalculatePremiumViewModel()
        {
            Policy_Holder = new insured();
            Insureds = new List<insured>();
            additional_charges = new List<int>();
            Additional_charges = new List<additional_charge>();
        }
        public int Exchange_RateID { get; set; }
        public int CountryID { get; set; }
        public int Policy_TypeID { get; set; }
        public int Retaining_RiskID { get; set; }
        public DateTime Start_Date { get; set; }
        public int Valid_Days { get; set; }
        public int Group_Members { get; set; }
        public insured Policy_Holder { get; set; }
        public List<additional_charge> Additional_charges { get; set; }
        public List<insured> Insureds { get; set; }
        public List<int> additional_charges { get; set; }
    }
}