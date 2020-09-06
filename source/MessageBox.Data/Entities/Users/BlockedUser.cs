using MessageBox.Data.BaseEntities;

namespace MessageBox.Data.Entities
{
    public partial class BlockedUser : BaseMySqlEntity
    {
        /// <summary>
        /// BlockingUserId as foreign key.
        /// </summary>
        public int BlockingUserId { get; set; }

        /// <summary>
        /// BlockedUserId  as foreign key.
        /// </summary>
        public int BlockedUserId { get; set; }
    }
}
