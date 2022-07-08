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

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    /// <summary>
    /// Gruppi interfacce
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupInterfaceController : ControllerBase
    {
        private readonly ILogger<GroupInterfaceController> _logger;
        private readonly IControllerHelper<InterfaceGroupDto, IGroupInterfaceRepository, Ts400InterfacceGruppi> _helper;
        private readonly IGatewayRepository _gatewayRepository;
        private readonly IGroupInterfaceRepository _giRepository;
        private readonly IAdapterRepository _adapterRepository;
        private readonly IMapper _mapper;

        public GroupInterfaceController(
            IControllerHelper<InterfaceGroupDto, IGroupInterfaceRepository, Ts400InterfacceGruppi> helper,
            IGatewayRepository gatewayRepository,
            IGroupInterfaceRepository giRepository,
            IAdapterRepository adapterRepository,
            IMapper mapper, ILogger<GroupInterfaceController> logger)
        {
            _gatewayRepository = gatewayRepository;
            _giRepository = giRepository;
            _adapterRepository = adapterRepository;
            _mapper = mapper;
            _helper = helper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<InterfaceGroupDto>))]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            var response = new List<InterfaceGroupDto>();
            var groups = await _giRepository.GetAll();
            groups.ForEach(g => response.Add(_mapper.Map<InterfaceGroupDto>(g)));
            response.ForEach(r => r.Interfaces = getAllByGroup(r.Id).Result);
            _logger.LogTrace("Returned items: ", response.Count());
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(InterfaceGroupDto))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(id);
            if (id == null)
                return BadRequest(new InfoMsg(400, "Id non valido"));
            var gi = await _giRepository.GetById(id);
            if (gi == null)
                return NotFound(new InfoMsg(404, $"Non è stato trovato alcun gruppo interfacce con id {id}"));
            var response = _mapper.Map<InterfaceGroupDto>(gi);
            response.Interfaces = getAllByGroup(id).Result;
            _logger.LogTrace("Returned item: ", response);
            return Ok(response);
        }

        /// <summary>
        /// Restituisce i gruppi interfacce associati allo specifico gateway
        /// </summary>
        /// <param name="idGateway"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<InterfaceGroupDto>))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetByGateway(string idGateway)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(idGateway);
            if (idGateway == null)
                return BadRequest(new InfoMsg(400, "Id gateway non valido"));
            var response = new List<InterfaceGroupDto>();
            var groups = await _giRepository.GetGroupsByGateway(idGateway);
            if (groups.Count() != 0)
                groups.ForEach(g => response.Add(_mapper.Map<InterfaceGroupDto>(g)));
            response.ForEach(r => r.Interfaces = getAllByGroup(r.Id).Result);
            _logger.LogTrace("Returned items: ", response.Count());
            return Ok(response);
        }

        /// <summary>
        /// Dissocia le interfacce dal gruppo appartenente
        /// </summary>
        /// <param name="idGroup"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        public async Task<IActionResult> DissociatesInterfaces(string idGroup)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(idGroup);
            if (idGroup == null)
                return BadRequest(new InfoMsg(400, "Id gruppo interfacce non valido"));
            bool result = await _adapterRepository.DeleteGroup(idGroup);
            if (!result)
                return StatusCode(500, $"Ci sono stati problemi nella dissociazione del gruppo {idGroup} dalle interfacce");
            _logger.LogTrace("Returned success: ", result);
            return Ok(new InfoMsg(200, $"Tutte le interfacce del gruppo {idGroup} sono state dissociate con successo"));
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Post([FromBody] InterfaceGroupDto value)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(value);
            if (value == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gruppo interfacce"));
            var presentGateway = _gatewayRepository.CheckGateway(value.GatewayId);
            if (presentGateway == null && !string.IsNullOrEmpty(value.GatewayId))
                return BadRequest(new InfoMsg(400, $"Gateway con id {value.GatewayId} non presente"));

            return _helper.PostReturnBool(
                req: value,
                isPresent: _giRepository.CheckGroup(value.Id),
                mState: ModelState,
                msg422: $"Gruppo con Id {value.Id} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento del gruppo interfacce {value.Id}",
                msg200: $"Gruppo interfacce {value.Id} inserito con successo");

            #region OldCode
            //if (value == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gruppo interfacce"));
            //if (!ModelState.IsValid)
            //{
            //    StringBuilder ErrVal = new StringBuilder(string.Empty);
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
            //var isPresent = _giRepository.CheckGroup(value.Id);
            //if (isPresent != null)
            //    return StatusCode(422, new InfoMsg(422, $"Gruppo con Id {value.Id} già presente. Impossibile inserire!"));
            //#region prima del base
            //var presentGateway = _gatewayRepository.CheckGateway(value.GatewayId);
            //if (presentGateway == null && !string.IsNullOrEmpty(value.GatewayId))
            //    return StatusCode(422, new InfoMsg(422, $"Gateway con id {value.GatewayId} non presente. Impossibile inserire!"));
            //#endregion 
            //if (!_giRepository.Insert(_mapper.Map<Ts400InterfacceGruppi>(value)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nell'inserimento del gruppo {value.Id}"));
            //return Ok(new InfoMsg(200, $"Gruppo interfacce {value.Id} inserito con successo"));
            #endregion
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Put([FromBody] InterfaceGroupDto value)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(value);
            if (value == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gruppo interfacce"));
            var presentGateway = _gatewayRepository.CheckGateway(value.GatewayId);
            if (presentGateway == null && !string.IsNullOrEmpty(value.GatewayId))
                return BadRequest(new InfoMsg(400, $"Gateway con id {value.GatewayId} non presente"));

            return _helper.Put(
                req: value,
                isPresent: _giRepository.CheckGroup(value.Id),
                mState: ModelState,
                msg422: $"Gruppo con Id {value.Id} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica del gruppo interfacce {value.Id}",
                msg200: $"Gruppo interfacce {value.Id} modificato con successo");

            #region OldCode
            //if (value == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gruppo interfacce"));
            //var isPresent = _giRepository.CheckGroup(value.Id);
            //if (!ModelState.IsValid)
            //{
            //    StringBuilder ErrVal = new StringBuilder(string.Empty);
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
            //    return StatusCode(422, new InfoMsg(422, $"Gruppo con Id {value.Id} non presente. Impossibile modificare!"));
            //#region prima del base
            //var presentGateway = _gatewayRepository.CheckGateway(value.GatewayId);
            //if (presentGateway == null && !string.IsNullOrEmpty(value.GatewayId))
            //    return StatusCode(422, new InfoMsg(422, $"Gateway con id {value.GatewayId} non presente. Impossibile modificare!"));
            //#endregion
            //if (!_giRepository.Update(_mapper.Map<Ts400InterfacceGruppi>(value)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nella modifica del gruppo {value.Id}"));
            //return Ok(new InfoMsg(200, $"Gruppo interfacce {value.Id} modificato con successo"));
            #endregion
        }

        /// <summary>
        /// Elimina il gruppo e dissocia le interfacce associate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public IActionResult Delete(string id)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            if (id == null)
                return BadRequest(new InfoMsg(400, "Id non valido"));
            var response = _helper.Delete(
                isPresent: _giRepository.CheckGroup(id),
                msg404: $"Non è stato trovato alcun gruppo interfacce con id {id}",
                msg500: $"Ci sono stati problemi nell'eliminazione del gruppo interfacce{id}",
                msg200: $"Gruppo interfacce {id} eliminato con successo");

            if (!_adapterRepository.DeleteGroup(id).Result)
                return StatusCode(500, $"Ci sono stati problemi nell'eliminazione del gruppo {id} dalle interfacce associate");

            return response;

            #region OldCode
            //var gi = _giRepository.CheckGroup(id);
            //if (gi == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovato alcun gruppo interfacce con id {id}"));
            //if (!_giRepository.Delete(gi))
            //    return StatusCode(500, $"Ci sono stati problemi nell'eliminazione del gruppo {id}");
            //if (!_adapterRepository.DeleteGroup(id).Result)
            //    return StatusCode(500, $"Ci sono stati problemi nell'eliminazione del gruppo {id} dalle interfacce associate");
            //return Ok(new InfoMsg(200, $"Gruppo interfacce {id} eliminato con successo"));
            #endregion
        }

        /// <summary>
        /// Elimina il gruppo insieme alle interfacce associate
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public IActionResult DeleteWithInterfaces(string id)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(id);
            if (id == null)
                return BadRequest(new InfoMsg(400, "Id non valido"));
            var response = _helper.Delete(
                isPresent: _giRepository.CheckGroup(id),
                msg404: $"Non è stato trovato alcun gruppo interfacce con id {id}",
                msg500: $"Ci sono stati problemi nell'eliminazione del gruppo interfacce{id}",
                msg200: $"Gruppo interfacce {id} eliminato con successo");

            if (!_adapterRepository.RemoveByGroup(id))
                return StatusCode(500, $"Ci sono stati problemi nell'eliminazione delle interfacce associate al gruppo {id}");

            return response;
        }

        private async Task<List<InterfaceDto>> getAllByGroup(string groupId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            var result = new List<InterfaceDto>();
            var adapters = await _adapterRepository.GetAdatptersByGroup(groupId);
            adapters.ForEach(i => result.Add(_mapper.Map<InterfaceDto>(i)));
            return result;
        }
    }
}
