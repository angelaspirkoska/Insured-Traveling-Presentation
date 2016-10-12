using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    [Serializable]
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string SSN { get; set; }
        //public System.DateTime DateBirth { get; set; }
        //public Nullable<int> Age { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        //public string Postal_Code { get; set; }
        public string Address { get; set; }
    }
}