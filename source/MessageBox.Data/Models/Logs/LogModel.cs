using MessageBox.Data.Enums;

namespace MessageBox.Data.Models
{
    public partial class LogModel
    {
        /// <summary>
        /// LogType
        /// </summary>
        public LogType LogType { get; set; }

        /// <summary>
        /// A log can belong to a user.
        /// UserId used as foreign key
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Log title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Log message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// An error can be a bug in system.
        /// So it should be fixed.
        /// </summary>
        public bool Fixed { get; set; }
    }
}
