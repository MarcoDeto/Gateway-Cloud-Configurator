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
    /// Tipologie di canale
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChannelTypeController : ControllerBase
    {
        private readonly ILogger<ChannelTypeController> _logger;
        private readonly IControllerHelper<ChannelTypeDto, IChannelTypeRepository, Ts400TipiCanale> _helper;
        private readonly IChannelTypeRepository _repository;
        //private readonly IMapper _mapper;

        public ChannelTypeController(
            ILogger<ChannelTypeController> logger,
            IControllerHelper<ChannelTypeDto, IChannelTypeRepository, Ts400TipiCanale> helper,
            //IMapper mapper,
            IChannelTypeRepository repository)
        {
            _logger = logger;
            _repository = repository;
            //_mapper = mapper;
            _helper = helper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ChannelTypeDto>))]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            return await _helper.Get();

            #region OldCode
            //var result = new List<ChannelTypeDto>();
            //var entities = await _repository.GetAll();
            //entities.ForEach(g => result.Add(_mapper.Map<ChannelTypeDto>(g)));
            //return Ok(result);
            #endregion
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ChannelTypeDto))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public async Task<IActionResult> GetByType(string type)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(type);
            if (type == null)
                return BadRequest(new InfoMsg(400, "Tipologia non valida"));
            return await _helper.Get(
                await _repository.GetByType(type), 
                $"Non è stato trovato alcun tipo canale {type}");

            #region OldCode
            //var result = await _repository.GetByType(type);
            //if (result == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovato alcun tipo canale {type}"));
            //var response = _mapper.Map<ChannelTypeDto>(result);
            //return Ok(response);
            #endregion
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Post([FromBody] ChannelTypeDto entity)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(entity);
            if (entity == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del tipo canale"));
            return _helper.PostReturnBool(
                req: entity,
                isPresent: _repository.CheckChannelType(entity.Type),
                mState: ModelState,
                msg422: $"Tipo canale {entity.Type} già presente. Impossibile inserire!",
                msg500: $"Ci sono stati problemi nell'inserimento del tipo canale {entity.Type}",
                msg200: $"Tipo canale {entity.Type} inserito con successo");

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
            //var isPresent = _repository.CheckChannelType(entity.Type);
            //if (isPresent != null)
            //    return StatusCode(422, new InfoMsg(422, $"Tpo canale {entity.Type} già presente. Impossibile inserire!"));
            //if (!_repository.Insert(_mapper.Map<Ts400TipiCanale>(entity)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nell'inserimento del tipo canale {entity.Type}"));
            //return Ok(new InfoMsg(200, $"Tipo canale {entity.Type} inserito con successo"));
            #endregion
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public IActionResult Put([FromBody] ChannelTypeDto entity)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(entity);
            if (entity == null)
                return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del tipo canale"));
            return _helper.Put(
                req: entity,
                isPresent: _repository.CheckChannelType(entity.Type),
                mState: ModelState,
                msg422: $"Tipo canale {entity.Type} non presente. Impossibile modificare!",
                msg500: $"Ci sono stati problemi nella modifica del tipo canale {entity.Type}",
                msg200: $"Tipo canale {entity.Type} modificato con successo");

            #region OldCode
            //if (entity == null)
            //    return BadRequest(new InfoMsg(400, "E' necessario inserire i dati del gateway"));
            //var isPresent = _repository.CheckChannelType(entity.Type);
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
            //    return StatusCode(422, new InfoMsg(422, $"Tipo canale {entity.Type} non presente. Impossibile modificare!"));
            //if (!_repository.Update(_mapper.Map<Ts400TipiCanale>(entity)))
            //    return StatusCode(500, new InfoMsg(500, $"Ci sono stati problemi nella modifica del tipo canale {entity.Type}"));
            //return Ok(new InfoMsg(200, $"Tipo canale {entity.Type} modificato con successo"));
            #endregion
        }

        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(InfoMsg))]
        [ProducesResponseType(404, Type = typeof(InfoMsg))]
        [ProducesResponseType(400, Type = typeof(InfoMsg))]
        public IActionResult Delete(string type)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(type);
            if (type == null)
                return BadRequest(new InfoMsg(400, "Tipologia non valida"));
            return _helper.Delete(
                isPresent: _repository.CheckChannelType(type),
                msg404: $"Non è stato trovata alcuna tipo canale {type}",
                msg500: $"Ci sono stati problemi nell'eliminazione del tipo canale {type}",
                msg200: $"Tipo canale {type} eliminata con successo");

            #region OldCode
            //var entity = _repository.CheckChannelType(type);
            //if (entity == null)
            //    return NotFound(new InfoMsg(404, $"Non è stato trovata alcuna tipo canale {type}"));
            //if (!_repository.Delete(entity))
            //    return StatusCode(500, $"Ci sono stati problemi nell'eliminazione del tipo canale {type}");
            //return Ok(new InfoMsg(200, $"Tipo canale {type} eliminata con successo"));
            #endregion
        }
    }
}
