using MessageBox.Data.BaseModels;

namespace MessageBox.Data.Models
{
    public partial class UserModel : BaseModel
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
