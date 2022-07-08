using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface ITypeEntityRepository : IBaseRepository<Ts400EntitaTipologium>
    {
        Task<List<Ts400EntitaTipologium>> GetAllByEntity(string entity);
        Task<Ts400EntitaTipologium> GetById(string id);
        Ts400EntitaTipologium CheckTypeEntity(string id);
    }
}
