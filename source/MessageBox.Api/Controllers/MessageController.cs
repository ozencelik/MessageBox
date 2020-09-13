using AutoMapper;
using MessageBox.Api.Configuration;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Logs;
using MessageBox.Core.Services.Messages;
using MessageBox.Core.Services.Uris;
using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;
using MessageBox.Data.Models.Pagers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessageBox.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class MessageController : Controller
    {
        #region Fields
        private readonly IActivityLogService _activityLogService;
        private readonly ILogService _logService;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IBlockedUserService _blockedUserService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        #endregion

        #region Ctor
        public MessageController(IActivityLogService activityLogService,
            ILogService logService, 
            IMessageService messageService,
            IUserService userService,
            IBlockedUserService blockedUserService,
            IMapper mapper,
            IUriService uriService)
        {
            this._activityLogService = activityLogService;
            this._logService = logService;
            this._messageService = messageService;
            this._userService = userService;
            this._blockedUserService = blockedUserService;
            this._mapper = mapper;
            this._uriService = uriService;
        }
        #endregion

        #region Methods
        [HttpDelete(ApiRoutes.Messages.Delete)]
        public async Task<IActionResult> Delete(int messageId)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            var message = await _messageService.GetMessageBySenderUserIdAsync(messageId, currentUser.Id);

            if (message is null
                || message is default(Message))
                return NotFound("No message found by sent from you.");

            await _messageService.DeleteMessageAsync(message);

            return Ok("Message deleted ✔");
        }

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
                    Id = message.Id,
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
                    Id = message.Id,
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
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            // Authorized users can only get their messages.
            // They cannot acces someone's messages.
            var pagedData = await _messageService.GetAllMessagesByUserIdWithPaginationAsync(currentUser.Id, validFilter);

            if (pagedData is null
                || !pagedData.Any())
                return Ok("No message found ✔");

            var totalRecords = await _messageService.GetAllMessagesByUserIdAsync(currentUser.Id);
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords.Count(), _uriService, route);

            return Ok(pagedReponse);
        }

        [HttpGet(ApiRoutes.Messages.GetAllUnRead)]
        public async Task<IActionResult> GetAllUnReadMessage([FromQuery] PaginationFilter filter)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            // Authorized users can only get their messages.
            // They cannot acces someone's messages.
            var pagedData = await _messageService.GetAllUnreadMessagesByReceiverUserIdWithPaginationAsync(currentUser.Id, validFilter);

            if (pagedData is null
                || !pagedData.Any()) 
                return Ok("No unread message found ✔");

            var totalRecords = await _messageService.GetAllMessagesByUserIdAsync(currentUser.Id);
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords.Count(), _uriService, route);

            return Ok(pagedReponse);
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

            if (string.Equals(model.ReceiverUserName, currentUser.Username))
                return BadRequest("You cannot send a message to yourself.\nPlease change the username to send a message to another user.");

            // Check receiver user is exist.
            // Check receiver user is blocked the sender user.
            // In this situation we save the message coming from
            // blocked user but not showed up to the receiver user.
            var receiverUser = await _userService.GetUserByUsernameAsync(model.ReceiverUserName);

            if (receiverUser is null
                || receiverUser is default(User))
                return NotFound("Receiver User not found. Please check the username or send a message to another user.");

            try
            {
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
                {
                    await _activityLogService.LogSendMessageActivityAsync(new ActivityLog()
                    {
                        UserId = currentUser.Id,
                        Message = string.Format("{0} user send message to {1} user"
                           , currentUser.Id, receiverUser.Id)
                    });
                    return Ok("Message sent ✔");
                }

                message.Blocked = true;
                await _messageService.UpdateMessageAsync(message);
            }
            catch (Exception ex)
            {
                await _logService.LogErrorAsync(new CreateLogModel()
                {
                    UserId = currentUser.Id,
                    Title = "SendMessage Error",
                    Message = "Error happened in Message Controller, SendMessage function",
                    Exception = ex
                });

                return Ok("Message not sent.");
            }

            await _activityLogService.LogInvalidSendMessageActivityAsync(new ActivityLog()
            {
                UserId = currentUser.Id,
                Message = string.Format("{0} user send a blocked message to {1} user. This message will not be showed up to receiver"
                           , currentUser.Id, receiverUser.Id)
            });
            // If blocked, we show a user friendly message to current user.
            return Ok("Message received  ✔\nThis message will not be showed to the receiver. Because you are blocked by the receiver user.");
        }

        [HttpPut(ApiRoutes.Messages.Update)]
        public async Task<IActionResult> Update(int messageId, [FromBody] UpdateMessageModel model)
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser is null
                || currentUser is default(User))
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            if (messageId < 0)
                return BadRequest("Message id must bigger than zero.");
            
            if (model is null)
                return BadRequest("Model is required.");

            var message = await _messageService.GetMessageBySenderUserIdAsync(messageId, currentUser.Id);

            if (message is null
                || message is default(Message))
                return NotFound("No message found.");

            try
            {
                //map model to message entity
                message = _mapper.Map<Message>(model);

                // update message 
                await _messageService.UpdateMessageAsync(message);
                return Ok("Message updated ✔");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
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

            var currentUser = await GetCurrentUserAsync();

            var result = new List<MessageModel>();

            foreach (var message in messages)
            {
                //SetDeliveredAndReadTimeForMessageAsync(currentUser.Id, message);
                result.Add(new MessageModel()
                {
                    Id = message.Id,
                    SenderUserName = currentUser.Id == message.SenderUserId ? currentUser.Username
                                                                            : await _userService.GetUsernameByUserIdAsync(message.SenderUserId),
                    ReceiverUserName = currentUser.Id == message.ReceiverUserId ? currentUser.Username 
                                                                                : await _userService.GetUsernameByUserIdAsync(message.ReceiverUserId),
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
