using System;

namespace Authentication.WEB.Models
{
    public class Policy
    {
        public int policyNumber { get; set; }
        public string zemjaNaPatuvanje { get; set; }
        public double kurs { get; set; }
        public double procentFranchise { get; set; } // ?
        public string Franchise { get; set; } //Deductable
        public string vidPolisa { get; set; }
        public string ednoPatuvanje { get; set; } // true false ??
        public string povekePatuvanja { get; set; } // true false ??
        public string polisaPoedinecno { get; set; }
        public string polisaFamilijarno { get; set; }
        public string polisaGrupno { get; set; }
        public string brojPatuvanja { get; set; }
        public double? VkupnaPremija { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int vaziDenovi { get; set; }
        public double popustDenovi { get; set; }
        public string imePrezime { get; set; }
        public string Adresa { get; set; }
        public string EMBG { get; set; }
        public string brojPasos { get; set; }
        public string brojOsigureniciVid { get; set; }
        public int brojLicaGrupa { get; set; }
        public string doplatok1 { get; set; }
        public string doplatok2 { get; set; }
    }
}
