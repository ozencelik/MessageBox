using MessageBox.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Messages
{
    public interface IMessageService
    {
        /// <summary>
        /// Delete message
        /// </summary>
        /// <param name="message">Message</param>
        Task<int> DeleteMessageAsync(Message message);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Categories</returns>
        Task<IList<Message>> GetAllMessagesAsync();

        /// <summary>
        /// Gets a message
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        /// <returns>Message</returns>
        Task<Message> GetMessageByIdAsync(int messageId);

        /// <summary>
        /// Inserts message
        /// </summary>
        /// <param name="message">Message</param>
        Task<int> InsertMessageAsync(Message message);

        /// <summary>
        /// Updates the message
        /// </summary>
        /// <param name="message">Message</param>
        Task<int> UpdateMessageAsync(Message message);
    }
}