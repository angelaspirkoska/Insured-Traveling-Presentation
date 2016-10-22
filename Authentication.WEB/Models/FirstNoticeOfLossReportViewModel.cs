using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace InsuredTraveling.Models
{
    public class FirstNoticeOfLossReportViewModel
    {
        public FirstNoticeOfLossReportViewModel()
        {
            Policies = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public bool? ShortDetailed { get; set; }
        public bool? WebMobile { get; set; }

        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        public int PolicyNumber { get; set; }
        public List<SelectListItem> Policies { get; set; }
        public List<SelectListItem> PolicyNumberList { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }


        //PolicyHolderData
        public int PolicyHolderId { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderName { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string  PolicyHolderAdress { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderPhoneNumber { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderSsn { get; set; }

        public bool PolicyHolderExistentBankAccount { get; set; }
        public bool PolicyHolderForeignBankAccount { get; set; }
        public int PolicyHolderForeignBankAccountId { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderBankAccountNumber  { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderBankName { get; set; }

        //ClaimantData

        public int ClaimantId { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string ClaimantName { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string ClaimantAdress { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string ClaimantPhoneNumber { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string ClaimantSsn { get; set; }

        public bool ClaimantExistentBankAccount { get; set; }
        public bool ClaimantForeignBankAccount { get; set; }
        public int ClaimantForeignBankAccountId { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string ClaimantBankAccountNumber { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string ClaimantBankName { get; set; }

        public string RelationClaimantPolicyHolder { get; set; }

        //TravelData
        [Required(ErrorMessage = "Полето е задолжително")]
        public string Destination { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public DateTime DepartDateTime { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public DateTime ArrivalDateTime { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public TimeSpan? DepartTime { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        public TimeSpan? ArriveTime { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string TransportMeans { get; set; }


        //AdditionalInfo

        public int AdditionalInfoId { get; set; }      
        public bool IsHealthInsurance { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public DateTime? AccidentDateTimeHealth { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public DateTime? AccidentDateTimeLuggage { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public TimeSpan? AccidentTimeHealth { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public TimeSpan? AccidentTimeLuggage { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string   AccidentPlaceHealth { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string  AccidentPlaceLuggage { get; set; }


        //LugaggeInsuranceInfo
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PlaceDescription { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string DetailDescription { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string ReportPlace { get; set; }
        public string Floaters { get; set; }
        public string FloatersValue { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public TimeSpan? LugaggeCheckingTime { get; set; }

        //documents?

        //HealthInsuranceInfo
        [Required(ErrorMessage = "Полето е задолжително")]
        public DateTime? DoctorVisitDateTime { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string DoctorInfo { get; set; }
        public string MedicalCaseDescription { get; set; }
        public bool PreviousMedicalHistory { get; set; }
        public string ResponsibleInstitution { get; set; }

        //TotalCost
        [Required(ErrorMessage = "Полето е задолжително")]
        public float TotalCost { get; set; }


    }
}