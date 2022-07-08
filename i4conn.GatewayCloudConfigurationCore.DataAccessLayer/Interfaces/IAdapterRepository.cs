using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IAdapterRepository : IBaseRepository<Ts400Interfacce>
    {
        Task<List<Ts400Interfacce>> GetAdaptersByGateway(string gatewayId);
        Task<List<Ts400Interfacce>> GetAdatptersByGroup(string groupId);
        Task<List<Ts400Interfacce>> GetAvailableAdaptersByGroup(string groupId);
        Task<Ts400Interfacce> GetById(string id);
        Ts400Interfacce CheckInterface(string id);
        int CountDevicesByGateway(string gatewayId);
        Task<bool> DeleteGroup(string groupId);
        bool RemoveByGroup(string groupId);
        string GetTypeByGroup(string groupId);
    }
}
