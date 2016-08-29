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
        public IEnumerable<Profile> Profiles
        {
            get
            {
                return Context.Profiles;
            }
        }
        
        public bool SaveProfile(Profile profile)
        {
            if (Context.Profiles.Find(profile.ProfileId) == null)
            {
                Context.Profiles.Add(profile);
            }
            else
            {
                Profile dbEntry = Context.Profiles.Find(profile.ProfileId);
                if (dbEntry != null)
                {
                    dbEntry.Annotation = profile.Annotation;
                    dbEntry.country = profile.country;
                    dbEntry.dob = profile.dob;
                    dbEntry.fName = profile.fName;
                    dbEntry.Gender = profile.Gender;
                    dbEntry.language = profile.language;
                    dbEntry.lName = profile.lName;
                    dbEntry.mName = profile.mName;
                    dbEntry.rStatus = profile.rStatus;
                    dbEntry.showEmail = profile.showEmail;

                    //dbEntry.language = profile.language;
                    dbEntry.languageForDB = profile.languageForDB;
                    dbEntry.AvatarImageId = profile.AvatarImageId;
                }
            }
            return Context.SaveChanges() > 0;
        }

        public Profile DeleteProfile(Guid? ProfileId)
        {
            Profile dbEntry = Context.Profiles.Find(ProfileId);
            if (dbEntry != null)
            {
                Context.Profiles.Remove(dbEntry);
                Context.SaveChanges();
            }
            return dbEntry;

        }
    }
}
