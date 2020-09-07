using MessageBox.Data;
using MessageBox.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Users
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IRepository<User> _userRepository;
        #endregion

        #region Ctor
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Methods
        public async Task<int> DeleteUserAsync(User user)
        {
            return await _userRepository.DeleteAsync(user);
        }

        public async Task<IList<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.Table
                .Where(u => string.Equals(u.Email, email))?.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _userRepository.Table
                .Where(u => string.Equals(u.Username, userName))?.FirstOrDefaultAsync();
        }

        public async Task<int> InsertUserAsync(User user)
        {
            return await _userRepository.InsertAsync(user);
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            return await _userRepository.UpdateAsync(user);
        }
        #endregion
    }
}
