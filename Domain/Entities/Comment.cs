using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string text { get; set; }

        public DateTime creationTime { get; set; }
        public Guid ProfileId { get; set; }

        public Guid TargetId { get; set; }


        public virtual Profile Profile { get; set; }

        //[ForeignKey("TargetId")]
        //public virtual List<Like> Likes { get; set; }

        //[ForeignKey("TargetId")]
        //public virtual List<Dislike> Dislikes { get; set; }
    }
}
