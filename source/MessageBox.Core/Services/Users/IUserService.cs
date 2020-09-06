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
        Task<int> DeleteUserAsync(NewUserRequest user);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Categories</returns>
        Task<IList<NewUserRequest>> GetAllUsersAsync();

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>User</returns>
        Task<NewUserRequest> GetUserByIdAsync(int userId);

        /// <summary>
        /// Inserts user
        /// </summary>
        /// <param name="user">User</param>
        Task<int> InsertUserAsync(NewUserRequest user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task<int> UpdateUserAsync(NewUserRequest user);
    }
}