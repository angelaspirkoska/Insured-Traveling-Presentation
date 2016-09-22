using Authentication.WEB.Services;
using InsuredTraveling;
using InsuredTraveling.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Authentication.WEB.Controllers
{
    [RoutePrefix("api/News")]
    public class NewsApiController : ApiController
    {
        [Route("getLatestNews")]
        [HttpGet]
        public IHttpActionResult getLatestNews()
        {
            MailNewsService mailNewsService = new MailNewsService();
            mailNewsService.getUnreadEmails();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            IQueryable<news_all> eurolinkNews = entities.news_all.OrderByDescending(x => x.DataCreated).Take(20);

            List<News> news = new List<News>();

            foreach (news_all n in eurolinkNews)
            {
                News npom = new News();
                npom.id = n.ID.ToString();
                npom.title = n.Title;
                npom.content = n.Content;
                npom.InsuranceCompany = n.InsuranceCompany;
                
                news.Add(npom);
            }

            return Ok(new { News = eurolinkNews });
        }

        [Route("getNotifications")]
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
                unreadNews = entities.news_all.OrderByDescending(x => x.DataCreated).Take(10);
            else
                unreadNews = entities.news_all.Where(x => x.DataCreated > lastReadNews.DataCreated).
                    OrderByDescending(x => x.DataCreated);

            if (unreadNews == null)
                return null;

            List<News> notifications = new List<News>();
            foreach (news_all n in unreadNews)
            {
                if ((bool)n.isNotification)
                {
                    News npom = new News();
                    npom.id = n.ID.ToString();
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
