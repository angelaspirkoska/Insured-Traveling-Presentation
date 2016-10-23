using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    public class SearchClientsViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string SSN { get; set; }
        public string Phone_Number { get; set; }
        public string City { get; set; }
        public string Postal_Code { get; set; }
        public string Address { get; set; }
        public string Passport_Number_IdNumber { get; set; }
        public string Email { get; set; }
    }
}