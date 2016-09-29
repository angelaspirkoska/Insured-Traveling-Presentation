using Authentication.WEB.Models;
using Authentication.WEB.Services;
using InsuredTraveling;
using InsuredTraveling.Models;
using Rotativa;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using InsuredTraveling.DI;

namespace Authentication.WEB.Controllers
{
    [RoutePrefix("Payment")]
    public class PaymentController : Controller
    {
        private IUserService _us;
        private IPolicyService _ps;
        public PaymentController(IUserService us, IPolicyService ps)
        {
            this._us = us;
            this._ps = ps;
        }

        // GET: Payment
        public ActionResult Index()
        {
            PaymentModel model = new PaymentModel();

            model.clientId = "180000069";                   //Merchant Id defined by bank to user
            model.amount = "9.95";                         //Transaction amount
            model.oid = "";                                //Order Id. Must be unique. If left blank, system will generate a unique one.
            model.okUrl = ConfigurationManager.AppSettings["webpage_url"] + "/Payment/PaymentSuccess";                      //URL which client be redirected if authentication is successful
            model.failUrl = ConfigurationManager.AppSettings["webpage_url"] + "/Payment/PaymentFail";                    //URL which client be redirected if authentication is not successful
            model.rnd = DateTime.Now.ToString();           //A random number, such as date/time

            model.currency = "807";                        //Currency code, 949 for TL, ISO_4217 standard
            model.instalmentCount = "";                    //Instalment count, if there is no instalment should left blank
            model.transactionType = "Auth";                //Transaction type
            model.storekey = "SKEY3545";                     //Store key value, defined by bank.
            model.hashstr = model.clientId + model.oid + model.amount + model.okUrl + model.failUrl + model.transactionType + model.instalmentCount + model.rnd + model.storekey;
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            model.hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(model.hashstr);
            model.inputbytes = sha.ComputeHash(model.hashbytes);


            model.hash = Convert.ToBase64String(model.inputbytes); //Hash value used for validation

            //p.Created_By = _us.GetUserIdByUsername(System.Web.HttpContext.Current.User.Identity.Name);
            //p.Date_Created = DateTime.Now;
            //InsuredTravelingEntity _db = new InsuredTravelingEntity();
            //var p1 = _db.travel_policy.Create();
            //p1 = Mapper.Map<Policy, travel_policy>(p);
            //model.Pat = p1;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Policy p)
        {
            PaymentModel model = new PaymentModel();
            InsuredTravelingEntity _db = new InsuredTravelingEntity();

            model.clientId = "180000069";                   //Merchant Id defined by bank to user
            model.amount = "9.95";                         //Transaction amount
            model.oid = "";                                //Order Id. Must be unique. If left blank, system will generate a unique one.
            model.okUrl = ConfigurationManager.AppSettings["webpage_url"] + "/Payment/PaymentSuccess";                      //URL which client be redirected if authentication is successful
            model.failUrl = ConfigurationManager.AppSettings["webpage_url"] + "/Payment/PaymentFail";                    //URL which client be redirected if authentication is not successful
            model.rnd = DateTime.Now.ToString();           //A random number, such as date/time

            model.currency = "807";                        //Currency code, 949 for TL, ISO_4217 standard
            model.instalmentCount = "";                    //Instalment count, if there is no instalment should left blank
            model.transactionType = "Auth";                //Transaction type
            model.storekey = "SKEY3545";                     //Store key value, defined by bank.
            model.hashstr = model.clientId + model.oid + model.amount + model.okUrl + model.failUrl + model.transactionType + model.instalmentCount + model.rnd + model.storekey;
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            model.hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(model.hashstr);
            model.inputbytes = sha.ComputeHash(model.hashbytes);


            model.hash = Convert.ToBase64String(model.inputbytes); //Hash value used for validation

            var currentUser = _us.GetUserDataByUsername(System.Web.HttpContext.Current.User.Identity.Name);
            p.Created_By = _us.GetUserIdByUsername(System.Web.HttpContext.Current.User.Identity.Name);
            p.Date_Created = DateTime.Now;

            CreateClientModel client = new CreateClientModel();
            ValidationService validation = new ValidationService();
            client.Name = p.Name;
            client.LastName = p.LastName;
            client.Email = currentUser.Email;
            client.Address = p.Address;
            client.SSN = p.SSN;
            client.PhoneNumber = currentUser.PhoneNumber;
            client.Passport_Number_IdNumber = p.PassportNumber_ID;
            client.Postal_Code = currentUser.PostalCode;
            client.DateBirth = currentUser.DateOfBirth;
            client.Created_By = currentUser.Id;
            client.Date_Created = DateTime.Now.Date;
            client.City = currentUser.City;
            client.Age = validation.countAge(DateTime.Now, p.SSN);

            var insured = _db.insureds.Create();
            insured = Mapper.Map<CreateClientModel, insured>(client);

            var a1ID = p.additional_charges[0].ID;
            var a2ID = p.additional_charges[1].ID;
            ViewBag.insured = insured;
            ViewBag.additional_charge1 = _db.additional_charge.Where(x=>x.ID == a1ID).Single().Doplatok;
            ViewBag.additional_charge2 = _db.additional_charge.Where(x => x.ID == a2ID).Single().Doplatok;

            var p1 = _db.travel_policy.Create();
            p1 = Mapper.Map<Policy, travel_policy>(p);
            policy_type policy_type = _db.policy_type.Where(x => x.ID == p.Policy_TypeID).Single();
            p1.policy_type = policy_type;
            country c = _db.countries.Where(x => x.ID == p.CountryID).Single();
            p1.country = c;

            model.Pat = p1;
            return View(model);
        }

        [Route("PaymentSuccess")]
        public ActionResult PaymentSuccess()
        {
            PaymentSuccessModel model = new PaymentSuccessModel();
            model.amount = Request.Form.Get("amount");
            model.oid = Request.Form.Get("ReturnOid");
            model.TransId = Request.Form.Get("TransId");
            model.AuthCode = Request.Form.Get("AuthCode");
            model.mdStatus = Request.Form.Get("mdStatus");
            if (model.mdStatus == "1" || model.mdStatus == "2" || model.mdStatus == "3" || model.mdStatus == "4")
            {
                string fullPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PolicyPDF/" + model.TransId + model.amount + ".pdf");
                var actionResult = new ActionAsPdf("../Policy/Print");
                var byteArray = actionResult.BuildPdf(ControllerContext);
                var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();



                // ADD MAIL ADRESS

                MailService mailService = new MailService("asaid@optimalreinsurance.com");
                mailService.setSubject("Издадена полиса број: " + model.oid);
                string bodyText = "Успешно извршена трансакција \n Уплатена сума: " + model.amount + " ден. \n Трансакциски код: " + model.TransId + "\n Автентикациски код: " + model.AuthCode;

                mailService.setBodyText(bodyText);
                mailService.attach(new System.Net.Mail.Attachment(fullPath));
                mailService.sendMail();
            }
            return View(model);
        }

        [Route("PaymentFail")]
        public ActionResult PaymentFail()
        {
            PaymentFailModel model = new PaymentFailModel();
            model.ErrMsg = Request.Form.Get("ErrMsg");
            model.mdErrorMsg = Request.Form.Get("mdErrorMsg");
            return View(model);
        }
    }
}