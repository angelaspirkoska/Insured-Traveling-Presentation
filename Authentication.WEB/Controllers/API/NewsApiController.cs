using Authentication.WEB.Services;
using InsuredTraveling;
using InsuredTraveling.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using InsuredTraveling.DI;

namespace Authentication.WEB.Controllers
{
    [RoutePrefix("api/News")]
    public class NewsApiController : ApiController
    {
        private INewsService _ns;

        public NewsApiController(INewsService ns)
        {
            _ns = ns;
        }

        [Route("getLatestNews")]
        [HttpGet]
        public IHttpActionResult getLatestNews()
        {
            MailNewsService mailNewsService = new MailNewsService();
            mailNewsService.getUnreadEmails();

            IQueryable<news_all> lastTwentyNews = _ns.GetLatestTwentyNews();


            List<News> news = new List<News>();

            foreach (news_all lastNews in lastTwentyNews)
            {
                lastNews.ImageLocation = System.Configuration.ConfigurationManager.AppSettings["webpage_apiurl"].ToString() + "/News/" + lastNews.ImageLocation;        
            }

            return Ok(new { News = lastTwentyNews });
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
                    npom.Id = n.ID.ToString();
                    npom.Title = n.Title;
                    npom.Content = n.Content;
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
