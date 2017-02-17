using InsuredTraveling.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;


namespace InsuredTraveling.ViewModels
{
    public class FirstNoticeOfLossEditViewModel
    {
        public FirstNoticeOfLossEditViewModel()
        {
            PolicyHolderBankAccounts = new List<bank_account_info>();
            ClaimantBankAccounts = new List<bank_account_info>();
            Invoices = new List<FileDescriptionViewModel>();
            InsuranceInfoDoc = new List<FileDescriptionViewModel>();
        }

        public string username { get; set; }
        public int Id { get; set; }
        public string FNOLNumber { get; set; }
        public bool? ShortDetailed { get; set; }
        public JObject BankPrefixes { get; set; }

        public int PolicyId { get; set; }
        public int PolicyNumber { get; set; }
        public string Message { get; set; }

        public DateTime ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }

        //PolicyHolderData
        public int PolicyHolderId { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderName { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderAdress { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderPhoneNumber { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderSsn { get; set; }
        public bool PolicyHolderExistentBankAccount { get; set; }
        public bool PolicyHolderBankAccount { get; set; }
        public int PolicyHolderBankAccountId { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderBankAccountNumber { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string PolicyHolderBankName { get; set; }
        public List<bank_account_info> PolicyHolderBankAccounts { get; set; }

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
        public bool ClaimantBankAccount { get; set; }
        public int ClaimantBankAccountId { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        public string ClaimantBankAccountNumber { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string ClaimantBankName { get; set; }
        public List<bank_account_info> ClaimantBankAccounts { get; set; }
        public string RelationClaimantPolicyHolder { get; set; }
        //TravelData
        [Required(ErrorMessage = "Полето е задолжително")]
        public string Destination { get; set; }
        
        [Required(ErrorMessage = "Полето е задолжително")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yy}")]
        public DateTime DepartDateTime { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yy}")]
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yy}")]
        public DateTime? AccidentDateTimeHealth { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yy}")]
        [Required(ErrorMessage = "Полето е задолжително")]
        public DateTime? AccidentDateTimeLuggage { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public TimeSpan? AccidentTimeHealth { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public TimeSpan? AccidentTimeLuggage { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string AccidentPlaceHealth { get; set; }
        [Required(ErrorMessage = "Полето е задолжително")]
        public string AccidentPlaceLuggage { get; set; }
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
        public List<FileDescriptionViewModel> Invoices { get; set; }
        public List<FileDescriptionViewModel> InsuranceInfoDoc { get; set; }
        //HealthInsuranceInfo
        [Required(ErrorMessage = "Полето е задолжително")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yy}")]
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