namespace MessageBox.Data.Models
{
    public partial class SendMessageModel
    {
        /// <summary>
        /// Receiver user name
        /// </summary>
        public string ReceiverUserName { get; set; }

        /// <summary>
        /// Message content
        /// </summary>
        public string Content { get; set; }
    }
}
