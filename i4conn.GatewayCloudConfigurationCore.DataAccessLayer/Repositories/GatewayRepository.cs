using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Repositories
{
    public class GatewayRepository : BaseRepository<Ts400Gateway>, IGatewayRepository
    {
        private readonly ILogger<GatewayRepository> _logger;
        private readonly ConnContext _context;
        public GatewayRepository(ConnContext context, ILogger<GatewayRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Ts400Gateway CheckGateway(string id)
        {
            return _context.Ts400Gateways
                .AsNoTracking()
                .Where(g => g.IdGateway.Trim().ToUpper() == id.Trim().ToUpper())
                .FirstOrDefault();
        }

        public async Task<Ts400Gateway> GetById(string id)
        {
            return await _context.Ts400Gateways
                .Where(g => g.IdGateway.Trim().ToUpper() == id.Trim().ToUpper())
                .FirstOrDefaultAsync();
        }

        public override bool Update(Ts400Gateway entity)
        {
            entity.Recno = CheckGateway(entity.IdGateway).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400Gateway entity)
        {
            entity.Recno = CheckGateway(entity.IdGateway).Recno;
            return base.Delete(entity);
        }
    }
}
