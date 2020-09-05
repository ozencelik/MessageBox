using MessageBox.Data;
using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Users
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IRepository<User> _userRepository;
        #endregion

        #region Ctor
        public MessageService(IRepository<User> userRepository)
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

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
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
