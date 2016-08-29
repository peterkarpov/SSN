using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public partial class EFRepository : IRepository
    {
        public IEnumerable<News> News
        {
            get
            {
                return Context.News;
            }
        }

        public bool SaveNews(News News)
        {
            var dbEntry = Context.News.Find(News.NewsId);

            if (dbEntry == null)
            {
                if (News.NewsId == default(Guid)) News.NewsId = Guid.NewGuid();
                Context.News.Add(News);
            }
            else
            {
                dbEntry.creationTime = News.creationTime;
                dbEntry.theme = News.theme;
            }

            return Context.SaveChanges() > 0;
        }

        public bool DeleteNews(Guid NewsId)
        {
            var dbEntry = Context.News.Find(NewsId);

            if (dbEntry != null)
                Context.News.Remove(dbEntry);

            return Context.SaveChanges() > 0;
        }
    }
}
