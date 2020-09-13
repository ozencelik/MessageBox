using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Logs
{
    public interface IActivityLogService
    {
        /// <summary>
        /// Delete activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        Task<int> DeleteActivityLogAsync(ActivityLog activityLog);

        /// <summary>
        /// Gets all activity logs
        /// </summary>
        /// <returns>Categories</returns>
        Task<IList<ActivityLog>> GetAllActivityLogsAsync();

        /// <summary>
        /// Gets all activity logs
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Categories</returns>
        Task<IList<ActivityLog>> GetAllActivityLogsAsync(int userId);

        /// <summary>
        /// Gets a activityLog
        /// </summary>
        /// <param name="activityLogId">ActivityLog identifier</param>
        /// <returns>ActivityLog</returns>
        Task<ActivityLog> GetActivityLogByIdAsync(int activityLogId);

        /// <summary>
        /// Gets a activityLog
        /// </summary>
        /// <param name="activityLogId">ActivityLog identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>ActivityLog</returns>
        Task<ActivityLog> GetActivityLogByIdAsync(int activityLogId, int userId);

        /// <summary>
        /// Insert a blocked user activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        /// <returns>ActivityLog id</returns>
        Task<int> LogBlockedUserActivityAsync(ActivityLog activityLog);

        /// <summary>
        /// Insert an invalid blocked user activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        /// <returns>ActivityLog id</returns>
        Task<int> LogInvalidBlockedUserActivityAsync(ActivityLog activityLog);

        /// <summary>
        /// Insert an login activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        /// <returns>ActivityLog id</returns>
        Task<int> LogLoginActivityAsync(ActivityLog activityLog);

        /// <summary>
        /// Insert an invalid login activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        /// <returns>ActivityLog id</returns>
        Task<int> LogInvalidLoginActivityAsync(ActivityLog activityLog);

        /// <summary>
        /// Insert an send message activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        /// <returns>ActivityLog id</returns>
        Task<int> LogSendMessageActivityAsync(ActivityLog activityLog);

        /// <summary>
        /// Insert an invalid send message activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        /// <returns>ActivityLog id</returns>
        Task<int> LogInvalidSendMessageActivityAsync(ActivityLog activityLog);

        /// <summary>
        /// Insert an register activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        /// <returns>ActivityLog id</returns>
        Task<int> LogRegisterActivityAsync(ActivityLog activityLog);

        /// <summary>
        /// Insert an invalid register activityLog
        /// </summary>
        /// <param name="activityLog">ActivityLog</param>
        /// <returns>ActivityLog id</returns>
        Task<int> LogInvalidRegisterActivityAsync(ActivityLog activityLog);

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