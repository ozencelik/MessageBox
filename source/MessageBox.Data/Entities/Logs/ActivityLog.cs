using MessageBox.Data.BaseEntities;
using MessageBox.Data.Enums;

namespace MessageBox.Data.Entities
{
    public partial class ActivityLog : BaseMySqlEntity
    {
        /// <summary>
        /// ActivityLogType
        /// </summary>
        public ActivityLogType ActivityLogType { get; set; }

        /// <summary>
        /// An activity log belongs to a user.
        /// UserId used as foreign key
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Activitylog message.
        /// </summary>
        public string Message { get; set; }
    }
}
