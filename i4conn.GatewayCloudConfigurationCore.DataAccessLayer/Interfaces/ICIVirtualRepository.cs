using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface ICIVirtualRepository : IBaseRepository<Ts400InterfacceCanaliValori>
    {
        Task<List<Ts400InterfacceCanaliValori>> GetVirtualValues(string interfaceId);
        Task<Ts400InterfacceCanaliValori> GetVirtualValue(string interfaceId, string channelId, string direction);
        Ts400InterfacceCanaliValori CheckVirtualValue(string interfaceId, string channelId, string direction);
    }
}
