using System;
using Twilio;

namespace Authentication.WEB.Services
{

    public class SMSvalidation
    {
        public string SendMessage(string PhoneNumber = "+38975358537", string ValidationCode = "testCode", bool UseCode = false)
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

            string AccountSid = "AC3fb1aa69fa5a4982d954366956a1e65f";
            string AuthToken = "04f48ad727a3b427cd1f4055a38e144c";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            
            var message = twilio.SendMessage("+18503168234", PhoneNumber, tempCode);
            Console.WriteLine(message.Sid);

            return tempCode;
        }

    }
}