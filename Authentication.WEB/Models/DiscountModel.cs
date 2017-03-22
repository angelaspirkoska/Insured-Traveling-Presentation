using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.Models
{
    public class DiscountModel
    {
        public int Id { get; set; }
        public string Discount_Name { get; set; }
        public double Discount_Coef { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set;  }
    
    }
}