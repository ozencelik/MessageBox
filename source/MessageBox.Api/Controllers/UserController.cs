using AutoMapper;
using MessageBox.Api.Configuration;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Logs;
using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MessageBox.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        #region Fields
        private readonly IActivityLogService _activityLogService;
        private readonly ILogService _logService;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public UserController(IActivityLogService activityLogService,
            ILogService logService,
            IOptions<AppSettings> appSettings,
            IMapper mapper,
            IUserService userService)
        {
            this._activityLogService = activityLogService;
            this._logService = logService;
            this._appSettings = appSettings.Value;
            this._mapper = mapper;
            this._userService = userService;
        }
        #endregion

        #region Methods
        [HttpGet(ApiRoutes.Users.Get)]
        public async Task<IActionResult> GetById(int userId)
        {
            if (userId <= 0)
                return BadRequest("Id value must be greater than zero.");

            var user = await _userService.GetUserByIdAsync(userId);

            var model = _mapper.Map<UserModel>(user);

            return Ok(model);
        }

        [HttpGet(ApiRoutes.Users.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();

            var model = _mapper.Map<IList<UserModel>>(users);

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Users.Login)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model is null)
                return BadRequest("Model cannot be null.");

            try
            {
                var user = await _userService.LoginUserWithUsernameAsync(model.Username, model.Password);

                if (user is null)
                {
                    await _activityLogService.LogInvalidLoginActivityAsync(new ActivityLog()
                    {
                        UserId = default,
                        Message = string.Format("{0} user cannot login with the password {1}."
                              , model.Username, model.Password)
                    });
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.Now.AddDays(_appSettings.LoginExpirationDay),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);


                await _activityLogService.LogLoginActivityAsync(new ActivityLog()
                {
                    UserId = user.Id,
                    Message = string.Format("{0} user successfully login. ✔"
                              , model.Username)
                });
                // return basic user info and authentication token
                return Ok(new
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    Message = string.Format("Your login session will be take {0} day.", _appSettings.LoginExpirationDay),
                    Token = tokenString
                });
            }
            catch (Exception ex)
            {
                await _logService.LogErrorAsync(new CreateLogModel()
                {
                    UserId = default,
                    Title = "Login Error",
                    Message = "Error happened in User Controller, Login function",
                    Exception = ex
                });

                return Ok("User not login.");
            }
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Users.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model is null)
                return BadRequest(nameof(model));

            if (string.IsNullOrEmpty(model.Email))
                return BadRequest("Email cannot be null.");

            if (string.IsNullOrEmpty(model.Username))
                return BadRequest("Username cannot be null.");

            var existUserByEmail = await _userService.GetUserByEmailAsync(model.Email);
            var existUserByUsername = await _userService.GetUserByUsernameAsync(model.Username);

            if (existUserByEmail != null
                || existUserByUsername != null)
            {
                await _activityLogService.LogInvalidRegisterActivityAsync(new ActivityLog()
                {
                    UserId = existUserByUsername.Id,
                    Message = string.Format("{0} user is already exist."
                              , existUserByUsername.Username)
                });
                return Content("This user is already exist !!!\nPlease use different email and username.");
            }

            if (existUserByEmail is null
                && existUserByUsername is null)
            {
                // map model to entity
                var user = _mapper.Map<User>(model);

                try
                {
                    // create user
                    await _userService.RegisterUserAsync(user, model.Password);
                    await _activityLogService.LogRegisterActivityAsync(new ActivityLog()
                    {
                        UserId = user.Id,
                        Message = string.Format("{0} user registered successfully. ✔"
                                 , user.Username)
                    });
                }
                catch (Exception ex)
                {
                    // return error message if there was an exception
                    await _logService.LogErrorAsync(new CreateLogModel()
                    {
                        UserId = default,
                        Title = "Register Error",
                        Message = "Error happened in User Controller, Register function",
                        Exception = ex
                    });

                    return Ok("User not registered.");
                }
            }

            return Ok("User registered ✔");
        }

        [HttpPut(ApiRoutes.Users.Update)]
        public async Task<IActionResult> Update(int userId, [FromBody] UpdateModel model)
        {
            if (userId <= 0)
                return BadRequest("Id value must be greater than zero.");

            if (model is null)
                return BadRequest("Model is required.");

            // check the user is exist
            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null)
                return NotFound("User not found.");

            try
            {
                //map model to user entity
                user = _mapper.Map<User>(model);
                user.Id = userId;

                // update user 
                await _userService.UpdateUserAsync(user, model.Password);
                return Ok("User updated ✔");
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}
