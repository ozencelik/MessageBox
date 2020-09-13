using MessageBox.Data;
using MessageBox.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
            user.Deleted = true;
            return await UpdateUserAsync(user);
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
                .Where(u => string.Equals(u.Username, userName)
                && u.Active
                && !u.Deleted)?.FirstOrDefaultAsync();
        }

        public async Task<string> GetUsernameByUserIdAsync(int userId)
        {
            var user = await _userRepository.Table
                .Where(u => u.Id == userId
                && u.Active
                && !u.Deleted)?.FirstOrDefaultAsync();

            if (user is null
                || user is default(User))
                return default;

            return user.Username;
        }

        public async Task<User> LoginUserWithEmailAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(password))
                return null;

            var user = await GetUserByEmailAsync(email);

            // check if email exists
            if (user is null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // login/authentication successful
            return user;
        }

        public async Task<User> LoginUserWithUsernameAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
                return null;

            var user = await GetUserByUsernameAsync(username);

            // check if username exists
            if (user is null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // login/authentication successful
            return user;
        }

        public async Task<int> InsertUserAsync(User user)
        {
            return await _userRepository.InsertAsync(user);
        }

        public async Task<User> RegisterUserAsync(User user, string password)
        {
            // validation
            if (user is null)
                throw new ArgumentNullException("User is required");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password is required");

            if (await GetUserByUsernameAsync(user.Username) != null)
                throw new ArgumentNullException("Username \"" + user.Username + "\" is already taken");

            if (await GetUserByEmailAsync(user.Email) != null)
                throw new ArgumentNullException("Email \"" + user.Email + "\" is already taken");

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await InsertUserAsync(user);

            return user;
        }

        public async Task<int> UpdateUserAsync(User userParams, string password = null)
        {
            var user = await GetUserByIdAsync(userParams.Id);

            if (user is null)
                throw new ArgumentNullException("User not found.");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(user.Username)
                && !string.Equals(user.Username, userParams.Username))
            {
                // throw error if the new username is already taken
                if (await GetUserByUsernameAsync(userParams.Username) is null)
                    user.Username = userParams.Username;
                else
                    throw new ArgumentNullException("Username " + userParams.Username + " is already taken");
            }

            // update email if it has changed
            if (!string.IsNullOrWhiteSpace(user.Email)
                && !string.Equals(user.Email, userParams.Email))
            {
                // throw error if the new email is already taken
                if (await GetUserByUsernameAsync(userParams.Email) is null)
                    user.Email = userParams.Email;
                else
                    throw new ArgumentNullException("Email " + userParams.Email + " is already taken");
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParams.Name))
                user.Name = userParams.Name;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            user.UpdatedOn = DateTime.Now;
            return await _userRepository.UpdateAsync(user);
        }
        #endregion

        #region Private Helper Methods
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");

            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }

            return true;
        }
        #endregion
    }
}
