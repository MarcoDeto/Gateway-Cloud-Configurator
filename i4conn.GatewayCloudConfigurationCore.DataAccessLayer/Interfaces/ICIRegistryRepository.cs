using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface ICIRegistryRepository : IBaseRepository<Ts400InterfacceCanaliAnagr>
    {
        Ts400InterfacceCanaliAnagr CheckRegistry(string channelId, string typeInterface, string direction);
        Task<List<Ts400InterfacceCanaliAnagr>> GetAssociateRealChannels(string typeInterface, string direction);
    }
}
