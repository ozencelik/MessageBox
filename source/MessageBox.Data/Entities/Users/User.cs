using MessageBox.Data.BaseEntities;

namespace MessageBox.Data.Entities
{
    public partial class User : BaseEntity
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
        /// PasswordHash
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// PasswordSalt
        /// </summary>
        public byte[] PasswordSalt { get; set; }
    }
}
