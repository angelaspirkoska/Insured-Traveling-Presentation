using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    [Serializable]
    public class BankAccountInfoViewModel
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
    }
}