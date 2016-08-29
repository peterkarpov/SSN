using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Message
    {
        [Key]
        public Guid MessageId { get; set; }
        public Guid from { get; set; }

        public string text { get; set; }

        public DateTime creationTime { get; set; }

        public Guid TalkId { get; set; }
        
        public Guid to { get; set; }

        [ForeignKey("to")]
        public virtual Profile ProfileTo { get; set; }
        
        [ForeignKey("from")]
        public virtual Profile ProfileFrom { get; set; }

        // Ссылка на контент
        //public virtual List<Content> Contents { get; set; }
    }
}
