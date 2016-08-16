using System;
using Twilio;

namespace Authentication.WEB.Services
{

    public class SMSvalidation
    {
        public string SendMessage(string PhoneNumber = "+38975209792", string ValidationCode = "testCode", bool UseCode = false)
        {
            string tempCode = null;
            if (UseCode)
            {
                tempCode = ValidationCode;
            }
            else
            {
                Random rnd = new Random();
                int num = rnd.Next(0000000, 9999999);
                tempCode = num.ToString();
            }

            string AccountSid = "ACf2fa685be0cc46fd23e36c0724589334";
            string AuthToken = "ad98f7a583ac0a12c77c204526fdf557";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var message = twilio.SendMessage("+13153713573", PhoneNumber, tempCode);
            Console.WriteLine(message.Sid);

            return tempCode;
        }

    }
}

