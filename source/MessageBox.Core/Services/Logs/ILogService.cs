using MessageBox.Data.Entities;
using MessageBox.Data.Models;
using System;
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
        /// Insert a debug log
        /// </summary>
        /// <param name="model">Log model</param>
        /// <returns>Log id</returns>
        Task<int> LogDebugAsync(CreateLogModel model);

        /// <summary>
        /// Insert an error log
        /// </summary>
        /// <param name="model">Log model</param>
        /// <returns>Log id</returns>
        Task<int> LogErrorAsync(CreateLogModel model);

        /// <summary>
        /// Insert an infromation log
        /// </summary>
        /// <param name="model">Log model</param>
        /// <returns>Log id</returns>
        Task<int> LogInformationAsync(CreateLogModel model);

        /// <summary>
        /// Insert a fatal error log
        /// </summary>
        /// <param name="model">Log model</param>
        /// <returns>Log id</returns>
        Task<int> LogFatalErrorAsync(CreateLogModel model);

        /// <summary>
        /// Insert a warning log
        /// </summary>
        /// <param name="model">Log model</param>
        /// <returns>Log id</returns>
        Task<int> LogWarningAsync(CreateLogModel model);

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