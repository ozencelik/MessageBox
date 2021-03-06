﻿using MessageBox.Data;
using MessageBox.Data.Entities;
using MessageBox.Data.Models.Pagers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBox.Core.Services.Messages
{
    public class MessageService : IMessageService
    {
        #region Fields
        private readonly IRepository<Message> _messageRepository;
        #endregion

        #region Ctor
        public MessageService(IRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }
        #endregion

        #region Methods
        public async Task<int> DeleteMessageAsync(Message message)
        {
            message.Deleted = true;
            return await UpdateMessageAsync(message);
        }

        public async Task<IList<Message>> GetAllMessagesAsync()
        {
            return await _messageRepository.GetAllAsync();
        }

        public async Task<IList<Message>> GetAllMessagesByUserIdAsync(int userId)
        {
            return await _messageRepository.Table
                .Where(m => m.Active
                && !m.Deleted
                && (m.SenderUserId == userId 
                || m.ReceiverUserId == userId))?.ToListAsync();
        }

        public async Task<IList<Message>> GetAllMessagesBySenderUserIdAsync(int userId)
        {
            return await _messageRepository.Table
                .Where(m => m.Active
                && !m.Deleted
                && m.SenderUserId == userId)?.ToListAsync();
        }

        public async Task<IList<Message>> GetAllMessagesByReceiverUserIdAsync(int userId)
        {
            return await _messageRepository.Table
                .Where(m => m.Active
                && !m.Deleted
                && m.ReceiverUserId == userId)?.ToListAsync();
        }
        
        public async Task<IList<Message>> GetAllUnreadMessagesByReceiverUserIdAsync(int userId)
        {
            return await _messageRepository.Table
                .Where(m => m.Active
                && !m.Deleted
                && m.ReceiverUserId == userId
                && !m.Blocked
                && m.ReadOn == default)?.ToListAsync();
        }

        public async Task<IList<Message>> GetAllMessagesByUserIdWithPaginationAsync(int userId, PaginationFilter filter)
        {
            return await _messageRepository.TableWithPagination(filter)
                .Where(m => m.Active
                && !m.Deleted
                && (m.SenderUserId == userId
                || m.ReceiverUserId == userId))?.ToListAsync();
        }

        public async Task<IList<Message>> GetAllMessagesBySenderUserIdWithPaginationAsync(int userId, PaginationFilter filter)
        {
            return await _messageRepository.TableWithPagination(filter)
                .Where(m => m.Active
                && !m.Deleted
                && m.SenderUserId == userId)?.ToListAsync();
        }

        public async Task<IList<Message>> GetAllMessagesByReceiverUserIdWithPaginationAsync(int userId, PaginationFilter filter)
        {
            return await _messageRepository.TableWithPagination(filter)
                .Where(m => m.Active
                && !m.Deleted
                && m.ReceiverUserId == userId)?.ToListAsync();
        }

        public async Task<IList<Message>> GetAllUnreadMessagesByReceiverUserIdWithPaginationAsync(int userId, PaginationFilter filter)
        {
            return await _messageRepository.TableWithPagination(filter)
                .Where(m => m.Active
                && !m.Deleted
                && m.ReceiverUserId == userId
                && !m.Blocked
                && m.ReadOn == default)?.ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _messageRepository.GetByIdAsync(messageId);
        }
        
        public async Task<Message> GetMessageBySenderUserIdAsync(int messageId, int userId)
        {
            return await _messageRepository.Table
                .Where(m => m.Id == messageId && m.SenderUserId == userId)?.FirstOrDefaultAsync();
        }

        public async Task<int> InsertMessageAsync(Message message)
        {
            return await _messageRepository.InsertAsync(message);
        }

        public async Task<int> UpdateMessageAsync(Message message)
        {
            message.UpdatedOn = DateTime.Now;
            return await _messageRepository.UpdateAsync(message);
        }
        #endregion
    }
}
