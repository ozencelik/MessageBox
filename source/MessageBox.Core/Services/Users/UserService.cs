using MessageBox.Data;
using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Users
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IRepository<NewUserRequest> _userRepository;
        #endregion

        #region Ctor
        public UserService(IRepository<NewUserRequest> userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Methods
        public async Task<int> DeleteUserAsync(NewUserRequest user)
        {
            return await _userRepository.DeleteAsync(user);
        }

        public async Task<IList<NewUserRequest>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<NewUserRequest> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<int> InsertUserAsync(NewUserRequest user)
        {
            return await _userRepository.InsertAsync(user);
        }

        public async Task<int> UpdateUserAsync(NewUserRequest user)
        {
            return await _userRepository.UpdateAsync(user);
        }
        #endregion
    }
}
