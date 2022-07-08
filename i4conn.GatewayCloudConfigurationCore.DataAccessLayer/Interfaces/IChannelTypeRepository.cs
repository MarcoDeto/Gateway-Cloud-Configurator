using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IChannelTypeRepository : IBaseRepository<Ts400TipiCanale>
    {
        Ts400TipiCanale CheckChannelType(string type);
        Task<Ts400TipiCanale> GetByType(string type);
    }
}
