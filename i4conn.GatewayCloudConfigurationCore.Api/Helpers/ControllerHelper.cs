using AutoMapper;
using i4conn.GatewayCloudConfigurationCore.Api.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Helpers;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.Api.Helpers
{
    public class ControllerHelper<T, U, V> : ControllerBase, IControllerHelper<T, U, V>
        where T : class
        where V : class
        where U : IBaseRepository<V>
    {
        private readonly U _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ControllerHelper<T, U, V>> _logger;

        public ControllerHelper(U repository, IMapper mapper, ILogger<ControllerHelper<T, U, V>> logger)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _logger.LogDebug("ctor");
        }

        public async Task<IActionResult> Get()
        {
            _logger.LogDebug("Get (list)");
            var res = new List<T>();
            var result = await _repository.GetAll();
            result.ForEach(r => res.Add(_mapper.Map<T>(r)));
            _logger.LogTrace($"200. Returned items: {res.Count()}");
            return Ok(res);
        }

        public async Task<IActionResult> Get(
            V isPresent,
            string msg404)
        {
            _logger.LogDebug("Get (item)");
            if (isPresent == null)
            {
                _logger.LogTrace("404. Not found");
                return NotFound(new InfoMsg(404, msg404));
            }
            var result = _mapper.Map<T>(isPresent);
            _logger.LogTrace($"200. Returned item", result);
            return Ok(result);
        }

        public async Task<IActionResult> GetFiltered(List<V> filteredList)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            var result = new List<T>();
            if (filteredList.Count() != 0)
                filteredList.ForEach(g => result.Add(_mapper.Map<T>(g)));
            _logger.LogTrace($"200. Returned items: {result.Count()}");
            return Ok(result);
        }

        public IActionResult PostReturnBool(T req,
            V isPresent,
            ModelStateDictionary mState,
            string msg422,
            string msg500,
            string msg200)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            _logger.LogTrace("Request: ", req);
            var partial = ModelStateControl(mState);
            if (partial != null) 
                return StatusCode(partial.StatusCode, partial);
            if (isPresent != null)
            {
                _logger.LogWarning("422. Unprocessable entity", isPresent);
                return StatusCode(422, new InfoMsg(422, msg422));
            }
            var obj = _mapper.Map<V>(req);
            if (!_repository.Insert(obj))
            {
                _logger.LogWarning($"500. Server error. Insert failed");
                return StatusCode(500, new InfoMsg(500, msg500));
            }
            _logger.LogTrace($"200. Returned successful");
            return Ok(new InfoMsg(200, msg200));
        }

        public IActionResult PostReturnObj(T req,
            V isPresent,
            ModelStateDictionary mState,
            string msg422,
            string msg500,
            string msg200)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            _logger.LogTrace("Request: ", req);
            var partial = ModelStateControl(mState);
            if (partial != null) 
                return StatusCode(partial.StatusCode, partial);
            if (isPresent != null)
            {
                _logger.LogWarning("422. Unprocessable entity", isPresent);
                return StatusCode(422, new InfoMsg(422, msg422));
            }
            var obj = _mapper.Map<V>(req);
            var newObj = _repository.Add(obj);
            if (newObj == null)
            {
                _logger.LogWarning($"500. Server error. Insert failed");
                return StatusCode(500, new InfoMsg(500, msg500));
            }
            var res = _mapper.Map<T>(newObj);
            _logger.LogTrace($"200. Returned item", res);
            return Ok(new InfoMsg<T>(res, 200, msg200));
        }

        public IActionResult Put(
            T req,
            V isPresent,
            ModelStateDictionary mState,
            string msg422,
            string msg500,
            string msg200)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            _logger.LogTrace("Request: ", req);
            var partial = ModelStateControl(mState);
            if (partial != null) 
                return StatusCode(partial.StatusCode, partial);
            if (isPresent == null)
            {
                _logger.LogWarning("422. Unprocessable entity", isPresent);
                return StatusCode(422, new InfoMsg(422, msg422));
            }
            var obj = _mapper.Map<V>(req);
            if (!_repository.Update(obj))
            {
                _logger.LogWarning("500. Server error. Update failed");
                return StatusCode(500, new InfoMsg(500, msg500));
            }
            _logger.LogTrace($"200. Returned successful");
            return Ok(new InfoMsg(200, msg200));
        }

        public IActionResult Delete(
            V isPresent,
            string msg404,
            string msg500,
            string msg200)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            if (isPresent == null)
            {
                _logger.LogTrace("404. Not found");
                return NotFound(new InfoMsg(404, msg404));
            } 
            if (!_repository.Delete(isPresent))
            {
                _logger.LogWarning("500. Server error");
                return StatusCode(500, msg500);
            }
            _logger.LogTrace($"200. Returned successful");
            return Ok(new InfoMsg(200, msg200));
        }

        public InfoMsg ModelStateControl(ModelStateDictionary mState)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            if (!mState.IsValid)
            {
                StringBuilder ErrVal = new StringBuilder(string.Empty);
                foreach (var modelState in mState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        ErrVal.Append(modelError.ErrorMessage);
                        ErrVal.Append(" - ");
                    }
                }
                _logger.LogWarning($"400. Validation is not valid: {ErrVal}");
                return new InfoMsg(400, ErrVal.ToString());
            }
            return null;
        }
    }
}
