using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Concrete
{
    public partial class EFRepository : IRepository
    {
        public IEnumerable<Like> Likes
        {
            get
            {
                return Context.Likes;
            }
        }
        public IEnumerable<Dislike> Dislikes
        {
            get
            {
                return Context.Dislikes;
            }
        }
        
        public bool SetLike(Like Like) // make dbo.Like.(enum{Like,Dislike})type
        {
            var like = Context.Likes
                 .Where(p => p.ProfileId == Like.ProfileId)
                 .Where(t => t.TargetId == Like.TargetId)
                 .FirstOrDefault();

            if (like == null)
            {
                Context.Likes.Add(Like);
            }
            else
            {
                Context.Likes.Remove(like);
            }

            return Context.SaveChanges() > 0;

        }
        
        public bool SetDislike(Dislike Dislike)
        {
            var dislike = Context.Dislikes
                .Where(p => p.ProfileId == Dislike.ProfileId)
                .Where(t => t.TargetId == Dislike.TargetId)
                .FirstOrDefault();

            if (dislike == null)
            {
                Context.Dislikes.Add(Dislike);
            }
            else
            {
                Context.Dislikes.Remove(dislike);
            }

            return Context.SaveChanges() > 0;

        }
        
    }
}
