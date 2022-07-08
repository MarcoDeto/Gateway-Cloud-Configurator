using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.Api.Interfaces
{
    public interface IControllerHelper<T, U, V>
        where T : class
        where V : class
        where U : IBaseRepository<V>
    {
        InfoMsg ModelStateControl(ModelStateDictionary mState);
        Task<IActionResult> Get();
        Task<IActionResult> Get(V isPresent, string msg404);
        Task<IActionResult> GetFiltered(List<V> filteredList);
        IActionResult PostReturnBool(
            T req,
            V isPresent,
            ModelStateDictionary mState,
            string msg422,
            string msg500,
            string msg200);
        IActionResult PostReturnObj(
            T req,
            V isPresent,
            ModelStateDictionary mState,
            string msg422,
            string msg500,
            string msg200);
        IActionResult Put(
            T req, 
            V isPresent,
            ModelStateDictionary mState, 
            string msg422, 
            string msg500, 
            string msg200);
        IActionResult Delete(V isPresent, string msg404, string msg500, string msg200);
    }
}
