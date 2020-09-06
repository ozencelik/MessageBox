using MessageBox.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBox.Data
{
    public class MySqlDbContext : DbContext
    {
        #region Fields
        public DbSet<User> User { get; set; }

        public DbSet<BlockedUser> BlockedUser { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<Log> Log { get; set; }

        public DbSet<ActivityLog> ActivityLog { get; set; }
        #endregion

        #region Ctor
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
            : base(options)
        {
        }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().HasKey(e => e.Id);
            modelBuilder.Entity<User>().Property(e => e.Name);
            modelBuilder.Entity<User>().Property(e => e.Username);
            modelBuilder.Entity<User>().Property(e => e.Email);
            modelBuilder.Entity<User>().Property(e => e.Password);
            //modelBuilder.Entity<User>().HasOne(e => e.ParentCategory);
            modelBuilder.Entity<User>().Property(e => e.Active);
            modelBuilder.Entity<User>().Property(e => e.Deleted);
            modelBuilder.Entity<User>().Property(e => e.CreatedOn);
            modelBuilder.Entity<User>().Property(e => e.UpdatedOn);

            modelBuilder.Entity<BlockedUser>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<BlockedUser>().HasKey(e => e.Id);
            modelBuilder.Entity<BlockedUser>().Property(e => e.BlockingUserId);
            modelBuilder.Entity<BlockedUser>().Property(e => e.BlockedUserId);
            modelBuilder.Entity<BlockedUser>().Property(e => e.Active);
            modelBuilder.Entity<BlockedUser>().Property(e => e.Deleted);
            modelBuilder.Entity<BlockedUser>().Property(e => e.CreatedOn);
            modelBuilder.Entity<BlockedUser>().Property(e => e.UpdatedOn);

            modelBuilder.Entity<Message>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Message>().HasKey(e => e.Id);
            modelBuilder.Entity<Message>().Property(e => e.Content);
            modelBuilder.Entity<Message>().Property(e => e.DeliveredOn);
            modelBuilder.Entity<Message>().Property(e => e.ReadOn);
            modelBuilder.Entity<Message>().Property(e => e.Active);
            modelBuilder.Entity<Message>().Property(e => e.Deleted);
            modelBuilder.Entity<Message>().Property(e => e.CreatedOn);
            modelBuilder.Entity<Message>().Property(e => e.UpdatedOn);

            modelBuilder.Entity<Log>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Log>().HasKey(e => e.Id);
            modelBuilder.Entity<Log>().Property(e => e.LogType);
            modelBuilder.Entity<Log>().Property(e => e.UserId);
            modelBuilder.Entity<Log>().Property(e => e.Title);
            modelBuilder.Entity<Log>().Property(e => e.Message);
            modelBuilder.Entity<Log>().Property(e => e.Fixed);
            modelBuilder.Entity<Log>().Property(e => e.Active);
            modelBuilder.Entity<Log>().Property(e => e.Deleted);
            modelBuilder.Entity<Log>().Property(e => e.CreatedOn);
            modelBuilder.Entity<Log>().Property(e => e.UpdatedOn);

            modelBuilder.Entity<ActivityLog>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ActivityLog>().HasKey(e => e.Id);
            modelBuilder.Entity<ActivityLog>().Property(e => e.ActivityLogType);
            modelBuilder.Entity<ActivityLog>().Property(e => e.UserId);
            modelBuilder.Entity<ActivityLog>().Property(e => e.Message);
            modelBuilder.Entity<ActivityLog>().Property(e => e.Active);
            modelBuilder.Entity<ActivityLog>().Property(e => e.Deleted);
            modelBuilder.Entity<ActivityLog>().Property(e => e.CreatedOn);
            modelBuilder.Entity<ActivityLog>().Property(e => e.UpdatedOn);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
        #endregion
    }
}
