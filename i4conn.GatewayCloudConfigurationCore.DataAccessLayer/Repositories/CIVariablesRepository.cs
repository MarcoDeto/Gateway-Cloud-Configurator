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
    public class CIVariablesRepository : BaseRepository<Ts400InterfacceCanaliVariabili>, ICIVariablesRepository
    {
        private readonly ILogger<CIVariablesRepository> _logger;
        private readonly ConnContext _context;

        public CIVariablesRepository(ConnContext context, ILogger<CIVariablesRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Ts400InterfacceCanaliVariabili CheckVariable(string interfaceId, string channelId, string name)
        {
            return _context.Ts400InterfacceCanaliVariabilis
                .AsNoTracking()
                .Where(v => v.IdInterfaccia.Trim() == interfaceId.Trim()
                && v.IdCanale.Trim() == channelId.Trim()
                && v.Nome.Trim() == name.Trim())
                .FirstOrDefault();
        }

        public async Task<List<Ts400InterfacceCanaliVariabili>> GetVariables(string interfaceId, string channelId, string direction)
        {
            return await _context.Ts400InterfacceCanaliVariabilis
                .Where(v => v.IdInterfaccia.Trim() == interfaceId.Trim() 
                && v.IdCanale.Trim() == channelId.Trim() && v.Direzione.Trim() == direction.Trim().ToUpper())
                .ToListAsync();
        }

        public override bool Update(Ts400InterfacceCanaliVariabili entity)
        {
            entity.Recno = CheckVariable(entity.IdInterfaccia, entity.IdCanale, entity.Nome).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400InterfacceCanaliVariabili entity)
        {
            entity.Recno = CheckVariable(entity.IdInterfaccia, entity.IdCanale, entity.Nome).Recno;
            return base.Delete(entity);
        }
    }
}
