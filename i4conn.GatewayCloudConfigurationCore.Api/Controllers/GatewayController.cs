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
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    /// <summary>
    /// Gateway
    /// </summary>
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GatewayController : 
        ControllerBase
    {
        private readonly ILogger<GatewayController> _logger;
        private readonly IControllerHelper<GatewayDto, IGatewayRepository, Ts400Gateway> _helper;
        private readonly IGatewayRepository _gatewayRepository;
        private readonly IAdapterRepository _adapterRepository;
        private readonly IMapper _mapper;

        public GatewayController(
            IGatewayRepository gatewayRepository,
            IMapper mapper,
            IAdapterRepository adapterRepository,
            IControllerHelper<GatewayDto, IGatewayRepository, Ts400Gateway> helper, ILogger<GatewayController> logger)
        {
            _gatewayRepository = gatewayRepository;
            _adapterRepository = adapterRepository;
            _mapper = mapper;
            _helper = helper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<GatewayDto>))]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            var result = new List<GatewayDto>();
            var gateways = await _gatewayRepository.GetAll();
            gateways.ForEach(g => result.Add(_mapper.Map<GatewayDto>(g)));
            result.ForEach(g => g.CounterAdapters = _adapterRepository.GetAdaptersByGateway(g.GatewayId).Result.Count());
            result.ForEach(g => g.CounterDevices = _adapterRepository.CountDevicesByGateway(g.GatewayId));
            _logger.LogTrace("Returned items: ", result.Count());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(GatewayDto))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(id);
            if (id == null)
                return BadRequest(new InfoMsg(400, "Id non valido"));
            var gateway = await _gatewayRepository.GetById(id);
            if (gateway == null)
                return NotFound(new InfoMsg(404, $"Non è stato trovato alcun gateway con id {id}"));
            var result = _mapper.Map<GatewayDto>(gateway);
            result.CounterAdapters = _adapterRepository.GetAdaptersByGateway(result.GatewayId).Result.Count();
            result.CounterDevices = _adapterRepository.CountDevicesByGateway(result.GatewayId);
            _logger.LogTrace("Returned item: ", result);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Post([FromBody] GatewayDto gateway)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(gateway);
            if (gateway == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gateway"));
            return _helper.PostReturnBool(
                req: gateway,
                isPresent: _gatewayRepository.CheckGateway(gateway.GatewayId),
                mState: ModelState,
                msg422: $"Gateway con Id {gateway.GatewayId} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento del gateway {gateway.GatewayId}",
                msg200: $"Gateway {gateway.GatewayId} inserito con successo");

            #region OldCode
            //if (gateway == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gateway"));
            //if (!ModelState.IsValid)
            //{
            //    StringBuilder ErrVal = new StringBuilder("");
            //    string errore = (this.HttpContext == null) ? "400" : this.HttpContext.Response.StatusCode.ToString();
            //    foreach (var modelState in ModelState.Values)
            //    {
            //        foreach (var modelError in modelState.Errors)
            //        {
            //            ErrVal.Append(modelError.ErrorMessage);
            //            ErrVal.Append(" - ");
            //        }
            //    }
            //    return BadRequest(new InfoMsg(400, ErrVal.ToString()));
            //}
            //var isPresent = _gatewayRepository.CheckGateway(gateway.GatewayId);
            //if (isPresent != null)
            //    return StatusCode(422, new InfoMsg(422, $"Gateway con Id {gateway.GatewayId} già presente. Impossibile inserire!"));
            //if (!_gatewayRepository.Insert(_mapper.Map<Ts400Gateway>(gateway)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nell'inserimento del gateway {gateway.GatewayId}"));
            //return Ok(new InfoMsg(200, $"Gateway {gateway.GatewayId} inserito con successo"));
            #endregion
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Put([FromBody] GatewayDto gateway)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(gateway);
            if (gateway == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gateway"));
            return _helper.Put(
                   req: gateway,
                   isPresent: _gatewayRepository.CheckGateway(gateway.GatewayId),
                   mState: ModelState,
                   msg422: $"Gateway con Id {gateway.GatewayId} non presente. Impossibile modificare!",
                   msg500: $"Ci sono stati problemi nella modifica del gateway {gateway.GatewayId}",
                   msg200: $"Gateway {gateway.GatewayId} modificato con successo");

            #region OldCode
            //if (gateway == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gateway"));
            //var isPresent = _gatewayRepository.CheckGateway(gateway.GatewayId);
            //if (!ModelState.IsValid)
            //{
            //    StringBuilder ErrVal = new StringBuilder("");
            //    string errore = (this.HttpContext == null) ? "400" : this.HttpContext.Response.StatusCode.ToString();
            //    foreach (var modelState in ModelState.Values)
            //    {
            //        foreach (var modelError in modelState.Errors)
            //        {
            //            ErrVal.Append(modelError.ErrorMessage);
            //            ErrVal.Append(" - ");
            //        }
            //    }
            //    return BadRequest(new InfoMsg(400, ErrVal.ToString()));
            //}
            //if (isPresent == null)
            //    return StatusCode(422, new InfoMsg(422, $"Gateway con Id {gateway.GatewayId} non presente. Impossibile modificare!"));
            //if (!_gatewayRepository.Update(_mapper.Map<Ts400Gateway>(gateway)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nella modifica del gateway {gateway.GatewayId}"));
            //return Ok(new InfoMsg(200, $"Gateway {gateway.GatewayId} modificato con successo"));
            #endregion
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public IActionResult Delete(string id)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(id);
            if (id == null)
                return BadRequest(new InfoMsg(400, "Id non valido"));
            return _helper.Delete(
                isPresent: _gatewayRepository.CheckGateway(id),
                msg404: $"Non è stato trovato alcun gateway con id {id}",
                msg500: $"Ci sono stati problemi nell'eliminazione del gateway {id}",
                msg200: $"Gateway {id} eliminato con successo");

            #region OldCode
            //var gateway = _gatewayRepository.CheckGateway(id);
            //if (gateway == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovato alcun gateway con id {id}"));
            //if (!_gatewayRepository.Delete(gateway))
            //    return StatusCode(500, $"Ci sono stati problemi nell'eliminazione del gateway {id}");
            //return Ok(new InfoMsg(200, $"Gateway {id} eliminato con successo"));
            #endregion
        }
    }
}
