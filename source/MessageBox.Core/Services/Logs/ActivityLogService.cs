using MessageBox.Data;
using MessageBox.Data.Entities;
using MessageBox.Data.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Logs
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

        public async Task<int> LogBlockedUserActivityAsync(ActivityLog activityLog)
        {
            if (activityLog is null
                || activityLog.UserId is default(int)
                || string.IsNullOrEmpty(activityLog.Message))
                return default;

            activityLog.ActivityLogType = ActivityLogType.BlockUser;

            return await InsertActivityLogAsync(activityLog);
        }

        public async Task<int> LogInvalidBlockedUserActivityAsync(ActivityLog activityLog)
        {
            if (activityLog is null
                || activityLog.UserId is default(int)
                || string.IsNullOrEmpty(activityLog.Message))
                return default;

            activityLog.ActivityLogType = ActivityLogType.InvalidBlockUser;

            return await InsertActivityLogAsync(activityLog);
        }

        public async Task<int> LogLoginActivityAsync(ActivityLog activityLog)
        {
            if (activityLog is null
                || activityLog.UserId is default(int)
                || string.IsNullOrEmpty(activityLog.Message))
                return default;

            activityLog.ActivityLogType = ActivityLogType.Login;

            return await InsertActivityLogAsync(activityLog);
        }

        public async Task<int> LogInvalidLoginActivityAsync(ActivityLog activityLog)
        {
            if (activityLog is null
                || string.IsNullOrEmpty(activityLog.Message))
                return default;

            activityLog.ActivityLogType = ActivityLogType.InvalidLogin;

            return await InsertActivityLogAsync(activityLog);
        }

        public async Task<int> LogSendMessageActivityAsync(ActivityLog activityLog)
        {
            if (activityLog is null
                || activityLog.UserId is default(int)
                || string.IsNullOrEmpty(activityLog.Message))
                return default;

            activityLog.ActivityLogType = ActivityLogType.SendMessage;

            return await InsertActivityLogAsync(activityLog);
        }

        public async Task<int> LogInvalidSendMessageActivityAsync(ActivityLog activityLog)
        {
            if (activityLog is null
                || activityLog.UserId is default(int)
                || string.IsNullOrEmpty(activityLog.Message))
                return default;

            activityLog.ActivityLogType = ActivityLogType.InvalidSendMessage;

            return await InsertActivityLogAsync(activityLog);
        }

        public async Task<int> LogRegisterActivityAsync(ActivityLog activityLog)
        {
            if (activityLog is null
                || activityLog.UserId is default(int)
                || string.IsNullOrEmpty(activityLog.Message))
                return default;

            activityLog.ActivityLogType = ActivityLogType.Register;

            return await InsertActivityLogAsync(activityLog);
        }
        
        public async Task<int> LogInvalidRegisterActivityAsync(ActivityLog activityLog)
        {
            if (activityLog is null
                || string.IsNullOrEmpty(activityLog.Message))
                return default;

            activityLog.ActivityLogType = ActivityLogType.InvalidRegister;

            return await InsertActivityLogAsync(activityLog);
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
