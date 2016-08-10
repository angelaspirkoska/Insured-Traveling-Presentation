using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Authentication.WEB.Services
{
    class MailService
    {
        public string receiver { get; set; }

        private SmtpClient smtp;
        private MailMessage email;

        public MailService(string receiver, string sender = "info@optimalreinsurance.com", string mailServer = "smtp.zoho.com", int port = 587, string passphrase = "")
        {
            string sentFrom, pass, sentTo, mailServ;
            int portNo;

            if (mailServer != null)
                mailServ = mailServer;
            else
                mailServ = "smtp.zoho.com";

            if (port > 0)
                portNo = port;
            else
                portNo = 587;

            if (sender != null)
                sentFrom = sender;
            else
                sentFrom = "info@optimalreinsurance.com";

            sentTo = receiver;

            if (passphrase != "")
                pass = passphrase;
            else
                pass = "Enter4Sy";

            smtp = new SmtpClient(mailServ, portNo);
            smtp.Credentials = new NetworkCredential(sentFrom, pass);
            smtp.EnableSsl = true;

            email = new MailMessage(sentFrom, sentTo, "Insurance Policy Notification",
                "This is an automated message sent to you as an information about the policy you ordered.");
            email.BodyEncoding = System.Text.Encoding.UTF8;
            email.SubjectEncoding = System.Text.Encoding.UTF8;
        }

        public void setDefaults()
        {
            smtp.Host = "smtp.zoho.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("info@optimalreinsurance.com", "Enter4Sy");
            email.Subject = "Insurance Policy Notification";
            email.Body = "This is an automated message sent to you as an information about the policy you ordered.";
        }

        public void attach(Attachment item)
        {
            email.Attachments.Add(item);
        }

        public void removeAllAttachments()
        {
            email.Attachments.Clear();
        }

        public void removeAttachment(Attachment target)
        {
            email.Attachments.Remove(target);
        }

        public void setBodyText(string text = "This is an automated message sent to you as an information about the policy you ordered.", bool IsHTML = false)
        {
            email.IsBodyHtml = IsHTML;
            email.Body = text;
        }

        public void setSubject(string subject = "Insurance Policy Notification")
        {
            email.Subject = subject;
        }

        [RequireHttps()]
        public void sendMail()
        {
            smtp.EnableSsl = true;
            smtp.Send(email);
        }
    }
}
