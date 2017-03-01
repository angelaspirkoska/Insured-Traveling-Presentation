using InsuredTraveling.ViewModels;
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
        public string username { get; set; }
        public int Id { get; set; }
        public bool? ShortDetailed { get; set; }
        public bool isMobile { get; set; }

        public int PolicyId { get; set; }

        public string FNOLNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public int PolicyNumber { get; set; }
        public List<SelectListItem> Policies { get; set; }
        public List<SelectListItem> PolicyNumberList { get; set; }
        public int ChatId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public bool IsArchived { get; set; }

        //PolicyHolderData
        public int PolicyHolderId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string PolicyHolderName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string  PolicyHolderAdress { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string PolicyHolderPhoneNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string PolicyHolderSsn { get; set; }

        public bool PolicyHolderExistentBankAccount { get; set; }
        public bool PolicyHolderForeignBankAccount { get; set; }
        public int PolicyHolderForeignBankAccountId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string PolicyHolderBankAccountNumber  { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string PolicyHolderBankName { get; set; }

        //ClaimantData

        public int ClaimantId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string ClaimantName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string ClaimantAdress { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string ClaimantPhoneNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string ClaimantSsn { get; set; }

        public bool ClaimantExistentBankAccount { get; set; }
        public bool ClaimantForeignBankAccount { get; set; }
        public int ClaimantForeignBankAccountId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string ClaimantBankAccountNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string ClaimantBankName { get; set; }

        public string RelationClaimantPolicyHolder { get; set; }

        //TravelData
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Destination { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public DateTime DepartDateTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public DateTime ArrivalDateTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public TimeSpan? DepartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public TimeSpan? ArriveTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string TransportMeans { get; set; }


        //AdditionalInfo

        public int AdditionalInfoId { get; set; }      
        public bool IsHealthInsurance { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public DateTime? AccidentDateTimeHealth { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public DateTime? AccidentDateTimeLuggage { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public TimeSpan? AccidentTimeHealth { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public TimeSpan? AccidentTimeLuggage { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string   AccidentPlaceHealth { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string  AccidentPlaceLuggage { get; set; }


        //LugaggeInsuranceInfo
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string PlaceDescription { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string DetailDescription { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string ReportPlace { get; set; }
        public string Floaters { get; set; }
        public string FloatersValue { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public TimeSpan? LugaggeCheckingTime { get; set; }

        //documents?
        public List<FileDescriptionViewModel> Invoices { get; set; }
        public List<FileDescriptionViewModel> InsuranceInfoDoc { get; set; }

        //HealthInsuranceInfo
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public DateTime? DoctorVisitDateTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string DoctorInfo { get; set; }
        public string MedicalCaseDescription { get; set; }
        public bool PreviousMedicalHistory { get; set; }
        public string ResponsibleInstitution { get; set; }

        //TotalCost
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public float TotalCost { get; set; }

    }
}