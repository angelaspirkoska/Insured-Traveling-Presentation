using Authentication.WEB.Services;
using InsuredTraveling;
using InsuredTraveling.Models;
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
            insured client = new insured();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            ValidationService validationService = new ValidationService();

            client.Name = model.NameSurename;
            client.Email = model.email;
            client.Phone_Number = model.Number;
            int temp;
            Int32.TryParse(model.PostCode, out temp);
            client.Passport_Number_IdNumber = model.Passport;
            client.Postal_Code = temp.ToString();
            client.Address = model.Adress;
            client.SSN = model.EMBG;
            client.City = model.City;

            if (validationService.ClientFormValidate(client))
            {
                entities.insureds.Add(client);
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