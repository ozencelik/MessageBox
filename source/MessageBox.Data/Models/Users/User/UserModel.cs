using System;

namespace MessageBox.Data.Models
{
    public partial class UserModel
    {
        /// <summary>
        /// User name
        /// </summary>
        public int Id { get; set; }

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
        /// Determine the entity is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Determine the entity is deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Created date of an entity
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Updated date of an entity in UTC time format
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
