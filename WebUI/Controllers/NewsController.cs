namespace WebUI.Controllers
{
    using Domain;
    using Domain.Entities;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class NewsController : Controller
    {
        [Inject]
        private IRepository repository { get; set; }
        public NewsController()
        {
            repository = DependencyResolver.Current.GetService<IRepository>();

        }

        public ActionResult AllNews()
        {
            var model = new List<News>();

            model = repository.News.OrderByDescending(n => n.creationTime).ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult OnFriendsNews(Guid? ProfileId, string login = null)
        {
            if (ProfileId == null)
            {
                var User = repository.Users.FirstOrDefault(u => u.login == login);
                if (User != null)
                {
                    ProfileId = User.UserId;
                }
            }

            var news = from f in repository.Friends
                       where f.ProfileId == ProfileId
                       join n in repository.News on f.subscriberId equals n.ProfileId
                       select n;

            var model = news.ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult OnUserNews(Guid? ProfileId, string login = null)
        {
            if (ProfileId == null)
            {
                var User = repository.Users.FirstOrDefault(u => u.login == login);
                if (User != null)
                {
                    ProfileId = User.UserId;
                }
            }

            var news = from n in repository.News
                        where n.ProfileId == ProfileId
                        select n;

            var model = news.OrderByDescending(n => n.creationTime).ToList();

            return View(model);
        }

        public ActionResult ShowOneNews(Guid NewsId)
        {
            var news = repository.News.FirstOrDefault(n => n.NewsId == NewsId);

            return View(news);
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateNews(Guid? ProfileId, string login = null)
        {

            if (ProfileId == null)
            {
                var User = repository.Users.FirstOrDefault(u => u.login == login);
                if (User != null)
                {
                    ProfileId = User.UserId;
                }
            }

            var News = new News() { ProfileId = (Guid)ProfileId };

            ViewBag.AutorName = repository.Profiles.FirstOrDefault(p => p.ProfileId == News.ProfileId).fName;

            return View(News);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateNews(News News, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                News.NewsId = Guid.NewGuid();
                News.creationTime = DateTime.Now;

                if (repository.SaveNews(News))
                {
                    return Redirect(returnUrl ?? Url.Action("OnUserNews", "News", new { ProfileId = News.ProfileId }));
                }
                else
                {
                    ModelState.AddModelError("", "Cant Save News");
                }
            }

            ViewBag.AutorName = repository.Profiles.FirstOrDefault(p => p.ProfileId == News.ProfileId).fName;

            return View();
        }

        [Authorize]
        public ActionResult DeleteNews(Guid NewsId, string returnUrl)
        {
            var news = repository.News.FirstOrDefault(n => n.NewsId == NewsId);

            if (User.Identity.Name.Split('|')[1] != news.ProfileId.ToString())
            {
                TempData["message-error"] = string.Format("News Deleted Failure");
            }

            if (repository.DeleteNews(NewsId))
            {
                TempData["message-complete"] = string.Format("News Deleted");
            }
            else
            {
                TempData["message-error"] = string.Format("News Deleted Failure");
                return Redirect(returnUrl ?? Url.Action("OnUserNews", "News", new { ProfileId = news.ProfileId }));
            }

            return Redirect(Url.Action("OnUserNews", "News", new { ProfileId = news.ProfileId }));
        }



    }
}