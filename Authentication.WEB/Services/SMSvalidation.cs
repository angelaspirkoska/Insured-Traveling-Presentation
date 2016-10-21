using System;
using Twilio;

namespace Authentication.WEB.Services
{

    public class SMSvalidation
    {
        public string SendMessage(string PhoneNumber = "+38976686228", string ValidationCode = "testCode", bool UseCode = false)
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

            string AccountSid = "AC8bc5e0a11c0ece70a5f68d9d0fd767c9";
            string AuthToken = "1fdaac08fd532aae714932b56aa87e5f";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var message = twilio.SendMessage("+16692312384", PhoneNumber, tempCode);
            Console.WriteLine(message.Sid);

            return tempCode;
        }

    }
}