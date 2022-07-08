using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.Api.Helpers;
using i4conn.GatewayCloudConfigurationCore.Api.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Helpers;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models;
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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EntityParamValueController : ControllerBase
    {
        private readonly ILogger<EntityParamValueController> _logger;
        private readonly IControllerHelper<EntityParam,
            IEntityParamValueRepository,
            Ts400ParamEntValori> _helper;
        private readonly IEntityParamValueRepository _repository;
        private readonly IEntityParamRegistryRepository _registryRepository;
        private readonly IAdapterRepository _adapterRepository;
        private readonly IMapper _mapper;

        public EntityParamValueController(
            IControllerHelper<EntityParam, IEntityParamValueRepository, Ts400ParamEntValori> helper,
            IEntityParamValueRepository repository,
            IEntityParamRegistryRepository registryRepository,
            IAdapterRepository adapterRepository,
            IMapper mapper, ILogger<EntityParamValueController> logger)
        {
            _helper = helper;
            _repository = repository;
            _mapper = mapper;
            _registryRepository = registryRepository;
            _adapterRepository = adapterRepository;
            _logger = logger;
        }

        /// <summary>
        /// Restituisce i parametri di una specifica interfaccia
        /// </summary>
        /// <param name="interfaceId"></param>
        /// <returns></returns>
        [HttpGet("{interfaceId}")]
        [ProducesResponseType(200, Type = typeof(List<EntityParam>))]
        public async Task<IActionResult> GetInterfaceParams(string interfaceId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId);
            if (interfaceId == null)
                return BadRequest(new InfoMsg(400, "Id interfaccia non valida"));
            var result = await _repository.GetInterfaceParams(interfaceId);
            result.ForEach(r => r.EntityId = r.EntityId ?? interfaceId);
            _logger.LogTrace("Returned items: ", result.Count());
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceId"></param>
        /// <param name="direction"></param>
        /// <param name="virtualCh"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<EntityParam>))]
        public async Task<IActionResult> GetRuleParams(string interfaceId, string direction, string virtualCh)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, direction, virtualCh);
            if (interfaceId == null || direction == null || virtualCh == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire l'interfaccia, la direzione e il canale virtuale"));
            return await _helper.GetFiltered(await _repository.GetRuleParams(interfaceId, direction, virtualCh));
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(EntityParam))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetParam(string paramName, string entity, string entityId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(paramName, entity, entityId);
            if (paramName == null || entity == null || entityId == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire il nome del parametro, l'entità e l'id dell'entità"));
            return await _helper.Get(
                await _repository.GetEntityParam(entity, entityId, paramName),
                $"Non è stato trovata alcun parametro con nome {paramName}");
        }

        #region GetParamsByEntity
        /// <summary>
        /// Restituisce tutti i parametri in base all'entità (per le interfacce usare GetInterfaceParams)
        /// </summary>
        /// <param name="entity">Es. "regola"</param>
        /// <returns></returns>
        //[HttpGet("{entity}")]
        //[ProducesResponseType(200, Type = typeof(List<EntityParam>))]
        //public async Task<IActionResult> GetParamsByEntity(string entity)
        //{
        //    if (entity == null)
        //        return BadRequest(new InfoMsg(400, "Entità non valida"));
        //    return await _helper.GetFiltered(await _repository.GetParamsByEntity(entity));
        //}
        #endregion

        /// <summary>
        /// Aggiunta di una regola passando anche interfaccia, direzione e canale virtuale 
        /// (EntityId viene generato da server in base a questi 3 parametri)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(EntityParam))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult PostRule([FromBody] RuleRequest req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            if (req == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del parametro"));
            var interfaceObj = _adapterRepository.CheckInterface(req.InterfaceId);
            if (interfaceObj == null)
                return BadRequest(new InfoMsg(400, $"Interfaccia {req.InterfaceId} non presente"));
            req.Content.EntityId = _repository.CreateId(req.InterfaceId, req.Direction, req.VirtualCh);
            _logger.LogTrace("Entity id created: " + req.Content.EntityId);

            return _helper.PostReturnObj(
                req: req.Content,
                isPresent: _repository.CheckEntityParam(req.Content.Entity, req.Content.EntityId, req.Content.ParamName),
                mState: ModelState,
                msg422: $"Parametro {req.Content.ParamName} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento del parametro {req.Content.ParamName}",
                msg200: $"Parametro {req.Content.ParamName} inserito con successo");
        }

        #region Post
        /// <summary>
        /// Aggiunta di un parametro (per le interfacce usare PostInterfaceParam)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ProducesResponseType(200, Type = typeof(InfoMsg))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        //[ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        //public IActionResult Post([FromBody] EntityParam req)
        //{
        //    if (req == null)
        //        return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del parametro"));

        //    return _helper.PostReturnBool(
        //        req: req,
        //        isPresent: _repository.CheckEntityParam(req.Entity, req.EntityId, req.ParamName),
        //        mState: ModelState,
        //        msg422: $"Parametro {req.ParamName} già presente. Impossibile inserire!",
        //        msg500: $"Ci sono stati problemi nell'inserimento del parametro {req.ParamName}",
        //        msg200: $"Parametro {req.ParamName} inserito con successo");
        //}
        #endregion

        /// <summary>
        /// Se il parametro è già presente lo modifica, altrimenti ne inserisce uno nuovo (vale solo per le ineterfacce)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult PostInterfaceParam([FromBody] EntityParam req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            if (req == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del parametro"));
            var interfaceObj = _adapterRepository.CheckInterface(req.EntityId);
            _logger.LogTrace("Interface is null: ", interfaceObj == null);
            if (interfaceObj == null)
                return BadRequest(new InfoMsg(400, $"Interfaccia {req.EntityId} non presente"));
            string type = interfaceObj.InterfacciaContapezzi;
            _logger.LogTrace("Type: " + type);
            bool control = _registryRepository.GetAllByType(type).Result.Any(p => p.ParamNome == req.ParamName);
            _logger.LogTrace("Parametro assegnato alla tipologia: " + control);
            if (!control)
                return BadRequest(new InfoMsg(400, $"Parametro non assegnato alla tipologia {type.Trim()}"));

            var partial = _helper.ModelStateControl(ModelState);
            if (partial != null)
                return StatusCode(partial.StatusCode, partial);
            var isPresent = _repository.CheckEntityParam(req.Entity, req.EntityId, req.ParamName);
            if (isPresent != null)
            {
                if (!_repository.Update(_mapper.Map<Ts400ParamEntValori>(req)))
                    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nella modifica del parametro {req.ParamName}"));
                return Ok(new InfoMsg(200, $"Parametro {req.ParamName} modificato con successo"));
            } 
            else
            {
                if (!_repository.Insert(_mapper.Map<Ts400ParamEntValori>(req)))
                    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nell'inserimento del parametro {req.ParamName}"));
                return Ok(new InfoMsg(200, $"Parametro {req.ParamName} inserito con successo"));
            }
        }

        /// <summary>
        /// Modifica di una regola passando anche interfaccia, direzione e canale virtuale
        /// (EntityId viene generato da server in base a questi 3 parametri e si può modificare solo il valore)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult PutRule([FromBody] RuleRequest req)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(req);
            if (req == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del parametro"));
            var interfaceObj = _adapterRepository.CheckInterface(req.InterfaceId);
            if (interfaceObj == null)
                return BadRequest(new InfoMsg(400, $"Interfaccia {req.InterfaceId} non presente"));
            req.Content.EntityId = _repository.CreateId(req.InterfaceId, req.Direction, req.VirtualCh);
            _logger.LogTrace("Entity id created: " + req.Content.EntityId);

            return _helper.Put(
               req: req.Content,
               isPresent: _repository.CheckEntityParam(req.Content.Entity, req.Content.EntityId, req.Content.ParamName),
               mState: ModelState,
               msg422: $"Parametro {req.Content.ParamName} non presente. Impossibile modificare!",
               msg500: $"Ci sono stati problemi nella modifica del parametro {req.Content.ParamName}",
               msg200: $"Parametro {req.Content.ParamName} modificato con successo");
        }

        #region Put
        /// <summary>
        /// Modifica di un parametro (per le interfacce usare PostInterfaceParam)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //[HttpPut]
        //[ProducesResponseType(200, Type = typeof(InfoMsg))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        //[ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        //public IActionResult Put([FromBody] EntityParam req)
        //{
        //    if (req == null)
        //        return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del parametro"));

        //    return _helper.Put(
        //        req: req,
        //        isPresent: _repository.CheckEntityParam(req.Entity, req.EntityId, req.ParamName),
        //        mState: ModelState,
        //        msg422: $"Parametro {req.ParamName} non presente. Impossibile modificare!",
        //        msg500: $"Ci sono stati problemi nella modifica del parametro {req.ParamName}",
        //        msg200: $"Parametro {req.ParamName} modificato con successo");
        //}
        #endregion

        /// <summary>
        /// Eliminazione del parametro. Da usare anche se l'utente imposta il valore di default per i parametri delle
        /// interfacce
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public IActionResult Delete(string paramName, string entity, string entityId)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(paramName, entity, entityId);
            if (paramName == null || entity == null || entityId == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire il nome del parametro, l'entità e l'id dell'entità"));
            return _helper.Delete(
                isPresent: _repository.CheckEntityParam(entity, entityId, paramName),
                msg404: $"Non è stato trovata alcun parametro con nome {paramName}",
                msg500: $"Ci sono stati problemi nell'eliminazione del parametro {paramName}",
                msg200: $"Parametro {paramName} eliminato con successo");
        }
    }
}
