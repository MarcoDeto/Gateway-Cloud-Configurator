using AutoMapper;
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
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    /// <summary>
    /// Dettaglio canali (tabella di sinistra)
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChannelInterfaceValuesController : ControllerBase
    {
        private readonly IAdapterRepository _interfaceRepository;
        private readonly ITypeEntityRepository _teRepository;
        private readonly ILogger<ChannelInterfaceValuesController> _logger;
        private readonly ICIValuesRepository _ciValuesRepository;
        private readonly ICIVariablesRepository _ciVariablesRepository;
        private readonly IMapper _mapper;
        private readonly 
            IControllerHelper<InterfaceChannelValuesDto, ICIValuesRepository, Ts400InterfacceCanaliValori> _helper;

        public ChannelInterfaceValuesController(ILogger<ChannelInterfaceValuesController> logger,
            IMapper mapper,
            ICIValuesRepository ciValuesRepository,
            ICIVariablesRepository ciVariablesRepository,
            IControllerHelper<InterfaceChannelValuesDto, ICIValuesRepository, Ts400InterfacceCanaliValori> helper,
            IAdapterRepository interfaceRepository, ITypeEntityRepository teRepository)
        {
            _logger = logger;
            _ciValuesRepository = ciValuesRepository;
            _ciVariablesRepository = ciVariablesRepository;
            _helper = helper;
            _mapper = mapper;
            _interfaceRepository = interfaceRepository;
            _teRepository = teRepository;
        }

        [HttpGet("{interfaceId}")]
        [ProducesResponseType(200, Type = typeof(List<InterfaceChannelValuesDto>))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetValuesByInterface(string interfaceId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId);
            if (interfaceId == null)
                return BadRequest(new InfoMsg(400, "Interfaccia non valida"));

            var response = new List<InterfaceChannelValuesDto>();
            var groups = await _ciValuesRepository.GetValues(interfaceId);
            groups.ForEach(g => response.Add(_mapper.Map<InterfaceChannelValuesDto>(g)));
            foreach (var res in response)
            {
                res.Variables = await getVariables(res.InterfaceId, res.ChannelId, res.Direction);
                var interf = _interfaceRepository.CheckInterface(interfaceId);
                var type = _teRepository.CheckTypeEntity(interf.InterfacciaContapezzi);
                res.AllowEditChannel = type.AllowEditChannel;
                res.AllowEditVariable = type.AllowEditVariable;
            }
            _logger.LogTrace($"200. Returned items: {response.Count()}");
            return Ok(response);
        }

        /// <summary>
        /// Action simile a GetValuesByInterface, con aggiunta di sistema di paginazione skip e take
        /// </summary>
        /// <param name="interfaceId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("{interfaceId}")]
        [ProducesResponseType(200, Type = typeof(PagingResponse<List<InterfaceChannelValuesDto>>))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetValuesByInterfacePaging(string interfaceId, int skip, int take)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId);
            if (interfaceId == null)
                return BadRequest(new InfoMsg(400, "Interfaccia non valida"));
            if (take <= 0 || skip < 0)
                return BadRequest(new InfoMsg(400, "Parametri di paginazione non validi"));

            var response = new PagingResponse<List<InterfaceChannelValuesDto>>();
            response.Content = new List<InterfaceChannelValuesDto>();
            var groups = await _ciValuesRepository.GetValues(interfaceId, skip, take);
            response.Count = groups.Count;
            groups.Content.ForEach(g => response.Content.Add(_mapper.Map<InterfaceChannelValuesDto>(g)));
            foreach (var res in response.Content)
            {
                res.Variables = await getVariables(res.InterfaceId, res.ChannelId, res.Direction);
                var interf = _interfaceRepository.CheckInterface(interfaceId);
                var type = _teRepository.CheckTypeEntity(interf.InterfacciaContapezzi);
                res.AllowEditChannel = type.AllowEditChannel;
                res.AllowEditVariable = type.AllowEditVariable;
            }
            _logger.LogTrace($"200. Returned items: {response.Content.Count()}");
            return Ok(response);
        }

        /// <summary>
        /// Estrae uno specifico canale passando l'id dell'interfaccia, l'id del canale e la direzione
        /// </summary>
        /// <param name="interfaceId"></param>
        /// <param name="channelId"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetValue(string interfaceId, string channelId, string direction)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            if (interfaceId == null || channelId == null || direction == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire la direzione, l'id del canale e l'id dell'interfaccia"));

            var result = await _ciValuesRepository.GetValue(interfaceId, channelId, direction);
            if (result == null)
                return NotFound(new InfoMsg(404, $"Non è stato trovato alcun canale virtuale {channelId}"));
            var res = _mapper.Map<InterfaceChannelValuesDto>(result);
            res.Variables = await getVariables(res.InterfaceId, res.ChannelId, res.Direction);
            var interf = _interfaceRepository.CheckInterface(interfaceId);
            var type = _teRepository.CheckTypeEntity(interf.InterfacciaContapezzi);
            res.AllowEditChannel = type.AllowEditChannel;
            res.AllowEditVariable = type.AllowEditVariable;
            _logger.LogTrace("200. Returned item", res);
            return Ok(res);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Post([FromBody] InterfaceChannelValuesDto req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            if (req == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del canale"));
            var presentInterface = _interfaceRepository.CheckInterface(req.InterfaceId);
            if (presentInterface == null)
                return BadRequest(new InfoMsg(400, "Interfaccia non esistente"));

            return _helper.PostReturnBool(
                req: req,
                isPresent: _ciValuesRepository.CheckValue(req.InterfaceId, req.ChannelId, req.Direction),
                mState: ModelState,
                msg422: $"Canale {req.ChannelId} {req.Direction} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento del canale {req.ChannelId} {req.Direction}",
                msg200: $"Canale {req.ChannelId} {req.Direction} inserito con successo");
        }

        /// <summary>
        /// Si può modificare solo la descrizione
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Put([FromBody] InterfaceChannelValuesDto req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            if (req == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del canale"));
            var presentInterface = _interfaceRepository.CheckInterface(req.InterfaceId);
            if (presentInterface == null)
                return BadRequest(new InfoMsg(400, "Interfaccia non esistente"));

            return _helper.Put(
                req: req,
                isPresent: _ciValuesRepository.CheckValue(req.InterfaceId, req.ChannelId, req.Direction),
                mState: ModelState,
                msg422: $"Canale {req.ChannelId} {req.Direction} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica del canale {req.ChannelId} {req.Direction}",
                msg200: $"Canale {req.ChannelId} {req.Direction} modificato con successo");
        }

        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public IActionResult Delete(string interfaceId, string channelId, string direction)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, channelId, direction);
            if (interfaceId == null || channelId == null || direction == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire il nome, l'id del canale e l'id dell'interfaccia"));

            return _helper.Delete(
               isPresent: _ciValuesRepository.CheckValue(interfaceId, channelId, direction),
               msg404: $"Non è stato trovato alcun canale {channelId} {direction}",
               msg500: $"Ci sono stati problemi nell'eliminazione della canale {channelId} {direction}",
               msg200: $"Canale {channelId} {direction} eliminato con successo");
        }

        private async Task<List<InterfaceChannelVariablesDto>> getVariables(string interfaceId, string channelId, string direction)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            var result = new List<InterfaceChannelVariablesDto>();
            var adapters = await _ciVariablesRepository.GetVariables(interfaceId, channelId, direction);
            adapters.ForEach(i => result.Add(_mapper.Map<InterfaceChannelVariablesDto>(i)));
            return result;
        }
    }
}
