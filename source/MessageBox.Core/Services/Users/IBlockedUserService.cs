using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Users
{
    public interface IBlockedUserService
    {
        /// <summary>
        /// Blocks the user for blocking user.
        /// </summary>
        /// <param name="blockingUser">Blocking User</param>
        /// <param name="blockedUser">Blocked User</param>
        /// <returns>BlockedUser Id</returns>
        Task<int> BlockUser(User blockingUser, User blockedUser);

        /// <summary>
        /// Gets whether blockedUser is blocked by
        /// blockinUser
        /// </summary>
        /// <param name="blockingUserId">Blocking User</param>
        /// <param name="blockedUserId">Blocked User</param>
        /// <returns>IsBlocked bool value</returns>
        Task<BlockedUser> CheckUserIsBlockedAsync(int blockingUserId, int blockedUserId);

        /// <summary>
        /// Delete blockedUser
        /// </summary>
        /// <param name="blockedUser">BlockedUser</param>
        Task<int> DeleteBlockedUserAsync(BlockedUser blockedUser);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Categories</returns>
        Task<IList<BlockedUser>> GetAllBlockedUsersAsync();

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="blockingUserId">Blocking User</param>
        /// <returns>Categories</returns>
        Task<IList<BlockedUser>> GetAllBlockedUsersAsync(int blockingUserId);

        /// <summary>
        /// Gets a blockedUser
        /// </summary>
        /// <param name="blockedUserId">BlockedUser identifier</param>
        /// <returns>BlockedUser</returns>
        Task<BlockedUser> GetBlockedUserByIdAsync(int blockedUserId);

        /// <summary>
        /// Get blockedUsers
        /// </summary>
        /// <param name="blockedUserId">BlockedUser identifier</param>
        /// <param name="blockingUserId">BlockedUser identifier</param>
        /// <returns>BlockedUser</returns>
        Task<BlockedUser> GetBlockedUserByBlockingUserIdAsync(int blockedUserId, int blockingUserId);

        /// <summary>
        /// Get blockedUsers
        /// </summary>
        /// <param name="userId">BlockedUser identifier</param>
        /// <returns>BlockedUser List</returns>
        Task<IList<BlockedUser>> GetBlockedUsersByBlockingUserIdAsync(int userId);

        /// <summary>
        /// Inserts blockedUser
        /// </summary>
        /// <param name="blockedUser">BlockedUser</param>
        Task<int> InsertBlockedUserAsync(BlockedUser blockedUser);

        /// <summary>
        /// Updates the blockedUser
        /// </summary>
        /// <param name="blockedUser">BlockedUser</param>
        Task<int> UpdateBlockedUserAsync(BlockedUser blockedUser);
    }
}