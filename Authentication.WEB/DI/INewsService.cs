using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuredTraveling.Models;

namespace InsuredTraveling.DI
{
   public interface INewsService
    {
        IQueryable<news_all> GetAllNews();

        IQueryable<news_all> GetLatestTwentyNews();
        void AddNews(news_all News);

        void DeleteNews(int id);

        bool IsNull(int id);
    }
}
