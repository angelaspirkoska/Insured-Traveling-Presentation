using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    [Serializable]
    public class BankPrefixViewModel
    {
        public int Prefix { get; set; }
        public string BankName { get; set; }
        //public int Bank_ID { get; set; }
    }
}