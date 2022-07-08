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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    /// <summary>
    /// Interfacce
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InterfacesController : ControllerBase
    {
        private readonly ILogger<InterfacesController> _logger;
        private readonly IControllerHelper<InterfaceDto, IAdapterRepository, Ts400Interfacce> _helper;
        private readonly IAdapterRepository _adapterRepository;
        private readonly IChannelInterfaceRepository _ciRepository;
        private readonly IGroupInterfaceRepository _giRepository;
        private readonly ITypeEntityRepository _teRepository;
        private readonly IMapper _mapper;

        public InterfacesController(
            IControllerHelper<InterfaceDto, IAdapterRepository, Ts400Interfacce> helper,
            IAdapterRepository adapterRepository,
            IChannelInterfaceRepository ciRepository,
            IMapper mapper,
            IGroupInterfaceRepository giRepository,
            ITypeEntityRepository teRepository, ILogger<InterfacesController> logger)
        {
            _teRepository = teRepository;
            _adapterRepository = adapterRepository;
            _ciRepository = ciRepository;
            _giRepository = giRepository;
            _mapper = mapper;
            _helper = helper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<InterfaceDto>))]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            return await _helper.Get();

            #region OldCode
            //var result = new List<InterfaceDto>();
            //var adapters = await _adapterRepository.GetAll();
            //adapters.ForEach(i => result.Add(_mapper.Map<InterfaceDto>(i)));
            //return Ok(result);
            #endregion
        }

        /// <summary>
        /// Restituisce le interfacce associate allo specifico gateway
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<InterfaceDto>))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetAllByGateway(string gatewayId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(gatewayId);
            if (gatewayId == null)
                return BadRequest(new InfoMsg(400, "Gateway id non valido"));
            return await _helper.GetFiltered(
                await _adapterRepository.GetAdaptersByGateway(gatewayId));

            #region OldCode
            //var result = new List<InterfaceDto>();
            //var adapters = await _adapterRepository.GetAdaptersByGateway(gatewayId);
            //adapters.ForEach(i => result.Add(_mapper.Map<InterfaceDto>(i)));
            //return Ok(result);
            #endregion
        }

        /// <summary>
        /// Resituisce le interfacce associate a uno specifico gruppo interfacce
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<InterfaceDto>))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetAllByGroup(string groupId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(groupId);
            if (groupId == null)
                return BadRequest(new InfoMsg(400, "Gruppo interfacce non valido"));
            return await _helper.GetFiltered(
                await _adapterRepository.GetAdatptersByGroup(groupId));

            #region OldCode
            //var result = new List<InterfaceDto>();
            //var adapters = await _adapterRepository.GetAdatptersByGroup(groupId);
            //adapters.ForEach(i => result.Add(_mapper.Map<InterfaceDto>(i)));
            //return Ok(result);
            #endregion
        }

        /// <summary>
        /// Restituisce le interfacce che si possono aggiungere allo specifico gruppo interfacce
        /// (action non valida se il gruppo è vuoto)
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<InterfaceDto>))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetAvailableAdaptersByGroup(string groupId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(groupId);
            if (groupId == null)
                return BadRequest(new InfoMsg(400, "Gruppo interfacce non valido"));
            return await _helper.GetFiltered(
                await _adapterRepository.GetAvailableAdaptersByGroup(groupId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(InterfaceDto))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(id);
            if (id == null)
                return BadRequest(new InfoMsg(400, "Id non valido"));
            return await _helper.Get(
                await _adapterRepository.GetById(id), 
                $"Non è stato trovato alcun adapter con id {id}");

            #region OldCode
            //var adapter = await _adapterRepository.GetById(id);
            //if (adapter == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovato alcun adapter con id {id}"));
            //var result = _mapper.Map<InterfaceDto>(adapter);
            //return Ok(result);
            #endregion
        }

        /// <summary>
        /// InterfaceId generato da server. Per aggiungere i canali autenticarsi
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg<InterfaceDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Post([FromBody] InterfaceDto value)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            var isValid = writeControl(value);
            if (isValid != null)
                return StatusCode(isValid.StatusCode, isValid);

            var partial = _helper.ModelStateControl(ModelState);
            if (partial != null)
                return StatusCode(partial.StatusCode, partial);
            var isPresent = _adapterRepository.CheckInterface(value.InterfaceId);
            if (isPresent != null)
            {
                _logger.LogWarning("422. Unprocessable entity", isPresent);
                return StatusCode(422, new InfoMsg(422, $"Interfaccia {value.InterfaceId} già presente. Impossibile inserire!"));
            }
            var newObj = _adapterRepository.Add(_mapper.Map<Ts400Interfacce>(value));
            if (newObj == null)
            {
                _logger.LogWarning($"500. Server error. Insert failed");
                return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nell'inserimento dell'interfaccia {newObj.IdInterfaccia}"));
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.Claims.Count() != 0)
            {
                var username = identity.FindFirst(ClaimTypes.Name).Value.Trim();
                _logger.LogTrace("User: ", username);
                bool initDetail = _ciRepository
                    .InitChannelDetail(newObj.IdInterfaccia, value.TypologyInterface, username).Result;
                bool initVariable = _ciRepository
                    .InitChannelVariable(newObj.IdInterfaccia, value.TypologyInterface, username).Result;
                bool confirm = _ciRepository.ConfirmChannelInterfaces(newObj.IdInterfaccia, username).Result;
                if (!(initDetail && initVariable && confirm))
                    return StatusCode(500, new InfoMsg(500, $"Aggiunta interfacce canali non andata a buon fine"));
            }

            var response = _mapper.Map<InterfaceDto>(newObj);
            _logger.LogTrace("Returned item: ", response);
            return Ok(new InfoMsg<InterfaceDto>(response,
                200, $"Interfaccia {value.InterfaceId} aggiunta con successo"));
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Put([FromBody] InterfaceDto value)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            var isValid = writeControl(value);
            if (isValid != null)
                return StatusCode(isValid.StatusCode, isValid);

            return _helper.Put(
                req: value,
                isPresent: _adapterRepository.CheckInterface(value.InterfaceId),
                mState: ModelState,
                msg422: $"Interfaccia {value.InterfaceId} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica dell'interfaccia {value.InterfaceId}",
                msg200: $"Interfaccia {value.InterfaceId} modificata con successo");

            #region OldCode
            //if (value == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati dell'interfaccia"));
            //var isPresent = _adapterRepository.CheckInterface(value.InterfaceId);
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
            //    return StatusCode(422, new InfoMsg(422, $"Interfaccia {value.InterfaceId} non presente. Impossibile modificare!"));
            //#region prima del base
            //var presentGroup = _giRepository.CheckGroup(value.InterfaceGroupId);
            //if (presentGroup == null && !string.IsNullOrEmpty(value.InterfaceGroupId))
            //    return StatusCode(422, new InfoMsg(422, $"Gruppo interfacce {value.InterfaceGroupId} non presente. Impossibile modificare!"));
            //#endregion
            //if (!_adapterRepository.Update(_mapper.Map<Ts400Interfacce>(value)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nella modifica dell'interfaccia {value.InterfaceId}"));
            //return Ok(new InfoMsg(200, $"Interfaccia {value.InterfaceId} modificata con successo"));
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
               isPresent: _adapterRepository.CheckInterface(id),
               msg404: $"Non è stato trovato alcun interfaccia {id}",
               msg500: $"Ci sono stati problemi nell'eliminazione dell'interfaccia {id}",
               msg200: $"Interfaccia {id} eliminata con successo");

            #region OldCode
            //var value = _adapterRepository.CheckInterface(id);
            //if (value == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovato alcun interfaccia {id}"));
            //if (!_adapterRepository.Delete(value))
            //    return StatusCode(500, $"Ci sono stati problemi nell'eliminazione dell'interfaccia {id}");
            //return Ok(new InfoMsg(200, $"Interfaccia {id} eliminata con successo"));
            #endregion
        }

        private InfoMsg writeControl(InterfaceDto value)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(value);
            if (value == null)
                return new InfoMsg(400, "E' necessario inserire i dati dell'interfaccia");
            var presentEntity = _giRepository.CheckGroup(value.InterfaceGroupId);
            if (presentEntity == null && !string.IsNullOrEmpty(value.InterfaceGroupId))
                return new InfoMsg(400, $"Gruppo interfacce {value.InterfaceGroupId} non presente");
            if (!string.IsNullOrEmpty(value.InterfaceGroupId))
                value.InterfaceGroupDescription = presentEntity.DesGruppoInterfacce;

            var presentType = _teRepository.CheckTypeEntity(value.TypologyInterface);
            if (presentType == null || presentType.Entita.Trim().ToUpper() != "INTERFACCIA")
                return new InfoMsg(400, $"Tipologia {value.TypologyInterface} non presente");

            if (!string.IsNullOrEmpty(value.InterfaceGroupId))
            {
                string requiredType = _adapterRepository.GetTypeByGroup(value.InterfaceGroupId);
                if (requiredType != null && !requiredType.Equals(value.TypologyInterface.Trim().ToUpper()))
                    return new InfoMsg(400, "Tipologia non associata al gruppo interfacce");
            }

            return null;
        }
    }
}
