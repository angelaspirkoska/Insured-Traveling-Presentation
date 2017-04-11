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
        private IUserService _userService;

        public ClientController(IInsuredsService ins, IUserService userService)
        {
            _ins = ins;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                Response.Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");
            ViewBag.BirthDate = new DateTime();
            ViewBag.PhoneNumber = 0;
            ViewBag.CountryCodes = 0;
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateClientModel model) 
        {
            var username = System.Web.HttpContext.Current.User.Identity.Name;
            InsuredTravelingEntity2 entities = new InsuredTravelingEntity2();
            var client = entities.insureds.Create();
            

            ValidationService validationService = new ValidationService();
            ViewBag.CountryCodes = model.countriesCodes;
            ViewBag.PhoneNumber = model.PhoneNumber;
            if (model.DateBirth != DateTime.MinValue)
            {
                ViewBag.BirthDate = model.DateBirth;
            }
            else
            {
                ViewBag.BirthDate = new DateTime(); 
            }
            
            if (ModelState.IsValid)
            {
                if(validationService.validateEMBG(model.SSN))
                {
                    model.Age = validationService.CountAgeByBirthDate(model.DateBirth);
                    client = Mapper.Map<CreateClientModel, insured>(model);
                    var insuredType = _ins.GetInsuredType();
                    client.Type_InsuredID = insuredType != null ? insuredType.ID : _ins.GetAllInsuredTypes().FirstOrDefault().ID;
                    client.Created_By = _userService.GetUserIdByUsername(username);
                    client.Date_Created = DateTime.UtcNow;

                    try
                    {
                        _ins.AddInsured(client);
                        ViewBag.SuccessMessage = InsuredTraveling.Resource.Client_SuccessfullyAdded;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = InsuredTraveling.Resource.Client_NotSuccessfullyAdded;
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = InsuredTraveling.Resource.SSNNotValid;
                }
                             
            }
            else
            {
                ViewBag.Message = InsuredTraveling.Resource.Client_DataNotValid;
            }
            return View();
        }
    }
}