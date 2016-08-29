using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Like
    {
        public Guid LikeId { get; set; }
        public Guid TargetId { get; set; }
        public Guid ProfileId { get; set; }
        
    }
}
