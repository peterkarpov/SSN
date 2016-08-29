namespace Domain
{
    using Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Security.Cryptography;
    using System.Text;

    public class SampleInitializer : DropCreateDatabaseIfModelChanges<EFDbContext>
    {
        // В этом методе можно заполнить таблицу по умолчанию
        protected override void Seed(EFDbContext context)
        {
            List<User> users = new List<User>
                {
                    new User
                    {
                        UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        login = "admin",
                        email = "admin@email.com",
                        password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("12345"))),
                        role = (int)Role.admin,

                    },
                    new User
                    {
                        UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        login = "admin2",
                        email = "admin2@email.com",
                        password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("12345"))),
                        role = (int)Role.admin,
                    }
                    // ...
                };

            foreach (var user in users)
            {
                context.Users.Add(user);
                context.Profiles.Add(new Profile
                {
                    ProfileId = user.UserId,
                    fName = user.login,
                    mName = "Ipsum",
                    lName = "Dolor",
                    dob = DateTime.Now,
                    Gender = Gender.none,
                    rStatus = RelationShipStatus.none,
                    country = Country.England,
                    Annotation = "Lorem ipsum dolor sit amet, consectetur adipisici elit, sed eiusmod tempor quis nostrud",

                });
            }

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
