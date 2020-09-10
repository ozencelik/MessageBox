using System.ComponentModel.DataAnnotations;

namespace MessageBox.Data.Models
{
    public partial class RegisterModel
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Username a unique value.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Unique email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
