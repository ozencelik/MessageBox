using AutoMapper;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Logs;
using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessageBox.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LogController : Controller
    {
        #region Fields
        private readonly IActivityLogService _activityLogService;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public LogController(IActivityLogService activityLogService,
            ILogService logService, 
            IMapper mapper,
            IUserService userService)
        {
            this._activityLogService = activityLogService;
            this._logService = logService;
            this._mapper = mapper;
            this._userService = userService;
        }
        #endregion

        #region Methods
        [HttpGet(ApiRoutes.Logs.Get)]
        public async Task<IActionResult> GetById(int id)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            if (id <= 0)
                return BadRequest("Id value must be greater than zero.");

            var log = await _logService.GetLogByIdAsync(id, currentUser.Id);

            var model = _mapper.Map<LogModel>(log);

            return Ok(model);
        }

        [HttpGet(ApiRoutes.Logs.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            var logs = await _logService.GetAllLogsAsync(currentUser.Id);

            var model = _mapper.Map<IList<LogModel>>(logs);

            return Ok(model);
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
