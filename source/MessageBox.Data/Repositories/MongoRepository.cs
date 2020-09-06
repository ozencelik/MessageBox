using MessageBox.Data.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBox.Data.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : BaseMongoEntity
    {
        #region Fields
        private readonly MongoDbContext _dbContext;
        #endregion

        #region Ctor
        public MongoRepository(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Properties
        public IQueryable<T> Table => _dbContext.Set<T>();
        #endregion

        #region Methods
        public async Task<int> DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<int> InsertAsync(T entity)
        {
            _dbContext.Add(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
