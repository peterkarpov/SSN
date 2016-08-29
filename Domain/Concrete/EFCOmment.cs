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
        public IEnumerable<Comment> Comments
        {
            get
            {
                return Context.Comments;
            }
        }

        public bool SaveComment(Comment comment)
        {
            if (comment.CommentId != default(Guid))
            {
                Context.Comments.Add(comment);
            }
            else if (string.IsNullOrEmpty(comment.text)) //deleting ?? need more tests
            {
                Comment dbEntry = Context.Comments.Find(comment.CommentId);
                if (dbEntry != null)
                {
                    Context.Comments.Remove(dbEntry);
                }
            }
            else
            {
                Comment dbEntry = Context.Comments.Find(comment.CommentId);
                if (dbEntry != null)
                {
                    dbEntry.text = comment.text;

                }
            }

            return Context.SaveChanges() > 0;
        }
    }
}
