using Domain.Entities.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [MetadataType(typeof(UserMetaData))]
    public class User
    {
        public Guid UserId { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int? role { get; set; }
        
        //[ForeignKey("UserId")]
        //internal virtual Profile Profile { get; set; }

    }

    public enum Role
    {
        none,
        user,
        moderator,
        admin,
    }
}
