namespace WebUI.Controllers
{
    using Domain;
    using Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using WebUI.Infrastructure;
    using WebUI.Models;

    public class ProfileController : Controller
    {
        private IRepository repository;

        public ProfileController(IRepository repo)
        {
            repository = repo;
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        private string GetFriendStatus(Guid? ProfileId)
        {
            if (!User.Identity.IsAuthenticated) return "no identity";

            var isYourProfile = ProfileId.ToString() == User.Identity.Name.Split('|')[1];

            var isYourFriend = repository.Friends.
                Where(p => p.ProfileId.ToString() == User.Identity.Name.Split('|')[1]).
                Where(p => p.subscriberId == ProfileId).
                FirstOrDefault() != null;

            var isYourSubscriber = repository.Friends.
                Where(p => p.ProfileId == ProfileId).
                Where(p => p.subscriberId.ToString() == User.Identity.Name.Split('|')[1]).
                FirstOrDefault() != null;
            
            var isBestFriends = isYourFriend & isYourSubscriber;

            string status = "none";

            if (isYourProfile)
            {
                status = "your profile";
            }
            else
            {
                if (isBestFriends)
                {
                    status = "your best friends";
                }
                else
                {
                    if (isYourFriend)
                    {
                        status = "your friend";
                    }

                    if (isYourSubscriber)
                    {
                        status = "your subscriber";
                    }
                }
            }

            return status;
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(ProfileViewModel model, string returnUrl)
        {
            model.Profile = repository.Profiles.FirstOrDefault(u => u.ProfileId.ToString() == User.Identity.Name.Split('|')[1]);

            if (model.Profile != null)
            {
                TempData["message-error"] = String.Format("profile already exist");
                return Redirect(returnUrl ?? Url.Action("Edit", "Profile", new { ProfileId = model.Profile.ProfileId }));
            }
            else
            {
                var Profile = new Profile
                {
                    ProfileId = LoginHelpers.GetIdByLogin(User.Identity.Name.Split('|')[0]),
                    fName = User.Identity.Name.Split('|')[0],
                };

                if (repository.SaveProfile(Profile))
                {
                    TempData["message-complete"] = String.Format("profile with name {0} created", Profile.fName);
                    return Redirect(returnUrl ?? Url.Action("Edit", "Profile", new { ProfileId = Profile.ProfileId }));
                }
            }
            TempData["message-error"] = String.Format("cant created profile");
            return View();
            
        }

        public ViewResult ShowOne(Guid? ProfileId, string login)
        {
            ProfileViewModel model = new ProfileViewModel();

            if (login != null)
            {
                model.User = repository.Users.FirstOrDefault(u => u.login == login);
            }

            if (model.User != null)
            {
                ProfileId = model.User.UserId;
            }
            
            model.Profile = repository.Profiles.FirstOrDefault(p => p.ProfileId == ProfileId);
            model.User = repository.Users.Where(u => u.UserId == ProfileId)
                .Select(u => new User {
                                        UserId = u.UserId,
                                        email = u.email,
                                        login = login,
                                        role = u.role
                                      }).FirstOrDefault();

            ViewBag.FriendStatus = GetFriendStatus(ProfileId);

            model.Statistic = new Dictionary<string, int>();
            model.Statistic["friends"] = repository.Friends.Where(f => f.ProfileId == ProfileId).Count();
            model.Statistic["subscribers"] = repository.Friends.Where(f => f.subscriberId == ProfileId).Count();
            model.Statistic["photobooks"] = repository.Photobooks.Where(f => f.ProfileId == ProfileId).Count();
            model.Statistic["news"] = repository.News.Where(f => f.ProfileId == ProfileId).Count();
            model.Statistic["comments"] = repository.Comments.Where(f => f.ProfileId == ProfileId).Count();

            return View(model);
        }

        [ProfileAuth(Order = 1)]
        [Authorize]
        public ActionResult Edit(Guid? ProfileId, string login, string returnUrl)
        {
            if (ProfileId == null)
                ProfileId = repository.Users.FirstOrDefault(u => u.login == login).UserId;

            var Profile = repository.Profiles.FirstOrDefault(p => p.ProfileId == ProfileId);

            if (User.Identity.Name.Split('|')[1] != Profile.ProfileId.ToString())
            {
                TempData["message-error"] = String.Format("you cant edit other profile");
                return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { ProfileId = ProfileId }));
            }
            
            return View(Profile);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Profile profile, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (repository.SaveProfile(profile))
                {
                    TempData["message-complete"] = String.Format("update profile {0} complete", profile.fName);
                    return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { ProfileId = profile.ProfileId }));
                }
                else
                {
                    TempData["message-error"] = String.Format("changes of Profile {0} have not been saved", profile.fName);
                    return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { ProfileId = profile.ProfileId }));
                }
            }
            else
            {
                ModelState.AddModelError("", "One or more parameters no valid");

                return View(profile);
            }
        }

        [Authorize]
        public ActionResult Delete(Guid? ProfileId, string returnUrl)
        {
            var user = repository.Users.FirstOrDefault(u => u.UserId == ProfileId);
            var profile = repository.Profiles.FirstOrDefault(p => p.ProfileId == ProfileId);

            if (user.login != User.Identity.Name.Split('|')[0])
            {
                TempData["message-error"] = String.Format("you can't delete other profile");
                return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { ProfileId = profile.ProfileId }));
            }

            if (repository.DeleteProfile(ProfileId) != null)
            {
                TempData["message-complete"] = String.Format("profile of {0} has been deleted", profile.fName);
                return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { ProfileId = profile.ProfileId }));
            }
            else
            {
                TempData["message-error"] = String.Format("delete profile failed");
                return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { ProfileId = profile.ProfileId }));
            }
        }

        [ProfileAuth(Order = 1)]
        [Authorize]
        public ActionResult SetAvatar(Guid ImageId, string returnUrl)
        {
            var Profile = LoginHelpers.GetProfileByLogin(User.Identity.Name.Split('|')[0]);
            
            Profile.AvatarImageId = ImageId;

            repository.SaveProfile(Profile);

            return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { ProfileId = Profile.ProfileId }));
        }

        [ProfileAuth(Order = 1)]
        [Authorize]
        public ActionResult ImagesForSetAvatar(Guid? ProfileId)
        {
            if (ProfileId == null) ProfileId = LoginHelpers.GetIdByLogin(User.Identity.Name.Split('|')[0]);

            var images = repository.Images
                .Where(i => i.ProfileId == ProfileId)
                .OrderByDescending(i => i.DateOfLoad)
                .Select(i => new Image { ImageId = i.ImageId });

            var model = images.ToList();

            return View(model);
        }
        
    }
}