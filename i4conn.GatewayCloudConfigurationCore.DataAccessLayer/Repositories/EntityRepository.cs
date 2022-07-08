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
    public class EntityRepository : BaseRepository<Ts400Entitum>, IEntityRepository
    {
        private readonly ILogger<EntityRepository> _logger;
        private readonly ConnContext _context;
        public EntityRepository(ConnContext context, ILogger<EntityRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public Ts400Entitum CheckEntita(string name)
        {
            return _context.Ts400Entita
                .AsNoTracking()
                .Where(e => e.Entita.Trim().ToUpper() == name.Trim().ToUpper())
                .FirstOrDefault();
        }

        public async Task<Ts400Entitum> GetByName(string name)
        {
            return await _context.Ts400Entita
                .Where(e => e.Entita.Trim().ToUpper() == name.Trim().ToUpper())
                .FirstOrDefaultAsync();
        }

        public override bool Update(Ts400Entitum entity)
        {
            entity.Recno = CheckEntita(entity.Entita).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400Entitum entity)
        {
            entity.Recno = CheckEntita(entity.Entita).Recno;
            return base.Delete(entity);
        }
    }
}
