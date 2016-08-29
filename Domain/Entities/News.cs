using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class News
    {
        public Guid NewsId { get; set; }
        public Guid ProfileId { get; set; }
        public DateTime creationTime { get; set; }
        [Required(ErrorMessage = "dont be a null")]
        public string theme { get; set; }


        ////
        //[ForeignKey("TargetId")]
        //public virtual List<Like> Likes { get; set; }

        public virtual Profile Profile { get; set; }

        public virtual List<Comment> Comments { get; set; }

        //[ForeignKey("TargetId")]
        //public virtual List<Dislike> Dislikes { get; set; }
    }
}
