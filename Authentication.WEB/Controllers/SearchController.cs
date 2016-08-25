using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    public class SearchController : Controller
    {
        private InsuredTravelingEntity _db;

        public SearchController()
        {
            _db = new InsuredTravelingEntity();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var type_policies = GetTypeOfPolicy();
            await Task.WhenAll(type_policies);
            ViewBag.TypeOfPolicy = type_policies.Result;
            return View();
        }


        [HttpGet]
        [Route("GetUsers")]
        public JObject GetUsers(string name, string embg, string address, string email, string postal_code, string phone, string city, string passport)
        {
            name = name.ToLower();
            embg = embg.ToLower();
            address = address.ToLower();
            city = city.ToLower();
            phone = phone.ToLower();
            email = email.ToLower();
            passport = passport.ToLower();

            InsuredTravelingEntity db = new InsuredTravelingEntity();
            var data = db.aspnetusers.Where(x => x.EMBG.Contains(embg) &&
                                             x.FirstName.ToLower().Contains(name) &&
                                             x.Address.ToLower().Contains(address) &&
                                             x.Email.ToLower().Contains(email) &&
                                             (x.PostalCode.Contains(postal_code)) &&
                                             x.PhoneNumber.Contains(phone) &&
                                             x.EMBG.Contains(embg) &&
                                             x.City.ToLower().Contains(city) &&
                                             x.PassportNumber.Contains(passport)
                                            ).ToArray();
           
            var j = new JObject();
            var data1 = new JArray();
            foreach(var v in data)
            {
                var j1 = new JObject();
                j1.Add("Id", v.Id);
                j1.Add("FirstName", v.FirstName);
                j1.Add("Email", v.Email);
                j1.Add("PostalCode", v.PostalCode);
                j1.Add("PassportNumber", v.PassportNumber);
                j1.Add("PhoneNumber", v.PhoneNumber);
                j1.Add("Address", v.Address);
                j1.Add("EMBG", v.EMBG);
                j1.Add("City", v.City);
                data1.Add(j1);
            }
            j.Add("data", data1);
            return j;
        }

        [HttpGet]
        [Route("GetPolicies")]
        public JObject GetPolicies(string name, string embg, string land, string address, string TypePolycies, string agency, string startDate, string endDate, string dateI, string dateS, string operatorStartDate, string operatorEndDate, string operatorDateI, string operatorDateS)
        {
            name = name.ToLower();
            embg = embg.ToLower();
            address = address.ToLower();
            agency = agency.ToLower();
            DateTime startDate1 = String.IsNullOrEmpty(startDate) ? new DateTime() : Convert.ToDateTime(startDate);
            DateTime endDate1 = String.IsNullOrEmpty(endDate) ? new DateTime() : Convert.ToDateTime(endDate);
            DateTime dateI1 = String.IsNullOrEmpty(dateI) ? new DateTime() : Convert.ToDateTime(dateI);
            DateTime dateS2 = String.IsNullOrEmpty(dateS) ? new DateTime() : Convert.ToDateTime(dateS);

            if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(embg) || !String.IsNullOrEmpty(address) || !String.IsNullOrEmpty(land) || !String.IsNullOrEmpty(agency) || !String.IsNullOrEmpty(TypePolycies))
            {
                var data = _db.patnickoes.Where(x => x.EMBG.Contains(embg) &&
                                                        x.Ime_I_Prezime.ToLower().Contains(name) &&
                                                        x.Adresa.ToLower().Contains(address) &&
                                                        x.Ovlastena_Agencija.ToLower().Contains(agency) &&
                                                        x.Vid_Polisa.ToLower().Contains(TypePolycies)
                                                       ).ToList();

                if (!String.IsNullOrEmpty(startDate))
                {
                    switch (operatorStartDate)
                    {
                        case "<": data = data.Where(x => x.Zapocnuva_Na < startDate1).ToList(); break;
                        case "=": data = data.Where(x => x.Zapocnuva_Na == startDate1).ToList(); break;
                        case ">": data = data.Where(x => x.Zapocnuva_Na > startDate1).ToList(); break;
                        default: break;
                    }
                }
                if (!String.IsNullOrEmpty(endDate))
                {
                    switch (operatorEndDate)
                    {
                        case "<": data = data.Where(x => x.Zavrsuva_Na < endDate1).ToList(); break;
                        case "=": data = data.Where(x => x.Zavrsuva_Na == endDate1).ToList(); break;
                        case ">": data = data.Where(x => x.Zavrsuva_Na > endDate1).ToList(); break;
                        default: break;
                    }
                }
                if (!String.IsNullOrEmpty(dateI))
                {
                    switch (operatorDateI)
                    {
                        case "<": data = data.Where(x => x.Datum_Na_Izdavanje < dateI1).ToList(); break;
                        case "=": data = data.Where(x => x.Datum_Na_Izdavanje == dateI1).ToList(); break;
                        case ">": data = data.Where(x => x.Datum_Na_Izdavanje > dateI1).ToList(); break;
                        default: break;
                    }
                }
                if (!String.IsNullOrEmpty(dateS))
                {
                    switch (operatorDateS)
                    {
                        case "<": data = data.Where(x => x.Datum_Na_Storniranje < dateS2).ToList(); break;
                        case "=": data = data.Where(x => x.Datum_Na_Storniranje == dateS2).ToList(); break;
                        case ">": data = data.Where(x => x.Datum_Na_Storniranje > dateS2).ToList(); break;
                        default: break;
                    }
                }

                var j = new JObject();
                var data1 = new JArray();
                foreach (var v in data)
                {
                    var j1 = new JObject();
                    j1.Add("Polisa_Broj", v.Polisa_Broj);
                    j1.Add("Ime_I_Prezime", v.Ime_I_Prezime);
                    j1.Add("EMBG", v.EMBG);
                    j1.Add("Zemja_Na_Patuvanje", v.Zemja_Na_Patuvanje);
                    j1.Add("Vid_Polisa", v.Vid_Polisa);
                    j1.Add("Zapocnuva_Na", v.Zapocnuva_Na);
                    j1.Add("Zavrsuva_Na", v.Zavrsuva_Na);
                    j1.Add("Adresa", v.Adresa);
                    j1.Add("Ovlastena_Agencija", v.Ovlastena_Agencija);
                    j1.Add("Datum_Na_Izdavanje", v.Datum_Na_Izdavanje);
                    j1.Add("Datum_Na_Storniranje", v.Datum_Na_Storniranje);
                    j1.Add("Status", v.Status);
                    j1.Add("Referent", v.Referent);

                    data1.Add(j1);
                }
                j.Add("data", data1);
                return j;

            }
            else if (!String.IsNullOrEmpty(startDate) || !String.IsNullOrEmpty(endDate) || !String.IsNullOrEmpty(dateI) || !String.IsNullOrEmpty(dateS))
            {
                var data = _db.patnickoes.ToList();
                if (!String.IsNullOrEmpty(startDate))
                {
                    switch (operatorStartDate)
                    {
                        case "<": data = data.Where(x => x.Zapocnuva_Na < startDate1).ToList(); break;
                        case "=": data = data.Where(x => x.Zapocnuva_Na == startDate1).ToList(); break;
                        case ">": data = data.Where(x => x.Zapocnuva_Na > startDate1).ToList(); break;
                        default: break;
                    }
                }
                if (!String.IsNullOrEmpty(endDate))
                {
                    switch (operatorEndDate)
                    {
                        case "<": data = data.Where(x => x.Zavrsuva_Na < endDate1).ToList(); break;
                        case "=": data = data.Where(x => x.Zavrsuva_Na == endDate1).ToList(); break;
                        case ">": data = data.Where(x => x.Zavrsuva_Na > endDate1).ToList(); break;
                        default: break;
                    }
                }
                if (!String.IsNullOrEmpty(dateI))
                {
                    switch (operatorDateI)
                    {
                        case "<": data = data.Where(x => x.Datum_Na_Izdavanje < dateI1).ToList(); break;
                        case "=": data = data.Where(x => x.Datum_Na_Izdavanje == dateI1).ToList(); break;
                        case ">": data = data.Where(x => x.Datum_Na_Izdavanje > dateI1).ToList(); break;
                        default: break;
                    }
                }
                if (!String.IsNullOrEmpty(dateS))
                {
                    switch (operatorDateS)
                    {
                        case "<": data = data.Where(x => x.Datum_Na_Storniranje < dateS2).ToList(); break;
                        case "=": data = data.Where(x => x.Datum_Na_Storniranje == dateS2).ToList(); break;
                        case ">": data = data.Where(x => x.Datum_Na_Storniranje > dateS2).ToList(); break;
                        default: break;
                    }
                }
                var j = new JObject();
                var data1 = new JArray();
                foreach (var v in data)
                {
                    var j1 = new JObject();
                    j1.Add("Polisa_Broj", v.Polisa_Broj);
                    j1.Add("Ime_I_Prezime", v.Ime_I_Prezime);
                    j1.Add("EMBG", v.EMBG);
                    j1.Add("Zemja_Na_Patuvanje", v.Zemja_Na_Patuvanje);
                    j1.Add("Vid_Polisa", v.Vid_Polisa);
                    j1.Add("Zapocnuva_Na", v.Zapocnuva_Na);
                    j1.Add("Zavrsuva_Na", v.Zavrsuva_Na);
                    j1.Add("Adresa", v.Adresa);
                    j1.Add("Ovlastena_Agencija", v.Ovlastena_Agencija);
                    j1.Add("Datum_Na_Izdavanje", v.Datum_Na_Izdavanje);
                    j1.Add("Datum_Na_Storniranje", v.Datum_Na_Storniranje);
                    j1.Add("Status", v.Status);
                    j1.Add("Referent", v.Referent);

                    data1.Add(j1);
                }
                j.Add("data", data1);
                return j;

            }
            else
            {
                var data = _db.patnickoes.ToArray();
                var j = new JObject();
                var data1 = new JArray();
                foreach (var v in data)
                {
                    var j1 = new JObject();
                    j1.Add("Polisa_Broj", v.Polisa_Broj);
                    j1.Add("Ime_I_Prezime", v.Ime_I_Prezime);
                    j1.Add("EMBG", v.EMBG);
                    j1.Add("Zemja_Na_Patuvanje", v.Zemja_Na_Patuvanje);
                    j1.Add("Vid_Polisa", v.Vid_Polisa);
                    j1.Add("Zapocnuva_Na", v.Zapocnuva_Na);
                    j1.Add("Zavrsuva_Na", v.Zavrsuva_Na);
                    j1.Add("Adresa", v.Adresa);
                    j1.Add("Ovlastena_Agencija", v.Ovlastena_Agencija);
                    j1.Add("Datum_Na_Izdavanje", v.Datum_Na_Izdavanje);
                    j1.Add("Datum_Na_Storniranje", v.Datum_Na_Storniranje);
                    j1.Add("Status", v.Status);
                    j1.Add("Referent", v.Referent);

                    data1.Add(j1);
                }
                j.Add("data", data1);
                return j;
            }

        }

        private async Task<List<SelectListItem>> GetTypeOfPolicy()
        {
            var policy = _db.patnicko_vid.Select(p => new SelectListItem
            {
                Text = p.Vid_Polisa,
                Value = p.Vid_Polisa.ToString()
            });
            return await policy.ToListAsync();
        }
    }
}