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
        public IEnumerable<Message> Messages
        {
            get
            {
                return Context.Messages;
                
            }
        }
        public bool SaveMessage(Message message)
        {
            if (Context.Messages.Find(message.MessageId) == null)
            {
                Context.Messages.Add(message);
            }
            else
            {
                Message dbEntry = Context.Messages.Find(message.MessageId);
                if (dbEntry != null)
                {
                    //dbEntry.Contents = message.Contents;
                    dbEntry.creationTime = message.creationTime;
                    dbEntry.to = message.to;
                    dbEntry.MessageId = message.MessageId;
                    //dbEntry.Profile = message.Profile;
                    //dbEntry.ProfileId = message.ProfileId;
                    dbEntry.from = message.from;
                    //dbEntry.theme = message.theme;
                    dbEntry.TalkId = message.TalkId;
                    dbEntry.text = message.text;
                }
            }

            return Context.SaveChanges() > 0;
        }
    }
}
