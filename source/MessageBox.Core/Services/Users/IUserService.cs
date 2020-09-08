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
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>User</returns>
        Task<User> GetUserByIdAsync(int userId);

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>User</returns>
        Task<User> GetUserByUsernameAsync(string userName);

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>User</returns>
        Task<User> LoginUserWithEmailAsync(string email, string password);

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>User</returns>
        Task<User> LoginUserWithUsernameAsync(string username, string password);

        /// <summary>
        /// Inserts user
        /// </summary>
        /// <param name="user">User</param>
        Task<int> InsertUserAsync(User user);

        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="password">Password</param>
        /// <returns>User</returns>
        Task<User> RegisterUserAsync(User user, string password);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="password">Password</param>
        /// <returns>User Id</returns>
        Task<int> UpdateUserAsync(User user, string password = null);
    }
}