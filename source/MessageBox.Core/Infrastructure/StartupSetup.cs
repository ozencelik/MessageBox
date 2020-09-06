using MessageBox.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace MessageBox.Core.Infrastructure
{
    public static class StartupSetup
    {
        #region Methods
        public static void AddMySqlDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<MySqlDbContext>(options =>
                options.UseMySql(connectionString,
                mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));

        public static void AddMongoDbContext(this IServiceCollection services, string connectionString, string dbName)
        {
            services.AddDbContext<MongoDbContext>();
            new MongoDbContext(connectionString, dbName);
        }
        #endregion
    }
}
