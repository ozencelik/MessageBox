using AutoMapper;
using MessageBox.Api.Configuration;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Logs;
using MessageBox.Core.Services.Users;
using MessageBox.Data.Entities;
using MessageBox.Data.Models;
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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LogController : Controller
    {
        #region Fields
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        #endregion

        #region Ctor
        public LogController(IOptions<AppSettings> appSettings,
            IMapper mapper,
            ILogService logService)
        {
            this._appSettings = appSettings.Value;
            this._mapper = mapper;
            this._logService = logService;
        }
        #endregion

        #region Methods
        [HttpGet(ApiRoutes.Logs.Get)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Id value must be greater than zero.");

            var log = await _logService.GetLogByIdAsync(id);

            var model = _mapper.Map<LogModel>(log);

            return Ok(model);
        }

        [HttpGet(ApiRoutes.Logs.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _logService.GetAllLogsAsync();

            var model = _mapper.Map<IList<LogModel>>(logs);

            return Ok(model);
        }
        #endregion
    }
}
