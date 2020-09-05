using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActivityLogBox.Core.Services.Logs
{
    public interface IActivityLogService
    {
        /// <summary>
        /// Delete activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        Task<int> DeleteActivityLogAsync(ActivityLog activityLog);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Categories</returns>
        Task<IList<ActivityLog>> GetAllActivityLogsAsync();

        /// <summary>
        /// Gets a activityLog
        /// </summary>
        /// <param name="activityLogId">ActivityLog identifier</param>
        /// <returns>ActivityLog</returns>
        Task<ActivityLog> GetActivityLogByIdAsync(int activityLogId);

        /// <summary>
        /// Inserts activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        Task<int> InsertActivityLogAsync(ActivityLog activityLog);

        /// <summary>
        /// Updates the activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        Task<int> UpdateActivityLogAsync(ActivityLog activityLog);
    }
}