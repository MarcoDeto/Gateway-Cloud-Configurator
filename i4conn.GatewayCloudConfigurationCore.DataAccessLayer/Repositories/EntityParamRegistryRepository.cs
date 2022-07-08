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
    public class EntityParamRegistryRepository : BaseRepository<Ts400ParamEntAnagr>, IEntityParamRegistryRepository
    {
        private readonly ILogger<EntityParamRegistryRepository> _logger;
        private readonly ConnContext _context;
        public EntityParamRegistryRepository(
            ConnContext context,
            ILogger<EntityParamRegistryRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public Ts400ParamEntAnagr CheckParam(string name, string entity, string type)
        {
            return _context.Ts400ParamEntAnagrs
                .AsNoTracking()
                .Where(p => p.ParamNome.Trim() == name.Trim()
                && p.Entita.Trim().ToUpper() == entity.Trim().ToUpper()
                && p.Tipologia.Trim().ToUpper() == type.Trim().ToUpper())
                .FirstOrDefault();
        }

        public async Task<List<Ts400ParamEntAnagr>> GetAllByEntity(string entityName)
        {
            return await _context.Ts400ParamEntAnagrs
                .Where(p => p.Entita.Trim().ToUpper() == entityName.Trim().ToUpper())
                .ToListAsync();
        }

        public async Task<List<Ts400ParamEntAnagr>> GetAllByType(string type)
        {
            return await _context.Ts400ParamEntAnagrs
                .Where(p => p.Tipologia.Trim().ToUpper() == type.Trim().ToUpper())
                .ToListAsync();
        }

        public async Task<Ts400ParamEntAnagr> GetParam(string name, string entity, string type)
        {
            return await _context.Ts400ParamEntAnagrs
                .Where(p => p.ParamNome.Trim() == name.Trim()
                && p.Entita.Trim().ToUpper() == entity.Trim().ToUpper()
                && p.Tipologia.Trim().ToUpper() == type.Trim().ToUpper())
                .FirstOrDefaultAsync();
        }

        public override bool Update(Ts400ParamEntAnagr entity)
        {
            entity.Recno = CheckParam(entity.ParamNome, entity.Entita, entity.Tipologia).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400ParamEntAnagr entity)
        {
            entity.Recno = CheckParam(entity.ParamNome, entity.Entita, entity.Tipologia).Recno;
            return base.Delete(entity);
        }
    }
}
