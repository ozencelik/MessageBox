using MessageBox.Data;
using MessageBox.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MessageBox.Core.Infrastructure
{
    public static class Seeder
    {
        #region Methods
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new MySqlDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<MySqlDbContext>>()))
            {
                //Look for any User.
                if (dbContext.User.Any())
                {
                    //DB has been seeded
                    return;
                }

                PopulateTestData(dbContext);
            }
        }
        public static void PopulateTestData(MySqlDbContext dbContext)
        {
            dbContext.User.Add(new User { Name = "Özenç" });
            dbContext.User.Add(new User { Name = "Betül" });

            dbContext.User.Add(new User { Name = "Özenç" });
            dbContext.User.Add(new User { Name = "Betül" });

            dbContext.SaveChanges();
        }
        #endregion
    }
}
