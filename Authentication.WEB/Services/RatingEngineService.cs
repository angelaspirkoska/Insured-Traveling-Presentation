using Authentication.WEB.Models;
using System;
using System.Linq;
using InsuredTraveling;

namespace Authentication.WEB.Services
{
    class RatingEngineService
    {
        public double? procentZemjaPatuvanje(string zemjaNaPatuvanje, string vidPolisa, string Franchise)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.p_zemja_na_patuvanje.Where(x => x.country.Name.Equals(zemjaNaPatuvanje) &&
                x.policy_type.type.Equals(vidPolisa) && x.Franchise.Equals(Franchise)).First()).Percentage;
            return popust;
        }

        public double? popustVozrast(int age)
        {
            string name = " ";
            if (age < 18)
            {
                name = "0<=17";
            } else if (age < 64)
            {
                name = "17<64";
            } else {
                name = ">64";
            }
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.discount_age.Where(x => x.Name == name).OrderByDescending(x => x.ID).First()).Discount;
            return popust;
        }

        public double? popustFranchise(string zemjaNaPatuvanje, string vidPolisa, string Franchise)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.p_zemja_na_patuvanje.Where(x => x.country.Name.Equals(zemjaNaPatuvanje) &&
                x.policy_type.type.Equals(vidPolisa) && x.Franchise.Equals(Franchise)).First()).Discount_franchise;
            return popust;
        }

        public double? popustDenovi(string vidPolisa, long denovi)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            var p = (entities.discount_days.Where(x => x.policy_type.ID.Equals(vidPolisa)).Where(x => x.travel_duration.Days.Equals(denovi)) //Treba da se vide shto se prima vo denovi 
                .OrderByDescending(x => x.Travel_durationID).First());
            double? popust = p.Discount;
            return popust;
        }

        public double? popustFamilija(string vidPolisa)
        {
            // InsuranceEntities entities = new InsuranceEntities();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.discount_family.Where(x => x.policy_type.type.Equals(vidPolisa)).First()).Discount;
            return popust;
        }

        public double? popustGrupa(string vidPolisa, long clenovi)
        {
            //  InsuranceEntities entities = new InsuranceEntities();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            double? popust = (entities.discount_group.Where(x => x.policy_type.type.Equals(vidPolisa)).Where(x => x.group.Memebers < clenovi)
                .OrderByDescending(x => x.group.Memebers).First()).Discount;
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
            double? pZemjaPatuvanje = procentZemjaPatuvanje(policy.zemjaNaPatuvanje, policy.vidPolisa, policy.Franchise);
            double? pFranchise = popustFranchise(policy.zemjaNaPatuvanje, policy.vidPolisa, policy.Franchise);
            double? pDenovi = popustDenovi(policy.vidPolisa, policy.vaziDenovi);

            InsuredTravelingEntity entities = new InsuredTravelingEntity();

            double? kurs = entities.exchange_rate.First().Value;
            double? osnovnaPremija = kurs * pZemjaPatuvanje * (1 - pFranchise) * policy.vaziDenovi * (1 - pDenovi);

            if (policy.brojOsigureniciVid.Equals("polisaPoedinecno"))
            {
                double? pVozrast = popustVozrast(countAge(policy.startDate, policy.EMBG));
                osnovnaPremija *= (1 - pVozrast);
            }
            if (policy.brojOsigureniciVid.Equals("polisaFamilijarno"))
            {
                double? pFamilija = popustFamilija(policy.vidPolisa);
                var insuredID = entities.policy_insurees.Where(x => x.PolicyID == policy.policyNumber).Single().InsuredID;
                var insured = entities.insureds.Where(x => x.ID == insuredID).ToArray();
                double? pVozrast1;

                foreach (insured i in insured)
                {
                    pVozrast1 = popustVozrast(countAge(policy.startDate, i.SSN));
                    double? osnovnaPremija1 = kurs * pZemjaPatuvanje * (1 - pFranchise) * policy.vaziDenovi * (1 - pDenovi) * (1 - pVozrast1);
                    osnovnaPremija += osnovnaPremija1;
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
