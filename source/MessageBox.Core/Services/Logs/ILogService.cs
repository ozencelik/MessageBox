using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Logs
{
    public interface ILogService
    {
        /// <summary>
        /// Delete log
        /// </summary>
        /// <param name="log">Log</param>
        Task<int> DeleteLogAsync(Log log);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Categories</returns>
        Task<IList<Log>> GetAllLogsAsync();

        /// <summary>
        /// Gets a log
        /// </summary>
        /// <param name="logId">Log identifier</param>
        /// <returns>Log</returns>
        Task<Log> GetLogByIdAsync(int logId);

        /// <summary>
        /// Inserts log
        /// </summary>
        /// <param name="log">Log</param>
        Task<int> InsertLogAsync(Log log);

        /// <summary>
        /// Updates the log
        /// </summary>
        /// <param name="log">Log</param>
        Task<int> UpdateLogAsync(Log log);
    }
}