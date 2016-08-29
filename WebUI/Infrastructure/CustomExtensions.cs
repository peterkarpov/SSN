using Domain;
using Domain.Entities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static WebUI.Infrastructure.Notification;

// its experemental branch

namespace WebUI.Infrastructure
{
    public static class LoginHelpers
    {
        private static IRepository repository = DependencyResolver.Current.GetService<IRepository>();

        public static Guid? GetIdByLogin(string login)
        {
            if (string.IsNullOrEmpty(login)) return null;

            return repository.Users.FirstOrDefault(u => u.login == login)?.UserId;
        }

        public static Profile GetProfileById(this Guid GuidId)
        {
            return repository.Profiles.FirstOrDefault(p => p.ProfileId == GuidId);
        }

        public static Profile GetProfileByLogin(string login)
        {
            var profile = from p in repository.Profiles
                          join u in repository.Users on p.ProfileId equals u.UserId
                          where u.login == login
                          select p;
            
            return profile.FirstOrDefault();
        }
        
    }

    public static class SelectedListGenerator<TEnum>
    {
        public static List<SelectListItem> GetSelectedList(TEnum selected)
        {
            var SelectList = new List<SelectListItem>();

            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                SelectList.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(TEnum), value),
                    Value = ((int)value).ToString(),
                    Selected = (object)selected == value,
                });
            }

            return SelectList;
        }
    }


    /// <summary>
    /// need to move EF
    /// </summary>
    public static class NotificationRepository
    {
        public static List<Notification> NotifyRepo = new List<Notification>();

        public static int GetNotyfyCount(Guid forProfileId, NotificationType nType)
        {
            return NotifyRepo.Where(n => n.forProfileId == forProfileId).Where(n => n.nType == nType).Count();
        }

        public static int GetNotyfyCount(Guid forProfileId, Guid TargetId, NotificationType nType)
        {
            return NotifyRepo.Where(n => n.TargetId == TargetId).Where(n => n.forProfileId == forProfileId).Where(n => n.nType == nType).Count();
        }
    }


    public static class NotificationLogic
    {
        public static bool AddNotification(Guid TargetId, Guid forProfileId, NotificationType nType)
        {
            var notify = new Notification()
            {
                forProfileId = forProfileId,
                TargetId = TargetId,
                nType = nType,
                NotificationId = Guid.NewGuid(),
            };
            NotificationRepository.NotifyRepo.Add(notify);       

            return true;
        }

        public static bool RemoveNotification(Guid forProfileId, Guid TargetId)
        {
            var notify = NotificationRepository.NotifyRepo.Where(n=>n.forProfileId == forProfileId).Where(n=>n.TargetId == TargetId).ToList();

            foreach (var item in notify)
            {
                NotificationRepository.NotifyRepo.Remove(item);
            }
            
            return true;
        }
    }

    public class Notification
    {
        public Guid NotificationId { get; set; }
        public NotificationType nType { get; set; }
        public Guid TargetId { get; set; }
        public Guid forProfileId { get; set; }

        public enum NotificationType
        {
            Message,
            Friends,
            News,
            Like,
        }
    }

    /// <summary>
    /// need to move EF
    /// rewrite on dbo.Users.(DateTime)lastActivityTime
    /// </summary>
    public static class OnLineLogic
    {
        private static IDictionary<Guid, DateTime> OnLineList = new Dictionary<Guid, DateTime>(); 

        public static void AddOnLineList(Guid UserId)
        {
            
            OnLineList.Add(UserId, DateTime.Now);
            
            CleanOld();
        }

        public static void RemoveOnLineList(Guid UserId)
        {
            OnLineList.Remove(UserId);
            CleanOld();
        }

        public static bool IsOnLine(Guid UserId)
        {
            return OnLineList.ContainsKey(UserId);
        }

        public static void CleanOld()
        {
            if (OnLineList.Count() > 0)
            {
                foreach (var key in OnLineList.Keys)
                {
                    if (DateTime.Now - OnLineList[key] > TimeSpan.FromMinutes(15))
                    {
                        OnLineList.Remove(key);
                    }
                }
            }


        }
    }
}