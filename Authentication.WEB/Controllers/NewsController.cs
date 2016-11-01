using InsuredTraveling;
using System;
using System.Linq;
using System.Web.Mvc;
using InsuredTraveling.DI;
using InsuredTraveling.Filters;

namespace Authentication.WEB.Controllers
{
    [SessionExpire]
    public class NewsController : Controller
    {

        private INewsService _ns;

        public NewsController(INewsService ns)
        {
            _ns = ns;
        }


        [HttpGet]
        public ActionResult Index()
        {


            IQueryable<news_all> news = _ns.GetAllNews();
            return View(news);
        }

        public ActionResult AddNews(string newsTitle = null, string newsContent = null, bool newsIsNotification = false)
        {
            if (newsTitle == null || newsContent == null || newsTitle == "" || newsContent == "")
                return Json(new { Success = "False", Message = "All fields are required" }, JsonRequestBehavior.AllowGet); ;

            news_all news = new news_all();
            news.Title = newsTitle;
            news.Content = newsContent;
            news.isNotification = newsIsNotification;
            news.DataCreated = DateTime.Now;
            news.InsuranceCompany = "Eurolink";

            try
            {
                _ns.AddNews(news);

                return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = "False", Message = "Database problem" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteNews(int newsId)
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();

            if (!_ns.IsNull(newsId))
                return Json(new { Success = "False", Message = "Database problem" }, JsonRequestBehavior.AllowGet);
            _ns.DeleteNews(newsId);

            try
            {
                _ns.DeleteNews(newsId);
                return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = "False", Message = "Database problem" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}