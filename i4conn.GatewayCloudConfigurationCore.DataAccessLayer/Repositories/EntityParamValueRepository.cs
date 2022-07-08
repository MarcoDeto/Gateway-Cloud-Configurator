using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Repositories
{
    public class EntityParamValueRepository : BaseRepository<Ts400ParamEntValori>, IEntityParamValueRepository
    {
        private readonly ILogger<EntityParamValueRepository> _logger;
        private readonly ConnContext _context;
        public EntityParamValueRepository(
            ConnContext context,
            ILogger<EntityParamValueRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Ts400ParamEntValori CheckEntityParam(string entity, string idEntity, string paramName)
        {
            return _context.Ts400ParamEntValoris
                .AsNoTracking()
                .Where(p => p.IdEntita.Trim().ToUpper() == idEntity.Trim().ToUpper() &&
                p.ParamNome.Trim() == paramName.Trim() &&
                p.Entita.Trim().ToUpper() == entity.Trim().ToUpper())
                .FirstOrDefault();
        }

        public async Task<Ts400ParamEntValori> GetEntityParam(string entity, string idEntity, string paramName)
        {
            return await _context.Ts400ParamEntValoris
                .Where(p => p.IdEntita.Trim().ToUpper() == idEntity.Trim().ToUpper() && 
                p.ParamNome.Trim() == paramName.Trim() &&
                p.Entita.Trim().ToUpper() == entity.Trim().ToUpper())
                .FirstOrDefaultAsync();
        }

        public async Task<List<EntityParam>> GetInterfaceParams(string entityId)
        {
            var completeList = await getParams();
            var type = _context.Ts400Interfacces
                .SingleOrDefault(i => i.IdInterfaccia == entityId.Trim()).InterfacciaContapezzi.Trim().ToUpper();
            return completeList.FindAll(e => e.Entity.Trim().ToUpper() == "INTERFACCIA" && e.Type.Trim().ToUpper() == type
                && (e.EntityId == null || e.EntityId.Trim() == entityId.Trim()));
        }

        private async Task<List<EntityParam>> getParams()
        {
            var query = from param1 in _context.Ts400ParamEntAnagrs
                        join param2 in _context.Ts400ParamEntValoris
                        on param1.ParamNome equals param2.ParamNome into p2
                        from subp in p2.DefaultIfEmpty()
                        select new EntityParam
                        {
                            Entity = param1.Entita.Trim(),
                            ParamName = param1.ParamNome.Trim(),
                            ParamDefaultValue = param1.ParamValoreDefault.Trim() ?? string.Empty,
                            Type = param1.Tipologia.Trim(),
                            ParamValue = subp.ParamValore.Trim() ?? string.Empty,
                            EntityId = subp.IdEntita.Trim(),
                            UseDefault = subp.ParamValore == null
                        };

            return await query.ToListAsync();
        }

        public async Task<List<Ts400ParamEntValori>> GetParamsByEntity(string entity)
        {
            return await _context.Ts400ParamEntValoris
                .Where(p => p.Entita.Trim() == entity.Trim().ToUpper())
                .ToListAsync();
        }

        public async Task<List<Ts400ParamEntValori>> GetRuleParams(string interfaceId, string direction, string virtualCh)
        {
            string entityId = CreateId(interfaceId, direction, virtualCh);
            return await _context.Ts400ParamEntValoris
                .Where(p => p.IdEntita.Trim().ToUpper() == entityId.Trim().ToUpper() &&
                p.Entita.Trim().ToUpper() == "REGOLA")
                .ToListAsync();
        }

        public string CreateId(string interfaceId, string direction, string virtualCh)
        {
            string dir = (direction.Trim().ToUpper() == "INPUT") ? "I" : "U";
            return interfaceId.Trim() + dir + virtualCh.Trim();
        }

        public override bool Update(Ts400ParamEntValori entity)
        {
            entity.Recno = CheckEntityParam(entity.Entita, entity.IdEntita, entity.ParamNome).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400ParamEntValori entity)
        {
            entity.Recno = CheckEntityParam(entity.Entita, entity.IdEntita, entity.ParamNome).Recno;
            return base.Delete(entity);
        }
    }
}
