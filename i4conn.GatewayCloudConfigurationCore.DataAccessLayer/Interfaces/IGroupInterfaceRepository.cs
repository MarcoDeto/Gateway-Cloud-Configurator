using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IGroupInterfaceRepository : IBaseRepository<Ts400InterfacceGruppi>
    {
        Task<List<Ts400InterfacceGruppi>> GetGroupsByGateway(string gatewayId);
        Task<Ts400InterfacceGruppi> GetById(string id);
        Ts400InterfacceGruppi CheckGroup(string id);
    }
}
