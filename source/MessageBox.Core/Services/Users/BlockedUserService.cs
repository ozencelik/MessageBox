using MessageBox.Data;
using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Users
{
    public class BlockedUserService : IBlockedUserService
    {
        #region Fields
        private readonly IRepository<BlockedUser> _blockedUserRepository;
        #endregion

        #region Ctor
        public BlockedUserService(IRepository<BlockedUser> blockedUserRepository)
        {
            _blockedUserRepository = blockedUserRepository;
        }
        #endregion

        #region Methods
        public async Task<int> DeleteBlockedUserAsync(BlockedUser blockedUser)
        {
            return await _blockedUserRepository.DeleteAsync(blockedUser);
        }

        public async Task<IList<BlockedUser>> GetAllBlockedUsersAsync()
        {
            return await _blockedUserRepository.GetAllAsync();
        }

        public async Task<BlockedUser> GetBlockedUserByIdAsync(int blockedUserId)
        {
            return await _blockedUserRepository.GetByIdAsync(blockedUserId);
        }

        public async Task<int> InsertBlockedUserAsync(BlockedUser blockedUser)
        {
            return await _blockedUserRepository.InsertAsync(blockedUser);
        }

        public async Task<int> UpdateBlockedUserAsync(BlockedUser blockedUser)
        {
            return await _blockedUserRepository.UpdateAsync(blockedUser);
        }
        #endregion
    }
}
