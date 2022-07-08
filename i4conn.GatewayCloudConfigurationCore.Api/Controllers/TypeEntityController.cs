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
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    /// <summary>
    /// Entità - tipologie
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TypeEntityController : ControllerBase
    {
        private readonly ILogger<TypeEntityController> _logger;
        private readonly IControllerHelper<TypeEntityDto, ITypeEntityRepository, Ts400EntitaTipologium> _helper;
        private readonly ITypeEntityRepository _repository;
        private readonly IChannelInterfaceRepository _ciRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly IMapper _mapper;

        public TypeEntityController(
            IControllerHelper<TypeEntityDto, ITypeEntityRepository, Ts400EntitaTipologium> helper,
            ITypeEntityRepository repository,
            IChannelInterfaceRepository ciRepository,
            IEntityRepository entityRepository,
            IMapper mapper,
            ILogger<TypeEntityController> logger)
        {
            _logger = logger;
            _ciRepository = ciRepository;
            _repository = repository;
            _entityRepository = entityRepository;
            _mapper = mapper;
            _helper = helper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TypeEntityDto>))]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            return await _helper.Get();

            #region OldCode
            //var result = new List<TypeEntityDto>();
            //var typeEntities = await _repository.GetAll();
            //typeEntities.ForEach(g => result.Add(_mapper.Map<TypeEntityDto>(g)));
            //return Ok(result);
            #endregion
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(TypeEntityDto))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(id);
            if (id == null)
                return BadRequest(new InfoMsg(400, "Id non valido"));
            return await _helper.Get(
                await _repository.GetById(id), 
                $"Non è stato trovato alcuna entità tipologia con id {id}");

            #region OldCode
            //var typeEntity = await _repository.GetById(id);
            //if (typeEntity == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovato alcuna entità tipologia con id {id}"));
            //var result = _mapper.Map<TypeEntityDto>(typeEntity);
            //return Ok(result);
            #endregion
        }

        /// <summary>
        /// Restituisce tutte le tipologie di una specifica entità
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TypeEntityDto>))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetByEntity(string entity)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(entity);
            if (entity == null)
                return BadRequest(new InfoMsg(400, "Entità non valida"));
            return await _helper.GetFiltered(
                await _repository.GetAllByEntity(entity));

            #region OldCode
            //var result = new List<TypeEntityDto>();
            //var typeEntities = await _repository.GetAllByEntity(entity);
            //if (typeEntities.Count() == 0)
            //    return NotFound(new InfoMsg(404, $"Entità {entity} non presente"));
            //typeEntities.ForEach(g => result.Add(_mapper.Map<TypeEntityDto>(g)));
            //return Ok(result);
            #endregion
        }

        /// <summary>
        /// Numero di ingressi e di uscite per tipologia di interfaccia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IONumberResponse))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> TypeInterfacesInputOutputNumber(string id)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(id);
            if (id == null)
                return BadRequest(new InfoMsg(400, "Id non valido"));
            var typeEntity = await _repository.GetById(id);
            if (typeEntity == null)
                return NotFound(new InfoMsg(404, $"Non è stato trovato alcuna tipologia interfaccia con id {id}"));
            if (typeEntity.Entita.Trim().ToUpper() != "INTERFACCIA")
                return BadRequest(new InfoMsg(400, $"Tipologia interfaccia non valida"));
            var obj = await _ciRepository.TypeInterfacesInputOutputNumber(id);
            _logger.LogTrace("Returned item: ", obj);
            return Ok(obj);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Post([FromBody] TypeEntityDto entity)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(entity);
            if (entity == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati dell'entità tipologia"));
            var presentEntity = _entityRepository.CheckEntita(entity.Entity);
            if (presentEntity == null)
                return BadRequest(new InfoMsg(400, $"Entità {entity.Entity} non presente"));

            return _helper.PostReturnBool(
                req: entity,
                isPresent: _repository.CheckTypeEntity(entity.Id),
                mState: ModelState,
                msg422: $"Entità tipologia {entity.Id} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento dell'entità tipologia {entity.Id}",
                msg200: $"Entità tipologia {entity.Id} inserita con successo");

            #region OldCode
            //if (entity == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gateway"));
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
            //var isPresent = _repository.CheckTypeEntity(entity.Id);
            //if (isPresent != null)
            //    return StatusCode(422, new InfoMsg(422, $"Entità tipologia {entity.Id} già presente. Impossibile inserire!"));
            //#region prima del base
            //var presentEntity = _entityRepository.CheckEntita(entity.Entity);
            //if (presentEntity == null)
            //    return StatusCode(422, new InfoMsg(422, $"Entità {entity.Entity} non presente. Impossibile inserire!"));
            //#endregion
            //if (!_repository.Insert(_mapper.Map<Ts400EntitaTipologium>(entity)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nell'inserimento dell'entità tipologia {entity.Id}"));
            //return Ok(new InfoMsg(200, $"Entità tipologia {entity.Id} inserita con successo"));
            #endregion
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Put([FromBody] TypeEntityDto entity)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(entity);
            if (entity == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati dell'entità tipologia"));
            var presentEntity = _entityRepository.CheckEntita(entity.Entity);
            if (presentEntity == null)
                return BadRequest(new InfoMsg(400, $"Entità {entity.Entity} non presente"));

            return _helper.Put(
                req: entity,
                isPresent: _repository.CheckTypeEntity(entity.Id),
                mState: ModelState,
                msg422: $"Entità tipologia {entity.Id} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica dell'entità tipologia {entity.Id}",
                msg200: $"Entità tipologia {entity.Id} modificata con successo");

            #region OldCode
            //if (entity == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gateway"));
            //var isPresent = _repository.CheckTypeEntity(entity.Id);
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
            //    return StatusCode(422, new InfoMsg(422, $"Entità tipologia {entity.Id} non presente. Impossibile modificare!"));
            //#region prima del base
            //var presentEntity = _entityRepository.CheckEntita(entity.Entity);
            //if (presentEntity == null)
            //    return StatusCode(422, new InfoMsg(422, $"Entità {entity.Entity} non presente. Impossibile inserire!"));
            //#endregion
            //if (!_repository.Update(_mapper.Map<Ts400EntitaTipologium>(entity)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nella modifica dell'entità tipologia {entity.Id}"));
            //return Ok(new InfoMsg(200, $"Entità tipologia {entity.Id} modificata con successo"));
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
               isPresent: _repository.CheckTypeEntity(id),
               msg404: $"Non è stato trovata alcuna entità tipologia con nome {id}",
               msg500: $"Ci sono stati problemi nell'eliminazione dell'entità tipologia con id {id}",
               msg200: $"Entità tipologia {id} eliminata con successo");

            #region OldCode
            //var entity = _repository.CheckTypeEntity(id);
            //if (entity == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovata alcuna entità tipologia con nome {id}"));
            //if (!_repository.Delete(entity))
            //    return StatusCode(500, $"Ci sono stati problemi nell'eliminazione dell'entità tipologia con id {id}");
            //return Ok(new InfoMsg(200, $"Entità tipologia {id} eliminata con successo"));
            #endregion
        }
    }
}
