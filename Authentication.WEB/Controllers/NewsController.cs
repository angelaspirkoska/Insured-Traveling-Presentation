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
using System.Configuration;
using InsuredTraveling.Models;
using AutoMapper;

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
        //public ActionResult Index()
        //{
        //    if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        //        Response.Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");
        //    IQueryable<news_all> news = _ns.GetAllNews();
        //    return View(news);
        //}
        public ActionResult News()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                Response.Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");
            var news = _ns.GetAllNews();
            var newsList = news.Select(Mapper.Map<news_all, News>).ToList();
            NewNews newsNew = new NewNews();
            newsNew.ListNews = newsList;
            return View(newsNew);

        }
        public ActionResult AddNews1(News NewsModel)
        {
            
            var news = _ns.GetAllNews();
            var newsList = news.Select(Mapper.Map<news_all, News>).ToList();
            NewNews newsNew = new NewNews();
            newsNew.ListNews = newsList;
            if (ModelState.IsValid)
            {
                if (NewsModel.Image != null)
                {
                    var lastNewsId = _ns.LastNewsId() + 1;
                    string fileName = lastNewsId + "_" + NewsModel.Image.FileName;
                    var path = @"~/News/" + fileName;
                    NewsModel.Image.SaveAs(Server.MapPath(path));
                    NewsModel.ImageLocation = fileName;
               
                }
                NewsModel.ImageLocation = " ";
                NewsModel.InsuranceCompany = "Sava";
                try
                {
                    _ns.AddNewsNew(NewsModel);
                    ViewBag.Success = true;
                    var news1 = _ns.GetAllNews();
                    var newsList1 = news1.Select(Mapper.Map<news_all, News>).ToList();
                    NewNews newsNew1 = new NewNews();
                    newsNew1.ListNews = newsList1;
                    return View("News", newsNew1);
                }
                catch(Exception ex)
                {
                    // return Json(new { Success = "False", Message = "Database problem" }, JsonRequestBehavior.AllowGet);
                    ViewBag.Success = false;
                    // news1 = _ns.GetAllNews();
                    
                }
                return View("News", newsNew);
            }
            else
            {

                return View("News", newsNew);
            }
        }
        //public ActionResult AddNews(HttpPostedFileBase newsImage, string newsTitle = null, string newsContent = null, bool newsIsNotification = false)
        //{
        //    IQueryable<news_all> news1 = _ns.GetAllNews();
        //    if (newsImage == null || newsTitle == null || newsContent == null || newsTitle == "" || newsContent == "")
        //        return View("Index", news1);

        //    var lastNewsId = _ns.LastNewsId() + 1;
        //    string fileName = lastNewsId + "_" + newsImage.FileName;


        //    var path = @"~/News/" + fileName;
        //    newsImage.SaveAs(Server.MapPath(path));

        //    news_all news = new news_all();
        //    news.Title = newsTitle;
        //    news.Content = newsContent;
        //    news.isNotification = newsIsNotification;
        //    news.DataCreated = DateTime.Now;
        //    news.InsuranceCompany = "Eurolink";
        //    news.ImageLocation = fileName;

        //    try
        //    {
        //        _ns.AddNews(news);
        //        ViewBag.Success = true;
        //         news1 = _ns.GetAllNews();
        //        return View("Index",news1);
        //    }
        //    catch
        //    {
        //        // return Json(new { Success = "False", Message = "Database problem" }, JsonRequestBehavior.AllowGet);
        //        ViewBag.Success = false;
        //         news1 = _ns.GetAllNews();
        //        return View("Index", news1);
               
        //    }
        //}
        [HttpPost]
        public ActionResult DeleteNews(int newsId)
        {

            var news = _ns.GetAllNews();
            var newsList = news.Select(Mapper.Map<news_all, News>).ToList();
            NewNews newsNew = new NewNews();
            newsNew.ListNews = newsList;

            var news1 = _ns.GetNewsById(newsId);

            if (news1 == null)
            {
                ViewBag.Success = false;
                return View("News", newsNew);
            }
            else
            {
                if (news1.ImageLocation != null)
                {
                    var fixedPath = Server.MapPath("~/News/" + news1.ImageLocation);

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
                    ViewBag.Success = true;
                    var newsU = _ns.GetAllNews();
                    var newsListU = news.Select(Mapper.Map<news_all, News>).ToList();
                    NewNews newsNewU = new NewNews();
                    newsNewU.ListNews = newsListU;
                    return View("News", newsNewU);
                }
                catch
                {

                    ViewBag.Success = false;
                }
                return View("News", newsNew);
            }
        }
    }
}