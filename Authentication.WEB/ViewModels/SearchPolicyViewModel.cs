using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsuredTraveling.ViewModels
{
    public class SearchPolicyViewModel
    {
        public int Polisa_Id { get; set; }
        public string PolicyHolderName { get; set; }
        public string Polisa_Broj { get; set; }
        public string Country { get; set; }
        public string Policy_type { get; set; }
        public string Zapocnuva_Na { get; set; }
        public string Zavrsuva_Na { get; set; }
        public string Datum_Na_Izdavanje { get; set; }
        public string Datum_Na_Storniranje { get; set; }
    }
}