using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
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
        private readonly IMapper _mapper;
        private readonly IBlockedUserService _blockedUserService;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public BlockedUserController(IMapper mapper,
            IBlockedUserService blockedUserService,
            IUserService userService)
        {
            this._mapper = mapper;
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

            var blockedUser = await _userService.GetUserByUsernameAsync(model.BlockedUserName);

            if (blockedUser is null)
                return NotFound("Blocked user not found.");

            var result = await _blockedUserService.BlockUser(blockingUser, blockedUser);

            if (result is default(int))
                return Ok("User not blocked.\n The user can be already blocked.");

            return Ok("User blocked ✔\nMessages coming from blocked user will not be shoed up to you.");
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
