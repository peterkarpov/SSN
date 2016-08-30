using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MetaData
{
    internal partial class UserMetaData
    {
        [Key]
        public Guid UserId { get; set; }
        

        [StringLength(50)]
        [Required]
        public string login { get; set; }
        

        [StringLength(50)]
        [Required]

        public string password { get; set; }
        

        [StringLength(50)]
        [Required]
        public string email { get; set; }


    }
}
