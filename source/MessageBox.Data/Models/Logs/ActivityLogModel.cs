using MessageBox.Data.Enums;

namespace MessageBox.Data.Models
{
    public partial class ActivityLogModel
    {
        /// <summary>
        /// ActivityLogType
        /// </summary>
        public ActivityLogType ActivityLogType { get; set; }

        /// <summary>
        /// A log can belong to a user.
        /// UserId used as foreign key
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Activitylog message.
        /// </summary>
        public string Message { get; set; }
    }
}
