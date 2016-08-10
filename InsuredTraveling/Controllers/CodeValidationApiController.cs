using InsuredTraveling.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twilio;

namespace Authentication.WEB.Controllers
{
    public class CodeValidationApiController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Pay(ValidationModel CodeModel)
        {

            InsuredTravelingEntity entity = new InsuredTravelingEntity();
            user_all user = new user_all();

            var dict2 = new JObject();

            if (String.IsNullOrEmpty(CodeModel.ActivationCode))
            {
                dict2.Add("IsValid", "false");
                dict2.Add("Error", "Кодот не е пронајден во пораката");
                return Json(dict2);

            }
            else
            {

                var tempID = int.Parse(CodeModel.ID);

                if (entity.user_all.SingleOrDefault(f => f.ID == tempID).ActivationCode == CodeModel.ActivationCode)
                {
                    DateTime now = DateTime.Now;
                    var baz = entity.user_all.SingleOrDefault(f => f.ID == tempID).DateTimer;

                    var ex = now.Ticks - long.Parse(baz);
                    TimeSpan ts = TimeSpan.FromTicks(ex);
                    double minutesFromTs = ts.TotalMinutes;
                    if (minutesFromTs > 30)
                    {

                        dict2.Add("IsValid", "false");
                        dict2.Add("Error", "Времето за порака е истечено, испратете порака за валидација повторно");
                        return Json(dict2);
                    }
                    else
                    {
                        entity.user_all.SingleOrDefault(f => f.ID == tempID).IsValidSMS = true;
                        entity.SaveChanges();
                        dict2.Add("IsValid", "true");
                        dict2.Add("Error", "Точен валидациски код, успешна валидација на вашата сметка");

                        return Json(dict2);
                    }


                }
                else
                {

                    dict2.Add("IsValid", "false");
                    dict2.Add("Error", "Не постои корисник");

                    return Json(dict2);
                }
            }
        }

    }
}

