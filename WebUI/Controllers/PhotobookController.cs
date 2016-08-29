using Domain;
using Domain.Entities;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using WebUI.Infrastructure;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class PhotobookController : Controller
    {
        private IRepository repository;

        public PhotobookController(IRepository repo)
        {
            repository = repo;
        }

        private int PageSize = 6;

        public ActionResult AllPhotobooks(int page = 1)
        {
            PhotobookViewModel model = new PhotobookViewModel();

            model.Photobooks = repository.Photobooks
                .OrderBy(p => p.Title)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = repository.Photobooks.Count(),
            };

            ViewBag.TotalItems = model.PagingInfo.TotalItems;

            return View(model);
        }

        public ActionResult OnUser(string ProfileId, string login, int page = 1)
        {
            PhotobookViewModel model = new PhotobookViewModel();

            Guid UserId = default(Guid);

            if (!Guid.TryParse(ProfileId, out UserId))
            {
                User user = repository.Users.FirstOrDefault(u => u.login == login);
                if (user != null) UserId = user.UserId;
            }

            model.Photobooks = repository.Photobooks.Where(p => p.ProfileId == UserId)
                .OrderBy(p => p.Title)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = repository.Photobooks.Where(p => p.ProfileId == UserId).Count(),
            };

            model.Profile = repository.Profiles.FirstOrDefault(p => p.ProfileId == UserId);

            if (model.Profile == null)
            {
                TempData["message-error"] = string.Format("Profile with login {0} not exist", login);
                return RedirectToAction("AllPhotobooks");
            }

            return View(model); //list
        }

        public ActionResult OnFriends(string ProfileId, string login, int page = 1)
        {
            PhotobookViewModel model = new PhotobookViewModel();

            Guid UserId = default(Guid);

            if (!Guid.TryParse(ProfileId, out UserId))
            {
                User user = repository.Users.FirstOrDefault(u => u.login == login);
                if (user != null) UserId = user.UserId;
            }

            var photobooksOnFriends = from f in repository.Friends
                                      where f.ProfileId == UserId
                                      join ph in repository.Photobooks on f.subscriberId equals ph.ProfileId
                                      select ph;

            model.Photobooks = photobooksOnFriends
                .OrderBy(p => p.Title)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = photobooksOnFriends.Count(),
            };

            model.Profile = repository.Profiles.FirstOrDefault(p => p.ProfileId == UserId);

            if (model.Profile == null)
            {
                TempData["message-error"] = string.Format("Profile with login \"{0}\" not exist", login);
                return RedirectToAction("AllPhotobooks");
            }

            return View(model); //list
        }

        [HttpGet]
        public ActionResult AddPhotobook(Guid ProfileId)
        {
            var model = new Photobook() { ProfileId = ProfileId };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddPhotobook(Photobook Photobook)
        {
            if (ModelState.IsValid)
            {
                if (repository.SavePhotobook(Photobook))
                {
                    TempData["message-complete"] = string.Format("new photobook added");
                }
                else
                {
                    TempData["message-error"] = string.Format("cant add photobook");
                }
            }
            else
            {
                TempData["message-error"] = string.Format("data not valid");
                return View(Photobook);
            }

            var news = new News()
            {
                creationTime = DateTime.Now,
                NewsId = Guid.NewGuid(),
                ProfileId = Photobook.ProfileId,
                theme = String.Format("[Automatic message:] I'm create new photobook: " + Photobook.Title)
            };

            repository.SaveNews(news);

            TempData["message-complete"] = string.Format("update complete");
            return RedirectToAction("OnePhotobook", new { PhotobookId = Photobook.PhotobookId });
        }

        [Authorize]
        [ProfileAuth]
        public ActionResult DeleteImage(Guid ImageId, string returnUrl = null)
        {
            var image = repository.Images.FirstOrDefault(i => i.ImageId == ImageId);

            if (image.Photobook.ProfileId.ToString() == User.Identity.Name.Split('|')[1])
            {
                if (repository.DeleteImage(ImageId))
                {
                    TempData["message-complete"] = string.Format("image successfully deleted");
                }
                else
                {
                    TempData["message-error"] = string.Format("image removal has failed");
                }
            }
            else
            {
                TempData["message-error"] = string.Format("you can not delete other profile's images");

            }
            return Redirect(returnUrl ?? Url.Action("OnePhotobook", new { PhotobookId = image.PhotobookId }));
        }

        [Authorize]
        public ActionResult MoveImageToPhotobook(Guid ImageId, Guid PhotobookId)
        {
            var image = repository.Images.FirstOrDefault(i => i.ImageId == ImageId);
            var photobook = repository.Photobooks.FirstOrDefault(p => p.PhotobookId == PhotobookId);

            bool IsImageOfProfile = true; // image.ProfileId.ToString() == User.Identity.Name.Split('|')[1];
            bool IsPhotobookOfProfile = photobook.ProfileId.ToString() == User.Identity.Name.Split('|')[1];

            if (IsImageOfProfile & IsPhotobookOfProfile)
            {
                image.PhotobookId = PhotobookId;

                if (repository.SaveImage(image))
                {
                    TempData["message-complete"] = string.Format("image successfully moved");
                }
                else
                {
                    TempData["message-error"] = string.Format("image move has failed");
                }
            }
            else
            {
                TempData["message-error"] = IsImageOfProfile ? string.Format("you can not move other profile's images") : null;
                TempData["message-error"] = IsPhotobookOfProfile ? string.Format("you can not move to other profile's photobooks") : null;
            }
            return Redirect(Url.Action("OnePhotobook", new { PhotobookId = image.PhotobookId }));
        }

        public ActionResult OnePhotobook(Guid PhotobookId, int page = 1, string searchByFileName = null)
        {
            PhotobookViewModel model = new PhotobookViewModel();

            ViewBag.PhotobooksForMoveTo = repository.Photobooks.Where(p => p.ProfileId.ToString() == User.Identity.Name.Split('|')[1]).Select(p => new Photobook { Title = p.Title, PhotobookId = p.PhotobookId });

            IEnumerable<Image> imagesOnPhotobook;

            if (searchByFileName == null)
            {
                imagesOnPhotobook = from i in repository.Images
                                    where i.PhotobookId == PhotobookId
                                    select i;
            }
            else
            {
                imagesOnPhotobook = from i in repository.Images
                                    where i.PhotobookId == PhotobookId
                                    where i.fileName.ToUpper().Contains(searchByFileName.ToUpper())
                                    select i;
            }


            model.Images = imagesOnPhotobook
                .OrderBy(p => p.ImageId)
                .Skip((page - 1) * 6)   //PageSize
                .Take(6)    //PageSize
                .OrderByDescending(p => p.DateOfLoad)
                .ToList();



            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = 6,
                TotalItems = imagesOnPhotobook.Count(),
            };

            ViewBag.TotalItems = model.PagingInfo.TotalItems;

            model.Photobooks = new List<Photobook> { repository.Photobooks.FirstOrDefault(p => p.PhotobookId == PhotobookId) };
            model.Profile = repository.Profiles.FirstOrDefault(p => p.ProfileId == model.Photobooks.First().ProfileId);

            return View(model); //one
        }

        public PartialViewResult SelectPhotobookOnProfile(Guid? ProfileId)
        {
            var photobooks = repository.Photobooks
                .Where(p => p.ProfileId == ProfileId)
                .Select(p => new Photobook
                {
                    Title = p.Title,
                    PhotobookId = p.PhotobookId
                });

            var model = photobooks.ToList();

            return PartialView(model);
        }

        public ActionResult AddImageOnPhotobook(Guid PhotobookId)
        {
            var model = new AddImageViewModel();
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

        [HttpPost]
        public ActionResult ResizeImage(AddImageViewModel model, HttpPostedFileBase uploadImage)
        {
            HtmlInputHidden img1 = new HtmlInputHidden();
            img1.Value = Request["param"];

            img1.Value = img1.Value.Remove(0, img1.Value.IndexOf('?'));

            var fs = uploadImage.InputStream;
            //var image = repository.Images.FirstOrDefault(i => i.ImageId.ToString() == "7c9f9d7c-09ec-480a-9acb-7b26e313f19f");
            //string filePath = @"C:\Users\A\documents\visual studio 2015\Projects\SSN\WebUI\App_Data\camera_200.png";
            //fs = System.IO.File.OpenRead(filePath);


            if (uploadImage != null & !string.IsNullOrEmpty(img1.Value))
                ImageBuilder.Current.Build(uploadImage.InputStream, fs, new ResizeSettings(img1.Value));
            
            if (fs != default(FileStream))
            {
                var br = new BinaryReader(fs);
                var imgbyte8 = br.ReadBytes((int)fs.Length);

                var img8 = new Image()
                {
                    ImageData = imgbyte8,
                    ImageId = Guid.NewGuid(),
                    ImageMimeType = "image/jpeg",
                    PhotobookId = model.Image.PhotobookId,
                    fileName = "no name",
                    ProfileId = model.Image.ProfileId,
                };
                repository.SaveImage(img8);
            }

            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
            {
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            }
            var newImage = new Image()
            {
                ImageId = Guid.NewGuid(),
                PhotobookId = model.Image.PhotobookId,
                ProfileId = model.Image.ProfileId,

                ImageData = imageData,
                ImageMimeType = uploadImage.ContentType,
                fileName = uploadImage.FileName,
            };
            repository.SaveImage(newImage);
            
            TempData["message-complete"] = string.Format("update complete");
            return RedirectToAction("OnePhotobook", new { PhotobookId = model.Image.PhotobookId });
        }

        [HttpPost]
        public ActionResult AddImageOnPhotobook(AddImageViewModel model, HttpPostedFileBase uploadImage)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = this.Request.Files[i] as HttpPostedFileBase;

                if (file.ContentLength == 0) continue;

                byte[] imgbyte = new byte[file.ContentLength];
                
                file.InputStream.Read(imgbyte, 0, imgbyte.Length);

                var MIME = file.ContentType;
                var fileName = file.FileName;

                var img = new Image()
                {
                    ImageData = imgbyte,
                    ImageId = Guid.NewGuid(),
                    ImageMimeType = MIME,
                    PhotobookId = model.Image.PhotobookId,
                    fileName = fileName,
                    ProfileId = model.Image.ProfileId,
                };

                repository.SaveImage(img);
            }
            
            TempData["message-complete"] = string.Format("update complete");
            return RedirectToAction("OnePhotobook", new { PhotobookId = model.Image.PhotobookId });
        }

        public PartialViewResult OpenDetailsImage(Guid ImageId)
        {
            var image = repository.Images
                .Where(i => i.ImageId == ImageId)
                .Select(i => i);

            var model = image.FirstOrDefault();

            return PartialView(model);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public JsonResult Upload()
        {
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(upload.FileName);
                    // сохраняем файл в папку Files в проекте
                    upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                }
            }
            return Json("файл загружен");
        }


    }
}