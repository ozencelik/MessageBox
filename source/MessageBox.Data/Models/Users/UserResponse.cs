using MessageBox.Data.BaseEntities;
using System;

namespace MessageBox.Data.Models
{
    public partial class UserResponse
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
    }
}
