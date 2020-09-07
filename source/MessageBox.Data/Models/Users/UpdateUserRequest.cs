using MessageBox.Data.BaseEntities;
using System;

namespace MessageBox.Data.Models
{
    public partial class UpdateUserRequest
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Username a unique value.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Unique email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Determine the entity is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Determine the entity is deleted
        /// </summary>
        public bool Deleted { get; set; }
    }
}
