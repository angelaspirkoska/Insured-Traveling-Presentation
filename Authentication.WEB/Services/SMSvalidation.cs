using System;
using Twilio;

namespace Authentication.WEB.Services
{

    public class SMSvalidation
    {
        public string SendMessage(string PhoneNumber = "+38977916316", string ValidationCode = "testCode", bool UseCode = false)
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

            string AccountSid = "ACe3ee0e5c96203dc9d1fb0787b0c535f0";
            string AuthToken = "67fc302c8782cfd2a80aa4a828d1b0f9";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var message = twilio.SendMessage("+16313434353", PhoneNumber, tempCode);
            Console.WriteLine(message.Sid);

            return tempCode;
        }

    }
}