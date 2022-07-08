using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IFirmwareService : IBaseService
    {
        List<FirmwareDto> All(string path, params string[] types);
        Task<List<FirmwareDto>> AllAsync(string path, params string[] types);
    }
}