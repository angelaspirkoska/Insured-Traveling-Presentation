﻿using Authentication.WEB.Services;
using InsuredTraveling.DI;
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
            var kanbanService = new KanbanService();
            var tickets = kanbanService.GetTicketsForEmailNotifications();

            foreach (var ticket in tickets)
            {
                var ticketName = ticket.Name;
                var ticketPoolList = ticket.kanbanpoollist.Name;
                var deadline = ticket.DeadlineDate.ToString("dd/mmm/yyyy");

                foreach (var assignee in ticket.kanbanticketassignedtoes)
                {
                    var assigneeEmail = assignee.aspnetuser.Email;
                    var assigneeUserName = assignee.aspnetuser.UserName;
                    var mailService = new MailService(assigneeEmail);
                    var emailBody = "Dear " + assigneeUserName + ". Please check your ticket " + ticketName + " from the pool: " + ticketPoolList + ". Its deadline is: " + deadline + ".";
                    mailService.setBodyText(emailBody);
                    //mailService.sendMail();
                }
            }


        }
    }
}