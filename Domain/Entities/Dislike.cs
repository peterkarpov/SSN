using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Dislike
    {
        public Guid DislikeId { get; set; }
        public Guid ProfileId { get; set; }
        public Guid TargetId { get; set; }

    }
}
