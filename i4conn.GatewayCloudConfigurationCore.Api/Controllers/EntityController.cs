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
    /// Entità - famiglie
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EntityController : ControllerBase
    {
        private readonly ILogger<EntityController> _logger;
        private readonly IControllerHelper<EntityDto, IEntityRepository, Ts400Entitum> _helper;
        private readonly IEntityRepository _entityRepository;
        // private readonly IMapper _mapper;

        public EntityController(
            IEntityRepository entityRepository,
            //IMapper mapper,
            IControllerHelper<EntityDto, IEntityRepository, Ts400Entitum> helper, ILogger<EntityController> logger)
        {
            _helper = helper;
            _entityRepository = entityRepository;
            _logger = logger;
            // _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<EntityDto>))]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            return await _helper.Get();

            #region OldCode
            //var result = new List<EntityDto>();
            //var entities = await _entityRepository.GetAll();
            //entities.ForEach(g => result.Add(_mapper.Map<EntityDto>(g)));
            //return Ok(result);
            #endregion
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Post([FromBody] EntityDto entity)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(entity);
            if (entity == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati dell'entità"));
            return _helper.PostReturnBool(
                req: entity,
                isPresent: _entityRepository.CheckEntita(entity.Name),
                mState: ModelState,
                msg422: $"Entità {entity.Name} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento dell'entità {entity.Name}",
                msg200: $"Entità {entity.Name} inserita con successo");

            #region OldCode
            //if (entity == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati dell'entità"));
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
            //var isPresent = _entityRepository.CheckEntita(entity.Name);
            //if (isPresent != null)
            //    return StatusCode(422, new InfoMsg(422, $"Entità {entity.Name} già presente. Impossibile inserire!"));
            //if (!_entityRepository.Insert(_mapper.Map<Ts400Entitum>(entity)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nell'inserimento dell'entità {entity.Name}"));
            //return Ok(new InfoMsg(200, $"Entità {entity.Name} inserita con successo"));
            #endregion
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Put([FromBody] EntityDto entity)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(entity);
            if (entity == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati dell'entità"));
            return _helper.Put(
                req: entity,
                isPresent: _entityRepository.CheckEntita(entity.Name),
                mState: ModelState,
                msg422: $"Entità {entity.Name} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica dell'entità {entity.Name}",
                msg200: $"Entità {entity.Name} modificata con successo");

            #region OldCode
            //if (entity == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati dell'entità"));
            //var isPresent = _entityRepository.CheckEntita(entity.Name);
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
            //    return StatusCode(422, new InfoMsg(422, $"Entità {entity.Name} non presente. Impossibile modificare!"));
            //if (!_entityRepository.Update(_mapper.Map<Ts400Entitum>(entity)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nella modifica dell'entità {entity.Name}"));
            //return Ok(new InfoMsg(200, $"Entità {entity.Name} modificata con successo"));
            #endregion
        }

        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        public IActionResult Delete(string name)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(name);
            if (name == null)
                return BadRequest(new InfoMsg(400, "Nome non valido"));
            return _helper.Delete(
                isPresent: _entityRepository.CheckEntita(name),
                msg404: $"Non è stato trovata alcuna entità con nome {name}",
                msg500: $"Ci sono stati problemi nell'eliminazione dell'entità {name}",
                msg200: $"Entità {name} eliminata con successo");

            #region OldCode
            //var entity = _entityRepository.CheckEntita(name);
            //if (entity == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovata alcuna entità con nome {name}"));
            //if (!_entityRepository.Delete(entity))
            //    return StatusCode(500, $"Ci sono stati problemi nell'eliminazione dell'entità {name}");
            //return Ok(new InfoMsg(200, $"Entità {name} eliminata con successo"));
            #endregion
        }
    }
}
