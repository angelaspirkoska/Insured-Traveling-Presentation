﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class Policy
    {
        public string Policy_Number { get; set; }

        [Display(Name = "Курс")]
        public int Exchange_RateID { get; set; }

        [Display(Name = "Земја на патување:")]
        public int CountryID { get; set; }

        [Display(Name = "Вид на полиса:")]
        public int Policy_TypeID { get; set; }

        [Display(Name = "Франшиза:")]
        public int Retaining_RiskID { get; set; }

        [Display(Name = "Франшиза според возраст:")]
        public string Franchise_Age { get; set; }

        [Display(Name = "Започнува на:")]
        public DateTime Start_Date { get; set; }

        [Display(Name = "Завршува на:")]
        public DateTime End_Date { get; set; }

        [Display(Name = "Важи денови: ")]
        public int Valid_Days { get; set; }

        [Display(Name = "Франшиза според денови:")]
        public string Franchise_Days { get; set; }

        [Display(Name = "Број на патувања: ")]
        public int Travel_NumberID { get; set; }

        [Display(Name = "Вид на осигурителна полиса:")]
        public int Travel_Insurance_TypeID { get; set; }

        [Display(Name = "Број на членови: ")]
        public int Group_Members { get; set; }

        [Display(Name = "Вкупна премија во група:")]
        public double Group_Total_Premium { get; set; }

        [Display(Name = "Вкупна премија: ")]
        public double? Total_Premium { get; set; }


        public string Created_By { get; set; }
        public DateTime Date_Created { get; set; }
        public DateTime Date_Modified { get; set; }
        public string Modified_By { get; set; }
        public int StatusID { get; set; }
        public DateTime Date_Cancellation { get; set; }

        [Display(Name = "Име: ")]
        public string Name { get; set; }
        [Display(Name = "Презиме: ")]
        public string LastName { get; set; }
        [Display(Name = "Адреса: ")]
        public string Address { get; set; }
        [Display(Name = "Матичен број:")]
        public string SSN { get; set; }
        [Display(Name = "Број на пасош: ")]
        public string PassportNumber_ID { get; set; }

        public List<insured> insureds { get; set; }
        public List<additional_charge> additional_charges {get;set;}
    }
}
