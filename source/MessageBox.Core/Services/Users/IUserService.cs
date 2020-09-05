using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="user">User</param>
        Task<int> DeleteUserAsync(User user);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Categories</returns>
        Task<IList<User>> GetAllUsersAsync();

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>User</returns>
        Task<User> GetUserByIdAsync(int userId);

        /// <summary>
        /// Inserts user
        /// </summary>
        /// <param name="user">User</param>
        Task<int> InsertUserAsync(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task<int> UpdateUserAsync(User user);
    }
}