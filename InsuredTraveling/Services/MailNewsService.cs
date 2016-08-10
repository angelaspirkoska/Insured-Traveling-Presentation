﻿using S22.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.WEB.Services
{
    public class MailNewsService
    {
        public void getUnreadEmails()
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            string n = "[notification]";

            using (ImapClient client = new ImapClient("imap.zoho.com", 993,
                "eurolink@optimalreinsurance.com", "Eurolink1234", AuthMethod.Login, true))
            {
                IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());

                foreach (uint uid in uids)
                {
                    MailMessage message = client.GetMessage(uid);
                    // savanews news = new savanews();
                    news_all news = new news_all();
                    news.Title = message.Subject.Trim();
                    news.Content = message.Body.Trim();
                    news.DateCreated = DateTime.Now;
                    if (message.Subject.ToLower().StartsWith(n))
                        news.IsNotification = true;
                    else
                        news.IsNotification = false;

                    entities.news_all.Add(news);

                    client.MoveMessage(uid, "SeenNews");
                    client.DeleteMessage(uid, "Inbox");
                }

                client.Expunge("Inbox");
                client.Dispose();
            }

            entities.SaveChanges();
        }
    }
}
