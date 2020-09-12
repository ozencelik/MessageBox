using MessageBox.Data.Enums;
using System;

namespace MessageBox.Data.Models
{
    public partial class CreateLogModel
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
        /// Log exception.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
