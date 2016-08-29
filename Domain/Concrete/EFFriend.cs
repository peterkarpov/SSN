using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public partial class EFRepository : IRepository
    {

        public IEnumerable<Friend> Friends
        {
            get
            {
                return Context.Friends;
                
            }
        }

        public bool AddToFriends(Friend Friend)
        {
            var friend = from f in Context.Friends
                         where f.ProfileId == Friend.ProfileId
                         where f.subscriberId == Friend.subscriberId
                         select f;

            if (friend != null)
            {
                Context.Friends.Add(Friend);
            }

            return Context.SaveChanges() > 0;

        }

        public bool RemoveFromFriends(Friend Friend)
        {
            var friend = from f in Context.Friends
                         where f.ProfileId == Friend.ProfileId
                         where f.subscriberId == Friend.subscriberId
                         select f;

            Friend = friend.FirstOrDefault();

            if (Friend != null)
            {
                Context.Friends.Remove(Friend);
            }

            return Context.SaveChanges() > 0;
        }
        public bool setToFriends(Friend Friend)
        {
            var friend = from f in Context.Friends
                         where f.ProfileId == Friend.ProfileId
                         where f.subscriberId == Friend.subscriberId
                         select f;

            if (friend != null)
            {
                Context.Friends.Remove(Friend);
            }
            else
            {
                Context.Friends.Remove(Friend);
            }

            return Context.SaveChanges() > 0;
        }
        public bool isFriend(Friend Friend)
        {
            var friend = from f in Context.Friends
                         where f.ProfileId == Friend.ProfileId
                         where f.subscriberId == Friend.subscriberId
                         select f;

            return friend != null;
        }

        public bool isBestFriend(Friend Friend)
        {
            var friend1 = from f in Context.Friends
                          where f.ProfileId == Friend.ProfileId
                          where f.ProfileId == Friend.subscriberId
                          select f;

            var friend2 = from f in Context.Friends
                          where f.ProfileId == Friend.subscriberId
                          where f.subscriberId == Friend.ProfileId
                          select f;

            return (friend1 != null) && (friend2 != null);
        }
    }
}
