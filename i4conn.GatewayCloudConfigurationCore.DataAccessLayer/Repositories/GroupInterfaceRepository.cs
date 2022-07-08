using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Repositories
{
    public class GroupInterfaceRepository : BaseRepository<Ts400InterfacceGruppi>, IGroupInterfaceRepository
    {
        private readonly ILogger<GroupInterfaceRepository> _logger;
        private readonly ConnContext _context;
        public GroupInterfaceRepository(ConnContext context,
            ILogger<GroupInterfaceRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Ts400InterfacceGruppi CheckGroup(string id)
        {
            return _context.Ts400InterfacceGruppis
                .AsNoTracking()
                .Where(g => g.IdGruppoInterfacce.Trim().ToUpper() == id.Trim().ToUpper())
                .FirstOrDefault();
        }

        public async Task<Ts400InterfacceGruppi> GetById(string id)
        {
            return await _context.Ts400InterfacceGruppis
                .Where(g => g.IdGruppoInterfacce.Trim().ToUpper() == id.Trim().ToUpper())
                .FirstOrDefaultAsync();
        }

        public async Task<List<Ts400InterfacceGruppi>> GetGroupsByGateway(string gatewayId)
        {
            return await _context.Ts400InterfacceGruppis
                .Where(g => g.IdGateway.Trim().ToUpper() == gatewayId.Trim().ToUpper())
                .ToListAsync();
        }

        public override bool Update(Ts400InterfacceGruppi entity)
        {
            entity.Recno = CheckGroup(entity.IdGruppoInterfacce).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400InterfacceGruppi entity)
        {
            entity.Recno = CheckGroup(entity.IdGruppoInterfacce).Recno;
            return base.Delete(entity);
        }
    }
}
