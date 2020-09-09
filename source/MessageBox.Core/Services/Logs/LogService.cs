using MessageBox.Data;
using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Logs
{
    public class LogService : ILogService
    {
        #region Fields
        private readonly IRepository<Log> _logRepository;
        #endregion

        #region Ctor
        public LogService(IRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }
        #endregion

        #region Methods
        public async Task<int> DeleteLogAsync(Log log)
        {
            return await _logRepository.DeleteAsync(log);
        }

        public async Task<IList<Log>> GetAllLogsAsync()
        {
            return await _logRepository.GetAllAsync();
        }

        public async Task<Log> GetLogByIdAsync(int logId)
        {
            return await _logRepository.GetByIdAsync(logId);
        }

        public async Task<int> InsertLogAsync(Log log)
        {
            return await _logRepository.InsertAsync(log);
        }

        public async Task<int> UpdateLogAsync(Log log)
        {
            return await _logRepository.UpdateAsync(log);
        }
        #endregion
    }
}
