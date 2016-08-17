using InsuredTraveling.Models;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace InsuredTraveling.Controllers.API
{
    [RoutePrefix("api/mobile")]
    public class MobileApiController : ApiController
    {
        [Route("RetrieveUserInfo")]
        public JObject RetrieveUserInformation(Username username)
        {
            InsuredTravelingEntity db = new InsuredTravelingEntity();
            JObject data = new JObject();



            aspnetuser user = db.aspnetusers.Where(x => x.UserName == username.username).ToArray().First();
            var u = new JObject();
            u.Add("FirstName", user.FirstName);
            u.Add("LastName", user.LastName);
            u.Add("Municipality", user.Municipality);
            u.Add("MobilePhoneNumber", user.MobilePhoneNumber);
            u.Add("PassportNumber", user.PassportNumber);
            u.Add("PostalCode", user.PostalCode);
            u.Add("PhoneNumber", user.PhoneNumber);
            u.Add("EMBG", user.EMBG);
            u.Add("DateOfBirth", user.DateOfBirth);
            u.Add("Gender",user.Gender);
            u.Add("City", user.City);
            u.Add("Address", user.Address);
            u.Add("Email", user.Email);
            
            data.Add("user", u);

            JArray data1 = new JArray();
            patnicko [] policy = db.patnickoes.Where(x => x.Polisa_Broj > 3).ToArray();
            
            foreach(patnicko p1 in policy)
            {
                var p = new JObject();
                p.Add("policyNumber", p1.Polisa_Broj);
                p.Add("embg", p1.EMBG);
                p.Add("insuredAddress", p1.Adresa);
                p.Add("insuredPassport", p1.Broj_Pasos);
                p.Add("nameContractor", p1.Ime_I_Prezime);
                p.Add("nameInsured", p1.Osigurenik1_Ime_I_Prezime);
                p.Add("ValidFrom", p1.Zapocnuva_Na);
                p.Add("ValidUntil", p1.Zavrsuva_Na);
                p.Add("insuredDays", p1.Vazi_Denovi);
                p.Add("franchiseTravel", p1.Fransiza);
                p.Add("basicPremium", p1.Osnovna_Premija);
                p.Add("insuredBday", "1994.04.04");
                p.Add("additional", " ");
                p.Add("discount", p1.Popust_Fransiza);
                p.Add("packetTravel", "Optimum");
                p.Add("totalPremium", p1.Vkupna_Premija);

                data1.Add(p);
            }

            data.Add("policy",data1);

            JArray data2 = new JArray();
            patnicko[] policy2 = db.patnickoes.Where(x => x.Polisa_Broj <= 3).ToArray();

            foreach (patnicko p1 in policy2)
            {
                var p = new JObject();
                p.Add("policyNumber", p1.Polisa_Broj);
                p.Add("embg", p1.EMBG);
                p.Add("insuredAddress", p1.Adresa);
                p.Add("insuredPassport", p1.Broj_Pasos);
                p.Add("nameContractor", p1.Ime_I_Prezime);
                p.Add("nameInsured", p1.Osigurenik1_Ime_I_Prezime);
                p.Add("ValidFrom", p1.Zapocnuva_Na);
                p.Add("ValidUntil", p1.Zavrsuva_Na);
                p.Add("insuredDays", p1.Vazi_Denovi);
                p.Add("franchiseTravel", p1.Fransiza);
                p.Add("basicPremium", p1.Osnovna_Premija);
                p.Add("insuredBday", "1994.04.04");
                p.Add("additional", " ");
                p.Add("discount", p1.Popust_Fransiza);
                p.Add("packetTravel", "Optimum");
                p.Add("totalPremium", p1.Vkupna_Premija);

                data2.Add(p);
            }

            data.Add("quote",data2);

            return data;
        }
    }
}
