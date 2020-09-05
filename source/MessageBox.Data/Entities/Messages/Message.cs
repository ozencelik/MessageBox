using MessageBox.Data.BaseEntities;
using System;

namespace MessageBox.Data.Entities
{
    public partial class Message : BaseEntity
    {
        /// <summary>
        /// A message
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
