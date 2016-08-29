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

    public class HomeController : Controller
    {
        private IRepository repository;

        public HomeController(IRepository repo)
        {
            repository = repo;
        }

        public ActionResult Index()
        {
            ViewBag.ProfilesCount   = String.Format("Profiles: "   + repository.Profiles.Count());
            ViewBag.UsersCount      = String.Format("Users: "      + repository.Users.Count());
            ViewBag.ImagesCount     = String.Format("Images: "     + repository.Images.Count());
            ViewBag.PhotobooksCount = String.Format("Photobooks: " + repository.Photobooks.Count());
            
            return View();
        }

        public ActionResult Index8()
        {
            var model = new AddImageViewModel();

            var PhotobookId = repository.Photobooks
                .Where(p => p.ProfileId.ToString() == User.Identity.Name.Split('|')[1])
                .Select(p => p.PhotobookId)
                .FirstOrDefault();

            model.Image = new Image()
            {
                ImageId = Guid.NewGuid(),
                PhotobookId = PhotobookId,
                ProfileId = (Guid)LoginHelpers.GetIdByLogin(User.Identity.Name.Split('|')[0]),
            };

            var photobooksData = repository.Photobooks
                .Where(p => p.ProfileId == model.Image.ProfileId)
                .OrderBy(p => p.Title)
                .Select(p => new Photobook
                {
                    Title = p.Title,
                    PhotobookId = p.PhotobookId
                });

            model.SelectListPhotobook = new List<SelectListItem>(photobooksData.Count());

            foreach (var photobook in photobooksData)
            {
                model.SelectListPhotobook.Add(new SelectListItem
                {
                    Value = photobook.PhotobookId.ToString(),
                    Text = photobook.Title,
                    Selected = photobook.PhotobookId == PhotobookId ? true : false,
                });
            }

            return View(model);
        }
    }
}