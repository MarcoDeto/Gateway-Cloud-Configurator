using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IEntityParamValueRepository : IBaseRepository<Ts400ParamEntValori>
    {
        Task<List<Ts400ParamEntValori>> GetParamsByEntity(string entity);
        Task<List<Ts400ParamEntValori>> GetRuleParams(string interfaceId, string direction, string virtualCh);
        Task<List<EntityParam>> GetInterfaceParams(string entityId);
        Task<Ts400ParamEntValori> GetEntityParam(string entity, string idEntity, string paramName);
        Ts400ParamEntValori CheckEntityParam(string entity, string idEntity, string paramName);
        string CreateId(string interfaceId, string direction, string virtualCh);
    }
}
