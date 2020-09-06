using MessageBox.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MessageBox.Data
{
    public class MongoDbContext : DbContext
    {
        #region Fields
        private readonly IMongoDatabase _database;

        public IMongoCollection<Message> Message { get{ return _database.GetCollection<Message>("Message"); } }
        #endregion

        #region Ctor
        public MongoDbContext(DbContextOptions<MongoDbContext> options)
            : base(options)
        {
        }

        public MongoDbContext(string connectionString, string dbName)
        {
            _database = null;

            if (string.IsNullOrEmpty(connectionString)
                || string.IsNullOrEmpty(dbName))
                return;

            try
            {
                var client = new MongoClient(connectionString);
                if (client != null)
                    _database = client.GetDatabase(dbName);
            }
            catch
            {
                //Log errors.
            }
        }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Message>().HasKey(e => e.Id);
            modelBuilder.Entity<Message>().Property(e => e.Content);
            modelBuilder.Entity<Message>().Property(e => e.DeliveredOn);
            modelBuilder.Entity<Message>().Property(e => e.ReadOn);
            modelBuilder.Entity<Message>().Property(e => e.Active);
            modelBuilder.Entity<Message>().Property(e => e.Deleted);
            modelBuilder.Entity<Message>().Property(e => e.CreatedOn);
            modelBuilder.Entity<Message>().Property(e => e.UpdatedOn);

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
