using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IEntityRepository : IBaseRepository<Ts400Entitum>
    {
        Ts400Entitum CheckEntita(string name);
        Task<Ts400Entitum> GetByName(string name);
    }
}
