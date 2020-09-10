using System.ComponentModel.DataAnnotations;

namespace MessageBox.Data.Models
{
    public partial class LoginModel
    {
        /// <summary>
        /// Username a unique value.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
