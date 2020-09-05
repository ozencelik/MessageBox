using MessageBox.Data;
using MessageBox.Data.Entities;
using System.Collections.Generic;
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
            return await _messageRepository.DeleteAsync(message);
        }

        public async Task<IList<Message>> GetAllMessagesAsync()
        {
            return await _messageRepository.GetAllAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int messageId)
        {
            return await _messageRepository.GetByIdAsync(messageId);
        }

        public async Task<int> InsertMessageAsync(Message message)
        {
            return await _messageRepository.InsertAsync(message);
        }

        public async Task<int> UpdateMessageAsync(Message message)
        {
            return await _messageRepository.UpdateAsync(message);
        }
        #endregion
    }
}
