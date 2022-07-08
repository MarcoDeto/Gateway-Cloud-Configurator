using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface ICIVariablesRepository : IBaseRepository<Ts400InterfacceCanaliVariabili>
    {
        Task<List<Ts400InterfacceCanaliVariabili>> GetVariables(string interfaceId, string channelId, string direction);
        Ts400InterfacceCanaliVariabili CheckVariable(string interfaceId, string channelId, string name);
    }
}
