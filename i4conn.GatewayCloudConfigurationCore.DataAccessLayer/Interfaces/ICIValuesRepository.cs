using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface ICIValuesRepository : IBaseRepository<Ts400InterfacceCanaliValori>
    {
        Task<List<Ts400InterfacceCanaliValori>> GetValues(string interfaceId);
        Task<List<Ts400InterfacceCanaliValori>> GetValues(string interfaceId, string direction);
        Task<PagingResponse<List<Ts400InterfacceCanaliValori>>> GetValues(string interfaceId, int skip, int take);
        Ts400InterfacceCanaliValori CheckValue(string interfaceId, string channelId, string direction);
        Task<Ts400InterfacceCanaliValori> GetValue(string interfaceId, string channelId, string direction);
    }
}
