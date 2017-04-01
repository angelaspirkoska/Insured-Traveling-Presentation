using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class CreditCardInfoModel
    { 
        public CreditCardInfoModel()
        {

        }

        public CreditCard CreditCardInfo { get; set;} 
        public string Amount { get; set; }
        public int ?OrderId { get; set; }

    }

    public class CreditCard
    {
        public string Pan { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string Cvv { get; set; }


    }
}