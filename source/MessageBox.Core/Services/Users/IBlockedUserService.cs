using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Users
{
    public interface IBlockedUserService
    {
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
        /// Gets a blockedUser
        /// </summary>
        /// <param name="blockedUserId">BlockedUser identifier</param>
        /// <returns>BlockedUser</returns>
        Task<BlockedUser> GetBlockedUserByIdAsync(int blockedUserId);

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