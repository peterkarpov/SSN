namespace WebUI.Controllers
{
    using Domain;
    using Domain.Entities;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using WebUI.Infrastructure;
    using WebUI.Models;

    public class FriendsListController : Controller
    {
        private IRepository repository;

        private int PageSize = 4;

        public FriendsListController(IRepository repo)
        {
            repository = repo;
        }

        private Profile GetProfileByLogin(string login)
        {
            Profile profile = null;

            if (!String.IsNullOrEmpty(login))
            {
                var user = repository.Users.FirstOrDefault(u => u.login == login);

                if (user != null)
                {
                    profile = repository.Profiles.FirstOrDefault(p => p.ProfileId == user.UserId);
                }
            }

            return profile;
        }

        [Authorize]
        [ProfileAuth(Order = 1)]
        public ActionResult FriendsOfUser(Guid? ProfileId, string login, int page = 1)
        {
            if (ProfileId == null)
            {
                var User = repository.Users.FirstOrDefault(u => u.login == login);
                if (User != null)
                {
                    ProfileId = User.UserId;
                }
            }

            ProfilesListViewModel model = new ProfilesListViewModel();

            model.Profile = repository.Profiles
                .Where(p => p.ProfileId == ProfileId)
                .Select(p=>new Profile { fName = p.fName })
                .FirstOrDefault();

            var friendsOfProfile = from f in repository.Friends
                                   where f.ProfileId == ProfileId
                                   join p in repository.Profiles on f.subscriberId equals p.ProfileId
                                   select p;

            model.Profiles = friendsOfProfile
                .OrderBy(p => p.fName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = friendsOfProfile.Count(),
            };

            ViewBag.TotalItems = model.PagingInfo.TotalItems;

            return View(model);
        }

        [Authorize]
        [ProfileAuth(Order = 1)]
        public ActionResult AddToFriends(Guid? ProfileId, Guid? subscriberId, string returnUrl, string login = null, string subscriberlogin = null)
        {
            if (ProfileId == null)
            {
                var Profile = GetProfileByLogin(login);
                ProfileId = Profile.ProfileId;
            }

            if (subscriberId == null)
            {
                var subscriber = GetProfileByLogin(login);
                subscriberId = subscriber.ProfileId;
            }

            var friends = new Friend()
            {
                ProfileId = (Guid)ProfileId,
                subscriberId = (Guid)subscriberId,
            };

            if (repository.AddToFriends(friends))
            {
                TempData["message-complete"] = string.Format("you added to friends");
            }
            else
            {
                TempData["message-complete"] = string.Format("cant add to friends");
            }

            return Redirect(returnUrl);
        }

        public ActionResult RemoveFromFriends(Guid? ProfileId, Guid? subscriberId, string returnUrl, string login = null, string subscriberlogin = null)
        {
            if (ProfileId == null)
            {
                var Profile = GetProfileByLogin(login);
                ProfileId = Profile.ProfileId;
            }

            if (subscriberId == null)
            {
                var subscriber = GetProfileByLogin(login);
                subscriberId = subscriber.ProfileId;
            }

            var friends = new Friend()
            {
                ProfileId = (Guid)ProfileId,
                subscriberId = (Guid)subscriberId,
            };

            if (repository.RemoveFromFriends(friends))
            {
                TempData["message-complete"] = string.Format("you del to friends");
            }
            else
            {
                TempData["message-complete"] = string.Format("cant del to friends");
            }

            return Redirect(returnUrl);

        }

        [Authorize]
        public ActionResult SubscriberOfUser(Guid? ProfileId, string login, int page = 1)
        {
            if (ProfileId == null)
            {
                var User = repository.Users.FirstOrDefault(u => u.login == login);
                if (User != null)
                {
                    ProfileId = User.UserId;
                }
            }

            ProfilesListViewModel model = new ProfilesListViewModel();

            model.Profile = repository.Profiles
                .Where(p => p.ProfileId == ProfileId)
                .Select(p => new Profile { fName = p.fName })
                .FirstOrDefault();

            var subscribersOfUser = from s in repository.Friends
                                    where s.subscriberId == ProfileId
                                    join p in repository.Profiles on s.ProfileId equals p.ProfileId
                                    select p;

            model.Profiles = subscribersOfUser
                .OrderBy(p => p.fName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = subscribersOfUser.Count(),
            };

            ViewBag.TotalItems = model.PagingInfo.TotalItems;

            return View(model);
        }
    }
}