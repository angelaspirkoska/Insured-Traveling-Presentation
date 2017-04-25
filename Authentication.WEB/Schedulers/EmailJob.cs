using Authentication.WEB.Services;
using Quartz;
using System;
using System.Net;
using System.Net.Mail;

namespace InsuredTraveling.Schedulers
{
    public class EmailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //TODO: Check if ticket with due date expired exists and send e-mail to the aasigned user
            var ticketName = "[TicketName]";
            var ticketNumber = "[TicketNumber]";
            var userName = "[TicketUser]";
            var ticketDate = "[TicketDate]";

            //var mailService = new MailService("aleksandar.spaseski@hotmail.com");
            //var emailBody = "Dear " + userName + ". Please check your ticket " + ticketNumber + ": " + ticketName + ". " + "It is past it's due date of " + ticketDate + ".";
            //mailService.setBodyText(emailBody);
            //mailService.sendMail();
        }
    }
}