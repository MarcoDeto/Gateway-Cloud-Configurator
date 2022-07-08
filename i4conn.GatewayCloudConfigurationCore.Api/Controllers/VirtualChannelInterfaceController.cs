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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    /// <summary>
    /// Dettaglio canali virtuali
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VirtualChannelInterfaceController : ControllerBase
    {
        private readonly IAdapterRepository _interfaceRepository;
        private readonly ILogger<VirtualChannelInterfaceController> _logger;
        private readonly ICIVirtualRepository _ciVirtualRepository;
        private readonly ICIAssociatesRepository _ciAssociatesRepository;
        private readonly IMapper _mapper;
        private readonly
            IControllerHelper<InterfaceChannelValuesDto, ICIVirtualRepository, Ts400InterfacceCanaliValori> _helper;

        public VirtualChannelInterfaceController(
            ICIVirtualRepository ciVirtualRepository,
            ICIAssociatesRepository ciAssociatesRepository,
            IMapper mapper,
            IControllerHelper<InterfaceChannelValuesDto, ICIVirtualRepository, Ts400InterfacceCanaliValori> helper,
            ILogger<VirtualChannelInterfaceController> logger, IAdapterRepository interfaceRepository)
        {
            _ciVirtualRepository = ciVirtualRepository;
            _ciAssociatesRepository = ciAssociatesRepository;
            _mapper = mapper;
            _helper = helper;
            _logger = logger;
            _interfaceRepository = interfaceRepository;
        }

        [HttpGet("{interfaceId}")]
        [ProducesResponseType(200, Type = typeof(List<InterfaceChannelValuesDto>))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetVirtaulValues(string interfaceId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId);
            if (interfaceId == null)
                return BadRequest(new InfoMsg(400, "Id interfaccia non valida"));

            var response = new List<InterfaceChannelValuesDto>();
            var groups = await _ciVirtualRepository.GetVirtualValues(interfaceId);
            groups.ForEach(g => response.Add(_mapper.Map<InterfaceChannelValuesDto>(g)));
            response.ForEach(r => r.AssociateChannels = getAssociateCh(r.InterfaceId, r.ChannelId).Result);
            _logger.LogTrace("Returned items: " + response.Count());
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetVirtualValue(string interfaceId, string channelId, string direction)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, channelId, direction);
            if (interfaceId == null || channelId == null || direction == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire la direzione, l'id del canale e l'id dell'interfaccia"));
            return await _helper.Get(
                await _ciVirtualRepository.GetVirtualValue(interfaceId, channelId, direction),
                $"Non è stato trovato alcun canale virtuale {channelId}");
        }

        /// <summary>
        /// ChannelId generato da server
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InterfaceChannelValuesDto))]
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

            return _helper.PostReturnObj(
                req: req,
                isPresent: _ciVirtualRepository.CheckVirtualValue(req.InterfaceId, req.ChannelId ?? string.Empty, req.Direction),
                mState: ModelState,
                msg422: $"Canale {req.Description} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento del canale {req.Description}",
                msg200: $"Canale {req.Description} inserito con successo");
        }

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
                isPresent: _ciVirtualRepository.CheckVirtualValue(req.InterfaceId, req.ChannelId, req.Direction),
                mState: ModelState,
                msg422: $"Canale {req.Description} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica del canale {req.Description}",
                msg200: $"Canale {req.Description} modificato con successo");
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
               isPresent: _ciVirtualRepository.CheckVirtualValue(interfaceId, channelId, direction),
               msg404: $"Non è stato trovato alcun canale {channelId} {direction}",
               msg500: $"Ci sono stati problemi nell'eliminazione della canale {channelId} {direction}",
               msg200: $"Canale {channelId} {direction} eliminato con successo");
        }

        private async Task<List<InterfaceChannelAssociateDto>> getAssociateCh(string interfaceId, string virtualChId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            var result = new List<InterfaceChannelAssociateDto>();
            var adapters = await _ciAssociatesRepository.GetAssociateChannels(interfaceId, virtualChId);
            adapters.ForEach(i => result.Add(_mapper.Map<InterfaceChannelAssociateDto>(i)));
            return result;
        }
    }
}
