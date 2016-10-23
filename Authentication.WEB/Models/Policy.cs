using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class Policy
    {
        public string Policy_Number { get; set; }

        public int PaymentStatys = 0;

        public bool isMobile { get; set; }

        public string username { get; set; }

        [Display(Name = "Курс")]
        public int? Exchange_RateID { get; set; }

        [Required]
        [Display(Name = "Земја на патување:")]
        public int CountryID { get; set; }

        [Required]
        [Display(Name = "Вид на полиса:")]
        public int Policy_TypeID { get; set; }

        [Required]
        [Display(Name = "Франшиза:")]
        public int Retaining_RiskID { get; set; }

        [Display(Name = "Франшиза според возраст:")]
        public string Franchise_Age { get; set; }

        [Required]
        [Display(Name = "Започнува на:")]
        public DateTime Start_Date { get; set; }

        [Required]
        [Display(Name = "Завршува на:")]
        public DateTime End_Date { get; set; }

        [Required]
        [Display(Name = "Важи денови: ")]
        public int Valid_Days { get; set; }

        [Display(Name = "Франшиза според денови:")]
        public string Franchise_Days { get; set; }

        [Required]
        [Display(Name = "Број на патувања: ")]
        public int Travel_NumberID { get; set; }

        [Required]
        [Display(Name = "Вид на осигурителна полиса:")]
        public int Travel_Insurance_TypeID { get; set; }

        [Display(Name = "Број на членови: ")]
        public int? Group_Members { get; set; }

        [Display(Name = "Вкупна премија во група:")]
        public double? Group_Total_Premium { get; set; }

        [Display(Name = "Вкупна премија: ")]
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



        [Display(Name = "Име: ")]
        public string PolicyHolderName { get; set; }


        [Display(Name = "Презиме: ")]
        public string PolicyHolderLastName { get; set; }


        [Display(Name = "Адреса: ")]
        public string PolicyHolderAddress { get; set; }


        [Display(Name = "Email: ")]
        public string PolicyHolderEmail { get; set; }


        [Display(Name = "Дата на раѓање: ")]
        public DateTime PolicyHolderBirthDate { get; set; }


        [Display(Name = "Град: ")]
        public string PolicyHolderCity { get; set; }


        [Display(Name = "Поштенски број: ")]
        public string PolicyHolderPostalCode { get; set; }


        [Display(Name = "Матичен број:")]
        public string PolicyHolderSSN { get; set; }


        [Display(Name = "Број на пасош/лична карта: ")]
        public string PolicyHolderPassportNumber_ID { get; set; }


        [Display(Name = "Телефонски број: ")]
        public string PolicyHolderPhoneNumber { get; set; }


        //Podatoci za osigurenik
        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Име: ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Презиме: ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Адреса: ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Град: ")]
        public string City { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Поштенски број: ")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Дата на раѓање: ")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Матичен број:")]
        public string SSN { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Број на пасош/лична карта: ")]
        public string PassportNumber_ID { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Телефонски број: ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Email: ")]
        public string Email { get; set; }

        public int AdditionalChargeId1 { get; set; }
        public int AdditionalChargeId2 { get; set; }

        public List<insured> insureds { get; set; }
        public List<additional_charge> additional_charges {get;set;}
    }
}
