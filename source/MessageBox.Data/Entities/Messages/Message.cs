using MessageBox.Data.BaseEntities;
using System;

namespace MessageBox.Data.Entities
{
    public partial class Message : BaseEntity
    {
        /// <summary>
        /// SenderUserId is foreign key of the user
        /// that sending the message to another user.
        /// </summary>
        public int SenderUserId { get; set; }

        /// <summary>
        /// ReceiverUserId is foreign key of the user
        /// that receiving a message from another user.
        /// </summary>
        public int ReceiverUserId { get; set; }

        /// <summary>
        /// A message
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Define a message is blocked or not
        /// </summary>
        public bool Blocked { get; set; }

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
