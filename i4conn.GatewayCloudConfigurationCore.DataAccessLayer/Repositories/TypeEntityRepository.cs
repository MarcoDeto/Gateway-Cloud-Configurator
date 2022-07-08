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
    public class TypeEntityRepository : BaseRepository<Ts400EntitaTipologium>, ITypeEntityRepository
    {
        private readonly ILogger<TypeEntityRepository> _logger;
        private readonly ConnContext _context;

        public TypeEntityRepository(ConnContext context, ILogger<TypeEntityRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Ts400EntitaTipologium CheckTypeEntity(string id)
        {
            return _context.Ts400EntitaTipologia
                .AsNoTracking()
                .Where(e => e.IdTipo.Trim().ToUpper() == id.Trim().ToUpper())
                .FirstOrDefault();
        }

        public async Task<List<Ts400EntitaTipologium>> GetAllByEntity(string entity)
        {
            return await _context.Ts400EntitaTipologia
                .Where(e => e.Entita.Trim().ToUpper() == entity.Trim().ToUpper())
                .ToListAsync();
        }

        public async Task<Ts400EntitaTipologium> GetById(string id)
        {
            return await _context.Ts400EntitaTipologia
                .AsNoTracking()
                .Where(e => e.IdTipo.Trim().ToUpper() == id.Trim().ToUpper())
                .FirstOrDefaultAsync();
        }

        public override bool Update(Ts400EntitaTipologium entity)
        {
            entity.Recno = CheckTypeEntity(entity.IdTipo).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400EntitaTipologium entity)
        {
            entity.Recno = CheckTypeEntity(entity.IdTipo).Recno;
            return base.Delete(entity);
        }
    }
}
