using System;

namespace Authentication.WEB.Models
{
    public class Policy
    {
        public int policyNumber { get; set; }
        public int countryID { get; set; }
        public double exchnage_rate { get; set; }
        public string Franchise { get; set; } //Deductable
        public int typePolicyID { get; set; }
        public int numberTravelID { get; set; } // true false ??
        public int TravelInsuranceTypeID { get; set; }
        public double? TotalPremium { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int validDays { get; set; }
        public int GroupMembers { get; set; }
        public string doplatok1 { get; set; }
        public string doplatok2 { get; set; }
    }
}
