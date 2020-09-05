using MessageBox.Data;
using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActivityLogBox.Core.Services.Logs
{
    public class ActivityLogService : IActivityLogService
    {
        #region Fields
        private readonly IRepository<ActivityLog> _activityLogRepository;
        #endregion

        #region Ctor
        public ActivityLogService(IRepository<ActivityLog> activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }
        #endregion

        #region Methods
        public async Task<int> DeleteActivityLogAsync(ActivityLog activityLog)
        {
            return await _activityLogRepository.DeleteAsync(activityLog);
        }

        public async Task<IList<ActivityLog>> GetAllActivityLogsAsync()
        {
            return await _activityLogRepository.GetAllAsync();
        }

        public async Task<ActivityLog> GetActivityLogByIdAsync(int activityLogId)
        {
            return await _activityLogRepository.GetByIdAsync(activityLogId);
        }

        public async Task<int> InsertActivityLogAsync(ActivityLog activityLog)
        {
            return await _activityLogRepository.InsertAsync(activityLog);
        }

        public async Task<int> UpdateActivityLogAsync(ActivityLog activityLog)
        {
            return await _activityLogRepository.UpdateAsync(activityLog);
        }
        #endregion
    }
}
