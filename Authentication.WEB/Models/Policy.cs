using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class Policy
    {
        public string Policy_Number { get; set; }

        public int PaymentStatys { get; set; }

        public bool isMobile { get; set; }

        public string username { get; set; }

        [Display(Name = "Курс")]
        public int? Exchange_RateID { get; set; }

        [Required]
        [Display(Name = "Policy_TravelCountry", ResourceType = typeof(Resource))]
        public int CountryID { get; set; }

        [Required]
        [Display(Name = "Policy_PolicyType", ResourceType = typeof(Resource))]
        public int Policy_TypeID { get; set; }

        [Required]
        [Display(Name = "Policy_Deductible", ResourceType = typeof(Resource))]
        public int Retaining_RiskID { get; set; }

        [Display(Name = "Policy_DeductibleByAge")]
        public string Franchise_Age { get; set; }

        [Required]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yy}")]
        [Display(Name = "Policy_PolicyEffectiveDate", ResourceType = typeof(Resource))]
        public DateTime Start_Date { get; set; }

        public string test { get; set; }

        [Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yy}")]
        [Display(Name = "Policy_PolicyExpiryDate", ResourceType = typeof(Resource))]       
        public DateTime End_Date { get; set; }

        [Required]
        [Display(Name = "Policy_Duration", ResourceType = typeof(Resource))]
        public int Valid_Days { get; set; }

        [Display(Name = "Policy_DeductibleByDay", ResourceType = typeof(Resource))]
        public string Franchise_Days { get; set; }

        [Required]
        [Display(Name = "Policy_NumberTrips", ResourceType = typeof(Resource))]
        public int Travel_NumberID { get; set; }

        [Required]
        [Display(Name = "Policy_Type", ResourceType = typeof(Resource))]
        public int Travel_Insurance_TypeID { get; set; }

        [Display(Name = "Policy_NumberMembers", ResourceType = typeof(Resource))]
        public int? Group_Members { get; set; }

        [Display(Name = "Policy_TotalPremiumInGroup", ResourceType = typeof(Resource))]
        public double? Group_Total_Premium { get; set; }

        [Display(Name = "Policy_TotalPremium", ResourceType = typeof(Resource))]
        public double? Total_Premium { get; set; }

        public string Created_By { get; set; }

        public DateTime? Date_Created { get; set; }
        public DateTime? Date_Modified { get; set; }
        public string Modified_By { get; set; }
        public int? StatusID { get; set; }
        public DateTime? Date_Cancellation { get; set; }

        //Podatoci za dogovoruvac (policyHolder)

        public bool IsSamePolicyHolderInsured { get; set; }
        public bool IsExistentPolicyHolder { get; set; }
        public int PolicyHolderId { get; set; }

        [Display(Name = "Policy_HolderName", ResourceType = typeof(Resource))]
        public string PolicyHolderName { get; set; }

        [Display(Name = "Policy_HolderLastName", ResourceType = typeof(Resource))]
        public string PolicyHolderLastName { get; set; }

        [Display(Name = "Policy_HolderAddress", ResourceType = typeof(Resource))]
        public string PolicyHolderAddress { get; set; }

        [Display(Name = "Policy_HolderEmail", ResourceType = typeof(Resource))]
        public string PolicyHolderEmail { get; set; }

        [Display(Name = "Policy_HolderBirthDay", ResourceType = typeof(Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yy}")]
        public DateTime PolicyHolderBirthDate { get; set; }

        [Display(Name = "Policy_HolderCity", ResourceType = typeof(Resource))]
        public string PolicyHolderCity { get; set; }

        [Display(Name = "Policy_HolderPostalCode", ResourceType = typeof(Resource))]
        public string PolicyHolderPostalCode { get; set; }

        [Display(Name = "Policy_HolderSSN", ResourceType = typeof(Resource))]
        public string PolicyHolderSSN { get; set; }

        [Display(Name = "Policy_HolderPassport", ResourceType = typeof(Resource))]
        public string PolicyHolderPassportNumber_ID { get; set; }

        [Display(Name = "Policy_HolderPhone", ResourceType = typeof(Resource))]
        public string PolicyHolderPhoneNumber { get; set; }

        //Podatoci za osigurenik
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredName", ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredLastName", ResourceType = typeof(Resource))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredAddress", ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredCity", ResourceType = typeof(Resource))]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredPostalCode", ResourceType = typeof(Resource))]
        public string PostalCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredBirthDate", ResourceType = typeof(Resource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{00:dd/MM/yy}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredSSN", ResourceType = typeof(Resource))]
        public string SSN { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredPassport", ResourceType = typeof(Resource))]
        public string PassportNumber_ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredPhone", ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Policy_InsuredEmail", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        public int AdditionalChargeId1 { get; set; }
        public int AdditionalChargeId2 { get; set; }

        public List<insured> insureds { get; set; }
        public List<additional_charge> additional_charges {get;set;}

        public Policy()
        {
            PaymentStatys = 0;
        }

    }
}
