using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IGatewayRepository : IBaseRepository<Ts400Gateway>
    {
        Task<Ts400Gateway> GetById(string id);
        Ts400Gateway CheckGateway(string id);
    }
}
