using System;
using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class CreateClientModel
    {
        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name="Име: ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Презиме: ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Полето треба да е составено само од цифри")]
        [Display(Name = "Матичен број: ")]
        public string SSN { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Датум на раѓање: ")]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Возраст: ")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [EmailAddress(ErrorMessage = "Адресата не е валидна")]
        [Display(Name = "Е - маил: ")]
        public string Email { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Полето треба да е составено само од цифри")]
        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Телефонски број: ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Град: ")]
        public string City { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Полето треба да е составено само од цифри")]
        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Пошетенски број: ")]
        public string Postal_Code { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Адреса: ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Полето е задолжително")]
        [Display(Name = "Број на пасош/идентификација: ")]
        public string Passport_Number_IdNumber { get; set; }

        public int Type_InsuredID { get; set; }
        public DateTime Date_Created { get; set; }
        public string Created_By { get; set; }
        public DateTime Date_Modified { get; set; }
        public string Modified_By { get; set; }
    }
}
