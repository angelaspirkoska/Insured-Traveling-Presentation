using Authentication.WEB.Models;
using Authentication.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authentication.WEB.Controllers
{
    public class RegisterClientController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(CreateClientModel model)
        {
            InsuredTravelingEntity entity = new InsuredTravelingEntity();
            user_all user = new user_all();
            string ID_database = "";

            if (model != null)
            {
                string activationCode = Guid.NewGuid().ToString();
                //VALIDATE USER AND EMAIL IF EXIST THROW ERROR AND DO NOT SAVE IN DATABASE

                user.Roles = "user";
                user.Address = "Hardcoded Adress";
                user.InsurenceCompany = "HARDCODED COMPANY";
                user.Username = "user125true12";
                user.EMBG = "072 530 184";
                user.Password = " 1234";
                user.ActivationCode = activationCode;
                DateTime now1 = DateTime.Now;
                user.DateTimer = now1.Ticks.ToString();
                if (entity.user_all.Any(o => o.Username == user.Username))
                {
                    // Return korisnikot posoti
                    ViewBag.MyMessageToUsers = "<br/> <br/> <div style='color: red'>Корисник со тоа име веќе постои</div>";
                }
                else {
                    entity.user_all.Add(user);
                    entity.SaveChanges();


                    ID_database = (entity.user_all.OrderByDescending(p => p.ID).Select(r => r.ID).First()).ToString();
                    string body = "Hello " + "USPESHEN SUM " + ",";
                    body += "<br /><br />Please click the following link to activate your account";
                    body += "<br /><a href = '" + Request.Url.AbsoluteUri.Replace("CS.aspx", "CS_Activation.aspx") + "?ActivationCode=" + activationCode + "&ID=" + ID_database + "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";
                    //SEND SMS TO EMAIL
                    SMSvalidation sms = new SMSvalidation();
                    //    sms.SendMessage();

                    //SEND E-MAIL for Validation
                    MailService mailService = new MailService("iatanasovski@optimalreinsurance.com");
                    mailService.setSubject("Account Activation Validation");
                    mailService.setBodyText(body, true);
                    mailService.sendMail();
                    ViewBag.MyMessageToUsers = "<br/> <br/> <div style='color: red'>Успешно испратен е-маил и(или) СМС порака. Ве молиме проверете го вашиот е-маил.</div>";

                }
                return View();

            }
            else
            {


                return View();
            }
        }


        // GET: RegisterClient
        [HttpGet]
        public ActionResult Index(string ActivationCode, string ID)
        {
            InsuredTravelingEntity entity = new InsuredTravelingEntity();
            //    user_all user = new user_all();
            //   string ID_database = "";
            if (String.IsNullOrEmpty(ActivationCode))
            {

                return View();

            }
            else
            {

                var tempID = int.Parse(ID);

                if (entity.user_all.SingleOrDefault(f => f.ID == tempID).ActivationCode == ActivationCode)
                {
                    DateTime now = DateTime.Now;
                    var baz = entity.user_all.SingleOrDefault(f => f.ID == tempID).DateTimer;

                    var ex = now.Ticks - long.Parse(baz);
                    TimeSpan ts = TimeSpan.FromTicks(ex);
                    double minutesFromTs = ts.TotalMinutes;
                    if (minutesFromTs > 30)
                    {
                        ViewBag.MyMessageToUsers = "<br/> <br/> <div style='color: red'>Истечено време. Испрати е-маил повторно. </div>";
                        return View("RegistrationError");
                    }
                    else
                    {
                        entity.user_all.SingleOrDefault(f => f.ID == tempID).IsValidEmail = true;
                        entity.SaveChanges();
                        ViewBag.MyMessageToUsers = "<br/> <br/> <div style='color: red'> Потврена е-маил автентикација. </div>";
                        return View("SuccesfullRegistration");
                    }


                }
                else
                {
                    ViewBag.MyMessageToUsers = "<br/> <br/> <div style='color: red'>Грешка при валидација !!!  </div>";
                    return View("RegistrationError");
                }

            }
        }
    }
}