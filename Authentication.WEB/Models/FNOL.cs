using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Timers;
using System.Web;

namespace InsuredTraveling.Models
{
    public class FNOL
    {
        //Data of insured, current logged user
        public string username { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        public string TransactionAccount { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string deponent { get; set; }

        //Data of claimant(insured)
        [Required(ErrorMessage = "Полето е задолжително")]
        public string insuredName { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string insuredEMBG { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string insuredAddress { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string insuredTransactionAccount { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string insuredPhone { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string deponentInsured { get; set; }
        public string relationship { get; set; }

        //Data of trip
        [Required(ErrorMessage = "Полето е задолжително")]
        public string travelDestination { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public DateTime? travelDateFrom { get; set; }
        public TimeSpan? travelTimeFrom { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public DateTime? travelDateTo { get; set; }
        public TimeSpan? travelTimeTo { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string transportationType { get; set; }

        //Health Insurance
        //[Required(ErrorMessage = "Полето е задолжително")]
        public DateTime? lossDate { get; set; }
        public TimeSpan? lossTime { get; set; }
        //[Required(ErrorMessage = "Полето е задолжително")]
        public string placeLoss { get; set; }
        //[Required(ErrorMessage = "Полето е задолжително")]
        public string DoctorInfo { get; set; }
        public string illnessInfo { get; set; }
        public string documentsHanded { get; set; }
        public string additionalInfo { get; set; }

        //Lugguage insurance
        //[Required(ErrorMessage = "Полето е задолжително")]
        public DateTime? baggageLossDate { get; set; }
        //[Required(ErrorMessage = "Полето е задолжително")]
        public string placeBaggageLoss { get; set; }
       // [Required(ErrorMessage = "Полето е задолжително")]
        public string placeReported { get; set; }
        public string descriptionLostStolenThings { get; set; }
        public string detailedDescription { get; set; }
        public string documentsHanded2 { get; set; }
        //[Required(ErrorMessage = "Полето е задолжително")]
        public TimeSpan? airportArrivalTime { get; set; }
      //  [Required(ErrorMessage = "Полето е задолжително")]
        public TimeSpan? baggageDropTime { get; set; }


        //Data of costs and additional info 
        public string additionalDocumentsHanded { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public int? valueExpenses { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public int? policyNumber { get; set; }
        public string PolicyType { get; set; }
        public bool? HealthInsurance {get;set;}
        public bool? LuggageInsurance { get; set; }
        public bool? WebMobile { get; set; }
        public bool? ShortDetailed { get; set; }
        public string message { get; set; }
        public string insurance { get; set; }

    }
}