using InsuredTraveling;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Authentication.WEB.Controllers
{
    public class NewsController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            IQueryable<news_all> news = entities.news_all;
            return View(news);
        }



        public ActionResult AddNews(string newsTitle = null, string newsContent = null, bool newsIsNotification = false)
        {
            if (newsTitle == null || newsContent == null || newsTitle == "" || newsContent == "")
                return Json(new { Success = "False", Message = "All fields are required" }, JsonRequestBehavior.AllowGet);;
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            news_all news = new news_all();
            news.Title = newsTitle;
            news.Content = newsContent;
            news.isNotification = newsIsNotification;
            news.DataCreated = DateTime.Now;
            news.InsuranceCompany = "Eurolink";
            entities.news_all.Add(news);
            try
            {
                entities.SaveChanges();

                return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = "False", Message = "Database problem" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteNews(int newsId)
        {
            //SavaNewsEntities entities = new SavaNewsEntities();
            //savanews newsForDelete = entities.savanews.Where(x => x.ID == newsId).FirstOrDefault();
            InsuredTravelingEntity entities = new InsuredTravelingEntity();
            news_all newsForDelete = entities.news_all.Where(x => x.ID == newsId).FirstOrDefault();
            if (newsForDelete == null)
                return Json(new { Success = "False", Message = "Database problem" }, JsonRequestBehavior.AllowGet);

            // entities.savanews.Remove(newsForDelete);
            entities.news_all.Remove(newsForDelete);
            try
            {
                entities.SaveChanges();
                return Json(new { Success = "True" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Success = "False", Message = "Database problem" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}