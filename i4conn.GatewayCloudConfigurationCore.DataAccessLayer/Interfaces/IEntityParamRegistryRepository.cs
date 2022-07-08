using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IEntityParamRegistryRepository : IBaseRepository<Ts400ParamEntAnagr>
    {
        Task<List<Ts400ParamEntAnagr>> GetAllByEntity(string entityName);
        Task<List<Ts400ParamEntAnagr>> GetAllByType(string type);
        Task<Ts400ParamEntAnagr> GetParam(string name, string entity, string type);
        Ts400ParamEntAnagr CheckParam(string name, string entity, string type);
    }
}
