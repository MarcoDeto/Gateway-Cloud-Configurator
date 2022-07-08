using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using System.IdentityModel.Tokens.Jwt;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IUserRepository : IBaseRepository<Confute>
    {
        Confute CheckUser(string username);
        bool Authenticate(string username, string password);
        JwtSecurityToken GetToken(string username);
    }
}
