using MessageBox.Data;
using MessageBox.Data.Entities;
using MessageBox.Data.Enums;
using MessageBox.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            log.Deleted = true;
            return await UpdateLogAsync(log);
        }

        public async Task<IList<Log>> GetAllLogsAsync()
        {
            return await _logRepository.GetAllAsync();
        }
        
        public async Task<IList<Log>> GetAllLogsAsync(int userId)
        {
            return await _logRepository.Table
                .Where(u => u.UserId == userId)?.ToListAsync();
        }

        public async Task<Log> GetLogByIdAsync(int logId)
        {
            return await _logRepository.GetByIdAsync(logId);
        }
        
        public async Task<Log> GetLogByIdAsync(int logId, int userId)
        {
            return await _logRepository.Table
                .Where(u => u.Id == logId && u.UserId == userId)?.FirstOrDefaultAsync();
        }

        public async Task<int> LogDebugAsync(CreateLogModel model)
        {
            if (model is null
                || (string.IsNullOrEmpty(model.Title)
                && string.IsNullOrEmpty(model.Message)
                && model.Exception is null))
                return default;

            model.LogType = LogType.Debug;

            var log = new Log()
            {
                LogType = model.LogType,
                UserId = model.UserId,
                Title = model.Title,
                Message = model.Exception is null ? model.Message
                : string.Format("{0}\n\nException Message :\n{1}",
                model.Message, model.Exception.GetBaseException())
            };

            return await InsertLogAsync(log);
        }

        public async Task<int> LogErrorAsync(CreateLogModel model)
        {
            if (model is null
                || (string.IsNullOrEmpty(model.Title)
                && string.IsNullOrEmpty(model.Message)
                && model.Exception is null))
                return default;

            model.LogType = LogType.Error;

            var log = new Log()
            {
                LogType = model.LogType,
                UserId = model.UserId,
                Title = model.Title,
                Message = model.Exception is null ? model.Message
                : string.Format("{0}\n\nException Message :\n{1}",
                model.Message, model.Exception.GetBaseException())
            };

            return await InsertLogAsync(log);
        }

        public async Task<int> LogFatalErrorAsync(CreateLogModel model)
        {
            if (model is null
                || (string.IsNullOrEmpty(model.Title)
                && string.IsNullOrEmpty(model.Message)
                && model.Exception is null))
                return default;

            model.LogType = LogType.FatalError;

            var log = new Log()
            {
                LogType = model.LogType,
                UserId = model.UserId,
                Title = model.Title,
                Message = model.Exception is null ? model.Message
                : string.Format("{0}\n\nException Message :\n{1}",
                model.Message, model.Exception.GetBaseException())
            };

            return await InsertLogAsync(log);
        }

        public async Task<int> LogInformationAsync(CreateLogModel model)
        {
            if (model is null
                || (string.IsNullOrEmpty(model.Title)
                && string.IsNullOrEmpty(model.Message)
                && model.Exception is null))
                return default;

            model.LogType = LogType.Information;

            var log = new Log()
            {
                LogType = model.LogType,
                UserId = model.UserId,
                Title = model.Title,
                Message = model.Exception is null ? model.Message
                : string.Format("{0}\n\nException Message :\n{1}",
                model.Message, model.Exception.GetBaseException())
            };

            return await InsertLogAsync(log);
        }

        public async Task<int> LogWarningAsync(CreateLogModel model)
        {
            if (model is null
                || (string.IsNullOrEmpty(model.Title)
                && string.IsNullOrEmpty(model.Message)
                && model.Exception is null))
                return default;

            model.LogType = LogType.Warning;

            var log = new Log()
            {
                LogType = model.LogType,
                UserId = model.UserId,
                Title = model.Title,
                Message = model.Exception is null ? model.Message
                : string.Format("{0}\n\nException Message :\n{1}",
                model.Message, model.Exception.GetBaseException())
            };

            return await InsertLogAsync(log);
        }

        public async Task<int> InsertLogAsync(Log log)
        {
            return await _logRepository.InsertAsync(log);
        }

        public async Task<int> UpdateLogAsync(Log log)
        {
            log.UpdatedOn = DateTime.Now;
            return await _logRepository.UpdateAsync(log);
        }
        #endregion
    }
}
