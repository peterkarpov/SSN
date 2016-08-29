using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Photobook
    {
        [Key]
        public Guid PhotobookId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public Guid ProfileId { get; set; }
        public bool viewForFriend { get; set; }
        public bool viewForAll { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual List<Image> Images { get; set; }

    }
}
