using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace InsuredTraveling.Models
{
    public class FNOL
    {
        public string username { get; set; }
        public string insuredName { get; set; }
        public string insuredEMBG { get; set; }
        public string insuredAddress { get; set; }
        public string insuredTransactionAccount { get; set; }
        public string insuredPhone { get; set; }
        public string deponentInsured { get; set; }
        public string relationship { get; set; }
        public string travelDestination { get; set; }
        public DateTime travelDateFrom { get; set; }
        public TimeSpan travelTimeFrom { get; set; }
        public DateTime travelDateTo { get; set; }
        public TimeSpan travelTimeTo { get; set; }
        public string transportationType { get; set; }

        //Health Insurance
        public DateTime lossDate { get; set; }
        public TimeSpan lossTime { get; set; }
        public string placeLoss { get; set; }
        public string DoctorInfo { get; set; }
        public string illnessInfo { get; set; }
        public string documentsHanded { get; set; }
        public string additionalInfo { get; set; }

        //Lugguage insurance
        public DateTime baggageLossDate { get; set; }
        public string placeBaggageLoss { get; set; }
        public string placeReported { get; set; }
        public string descriptionLostStolenThings { get; set; }
        public string detailedDescription { get; set; }
        public string documentsHanded2 { get; set; }
        public TimeSpan airportArrivalTime { get; set; }
        public TimeSpan baggageDropTime { get; set; }


        public string additionalDocumentsHanded { get; set; }
        public int valueExpenses { get; set; }
        public int policyNumber { get; set; }
        public Boolean HealthInsurance {get;set;}
        public Boolean LuggageInsurance { get; set; }
        public Boolean WebMobile { get; set; }
        public Boolean ShortDetailed { get; set; }
        public string message { get; set; }

    }
}