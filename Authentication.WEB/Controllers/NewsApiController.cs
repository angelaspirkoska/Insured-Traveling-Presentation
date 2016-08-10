using Authentication.WEB.Models;
using Authentication.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Authentication.WEB.Controllers
{
    public class NewsApiController : ApiController
    {

        [HttpGet]
        public IHttpActionResult getLatestNews()
        {
            MailNewsService mailNewsService = new MailNewsService();
            mailNewsService.getUnreadEmails();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            IQueryable<news_all> eurolinkNews = entities.news_all.OrderByDescending(x => x.DateCreated).Take(20);

            List<News> news = new List<News>();

            foreach (news_all n in eurolinkNews)
            {
                News npom = new News();
                npom.id = n.ID;
                npom.title = n.Title;
                npom.content = n.Content;
                news.Add(npom);
            }

            return Ok(new { News = news });
        }

        [HttpGet]
        public IHttpActionResult getNotifications(int lastReadID = -1)
        {
            if (lastReadID == -1)
                return null;

            MailNewsService mailNewsService = new MailNewsService();
            mailNewsService.getUnreadEmails();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            news_all lastReadNews = entities.news_all.Where(x => x.ID == lastReadID).FirstOrDefault();
            IQueryable<news_all> unreadNews;
            if (lastReadNews == null)
                unreadNews = entities.news_all.OrderByDescending(x => x.DateCreated).Take(10);
            else
                unreadNews = entities.news_all.Where(x => x.DateCreated > lastReadNews.DateCreated).
                    OrderByDescending(x => x.DateCreated);

            if (unreadNews == null)
                return null;

            List<News> notifications = new List<News>();
            foreach (news_all n in unreadNews)
            {
                if ((bool)n.IsNotification)
                {
                    News npom = new News();
                    npom.id = n.ID;
                    npom.title = n.Title;
                    npom.content = n.Content;
                    notifications.Add(npom);
                }
            }

            if (notifications.Count == 0)
                return null;
            else
                return Ok(new { News = notifications });

        }
    }
}
