namespace WebUI.Controllers
{
    using Domain;
    using Domain.Entities;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using WebUI.Models;

    public class ProfilesListController : Controller
    {
        private IRepository repository;

        public ProfilesListController(IRepository repo)
        {
            repository = repo;
        }
        public int pageSize = 4;
        
        public ViewResult AllProfiles(int page = 1)
        {
            ProfilesListViewModel model = new ProfilesListViewModel
            {
                Profiles = repository.Profiles
                    .OrderBy(profile => profile.fName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Profiles.Count(),
                },
                sProfile = new SearchProfileListViewModel(),
            };

            ViewBag.Total = repository.Profiles.Count();

            return View(model);
        }

        [HttpPost]
        public ViewResult Filter(SearchProfileListViewModel sProfile = default(SearchProfileListViewModel), int page = 1)
        {
            var profiles = repository.Profiles;

            if (ModelState.IsValid)
            {
                if (sProfile.fName != null) profiles = profiles.Where(f => f.fName != null ? f.fName.Contains(sProfile.fName) : false);
                if (sProfile.lName != null) profiles = profiles.Where(l => l.lName != null ? l.lName.Contains(sProfile.lName) : false);
                if (sProfile.mName != null) profiles = profiles.Where(m => m.mName != null ? m.mName.Contains(sProfile.mName) : false);

                // add day\mounth\year, add GetAge() on Profile Logic
                if (sProfile.dob != null) profiles = profiles.Where(d => d.dob != null ? d.dob > sProfile.dob : false);

                if (sProfile.Gender != null) profiles = profiles.Where(g => g.Gender == sProfile.Gender);
            }

            ProfilesListViewModel model = new ProfilesListViewModel
            {
                sProfile = sProfile,
                
                Profiles = profiles
                    .OrderBy(profile => profile.fName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = profiles.Count(),
                },
            };

            ViewBag.Total = repository.Profiles.Count();

            return View("AllProfiles", model);
        }

        public JsonResult CheckGender(Gender gender)
        {
            var result = !Enum.IsDefined(typeof(Gender), gender);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        

    }
}