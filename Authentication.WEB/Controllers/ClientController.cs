using Authentication.WEB.Services;
using InsuredTraveling;
using InsuredTraveling.Models;
using AutoMapper;
using System.Web.Mvc;
using System;
using System.Linq;
using InsuredTraveling.DI;
using InsuredTraveling.Filters;
using InsuredTraveling.Controllers;
using System.Configuration;

namespace Authentication.WEB.Controllers
{
    [SessionExpire]
    public class ClientController : Controller
    {
        private IInsuredsService _ins;

        public ClientController(IInsuredsService ins)
        {
            _ins = ins;
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                Response.Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateClientModel model) 
        {
            
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            var client = entities.insureds.Create();
            ValidationService validationService = new ValidationService();
            if (ModelState.IsValid && validationService.validateEMBG(model.SSN))
            {
                client = Mapper.Map<CreateClientModel, insured>(model);
                try
                {
                    _ins.AddInsured(client);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View();
                }

                ViewBag.Message = "Успешно го креиравте клиентот!";
                return View();
            }
            return View();
        }
    }
}