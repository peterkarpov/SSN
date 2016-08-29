using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public partial class EFRepository : IRepository
    {
        public IEnumerable<Talk> Talks
        {
            get { return Context.Talks; }
        }

        public bool SaveTalk(Talk Talk)
        {
            if (Context.Talks.FirstOrDefault(t => t.TalkId == Talk.TalkId) != null) return false;

            Context.Talks.Add(Talk);
            return Context.SaveChanges() > 0;
        }
    }
}
