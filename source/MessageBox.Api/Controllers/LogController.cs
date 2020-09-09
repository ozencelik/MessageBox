using AutoMapper;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Logs;
using MessageBox.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageBox.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class LogController : Controller
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        #endregion

        #region Ctor
        public LogController(IMapper mapper,
            ILogService logService)
        {
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
