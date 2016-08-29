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
        public IEnumerable<User> Users
        {
            get
            {
                return Context.Users;
            }
        }
        
        public bool AddUser(User user)
        {
            Context.Users.Add(user);
            return Context.SaveChanges() > 0;
        }

        public bool EditUser(User user)
        {
            User dbEntry = Context.Users.Find(user.UserId);
            if (dbEntry != null)
            {
                dbEntry.login = user.login;
                dbEntry.email = user.email;
                dbEntry.password = user.password;
            }
            return Context.SaveChanges() > 0;
        }
    }
}
