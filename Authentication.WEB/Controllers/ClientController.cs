using Authentication.WEB.Models;
using Authentication.WEB.Services;
using InsuredTraveling;
using System;
using System.Web.Mvc;

namespace Authentication.WEB.Controllers
{
    public class ClientController : Controller
    {

        [HttpGet]
        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateClientModel model) 
        {
            client klient = new client();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            ValidationService validationService = new ValidationService();

            klient.FirstLastName = model.NameSurename;
            klient.MailAdress = model.email;
            klient.PhoneNumber = model.Number;
            int temp;
            Int32.TryParse(model.PostCode, out temp);
            klient.PassportNumber = model.Passport;
            klient.PostalCode = temp;
            klient.StreetNumber = model.Adress;
            klient.EMBG = model.EMBG;
            klient.City = model.City;
            if (validationService.ClientFormValidate(klient))
            {
                entities.clients.Add(klient);
                entities.SaveChanges();
            } // else return false
            else
            {
                ViewBag.MyMessageToUsers = "<br/> <br/> <div style='color: red'>Грешка при валидација !!! Внесете точни информации! </div>";
            }


            return View();
        }
    }
}