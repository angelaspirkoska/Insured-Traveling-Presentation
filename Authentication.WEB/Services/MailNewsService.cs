using InsuredTraveling;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Authentication.WEB.Services
{
    public class MailNewsService
    {
        public void getUnreadEmails()
        {
            InsuredTravelingEntity2 entities = new InsuredTravelingEntity2();
            string n = "[notification]";

            using (ImapClient client = new ImapClient("imap.zoho.com", 993,
                "news@insuredtraveling.com", "Enter4Sy", AuthMethod.Login, true))
            {
                IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());

                foreach (uint uid in uids)
                {
                    MailMessage message = client.GetMessage(uid);
                    news_all news = entities.news_all.Create(); ;
                    news.Title = message.Subject.Trim();
                    news.Content = message.Body.Trim();
                    news.DataCreated = (DateTime)message.Date();
                    news.InsuranceCompany = "Eurolink";
                    Random r = new Random();
                    //news.ID = r.Next(10000, 99999);
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

                if (uids.Count() != 0)
                    entities.SaveChanges();

            }

           
        }
    }
}
