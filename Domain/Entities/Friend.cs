using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Friend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FriendId { get; set; }
        public Guid ProfileId { get; set; }
        public Guid subscriberId { get; set; }


        [ForeignKey("ProfileId")]
        internal virtual Profile Profile {get;set;}

        [ForeignKey("subscriberId")]
        internal virtual Profile subscriber { get; set; }
    }
}
