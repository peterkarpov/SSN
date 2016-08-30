namespace Domain
{
    using Domain.Entities;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;

    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("SSN") //SSN-myaspnet
        //public EFDbContext() : base(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\A\Downloads\bp10\ESN.WebUI\App_Data\ESN3.mdf;Integrated Security=True")
        {
            // Установить новый инициализатор
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EFDbContext>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<EFDbContext>());
            Database.SetInitializer(new SampleInitializer());

        }

        // Отражение таблиц базы данных на свойства с типом DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Photobook> Photobooks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Talk> Talks { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Dislike> Dislikes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .Property(p => p.role)
                        .IsOptional();

            modelBuilder.Entity<User>()
                        .Property(p=>p.role)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Profile>()
                        .Property(t => t.fName)
                        .IsRequired();

            //Fluent API

            //modelBuilder.Entity<Profile>().Property(c => c.AvatarImageId)
            //            .HasColumnType("uniqueidentifier");

            //modelBuilder.Entity<Profile>().Property(c => c.fName)
            //            .HasMaxLength(50);

            //modelBuilder.Entity<Profile>().Property(c => c.AvatarImageId)
            //            .IsRequired();

            //modelBuilder.Entity<Profile>().HasKey(p => p.ProfileId);

            //modelBuilder.Entity<Profile>()
            //    .HasRequired(p => p.User)
            //    .WithOptional()
            //    .WillCascadeOnDelete(false);
            
            // block of Friends

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.Friends)
                .WithRequired(e => e.Profile)
                .HasForeignKey(e => e.ProfileId)
                .WillCascadeOnDelete(false);

            // block of Messages

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.ProfileFrom)
                .HasForeignKey(e => e.from)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.MessagesTo)
                .WithRequired(e => e.ProfileTo)
                .HasForeignKey(e => e.to)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.Talks1)
                .WithRequired(e => e.Profile1)
                .HasForeignKey(e => e.Profile1Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(e => e.Talks2)
                .WithRequired(e => e.Profile2)
                .HasForeignKey(e => e.Profile2Id)
                .WillCascadeOnDelete(false);

            //

            modelBuilder.Entity<Profile>()
                .HasMany(n => n.News)
                .WithRequired(n => n.Profile)
                .HasForeignKey(n => n.ProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(n => n.Photobooks)
                .WithRequired(n => n.Profile)
                .HasForeignKey(n => n.ProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(n => n.Images)
                .WithRequired(n => n.Profile)
                .HasForeignKey(n => n.ProfileId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<News>()
            //    .HasMany(c => c.Comments)
            //    .WithRequired()
            //    .HasForeignKey(n => n.CommentForId)
            //    .WillCascadeOnDelete(false);





        }
    }
}
