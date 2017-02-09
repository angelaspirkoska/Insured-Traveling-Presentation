using InsuredTraveling;
using System;
using System.Linq;
using System.Web.Mvc;
using InsuredTraveling.DI;
using InsuredTraveling.Filters;
using InsuredTraveling.Controllers;
using System.IO;
using System.Web;
using System.Collections.Generic;

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

        public ActionResult AddNews(HttpPostedFileBase newsImage, string newsTitle = null, string newsContent = null, bool newsIsNotification = false)
        {
            if (newsImage == null || newsTitle == null || newsContent == null || newsTitle == "" || newsContent == "")
                return Json(new { Success = "False", Message = "All fields are required" }, JsonRequestBehavior.AllowGet); 

            var path = @"~/News/" + newsImage.FileName;
            newsImage.SaveAs(Server.MapPath(path));

            news_all news = new news_all();
            news.Title = newsTitle;
            news.Content = newsContent;
            news.isNotification = newsIsNotification;
            news.DataCreated = DateTime.Now;
            news.InsuranceCompany = "Eurolink";
            news.ImageLocation = newsImage.FileName;

            try
            {
                _ns.AddNews(news);
                ViewBag.Success = true;
                return RedirectToAction("Index", "News");
            }
            catch
            {
                // return Json(new { Success = "False", Message = "Database problem" }, JsonRequestBehavior.AllowGet);
                ViewBag.Success = false;
                return RedirectToAction("Index", "News");
            }
        }

        public ActionResult DeleteNews(int newsId)
        {
            var news = _ns.GetNewsById(newsId);
            if (news == null)
                return Json(new { Success = "False", Message = "Not found" }, JsonRequestBehavior.AllowGet);
            if(news.ImageLocation != null)
            {
                var fixedPath = Server.MapPath("~/News/" + news.ImageLocation);

                if (System.IO.Directory.Exists(fixedPath))
                {
                    try
                    {
                        System.IO.File.Delete(fixedPath);
                    }
                    catch { }
                    finally { }
                }
            }
           
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