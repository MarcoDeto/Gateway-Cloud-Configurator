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
using System.Reflection;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    /// <summary>
    /// Dettaglio canali virtuali (tabella in basso)
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChannelInterfaceAssociateController : ControllerBase
    {
        private readonly ILogger<ChannelInterfaceAssociateController> _logger;
        private readonly ICIAssociatesRepository _repository;
        private readonly IAdapterRepository _adapterRepository;
        private readonly ICIVirtualRepository _ciVirtualRepository;
        private readonly ICIValuesRepository _cIValuesRepository;
        private readonly IMapper _mapper;
        private readonly
            IControllerHelper<InterfaceChannelAssociateDto, ICIAssociatesRepository, Ts400InterfacceCanaliAssociati> _helper;

        public ChannelInterfaceAssociateController(
            ICIAssociatesRepository repository,
            IAdapterRepository adapterRepository,
            ICIVirtualRepository ciVirtualRepository,
            ICIValuesRepository cIValuesRepository,
            IMapper mapper,
            IControllerHelper<InterfaceChannelAssociateDto, ICIAssociatesRepository, Ts400InterfacceCanaliAssociati> helper,
            ILogger<ChannelInterfaceAssociateController> logger)
        {
            _logger = logger;
            _repository = repository;
            _adapterRepository = adapterRepository;
            _ciVirtualRepository = ciVirtualRepository;
            _cIValuesRepository = cIValuesRepository;
            _mapper = mapper;
            _helper = helper;
        }

        #region Altra versione di GetAssociableChannels
        /// <summary>
        /// Canale reale associato (editazione canale associato)
        /// </summary>
        /// <param name="interfaceId"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        //[HttpGet]
        //[ProducesResponseType(200, Type = typeof(List<InterfaceChannelRegistryDto>))]
        //[ProducesResponseType(400, Type = typeof(InfoMsg))]
        //public async Task<IActionResult> GetAssociableChannels(string interfaceId, string direction)
        //{
        //    if (interfaceId == null || direction == null)
        //        return BadRequest(new InfoMsg(400, "E' necessario inserire tipologia interfaccia e direzione"));

        //    var interfaceObj = _adapterRepository.CheckInterface(interfaceId);
        //    if (interfaceObj == null)
        //        return BadRequest(new InfoMsg(400, $"Interfaccia {interfaceId} non presente"));
        //    string type = interfaceObj.InterfacciaContapezzi;

        //    var response = new List<InterfaceChannelRegistryDto>();
        //    var groups = await _repository.GetAssociableChannels(type, direction);
        //    if (groups.Count() != 0)
        //        groups.ForEach(g => response.Add(_mapper.Map<InterfaceChannelRegistryDto>(g)));
        //    return Ok(response);
        //}
        #endregion

        /// <summary>
        /// Canale reale associato (editazione canale associato)
        /// </summary>
        /// <param name="interfaceId"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<InterfaceChannelValuesDto>))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetAssociableChannels(string interfaceId, string direction)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, direction);
            if (interfaceId == null || direction == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire tipologia interfaccia e direzione"));
            
            var interfaceObj = _adapterRepository.CheckInterface(interfaceId);
            if (interfaceObj == null)
                return BadRequest(new InfoMsg(400, $"Interfaccia {interfaceId} non presente"));

            var response = new List<InterfaceChannelValuesDto>();
            var groups = await _cIValuesRepository.GetValues(interfaceId, direction);
            if (groups.Count() != 0)
                groups.ForEach(g => response.Add(_mapper.Map<InterfaceChannelValuesDto>(g)));
            _logger.LogTrace($"200. Returned items: {response.Count()}");
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<InterfaceChannelAssociateDto>))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetAssociateChannels(string interfaceId, string virtualChId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, virtualChId);
            if (interfaceId == null || virtualChId == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire l'id dell'interfaccia e l'id del canale virtuale"));

            var response = new List<InterfaceChannelAssociateDto>();
            var groups = await _repository.GetAssociateChannels(interfaceId, virtualChId);
            if (groups.Count() != 0)
                groups.ForEach(g => response.Add(_mapper.Map<InterfaceChannelAssociateDto>(g)));
            foreach (var item in response)
            {
                var ch = await _cIValuesRepository.GetValue(item.InterfaceId, item.ChannelId, item.Direction);
                var virtualCh = await _ciVirtualRepository.GetVirtualValue(item.InterfaceId, item.ChannelId, item.Direction);
                item.chDescription = ch.Descrizione.TrimEnd();
                item.chType = ch.TipoCanale.Trim();
                item.virtualChDescription = ch.Descrizione.Trim();
                item.virtualChType = ch.TipoCanale.Trim();
            }
            _logger.LogTrace($"200. Returned items: {response.Count()}");
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(InterfaceChannelAssociateDto))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetAssociateChannel(string interfaceId, string channelId, string virtualCh, string direction)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, channelId, virtualCh, direction);
            if (interfaceId == null || channelId == null || virtualCh == null || direction == null)
                return BadRequest(new InfoMsg(
                    400,
                    "E' necessario inserire il canale virtuale, l'id del canale reale, la direzione e l'id dell'interfaccia"));

            return await _helper.Get(
                await _repository.GetAssociateChannel(interfaceId, virtualCh, channelId, direction),
                $"Non è stato trovato alcun canale canale associato {channelId} {direction}");
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Post([FromBody] InterfaceChannelAssociateDto req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            if (req == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del canale associato"));

            var presentValue = _ciVirtualRepository.CheckVirtualValue(req.InterfaceId, req.VirtualChannelId, req.Direction);
            if (presentValue == null)
                return BadRequest(new InfoMsg(400, $"Canale virtuale {req.VirtualChannelId} non presente"));

            return _helper.PostReturnBool(
                req: req,
                isPresent: _repository.CheckAssociateChannel(
                    req.InterfaceId,
                    req.VirtualChannelId,
                    req.ChannelId,
                    req.Direction),
                mState: ModelState,
                msg422: $"Canale associato {req.ChannelId} {req.Direction} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento del canale associato {req.ChannelId} {req.Direction}",
                msg200: $"Canale associato {req.ChannelId} {req.Direction} inserito con successo");
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Put([FromBody] InterfaceChannelAssociateDto req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            if (req == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del canale associato"));

            var presentValue = _ciVirtualRepository.CheckVirtualValue(req.InterfaceId, req.VirtualChannelId, req.Direction);
            if (presentValue == null)
                return BadRequest(new InfoMsg(400, $"Canale virtuale {req.VirtualChannelId} non presente"));

            return _helper.Put(
                req: req,
                isPresent: _repository.CheckAssociateChannel(
                    req.InterfaceId,
                    req.VirtualChannelId,
                    req.ChannelId,
                    req.Direction),
                mState: ModelState,
                msg422: $"Canale associato {req.ChannelId} {req.Direction} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica del canale associato {req.ChannelId} {req.Direction}",
                msg200: $"Canale associato {req.ChannelId} {req.Direction} modificato con successo");
        }

        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public IActionResult Delete(string interfaceId, string channelId, string virtualCh, string direction)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, channelId, virtualCh, direction);
            if (interfaceId == null || channelId == null || virtualCh == null || direction == null)
                return BadRequest(new InfoMsg(
                   400,
                   "E' necessario inserire il canale virtuale, l'id del canale reale, la direzione e l'id dell'interfaccia"));
               
            return _helper.Delete(
               isPresent: _repository.CheckAssociateChannel(
                    interfaceId,
                    virtualCh,
                    channelId,
                    direction),
               msg404: $"Non è stato trovato alcun canale canale associato {channelId} {direction}",
               msg500: $"Ci sono stati problemi nell'eliminazione della canale associato {channelId} {direction}",
               msg200: $"Canale associato {channelId} {direction} eliminato con successo");
        }
    }
}
