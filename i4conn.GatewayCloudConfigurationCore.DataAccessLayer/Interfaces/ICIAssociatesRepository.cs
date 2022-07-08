using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface ICIAssociatesRepository : IBaseRepository<Ts400InterfacceCanaliAssociati>
    {
        Task<List<Ts400InterfacceCanaliAnagr>> GetAssociableChannels(string typeInterface, string direction);
        Task<List<Ts400InterfacceCanaliAssociati>> GetAssociateChannels(string interfaceId, string virtualChId);
        Task<Ts400InterfacceCanaliAssociati> GetAssociateChannel(
            string interfaceId,
            string virtualChId,
            string channelId,
            string direction);
        Ts400InterfacceCanaliAssociati CheckAssociateChannel(
            string interfaceId,
            string virtualChId,
            string channelId,
            string direction);
    }
}
