using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MessageBox.Api.Controllers
{
    public class UserController : Controller
    {
        #region Fields
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/CreateUser")]
        public async Task<IActionResult> Create(NewUserRequest model)
        {
            if (model is null)
                return BadRequest(nameof(model));

            if(string.IsNullOrEmpty(model.Email))
                return BadRequest("Email cannot be null.");

            if (string.IsNullOrEmpty(model.Username))
                return BadRequest("Username cannot be null.");

            var existUserByEmail = await _userService.GetUserByEmailAsync(model.Email);
            var existUserByUsername = await _userService.GetUserByUsernameAsync(model.Username);

            if (existUserByEmail != null
                || existUserByUsername != null)
                return Content("This user is already exit !!!\nPlease use diffrent email and username.");

            if (existUserByEmail is null
                && existUserByUsername is null)
            {
                var user = new User()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password
                };

                await _userService.InsertUserAsync(user);
            }

            return Ok("User added ✔");
        }
    }
}
