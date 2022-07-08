using System.Collections.Generic;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(long id);
        T Add(T entity); 
        bool Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();
    }
}
