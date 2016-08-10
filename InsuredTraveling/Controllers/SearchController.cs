using Authentication.WEB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InsuredTraveling.Controllers
{
    public class SearchController : Controller
    {

        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SearchPolyciesJson(string name, string embg, string land, string address, string TypePolycies, string agency, string startDate, string endDate, string dateI, string dateS, string operatorStartDate, string operatorEndDate, string operatorDateI, string operatorDateS)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            name = name.ToLower();
            embg = embg.ToLower();
            address = address.ToLower();
            agency = agency.ToLower();
            TypePolycies = (TypePolycies != "--Select type of policies--") ? TypePolycies = TypePolycies.ToLower() : TypePolycies = "";
            DateTime startDate1 = String.IsNullOrEmpty(startDate) ? new DateTime() : Convert.ToDateTime(startDate);
            DateTime endDate1 = String.IsNullOrEmpty(endDate) ? new DateTime() : Convert.ToDateTime(endDate);
            DateTime dateI1 = String.IsNullOrEmpty(dateI) ? new DateTime() : Convert.ToDateTime(dateI);
            DateTime dateS2 = String.IsNullOrEmpty(dateS) ? new DateTime() : Convert.ToDateTime(dateS);




            if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(embg) || !String.IsNullOrEmpty(address) || !String.IsNullOrEmpty(land) || !String.IsNullOrEmpty(agency) || !String.IsNullOrEmpty(TypePolycies))
            {
                var data = entities.patnicko.Where(x => x.EMBG.Contains(embg) &&
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
                var jsonData = Json(data, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, responseText = jsonData }, JsonRequestBehavior.AllowGet);
            }
            else if (!String.IsNullOrEmpty(startDate) || !String.IsNullOrEmpty(endDate) || !String.IsNullOrEmpty(dateI) || !String.IsNullOrEmpty(dateS))
            {
                var data = entities.patnicko.ToList();
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
                var jsonData = Json(data, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, responseText = jsonData }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return new JsonResult { };
            }

        }

        public JsonResult SearchClientsJson(string name, string embg, string address, string city, string postal_code, string phone, string email, string passport)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            name = name.ToLower();
            embg = embg.ToLower();
            address = address.ToLower();
            city = city.ToLower();
            int postal_code2 = (postal_code != "") ? Convert.ToInt32(postal_code) : 0;
            phone = phone.ToLower();
            email = email.ToLower();
            passport = passport.ToLower();


            if (postal_code2 != 0)
            {
                var data = entities.client.Where(x => x.EMBG.Contains(embg) &&
                                                        x.FirstLastName.ToLower().Contains(name) &&
                                                        x.StreetNumber.ToLower().Contains(address) &&
                                                        x.MailAdress.ToLower().Contains(email) &&
                                                        (x.PostalCode == postal_code2) &&
                                                        x.PhoneNumber.Contains(phone) &&
                                                        x.EMBG.Contains(embg) &&
                                                        x.City.ToLower().Contains(city) &&
                                                        x.PassportNumber.Contains(passport)
                                                       ).ToList();
                var jsonData = Json(data, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, responseText = jsonData }, JsonRequestBehavior.AllowGet);
            }
            else if (!String.IsNullOrEmpty(name) || !String.IsNullOrEmpty(embg) || !String.IsNullOrEmpty(address) || !String.IsNullOrEmpty(city) || !String.IsNullOrEmpty(email) || !String.IsNullOrEmpty(phone) || !String.IsNullOrEmpty(passport))
            {
                var data = entities.client.Where(x => x.EMBG.Contains(embg) &&
                                                       x.FirstLastName.ToLower().Contains(name) &&
                                                       x.StreetNumber.ToLower().Contains(address) &&
                                                       x.MailAdress.ToLower().Contains(email) &&
                                                       x.PhoneNumber.Contains(phone) &&
                                                       x.EMBG.Contains(embg) &&
                                                       x.City.ToLower().Contains(city) &&
                                                       x.PassportNumber.Contains(passport)
                                                      ).ToList();
                var jsonData = Json(data, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, responseText = jsonData }, JsonRequestBehavior.AllowGet);
            }

            return new JsonResult { };
        }
    }
}