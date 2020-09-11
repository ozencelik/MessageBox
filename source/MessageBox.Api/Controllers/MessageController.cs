using AutoMapper;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Messages;
using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessageBox.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class MessageController : Controller
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IBlockedUserService _blockedUserService;
        #endregion

        #region Ctor
        public MessageController(IMapper mapper,
            IMessageService messageService,
            IUserService userService,
            IBlockedUserService blockedUserService)
        {
            this._mapper = mapper;
            this._messageService = messageService;
            this._userService = userService;
            this._blockedUserService = blockedUserService;
        }
        #endregion

        #region Methods
        [HttpGet(ApiRoutes.Messages.Get)]
        public async Task<IActionResult> GetById(int messageId)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            if (messageId <= 0)
                return BadRequest("Id value must be greater than zero.");

            var message = await _messageService.GetMessageByIdAsync(messageId);

            if (message is null
                || message is default(Message))
                return NotFound(nameof(message));

            if (message.SenderUserId == currentUser.Id)
                return Ok(new MessageModel()
                {
                    SenderUserName = currentUser.Username,
                    ReceiverUserName = await _userService.GetUsernameByUserIdAsync(message.ReceiverUserId),
                    Content = message.Content,
                    DeliveredOn = message.DeliveredOn,
                    ReadOn = message.ReadOn
                });
            else if (message.ReceiverUserId == currentUser.Id)
            {
                if (message.Blocked)
                    return Ok("Blocked message cannot be read by receiver user.");

                SetDeliveredAndReadTimeForMessageAsync(currentUser.Id, message);
                return Ok(new MessageModel()
                {
                    SenderUserName = await _userService.GetUsernameByUserIdAsync(message.SenderUserId),
                    ReceiverUserName = currentUser.Username,
                    Content = message.Content,
                    DeliveredOn = message.DeliveredOn,
                    ReadOn = message.ReadOn,
                });
            }

            return Unauthorized("You cannot read someone's messages !!!");
        }

        [HttpGet(ApiRoutes.Messages.GetAll)]
        [SwaggerOperation(
            Summary = "Get All Messages",
            Description = "Gets all messages that were sent or received by the current user",
            OperationId = "Message.GetAll",
            Tags = new[] { "MessageEndpoints" })]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            // Authorized users can only get their messages.
            // They cannot acces someone's messages.
            var messages = await _messageService.GetAllMessagesByUserIdAsync(currentUser.Id);

            return Ok(await PrepareMessageModelAsync(messages));
        }

        [HttpPost(ApiRoutes.Messages.Send)]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            if (model is null
                || model is default(SendMessageModel))
                return BadRequest(nameof(model));

            if (string.IsNullOrEmpty(model.ReceiverUserName))
                return BadRequest("ReceiverUserName cannot be null.");

            if (string.IsNullOrWhiteSpace(model.Content))
                return BadRequest("You cannot send and empty message to another user.\nPlease add some message.");

            // Check receiver user is exist.
            // Check receiver user is blocked the sender user.
            // In this situation we save the message coming from
            // blocked user but not showed up to the receiver user.
            var receiverUser = await _userService.GetUserByUsernameAsync(model.ReceiverUserName);

            if (receiverUser is null
                || receiverUser is default(User))
                return NotFound("Receiver User not found. Please check the username or send a message to another user.");

            var message = new Message()
            {
                SenderUserId = currentUser.Id,
                ReceiverUserId = receiverUser.Id,
                Content = model.Content
            };

            await _messageService.InsertMessageAsync(message);

            // Check the receiver user is blocked the current user.
            var blockedUser = await _blockedUserService.CheckUserIsBlockedAsync(receiverUser.Id, currentUser.Id);
            if (blockedUser is null
                || blockedUser is default(BlockedUser))
                return Ok("Message sent ✔");

            // If blocked, we show a user friendly message to current user.
            return Ok("Message received  ✔\nThis message will not be showed to the receiver. Because you are blocked by the receiver user.");
        }
        #endregion

        #region Private Helper Methods
        private int GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userId))
                return default;

            Int32.TryParse(userId, out int result);
            return result;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId is 0)
                return default;

            return await _userService.GetUserByIdAsync(currentUserId);
        }

        private async void SetDeliveredAndReadTimeForMessageAsync(int currentUserId, Message message)
        {
            if (currentUserId < 0
                || message is null
                || message is default(Message)
                || currentUserId != message.ReceiverUserId)
                return;

            // Update delivered time and read time
            // Delivered time set once but read time set for the message call from API.
            if (message.DeliveredOn == default)
                message.DeliveredOn = DateTime.Now;
            message.ReadOn = DateTime.Now;
            await _messageService.UpdateMessageAsync(message);
        }

        private async Task<IList<MessageModel>> PrepareMessageModelAsync(IList<Message> messages)
        {
            if (messages is null
                || messages.Count() == 0)
                return default;

            var currentUserId = GetCurrentUserId();

            var result = new List<MessageModel>();

            foreach (var message in messages)
            {
                SetDeliveredAndReadTimeForMessageAsync(currentUserId, message);
                result.Add(new MessageModel()
                {
                    SenderUserName = await _userService.GetUsernameByUserIdAsync(message.SenderUserId),
                    ReceiverUserName = await _userService.GetUsernameByUserIdAsync(message.ReceiverUserId),
                    Content = message.Content,
                    DeliveredOn = message.DeliveredOn,
                    ReadOn = message.ReadOn
                });
            }

            return result;
        }
        #endregion
    }
}
