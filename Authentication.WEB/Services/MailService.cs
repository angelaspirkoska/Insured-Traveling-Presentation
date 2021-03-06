﻿using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Authentication.WEB.Services
{
    class MailService
    {
        public string receiver { get; set; }

        private SmtpClient smtp;
        private MailMessage email;

        public MailService(string receiver, string sender = "policies@insuredtraveling.com", string mailServer = "smtp.zoho.com", int port = 587, string passphrase = "Enter4Sy")
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
                sentFrom = "policies@insuredtraveling.com";

            sentTo = receiver;

            if (passphrase != "")
                pass = passphrase;
            else
                pass = "Enter4Sy"; // is this the password for info@insuredtraveling.com 

            smtp = new SmtpClient(mailServ, portNo);
            smtp.Credentials = new NetworkCredential(sentFrom, pass);
            smtp.EnableSsl = true;

            email = new MailMessage(sentFrom, sentTo, "Insurance Policy Notification",
                "This is an automated message sent to you as an information about the policy you ordered.");
            email.BodyEncoding = System.Text.Encoding.UTF8;
            email.SubjectEncoding = System.Text.Encoding.UTF8;
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
        public void AlternativeViews (AlternateView  filepath)
        {
            email.AlternateViews.Add(filepath);

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
            try
            {
                smtp.EnableSsl = true;
                smtp.Send(email);
            }
            catch (Exception e) { }
     
        }
    }
}
