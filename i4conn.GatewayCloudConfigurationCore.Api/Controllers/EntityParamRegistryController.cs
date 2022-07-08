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
    /// Configurazione parametri entità
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EntityParamRegistryController : ControllerBase
    {
        private readonly ILogger<EntityParamRegistryController> _logger;
        private readonly IControllerHelper<EntityParamRegistryDto, 
            IEntityParamRegistryRepository, 
            Ts400ParamEntAnagr> _helper;
        private readonly IEntityParamRegistryRepository _repository;
        private readonly ITypeEntityRepository _teRepository;
        private readonly IEntityRepository _entityRepository;

        public EntityParamRegistryController(
            IControllerHelper<EntityParamRegistryDto, IEntityParamRegistryRepository, Ts400ParamEntAnagr> helper,
            IEntityParamRegistryRepository repository,
            ITypeEntityRepository teRepository,
            IEntityRepository entityRepository, ILogger<EntityParamRegistryController> logger)
        {
            _helper = helper;
            _repository = repository;
            _teRepository = teRepository;
            _entityRepository = entityRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<EntityParamRegistryDto>))]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            return await _helper.Get();

            #region OldCode
            //var res = new List<EntityParamRegistryDto>();
            //var result = await _repository.GetAll();
            //result.ForEach(g => res.Add(_mapper.Map<EntityParamRegistryDto>(g)));
            //return Ok(res);
            #endregion
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(EntityParamRegistryDto))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetByName(string paramName, string entity, string type)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(paramName, entity, type);
            if (paramName == null || entity == null || type == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire il nome del parametro, l'entità e la tipologia"));
            return await _helper.Get(
                await _repository.GetParam(paramName, entity, type), 
                $"Non è stato trovato alcun parametro {paramName}");

            #region OldCode
            //var typeEntity = await _repository.GetByName(name);
            //if (typeEntity == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovato alcun parametro {name}"));
            //var result = _mapper.Map<EntityParamRegistryDto>(typeEntity);
            //return Ok(result);
            #endregion
        }

        /// <summary>
        /// Restituisce i parametri in base all'entità
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<EntityParamRegistryDto>))]
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
            //var result = new List<EntityParamRegistryDto>();
            //var typeEntities = await _repository.GetAllByEntity(entity);
            //if (typeEntities.Count() == 0)
            //    return NotFound(new InfoMsg(404, $"Entità {entity} non presente"));
            //typeEntities.ForEach(g => result.Add(_mapper.Map<EntityParamRegistryDto>(g)));
            //return Ok(result);
            #endregion
        }

        /// <summary>
        /// Restituisce i parametri in base alla tipologia
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<EntityParamRegistryDto>))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetByType(string type)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(type);
            if (type == null)
                return BadRequest(new InfoMsg(400, "Tipologia non valida"));
            return await _helper.GetFiltered(
                await _repository.GetAllByType(type));

            #region OldCode
            //var result = new List<EntityParamRegistryDto>();
            //var typeEntities = await _repository.GetAllByType(type);
            //if (typeEntities.Count() == 0)
            //    return NotFound(new InfoMsg(404, $"Tipologia {type} non presente"));
            //typeEntities.ForEach(g => result.Add(_mapper.Map<EntityParamRegistryDto>(g)));
            //return Ok(result);
            #endregion
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Post([FromBody] EntityParamRegistryDto req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            var isValid = writeControl(req);
            if (isValid != null)
                return StatusCode(isValid.StatusCode, isValid);

            return _helper.PostReturnBool(
                req,
                isPresent: _repository.CheckParam(req.ParamName, req.Entity, req.Type),
                mState: ModelState,
                msg422: $"Parametro {req.ParamName} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento del parametro {req.ParamName}",
                msg200: $"Parametro {req.ParamName} inserito con successo");

            #region OldCode
            //if (req == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del parametro"));
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
            //var isPresent = _repository.CheckParam(req.ParamName);
            //if (isPresent != null)
            //    return StatusCode(422, new InfoMsg(422, $"Parametro {req.ParamName} già presente. Impossibile inserire!"));
            //#region prima del base
            //var presentEntity = _entityRepository.CheckEntita(req.Entity);
            //if (presentEntity == null)
            //    return StatusCode(422, new InfoMsg(422, $"Entità {req.Entity} non presente. Impossibile inserire!"));
            //var presentType = _teRepository.CheckTypeEntity(req.Type);
            //if (presentType == null)
            //    return StatusCode(422, new InfoMsg(422, $"Tipologia {req.Entity} non presente. Impossibile inserire!"));
            //#endregion
            //if (!_repository.Insert(_mapper.Map<Ts400ParamEntAnagr>(req)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nell'inserimento del parametro {req.ParamName}"));
            //return Ok(new InfoMsg(200, $"Parametro {req.ParamName} inserito con successo"));
            #endregion
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Put([FromBody] EntityParamRegistryDto req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            var isValid = writeControl(req);
            if (isValid != null)
                return StatusCode(isValid.StatusCode, isValid);

            return _helper.Put(
                req,
                isPresent: _repository.CheckParam(req.ParamName, req.Entity, req.Type),
                mState: ModelState,
                msg422: $"Parametro {req.ParamName} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica del parametro {req.ParamName}",
                msg200: $"Prametro {req.ParamName} modificato con successo");

            #region OldCode
            //if (req == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del parametro"));
            //var isPresent = _repository.CheckParam(req.ParamName);
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
            //    return StatusCode(422, new InfoMsg(422, $"Parametro {req.ParamName} non presente. Impossibile modificare!"));
            //#region prima del base
            //var presentEntity = _entityRepository.CheckEntita(req.Entity);
            //if (presentEntity == null)
            //    return StatusCode(422, new InfoMsg(422, $"Entità {req.Entity} non presente. Impossibile inserire!"));
            //var presentType = _teRepository.CheckTypeEntity(req.Type);
            //if (presentType == null)
            //    return StatusCode(422, new InfoMsg(422, $"Tipologia {req.Entity} non presente. Impossibile inserire!"));
            //#endregion
            //if (!_repository.Update(_mapper.Map<Ts400ParamEntAnagr>(req)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nella modifica del parametro {req.ParamName}"));
            //return Ok(new InfoMsg(200, $"Prametro {req.ParamName} modificato con successo"));
            #endregion
        }

        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public IActionResult Delete(string paramName, string entity, string type)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(paramName, entity, type);
            if (paramName == null || entity == null || type == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire il nome del parametro, l'entità e la tipologia"));
            return _helper.Delete(
                isPresent: _repository.CheckParam(paramName, entity, type),
                msg404: $"Non è stato trovata alcun parametro con nome {paramName}",
                msg500: $"Ci sono stati problemi nell'eliminazione del parametro {paramName}",
                msg200: $"Parametro {paramName} eliminato con successo");

            #region OldCode
            //var result = _repository.CheckParam(name);
            //if (result == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovata alcun parametro con nome {name}"));
            //if (!_repository.Delete(result))
            //    return StatusCode(500, $"Ci sono stati problemi nell'eliminazione del parametro {name}");
            //return Ok(new InfoMsg(200, $"Parametro {name} eliminato con successo"));
            #endregion
        }

        private InfoMsg writeControl(EntityParamRegistryDto req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            if (req == null)
                return new InfoMsg(400, "E' necessario inserire i dati del parametro");
            var presentEntity = _entityRepository.CheckEntita(req.Entity);
            if (presentEntity == null)
                return new InfoMsg(400, $"Entità {req.Entity} non presente");
            var presentType = _teRepository.CheckTypeEntity(req.Type);
            if (presentType == null && !string.IsNullOrEmpty(req.Type))
                return new InfoMsg(400, $"Tipologia {req.Type} non presente");
            return null;
        }
    }
}
