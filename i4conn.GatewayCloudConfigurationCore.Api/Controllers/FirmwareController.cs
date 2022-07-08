using i4conn.GatewayCloudConfigurationCore.Api.Helpers;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Helpers;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FirmwareController : ControllerBase
    {
        private readonly ILogger<FirmwareController> _logger;
        private readonly IFirmwareService _service;

        public FirmwareController(IFirmwareService service, ILogger<FirmwareController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Inserire il percorso della directory contenente i firmware
        /// </summary>
        /// <param name="path">C:\i4conn\Server\Firmware</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<FirmwareDto>))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> Get(string path)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(path);
            if (path == null)
                return BadRequest(new InfoMsg(400, "Percorso non valido"));
            try
            {
                //var i4ConnDir = @"C:\i4conn\Server\";
                //var pathFirmware = $@"{i4ConnDir}Firmware\";
                string[] types = new string[5] { "DeviceManager", "Router", "RulesManager", "Supervisor", "WebApp" };
                _logger.LogTrace("Accepted types: ", types);
                var result = await _service.AllAsync(path + @"\", types);
                _logger.LogTrace("Result is null: ", result == null);
                if (result == null)
                    return NotFound(new InfoMsg(404, "Nessun firmware trovato"));
                _logger.LogTrace("Returned items " + result.Count());
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, $"UnauthorizedAccessException occurred at {LoggerHelper.GetActualMethodName()}");
                return BadRequest(new InfoMsg(400, "Accesso non consentito"));
            }
        }
    }
}
