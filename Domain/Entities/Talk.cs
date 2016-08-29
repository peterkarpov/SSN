using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Talk
    {
        [Key]
        public Guid TalkId { get; set; }
        public Guid Profile1Id { get; set; }
        public Guid Profile2Id { get; set; }

        [ForeignKey("Profile1Id")]
        public virtual Profile Profile1 { get; set; }

        [ForeignKey("Profile2Id")]
        public virtual Profile Profile2 { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}
