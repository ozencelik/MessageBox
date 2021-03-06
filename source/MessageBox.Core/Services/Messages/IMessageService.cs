﻿using MessageBox.Data.Entities;
using MessageBox.Data.Models.Pagers;
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
        /// Gets all messages
        /// </summary>
        /// <returns>Messages</returns>
        Task<IList<Message>> GetAllMessagesAsync();

        /// <summary>
        /// Gets all messages by sender and receiver user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Messages</returns>
        Task<IList<Message>> GetAllMessagesByUserIdAsync(int userId);

        /// <summary>
        /// Gets all messages by sender user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Messages</returns>
        Task<IList<Message>> GetAllMessagesBySenderUserIdAsync(int userId);

        /// <summary>
        /// Gets all messages by receiver user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Messages</returns>
        Task<IList<Message>> GetAllMessagesByReceiverUserIdAsync(int userId);

        /// <summary>
        /// Gets all messages by receiver user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Messages</returns>
        Task<IList<Message>> GetAllUnreadMessagesByReceiverUserIdAsync(int userId);

        /// <summary>
        /// Gets all messages by sender and receiver user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Messages</returns>
        Task<IList<Message>> GetAllMessagesByUserIdWithPaginationAsync(int userId, PaginationFilter filter);

        /// <summary>
        /// Gets all messages by sender user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Messages</returns>
        Task<IList<Message>> GetAllMessagesBySenderUserIdWithPaginationAsync(int userId, PaginationFilter filter);

        /// <summary>
        /// Gets all messages by receiver user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Messages</returns>
        Task<IList<Message>> GetAllMessagesByReceiverUserIdWithPaginationAsync(int userId, PaginationFilter filter);

        /// <summary>
        /// Gets all messages by receiver user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Messages</returns>
        Task<IList<Message>> GetAllUnreadMessagesByReceiverUserIdWithPaginationAsync(int userId, PaginationFilter filter);

        /// <summary>
        /// Gets a message
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        /// <returns>Message</returns>
        Task<Message> GetMessageByIdAsync(int messageId);

        /// <summary>
        /// Gets a message
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Message</returns>
        Task<Message> GetMessageBySenderUserIdAsync(int messageId, int userId);

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