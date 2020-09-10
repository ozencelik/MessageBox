using AutoMapper;
using AutoMapper.Internal;
using DocumentFormat.OpenXml.Drawing.Charts;
using MessageBox.Api.Configuration;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Messages;
using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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
        #endregion

        #region Ctor
        public MessageController(IMapper mapper,
            IMessageService messageService,
            IUserService userService)
        {
            this._mapper = mapper;
            this._messageService = messageService;
            this._userService = userService;
        }
        #endregion

        #region Methods
        [HttpGet(ApiRoutes.Messages.Get)]
        public async Task<IActionResult> GetById(int messageId)
        {
            if (messageId <= 0)
                return BadRequest("Id value must be greater than zero.");

            var message = await _messageService.GetMessageByIdAsync(messageId);

            var model = _mapper.Map<MessageModel>(message);

            return Ok(model);
        }

        [HttpGet(ApiRoutes.Messages.GetAll)]
        [SwaggerOperation(
            Summary = "Get All Messages",
            Description = "Gets all messages that were sent or received by the current user",
            OperationId = "Message.GetAll",
            Tags = new[] { "MessageEndpoints" })]
        public async Task<IActionResult> GetAll()
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId is 0)
                return Unauthorized("No authorized user found.\nPlease log in by using your credentials.");

            var user = await _userService.GetUserByIdAsync(currentUserId);

            if (user is null
                || user is default(User))
                return NotFound("User not found.");

            // Authorized users can only get their messages.
            // They cannot acces someone's messages.
            var messages = await _messageService.GetAllMessagesBySenderUserIdAsync(currentUserId);

            return Ok(await PrepareMessageModelAsync(messages));
        }
        /*
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Messages.Login)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model is null)
                return BadRequest("Model cannot be null.");

            var message = await _messageService.LoginMessageWithMessagenameAsync(model.Messagename, model.Password);

            if (message is null)
                return BadRequest(new { message = "Messagename or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, message.Id.ToString())
                }),
                Expires = DateTime.Now.AddMinutes(5),
                //Expires = DateTime.Now.AddDays(_appSettings.LoginExpirationDay),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic message info and authentication token
            return Ok(new
            {
                Id = message.Id,
                Email = message.Email,
                Messagename = message.Messagename,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Messages.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model is null)
                return BadRequest(nameof(model));

            if (string.IsNullOrEmpty(model.Email))
                return BadRequest("Email cannot be null.");

            if (string.IsNullOrEmpty(model.Messagename))
                return BadRequest("Messagename cannot be null.");

            var existMessageByEmail = await _messageService.GetMessageByEmailAsync(model.Email);
            var existMessageByMessagename = await _messageService.GetMessageByMessagenameAsync(model.Messagename);

            if (existMessageByEmail != null
                || existMessageByMessagename != null)
                return Content("This message is already exit !!!\nPlease use different email and messagename.");

            if (existMessageByEmail is null
                && existMessageByMessagename is null)
            {
                // map model to entity
                var message = _mapper.Map<Message>(model);

                try
                {
                    // create message
                    await _messageService.RegisterMessageAsync(message, model.Password);
                }
                catch (Exception ex)
                {
                    // return error message if there was an exception
                    return BadRequest(new { message = ex.Message });
                }
            }

            return Ok("Message registered ✔");
        }

        [HttpPut(ApiRoutes.Messages.Update)]
        public async Task<IActionResult> Update(int messageId, [FromBody] UpdateModel model)
        {
            if (messageId <= 0)
                return BadRequest("Id value must be greater than zero.");

            if (model is null)
                return BadRequest("Model is required.");

            // check the message is exist
            var message = await _messageService.GetMessageByIdAsync(messageId);

            if (message is null)
                return NotFound("Message not found.");

            try
            {
                //map model to message entity
                message = _mapper.Map<Message>(model);
                message.Id = messageId;

                // update message 
                await _messageService.UpdateMessageAsync(message, model.Password);
                return Ok("Message updated ✔");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }*/
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

        private async Task<IList<MessageModel>> PrepareMessageModelAsync(IList<Message> messages)
        {
            if (messages is null
                || messages.Count() == 0)
                return default;

            var result = new List<MessageModel>();

            foreach(var message in messages)
            {
                result.Add(new MessageModel()
                {
                    SenderUserName = await _userService.GetUsernameByUserIdAsync(message.SenderUserId),
                    ReceiverUserName = await _userService.GetUsernameByUserIdAsync(message.ReceiverUserId),
                    Content = message.Content
                });
            }

            return result;
        }
        #endregion
    }
}
