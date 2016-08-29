namespace ClassLibrary1
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Collections.Generic;

    public partial class Model1 : DbContext
    {
        public class SampleInitializer : DropCreateDatabaseIfModelChanges<Model1>
        {
            // В этом методе можно заполнить таблицу по умолчанию
            protected override void Seed(Model1 context)
            {
                List<User> users = new List<User>
                {
                    new User
                    {
                        UserId = Guid.NewGuid(),
                        login = "admin",
                        password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("12345")))
                    },
                    // ...
                };

                foreach (var user in users)
                    context.Users.Add(user);

                context.SaveChanges();
                base.Seed(context);
            }
        }


        public Model1()
            : base("Database1")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Model1>());
            Database.SetInitializer(new SampleInitializer());
        }

        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Content> Content { get; set; }
        public virtual DbSet<Dislikes> Dislikes { get; set; }
        public virtual DbSet<Friends> Friends { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Likes> Likes { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Photobooks> Photobooks { get; set; }
        public virtual DbSet<Profiles> Profiles { get; set; }
        public virtual DbSet<Talks> Talks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profiles>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.Profiles)
                .HasForeignKey(e => e.from)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profiles>()
                .HasMany(e => e.Messages1)
                .WithRequired(e => e.Profiles1)
                .HasForeignKey(e => e.to)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profiles>()
                .HasMany(e => e.Talks)
                .WithRequired(e => e.Profiles)
                .HasForeignKey(e => e.Profile1Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profiles>()
                .HasMany(e => e.Talks1)
                .WithRequired(e => e.Profiles1)
                .HasForeignKey(e => e.Profile2Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Friends)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.ProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.Profiles)
                .WithRequired(e => e.User);
        }
    }
}
