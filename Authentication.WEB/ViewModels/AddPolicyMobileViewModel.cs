using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    public class AddPolicyMobileViewModel
    {
        public AddPolicyMobileViewModel()
        {
            Insureds = new List<insured>();
            Additional_charges = new List<int>();
        }
        public string Username { get; set; }
        public string Policy_Number { get; set; }
        public int Exchange_RateID { get; set; }
        public int CountryID { get; set; }
        public int Policy_TypeID { get; set; }
        public int Retaining_RiskID { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public int Valid_Days { get; set; }
        public int Travel_NumberID { get; set; }
        public int Travel_Insurance_TypeID { get; set; }
        public double? Total_Premium { get; set; }
        public string Created_By { get; set; }
        public DateTime Date_Created { get; set; }
        public DateTime Date_Modified { get; set; }
        public string Modified_By { get; set; }
        public bool Payment_Status { get; set; }
        public DateTime? Date_Cancellation { get; set; }
        public int PolicyHolderId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public DateTime DateBirth { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string City { get; set; }
        public string Postal_Code { get; set; }
        public string Address { get; set; }
        public string Passport_Number_IdNumber { get; set; }
        public int Type_InsuredID { get; set; }
        public List<insured> Insureds { get; set; }
        public List<int> Additional_charges { get; set; }

        
    }
}