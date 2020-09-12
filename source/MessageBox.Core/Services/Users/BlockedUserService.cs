using Autofac.Builder;
using MessageBox.Data;
using MessageBox.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Users
{
    public class BlockedUserService : IBlockedUserService
    {
        #region Fields
        private readonly IRepository<BlockedUser> _blockedUserRepository;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public BlockedUserService(IRepository<BlockedUser> blockedUserRepository,
            IUserService userService)
        {
            _blockedUserRepository = blockedUserRepository;
            _userService = userService;
        }
        #endregion

        #region Methods
        public async Task<int> BlockUser(User blockingUserModel, User blockedUserModel)
        {
            if (blockingUserModel is null
                || blockingUserModel is default(User)
                || blockedUserModel is null
                || blockedUserModel is default(User))
                return default;

            // Check user is already blocked
            // In this situtaion, we are not adding a duplicate value.
            var blockedUser = await CheckUserIsBlockedAsync(blockingUserModel.Id, blockedUserModel.Id);
            if (blockedUser is null
                || blockedUser is default(BlockedUser))
            {
                return await InsertBlockedUserAsync(new BlockedUser()
                {
                    BlockingUserId = blockingUserModel.Id,
                    BlockedUserId = blockedUserModel.Id
                });
            }
            // If user is already blocked
            // Check the blocking opeation is active. 
            // If not active it.
            else if(!blockedUser.Active)
            {
                blockedUser.Active = true;
                return await UpdateBlockedUserAsync(blockedUser);
            }

            return blockedUser.Id;
        }
        
        public async Task<BlockedUser> CheckUserIsBlockedAsync(int blockingUserId, int blockedUserId)
        {
            var blockedUsers = await GetBlockedUsersByBlockingUserIdAsync(blockingUserId);

            // No users blocked.
            if (blockedUsers is null
                || blockedUsers is default(IList<BlockedUser>))
                return default;

            return blockedUsers.Where(bu => bu.BlockedUserId == blockedUserId)?
                .FirstOrDefault();
        }

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

        public async Task<IList<BlockedUser>> GetBlockedUsersByBlockingUserIdAsync(int userId)
        {
            if (userId < 0)
                return default;

            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null
                || user is default(User))
                return default;

            return await _blockedUserRepository.Table
                .Where(b => b.BlockingUserId == userId
                && !b.Deleted)?.ToListAsync();
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
