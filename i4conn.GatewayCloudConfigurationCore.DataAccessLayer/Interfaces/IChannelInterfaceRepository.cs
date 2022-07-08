using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IChannelInterfaceRepository
    {
        Task<bool> ConfirmChannelInterfaces(string interfaceId, string username);
        Task<bool> InitChannelDetail(string interfaceId, string typeInterface, string username);
        Task<bool> InitChannelVariable(string interfaceId, string typeInterface, string username);
        Task<IONumberResponse> TypeInterfacesInputOutputNumber(string typeInterface);
    }
}
