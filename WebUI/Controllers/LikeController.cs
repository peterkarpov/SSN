namespace WebUI.Controllers
{
    using Domain;
    using Domain.Entities;
    using Newtonsoft.Json;
    using Ninject;
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    // need to create One Entities for Like/Dislike and use enum type{ Like, Dislike, more... } 

    public class LikeController : Controller
    {
        [Inject]
        private IRepository repository { get; set; }
        public LikeController()
        {
            repository = DependencyResolver.Current.GetService<IRepository>();
        }

        [Authorize]
        [HttpGet]
        public void AjaxSetLike(string jsonData)
        {
            var ProfileId = Guid.Parse(User.Identity.Name.Split('|')[1]);

            var TargetId = Guid.Parse(jsonData);

            Like Like = new Like()
            {
                LikeId = Guid.NewGuid(),
                ProfileId = (Guid)ProfileId,
                TargetId = TargetId,
            };
            repository.SetLike(Like);

        }

        [Authorize]
        public int AjaxGetLike(string jsonData)
        {
            var TargetId = Guid.Parse(jsonData);
            
            return repository.Likes.Where(t => t.TargetId == TargetId).Count();
        }

        [Authorize]
        [HttpGet]
        public void AjaxSetDislike(string jsonData)
        {
            var ProfileId = Guid.Parse(User.Identity.Name.Split('|')[1]);

            var TargetId = Guid.Parse(jsonData);

            Dislike Dislike = new Dislike()
            {
                DislikeId = Guid.NewGuid(),
                ProfileId = (Guid)ProfileId,
                TargetId = TargetId,
            };
            repository.SetDislike(Dislike);
        }

        [Authorize]
        public int AjaxGetDislike(string jsonData)
        {
            var TargetId = Guid.Parse(jsonData);

            return repository.Dislikes.Where(t => t.TargetId == TargetId).Count();
        }











        // old version //
        [Authorize]
        public ActionResult SetLike(Guid TargetId, Guid? ProfileId, string login, string returnUrl)
        {
            if (ProfileId == null)
            {
                var User = repository.Users.FirstOrDefault(u => u.login == login);
                if (User != null)
                {
                    ProfileId = User.UserId;
                }
            }

            Like Like = new Like()
            {
                LikeId = Guid.NewGuid(),
                ProfileId = (Guid)ProfileId,
                TargetId = TargetId,
            };
            repository.SetLike(Like);

            return Redirect(returnUrl);

        }

        public int GetLike(Guid? TargetId)
        {
            return repository.Likes.Where(t => t.TargetId == TargetId).Count();
        }

        [Authorize]
        public ActionResult SetDislike(Guid TargetId, Guid? ProfileId, string login, string returnUrl)
        {
            if (ProfileId == null)
            {
                var User = repository.Users.FirstOrDefault(u => u.login == login);
                if (User != null)
                {
                    ProfileId = User.UserId;
                }
            }

            Dislike Dislike = new Dislike()
            {
                DislikeId = Guid.NewGuid(),
                ProfileId = (Guid)ProfileId,
                TargetId = TargetId,
            };
            repository.SetDislike(Dislike);

            return Redirect(returnUrl);

        }

        public int GetDislike(Guid? TargetId)
        {
            return repository.Dislikes.Where(t => t.TargetId == TargetId).Count();
        }

       
    }
}