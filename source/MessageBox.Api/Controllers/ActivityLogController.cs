using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Logs;
using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessageBox.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ActivityLogController : Controller
    {
        #region Fields
        private readonly IActivityLogService _activityLogService;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public ActivityLogController(IActivityLogService activityLogService,
            IUserService userService)
        {
            this._activityLogService = activityLogService;
            this._userService = userService;
        }
        #endregion

        #region Methods
        [HttpGet(ApiRoutes.ActivityLogs.Get)]
        public async Task<IActionResult> GetById(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            if (id <= 0)
                return BadRequest("Id value must be greater than zero.");

            var log = await _activityLogService.GetActivityLogByIdAsync(id, currentUser.Id);

            if (log is null
                || log is default(ActivityLog))
                return NotFound("ActivityLog not found.");

            return Ok(log);
        }

        [HttpGet(ApiRoutes.ActivityLogs.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            var logs = await _activityLogService.GetAllActivityLogsAsync(currentUser.Id);

            if (logs is null
                || logs is default(IList<ActivityLog>))
                return NotFound("No activity logs found.");

            return Ok(logs);
        }
        #endregion

        #region Private Helper Methods
        private int GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userId))
                return default;

            Int32.TryParse(userId, out int result);
            return result;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId is 0)
                return default;

            return await _userService.GetUserByIdAsync(currentUserId);
        }
        #endregion
    }
}
