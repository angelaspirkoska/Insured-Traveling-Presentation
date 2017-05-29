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
        public ActionResult News()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                Response.Redirect(ConfigurationManager.AppSettings["webpage_url"] + "/Login");
            var news = _ns.GetAllNews();
            var newsList = news.Select(Mapper.Map<news_all, News>).ToList();
            NewNews newsNew = new NewNews();
            newsNew.ListNews = newsList;
            newsNew.ListNews.Reverse();
            return View(newsNew);

        }
        public ActionResult AddNews1(News NewsModel)
        {
            
            var news = _ns.GetAllNews();
            var newsList = news.Select(Mapper.Map<news_all, News>).ToList();
            NewNews newsNew = new NewNews();
            newsNew.ListNews = newsList;
            newsNew.ListNews.Reverse();
            if (ModelState.IsValid || NewsModel.Title != null || NewsModel.Content != null)
            {
                if (NewsModel.Image != null)
                {
                    var lastNewsId = _ns.LastNewsId() + 1;
                    string fileName = lastNewsId + "_" + NewsModel.Image.FileName;
                    var path = @"~/News/" + fileName;
                    NewsModel.Image.SaveAs(Server.MapPath(path));
                    NewsModel.ImageLocation = fileName;

                }
                else
                {
                    NewsModel.ImageLocation = " ";
                }
                NewsModel.InsuranceCompany = "Sava";
                try
                {
                    _ns.AddNewsNew(NewsModel);
                 
                    var news1 = _ns.GetAllNews();
                    var newsList1 = news1.Select(Mapper.Map<news_all, News>).ToList();
                    NewNews newsNew1 = new NewNews();
                    newsNew1.ListNews = newsList1;
                    newsNew.ListNews.Reverse();
                    ViewBag.Success = true;
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
                ViewBag.Success = false;
                return View("News", newsNew);
            }
        }
 
        [HttpPost]
        public ActionResult DeleteNews(int newsId)
        {

            var news = _ns.GetAllNews();
            var newsList = news.Select(Mapper.Map<news_all, News>).ToList();
            NewNews newsNew = new NewNews();
            newsNew.ListNews = newsList;
            newsNew.ListNews.Reverse();
            var news1 = _ns.GetNewsById(newsId);

            if (news1 == null)
            {
               
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
                   
                    var newsU = _ns.GetAllNews();
                    var newsListU = news.Select(Mapper.Map<news_all, News>).ToList();
                    NewNews newsNewU = new NewNews();
                    newsNewU.ListNews = newsListU;
                    newsNewU.ListNews.Reverse();
                    return View("News", newsNewU);
                }
                catch
                {

                    
                }
                return View("News", newsNew);
            }
        }
    }
}