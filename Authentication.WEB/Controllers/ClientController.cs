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
            client client = new client();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            ValidationService validationService = new ValidationService();

            client.FirstLastName = model.NameSurename;
            client.MailAdress = model.email;
            client.PhoneNumber = model.Number;
            int temp;
            Int32.TryParse(model.PostCode, out temp);
            client.PassportNumber = model.Passport;
            client.PostalCode = temp;
            client.StreetNumber = model.Adress;
            client.EMBG = model.EMBG;
            client.City = model.City;

            if (validationService.ClientFormValidate(client))
            {
                entities.clients.Add(client);
                entities.SaveChanges();
            }
            else
            {
                ViewBag.MyMessageToUsers = "<br/> <br/> <div style='color: red'>Грешка при валидација !!! Внесете точни информации! </div>";
            }

            return View();
        }
    }
}