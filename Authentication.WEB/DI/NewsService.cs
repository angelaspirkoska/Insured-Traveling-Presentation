using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
    public class NewsService : INewsService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();
        public void AddNews(news_all News)
        {
            _db.news_all.Add(News);
            _db.SaveChanges();
        }

        public void DeleteNews(int id)
        {
            news_all newsForDelete = _db.news_all.Where(x => x.ID == id).FirstOrDefault();
            _db.news_all.Remove(newsForDelete);
        }

        public IQueryable<news_all> GetAllNews()
        {
            IQueryable<news_all> news = _db.news_all;
            return news;
        }

        public IQueryable<news_all> GetLatestTwentyNews()
        {
            return _db.news_all.OrderByDescending(x => x.DataCreated).Take(20);
        }

        public bool IsNull(int id)
        {
            if (_db.news_all.Where(x => x.ID == id).FirstOrDefault() == null)
                return true;
            else return false;
        }
    }
}