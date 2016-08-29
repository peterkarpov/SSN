namespace WebUI.Controllers
{
    using Domain.Entities;
    using WebUI.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Domain;

    public class MessagesController : Controller
    {
        private IRepository repository;

        public MessagesController(IRepository repo)
        {
            repository = repo;
        }
        
        public ActionResult Talks(Guid? ProfileId, string login)
        {
            if (ProfileId == null)
                ProfileId = repository.Users.FirstOrDefault(u => u.login == login).UserId;

            MessagesViewModel model = new MessagesViewModel();

            model.Profile = repository.Profiles.FirstOrDefault(p => p.ProfileId == ProfileId);

            model.Talks = repository.Talks
                .Where(t => (t.Profile1Id == model.Profile.ProfileId) | (t.Profile2Id == model.Profile.ProfileId))
                .OrderBy(o => o.Messages.First().creationTime)
                .ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult OneTalk(Guid? TalkId = null, int page = 1, int pageSize = 4)
        {
            TalkViewModel model = new TalkViewModel();

            Talk Talk = repository.Talks.FirstOrDefault(t => t.TalkId == TalkId);

            model.Messages = new List<Message>();
            model.Messages = Talk.Messages.OrderByDescending(m => m.creationTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            Guid fromProfileId = Guid.Parse(User.Identity.Name.Split('|')[1]);

            model.from = repository.Profiles.FirstOrDefault(p => p.ProfileId == fromProfileId);

            Guid toProfileId = fromProfileId == Talk.Profile1Id ? Talk.Profile2Id : Talk.Profile1Id;

            model.to = repository.Profiles.FirstOrDefault(p => p.ProfileId == toProfileId);

            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = Talk.Messages.Count(),
            };

            model.pageSize = pageSize;

            model.TalkId = TalkId;

            // Notification
            Infrastructure.NotificationLogic
                .RemoveNotification(forProfileId: fromProfileId, TargetId: (Guid)TalkId);

            return View(model);

        }

        [Authorize]
        [HttpGet]
        public ActionResult SendMessage(Guid fromProfileId, Guid toProfileId)
        {
            var message = new Message();

            message.from = fromProfileId;
            message.to = toProfileId;
            message.ProfileFrom = repository.Profiles.FirstOrDefault(p => p.ProfileId == message.from);
            message.ProfileTo = repository.Profiles.FirstOrDefault(p => p.ProfileId == message.to);

            return View(message);
        }

        [Authorize]
        [HttpGet]
        public PartialViewResult SendMessagePartial(Guid fromProfileId, Guid toProfileId)
        {
            var message = new Message();

            message.from = (Guid)fromProfileId;
            message.to = (Guid)toProfileId;

            message.ProfileFrom = repository.Profiles.FirstOrDefault(p => p.ProfileId == fromProfileId);

            return PartialView(message);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SendMessage(Message message)
        {
            message.MessageId = Guid.NewGuid();
            message.creationTime = DateTime.Now;
            
            var Talk1 = from t in repository.Talks
                        where t.Profile1Id == message.@from
                        where t.Profile2Id == message.to
                        select t;

            var Talk2 = from t in repository.Talks
                        where t.Profile2Id == message.@from
                        where t.Profile1Id == message.to
                        select t;

            var Talk = Talk1.FirstOrDefault() ?? Talk2.FirstOrDefault();


            if (Talk == null)
            {
                Talk = new Talk
                {
                    Profile1Id = message.from,
                    Profile2Id = message.to,
                    TalkId = Guid.NewGuid(),
                };
                repository.SaveTalk(Talk);
            }

            message.TalkId = Talk.TalkId;
            if (repository.SaveMessage(message))
            {
                // Notification
                Infrastructure.NotificationLogic
                    .AddNotification(message.TalkId, message.to, Infrastructure.Notification.NotificationType.Message);
            }

            return RedirectToAction("OneTalk", new { TalkId = message.TalkId });
        }


    }
}