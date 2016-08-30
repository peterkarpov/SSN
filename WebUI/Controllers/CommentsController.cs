namespace WebUI.Controllers
{
    using Domain;
    using Domain.Entities;
    using Infrastructure;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class CommentsController : Controller
    {
        [Inject]
        private IRepository repository { get; set; }
        public CommentsController()
        {
            repository = DependencyResolver.Current.GetService<IRepository>();
        }
        
        public PartialViewResult CommentsOnTarget(Guid TargetId, int? commentsCount = null)
        {
            var model = new List<Comment>();

            var comments = repository.Comments.Where(c => c.TargetId == TargetId).OrderByDescending(c => c.creationTime);

            if (commentsCount == null)
            {
                model = comments.ToList();
            }
            else
            {
                model = comments.Take((int)commentsCount).ToList();
            }
            
            return PartialView(model);
        }

        [ProfileAuth(Order = 1)]
        [Authorize]
        public PartialViewResult AddComment(Guid TargetId)
        {
            Comment newComment = new Comment();

            newComment.ProfileId = Guid.Parse(User.Identity.Name.Split('|')[1]);
            newComment.TargetId = TargetId;

            return PartialView(newComment);
        }

        [ProfileAuth(Order = 1)]
        [Authorize]
        [HttpPost]
        public ActionResult AddComment(Comment newComment, string returnUrl)
        {
            newComment.CommentId = Guid.NewGuid();
            newComment.creationTime = DateTime.Now;

            if (repository.SaveComment(newComment))
            {
                TempData["message-complete"] = string.Format("Comment added");
                return Redirect(returnUrl);
            }

            return Redirect(returnUrl);
        }

        [ProfileAuth(Order = 1)]
        [Authorize]
        [HttpPost]
        public void AjaxAddComment(Comment newComment, string returnUrl)
        {
            newComment.CommentId = Guid.NewGuid();
            newComment.creationTime = DateTime.Now;

            if (repository.SaveComment(newComment))
            {
                TempData["message-complete"] = string.Format("Comment added");
            }

        }

    }
}