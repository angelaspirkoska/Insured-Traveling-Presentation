using InsuredTraveling;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Authentication.WEB.Services
{
    public class MailNewsService
    {
        public void getUnreadEmails()
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            string n = "[notification]";

            using (ImapClient client = new ImapClient("imap.zoho.com", 993,
                "info@optimalreinsurance.com", "Enter4Sy", AuthMethod.Login, true))
            {
                IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());

                foreach (uint uid in uids)
                {
                    MailMessage message = client.GetMessage(uid);
                    // savanews news = new savanews();
                    news_all news = new news_all();
                    news.Title = message.Subject.Trim();
                    news.Content = message.Body.Trim();
                    news.DataCreated = DateTime.Now;
                    news.InsuranceCompany = "Eurolink";
                    Random r = new Random();
                    news.ID = r.Next(10000, 99999);
                    if (message.Subject.ToLower().StartsWith(n))
                        news.isNotification = true;
                    else
                        news.isNotification = false;

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
