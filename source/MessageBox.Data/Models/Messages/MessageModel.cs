using System;

namespace MessageBox.Data.Models
{
    public partial class MessageModel
    {
        /// <summary>
        /// Message identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sender user name
        /// </summary>
        public string SenderUserName { get; set; }

        /// <summary>
        /// Receiver user name
        /// </summary>
        public string ReceiverUserName { get; set; }

        /// <summary>
        /// Message content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Delivery date of a message
        /// </summary>
        public DateTime DeliveredOn { get; set; }

        /// <summary>
        /// Read date of a message
        /// </summary>
        public DateTime ReadOn { get; set; }
    }
}
