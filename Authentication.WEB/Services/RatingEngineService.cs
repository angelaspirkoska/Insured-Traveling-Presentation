using Authentication.WEB.Models;
using System;
using System.Linq;
using InsuredTraveling;

namespace Authentication.WEB.Services
{
    class RatingEngineService
    {
        public double? procentZemjaPatuvanje(string zemjaNaPatuvanje, string vidPolisa, string fransiza)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.p_zemja_na_patuvanje.Where(x => x.Zemja_Na_Patuvanje.Equals(zemjaNaPatuvanje) &&
                x.Vid_Polisa.Equals(vidPolisa) && x.Fransiza.Equals(fransiza)).First()).Procent_Zemja_Na_Patuvanje;
            return popust;
        }

        public double? popustVozrast(int age)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.p_vozrast.Where(x => x.Vozrast < age).OrderByDescending(x => x.Vozrast).First()).P_Vozrast1;
            return popust;
        }

        public double? popustFransiza(string zemjaNaPatuvanje, string vidPolisa, string fransiza)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.p_zemja_na_patuvanje.Where(x => x.Zemja_Na_Patuvanje.Equals(zemjaNaPatuvanje) &&
                x.Vid_Polisa.Equals(vidPolisa) && x.Fransiza.Equals(fransiza)).First()).Popust_Fransiza;
            return popust;
        }

        public double? popustDenovi(string vidPolisa, long denovi)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            var p = (entities.p_denovi.Where(x => x.Vid_Polisa.Equals(vidPolisa)).Where(x => x.Patuva_Denovi <= denovi)
                .OrderByDescending(x => x.Patuva_Denovi).First());
            double? popust = p.Popust_Denovi;
            return popust;
        }

        public double? popustFamilija(string vidPolisa)
        {
            // InsuranceEntities entities = new InsuranceEntities();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.p_familija.Where(x => x.Vid_Polisa.Equals(vidPolisa)).First()).Popust_Familija;
            return popust;
        }

        public double? popustGrupa(string vidPolisa, long clenovi)
        {
            //  InsuranceEntities entities = new InsuranceEntities();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.p_grupa.Where(x => x.Vid_Polisa.Equals(vidPolisa)).Where(x => x.Grupa < clenovi)
                .OrderByDescending(x => x.Grupa).First()).Popust_Grupa;
            return popust;
        }

        public double? procentDoplata(string doplatok1, string doplatok2)
        {
            //  InsuranceEntities entities = new InsuranceEntities();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? doplata = (entities.p_doplatoci.Where(x => x.Doplatok.Equals(doplatok1)).First()).P_Doplatok *
                (entities.p_doplatoci.Where(x => x.Doplatok.Equals(doplatok2)).First()).P_Doplatok;
            return doplata;
        }

        public int countAge(DateTime policyStart, string embg = null)
        {
            if (policyStart == null)
                return -1;

            int day;
            int month;
            int year;
            int.TryParse(embg.Substring(0, 2), out day);
            int.TryParse(embg.Substring(2, 2), out month);
            int.TryParse(embg.Substring(4, 3), out year);

            if (year < 900)
                year += 2000;
            else
                year += 1000;

            DateTime birthDate = new DateTime(year, month, day);

            int age = policyStart.Year - birthDate.Year;
            if (policyStart.Month < birthDate.Month || (policyStart.Month == birthDate.Month && policyStart.Day < birthDate.Day))
                age--;

            return age;
        }

        public double? vkupnaPremija(Policy policy)
        {
            double? pZemjaPatuvanje = procentZemjaPatuvanje(policy.zemjaNaPatuvanje, policy.vidPolisa, policy.fransiza);
            double? pFransiza = popustFransiza(policy.zemjaNaPatuvanje, policy.vidPolisa, policy.fransiza);
            double? pDenovi = popustDenovi(policy.vidPolisa, policy.vaziDenovi);

            InsuredTravelingEntity entities = new InsuredTravelingEntity();

            double? kurs = entities.p_kurs.First().Kurs;
            double? osnovnaPremija = kurs * pZemjaPatuvanje * (1 - pFransiza) * policy.vaziDenovi * (1 - pDenovi);

            if (policy.brojOsigureniciVid.Equals("polisaPoedinecno"))
            {
                double? pVozrast = popustVozrast(countAge(policy.startDate, policy.EMBG));
                osnovnaPremija *= (1 - pVozrast);
            }
            if (policy.brojOsigureniciVid.Equals("polisaFamilijarno"))
            {
                double? pFamilija = popustFamilija(policy.vidPolisa);

                double? pVozrast1;
                if (policy.osigurenik1ImePrezime != null && policy.osigurenik1MaticenBroj != null)
                {
                    pVozrast1 = popustVozrast(countAge(policy.startDate, policy.osigurenik1MaticenBroj));
                    double? osnovnaPremija1 = kurs * pZemjaPatuvanje * (1 - pFransiza) * policy.vaziDenovi * (1 - pDenovi) * (1 - pVozrast1);
                    osnovnaPremija += osnovnaPremija1;
                }
                double? pVozrast2;
                if (policy.osigurenik2ImePrezime != null && policy.osigurenik2MaticenBroj != null)
                {
                    pVozrast2 = popustVozrast(countAge(policy.startDate, policy.osigurenik2MaticenBroj));
                    double? osnovnaPremija2 = kurs * pZemjaPatuvanje * (1 - pFransiza) * policy.vaziDenovi * (1 - pDenovi) * (1 - pVozrast2);
                    osnovnaPremija += osnovnaPremija2;
                }
                double? pVozrast3;
                if (policy.osigurenik3ImePrezime != null && policy.osigurenik3MaticenBroj != null)
                {
                    pVozrast3 = popustVozrast(countAge(policy.startDate, policy.osigurenik3MaticenBroj));
                    double? osnovnaPremija3 = kurs * pZemjaPatuvanje * (1 - pFransiza) * policy.vaziDenovi * (1 - pDenovi) * (1 - pVozrast3);
                    osnovnaPremija += osnovnaPremija3;
                }
                double? pVozrast4;
                if (policy.osigurenik4ImePrezime != null && policy.osigurenik4MaticenBroj != null)
                {
                    pVozrast4 = popustVozrast(countAge(policy.startDate, policy.osigurenik4MaticenBroj));
                    double? osnovnaPremija4 = kurs * pZemjaPatuvanje * (1 - pFransiza) * policy.vaziDenovi * (1 - pDenovi) * (1 - pVozrast4);
                    osnovnaPremija += osnovnaPremija4;
                }
                double? pVozrast5;
                if (policy.osigurenik5ImePrezime != null && policy.osigurenik5MaticenBroj != null)
                {
                    pVozrast5 = popustVozrast(countAge(policy.startDate, policy.osigurenik5MaticenBroj));
                    double? osnovnaPremija5 = kurs * pZemjaPatuvanje * (1 - pFransiza) * policy.vaziDenovi * (1 - pDenovi) * (1 - pVozrast5);
                    osnovnaPremija += osnovnaPremija5;
                }
                double? pVozrast6;
                if (policy.osigurenik6ImePrezime != null && policy.osigurenik6MaticenBroj != null)
                {
                    pVozrast6 = popustVozrast(countAge(policy.startDate, policy.osigurenik6MaticenBroj));
                    double? osnovnaPremija6 = kurs * pZemjaPatuvanje * (1 - pFransiza) * policy.vaziDenovi * (1 - pDenovi) * (1 - pVozrast6);
                    osnovnaPremija += osnovnaPremija6;
                }

                osnovnaPremija *= (1 - pFamilija);
            }
            if (policy.brojOsigureniciVid.Equals("polisaGrupno"))
            {
                double? pGrupa = popustGrupa(policy.vidPolisa, policy.brojLicaGrupa);
                osnovnaPremija *= policy.brojLicaGrupa * (1 - pGrupa);
            }

            double? pDoplata = procentDoplata(policy.doplatok1, policy.doplatok2);
            osnovnaPremija *= pDoplata;

            int zaokruzena = (int)osnovnaPremija;
            zaokruzena = ((int)Math.Round(zaokruzena / 10.0)) * 10;

            if (zaokruzena < 200)
                zaokruzena = 200;

            return zaokruzena;
        }
    }
}
