using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> Block([FromBody] CreateBlockedUserModel model)
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
        
        [HttpDelete(ApiRoutes.BlockedUsers.UnBlock)]
        public async Task<IActionResult> UnBlock([FromBody] CreateBlockedUserModel model)
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
                return BadRequest("You cannot un block yourself.");

            var blockedUser = await _userService.GetUserByUsernameAsync(model.BlockedUserName);

            if (blockedUser is null)
                return NotFound("User not found.");

            // Remove blockedUser.
            try
            {
                var result = await _blockedUserService.GetBlockedUserByBlockingUserIdAsync(blockedUser.Id, blockingUser.Id);

                if (result is null
                    || result is default(BlockedUser))
                    return NotFound("Blocked User not found.");

                await _blockedUserService.DeleteBlockedUserAsync(result);

                return Ok("BlockedUser removed ✔");
            }
            catch (Exception ex)
            {
                await _logService.LogErrorAsync(new CreateLogModel()
                {
                    UserId = blockingUser.Id,
                    Title = "BlockedUser Error",
                    Message = "Error happened in BlockedUser Controller, UnBlock function",
                    Exception = ex
                });
                return Ok("BlockedUser not removed.");
            }
        }

        [HttpGet(ApiRoutes.BlockedUsers.Get)]
        public async Task<IActionResult> Get([FromBody] BlockedUserModel model)
        {
            var blockingUser = await GetCurrentUserAsync();

            if (blockingUser is null
                || blockingUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            if (model is null)
                return BadRequest("Model cannot be null.");

            if (string.IsNullOrEmpty(model.BlockedUserName))
                return BadRequest("BlockedUserName cannot be null.");

            if (string.Equals(model.BlockedUserName, blockingUser.Username))
                return BadRequest("Your username cannot be same with BlockedUsername.");

            var blockedUser = await _userService.GetUserByUsernameAsync(model.BlockedUserName);

            if (blockedUser is null)
                return NotFound("Blocked user not found.");

            // Get blockedUser.
            BlockedUser result;
            try
            {
                result = await _blockedUserService.GetBlockedUserByBlockingUserIdAsync(blockedUser.Id, blockingUser.Id);

                if (result is null
                    || result is default(BlockedUser))
                    return NotFound("No blockedUser found.");
            }
            catch (Exception ex)
            {
                await _logService.LogErrorAsync(new CreateLogModel()
                {
                    UserId = blockingUser.Id,
                    Title = "Get Error",
                    Message = "Error happened in BlockedUser Controller, Get function",
                    Exception = ex
                });
                return NotFound("No blockedUser found.");
            }

            return Ok(result);
        }
        
        [HttpGet(ApiRoutes.BlockedUsers.GetAllBlockedUser)]
        public async Task<IActionResult> GetAll()
        {
            var blockingUser = await GetCurrentUserAsync();

            if (blockingUser is null
                || blockingUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            // Get blockedUsers.
            try
            {
                var blockedUsers = await _blockedUserService.GetAllBlockedUsersAsync(blockingUser.Id);

                if (blockedUsers is null
                    || blockedUsers is default(IList<BlockedUser>))
                    return NotFound("No blockedUser found.");

                return Ok(await PrepareBlockedUserModelAsync(blockedUsers));
            }
            catch (Exception ex)
            {
                await _logService.LogErrorAsync(new CreateLogModel()
                {
                    UserId = blockingUser.Id,
                    Title = "GetAll Error",
                    Message = "Error happened in BlockedUser Controller, GetAll function",
                    Exception = ex
                });
                return NotFound("No blockedUser found.");
            }
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

        private async Task<IList<BlockedUserModel>> PrepareBlockedUserModelAsync(IList<BlockedUser> blockedUsers)
        {
            if (blockedUsers is null
                || blockedUsers.Count() == 0)
                return default;

            var currentUser = await GetCurrentUserAsync();

            var result = new List<BlockedUserModel>();

            foreach (var blockedUser in blockedUsers)
            {
                result.Add(new BlockedUserModel()
                {
                    Id = blockedUser.Id,
                    BlockingUserName = currentUser.Username,
                    BlockedUserName = await _userService.GetUsernameByUserIdAsync(blockedUser.BlockedUserId)
                });
            }

            return result;
        }
        #endregion
    }
}
