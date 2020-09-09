using MessageBox.Data.BaseEntities;

namespace MessageBox.Data.Entities
{
    public partial class BlockedUser : BaseEntity
    {
        /// <summary>
        /// BlockingUserId as foreign key.
        /// This user blocked another user.
        /// </summary>
        public int BlockingUserId { get; set; }

        /// <summary>
        /// BlockedUserId  as foreign key.
        /// This user blocked by another user.
        /// </summary>
        public int BlockedUserId { get; set; }
    }
}
