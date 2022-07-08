using i4conn.GatewayCloudConfigurationCore.Api.Helpers;
using i4conn.GatewayCloudConfigurationCore.Api.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Helpers;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    /// <summary>
    /// Dettaglio canali (tabella di destra)
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChannelInterfaceVariablesController : ControllerBase
    {
        private readonly ILogger<ChannelInterfaceVariablesController> _logger;
        private readonly ICIValuesRepository _ciValuesRepository;
        private readonly ICIVariablesRepository _ciVariablesRepository;
        private readonly
            IControllerHelper<InterfaceChannelVariablesDto, ICIVariablesRepository, Ts400InterfacceCanaliVariabili> _helper;

        public ChannelInterfaceVariablesController(
            ICIValuesRepository ciValuesRepository,
            ICIVariablesRepository ciVariablesRepository,
            IControllerHelper<InterfaceChannelVariablesDto, ICIVariablesRepository, Ts400InterfacceCanaliVariabili> helper, 
            ILogger<ChannelInterfaceVariablesController> logger)
        {
            _ciValuesRepository = ciValuesRepository;
            _ciVariablesRepository = ciVariablesRepository;
            _helper = helper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<InterfaceChannelVariablesDto>))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetVariablesByInterface(string interfaceId, string channelId, string direction)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, channelId, direction);
            if (interfaceId == null || channelId == null || direction == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire l'id dell'interfaccia e l'id del canale"));

            return await _helper.GetFiltered(
                await _ciVariablesRepository.GetVariables(interfaceId, channelId, direction));
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult PostVariable([FromBody] InterfaceChannelVariablesDto req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            if (req == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati della variabile"));
            var presentValue = _ciValuesRepository.CheckValue(req.InterfaceId, req.ChannelId, req.Direction);
            if (presentValue == null)
                return BadRequest(new InfoMsg(400, $"Canale {req.ChannelId} non presente"));

            return _helper.PostReturnBool(
                req: req,
                isPresent: _ciVariablesRepository.CheckVariable(req.InterfaceId, req.ChannelId, req.Name),
                mState: ModelState,
                msg422: $"Variabile {req.Name} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento della variabile {req.Name}",
                msg200: $"Variabile {req.Name} aggiunta con successo");
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult PutVariable([FromBody] InterfaceChannelVariablesDto req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            if (req == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati della variabile"));
            var presentValue = _ciValuesRepository.CheckValue(req.InterfaceId, req.ChannelId, req.Direction);
            if (presentValue == null)
                return BadRequest(new InfoMsg(400, $"Canale {req.ChannelId} non presente"));

            return _helper.Put(
                req: req,
                isPresent: _ciVariablesRepository.CheckVariable(req.InterfaceId, req.ChannelId, req.Name),
                mState: ModelState,
                msg422: $"Variabile {req.Name} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica della variabile {req.Name}",
                msg200: $"Variabile {req.Name} modificata con successo");
        }

        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public IActionResult DeleteVariable(string interfaceId, string channelId, string name)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, channelId, name);
            if (interfaceId == null || channelId == null || name == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire l'id dell'interfaccia e l'id del canale"));

            return _helper.Delete(
               isPresent: _ciVariablesRepository.CheckVariable(interfaceId, channelId, name),
               msg404: $"Non è stato trovato alcuna variabile {name}",
               msg500: $"Ci sono stati problemi nell'eliminazione della variabile {name}",
               msg200: $"Variabile {name} eliminata con successo");
        }
    }
}
