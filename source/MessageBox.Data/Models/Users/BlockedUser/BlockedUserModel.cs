namespace MessageBox.Data.Models.Users
{
    public partial class BlockedUserModel
    {
        /// <summary>
        /// BlockedUser identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// BlockingUserName.
        /// Authorized user.
        /// </summary>
        public string BlockingUserName { get; set; }

        /// <summary>
        /// BlockedUserName.
        /// This user blocked by another user.
        /// </summary>
        public string BlockedUserName { get; set; }
    }
}
