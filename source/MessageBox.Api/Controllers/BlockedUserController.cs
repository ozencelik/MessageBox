using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Logs;
using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;
using MessageBox.Data.Models.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageBox.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class BlockedUserController : Controller
    {
        #region Fields
        private readonly IActivityLogService _activityLogService;
        private readonly ILogService _logService;
        private readonly IBlockedUserService _blockedUserService;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public BlockedUserController(IActivityLogService activityLogService,
            ILogService logService,
            IBlockedUserService blockedUserService,
            IUserService userService)
        {
            this._activityLogService = activityLogService;
            this._logService = logService;
            this._blockedUserService = blockedUserService;
            this._userService = userService;
        }
        #endregion

        #region Methods
        [HttpPost(ApiRoutes.BlockedUsers.Block)]
        public async Task<IActionResult> Block([FromBody] BlockedUserModel model)
        {
            // Get authorized user.
            var blockingUser = await GetCurrentUserAsync();

            if (blockingUser is null
                || blockingUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            if (model is null)
                return BadRequest("Model cannot be null.");

            if (string.IsNullOrEmpty(model.BlockedUserName))
                return BadRequest("BlockedUserName cannot be null.");

            if (string.Equals(model.BlockedUserName, blockingUser.Username))
                return BadRequest("You cannot block yourself.");

            var blockedUser = await _userService.GetUserByUsernameAsync(model.BlockedUserName);

            if (blockedUser is null)
                return NotFound("Blocked user not found.");

            // Insert a new blockedUser.
            try
            {
                var result = await _blockedUserService.BlockUser(blockingUser, blockedUser);

                if (result is default(int))
                {
                    await _activityLogService.LogInvalidBlockedUserActivityAsync(new ActivityLog()
                    {
                        UserId = blockingUser.Id,
                        Message = string.Format("{0} user not blocked by {1} user"
                        , blockedUser.Id, blockingUser.Id)
                    });
                    return Ok("User not blocked.");
                }
            }
            catch (Exception ex)
            {
                await _logService.LogErrorAsync(new CreateLogModel()
                {
                    UserId = blockingUser.Id,
                    Title = "BlockedUser Error",
                    Message = "Error happened in BlockedUser Controller, Block function",
                    Exception = ex
                });
                return Ok("User not blocked.");
            }

            await _activityLogService.LogBlockedUserActivityAsync(new ActivityLog()
            {
                UserId = blockingUser.Id,
                Message = string.Format("{0} user blocked by {1} user ✔"
                , blockedUser.Id, blockingUser.Id)
            });
            return Ok("User blocked ✔\nMessages coming from blocked user will not be showed up to you.");
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
