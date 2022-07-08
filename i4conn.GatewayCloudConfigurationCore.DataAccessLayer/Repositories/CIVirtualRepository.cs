using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Repositories
{
    public class CIVirtualRepository : BaseRepository<Ts400InterfacceCanaliValori>, ICIVirtualRepository
    {
        private readonly ILogger<CIVirtualRepository> _logger;
        private readonly ConnContext _context;

        public CIVirtualRepository(ConnContext context, ILogger<CIVirtualRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Ts400InterfacceCanaliValori>> GetVirtualValues(string interfaceId)
        {
            return await _context.Ts400InterfacceCanaliValoris
                .Where(v => v.IdInterfaccia.Trim() == interfaceId.Trim() && v.FlgVirtual)
                .ToListAsync();
        }

        public async Task<Ts400InterfacceCanaliValori> GetVirtualValue(string interfaceId, string channelId, string direction)
        {
            return await _context.Ts400InterfacceCanaliValoris
                .Where(v => v.IdInterfaccia == interfaceId.Trim()
                && v.IdCanale.Trim() == channelId.Trim()
                && v.Direzione.Trim() == direction.Trim().ToUpper()
                && v.FlgVirtual)
                .FirstOrDefaultAsync();
        }

        public Ts400InterfacceCanaliValori CheckVirtualValue(string interfaceId, string channelId, string direction)
        {
            return _context.Ts400InterfacceCanaliValoris
                .AsNoTracking()
                .Where(v => v.IdInterfaccia == interfaceId.Trim()
                && (v.IdCanale != null && v.IdCanale.Trim() == channelId.Trim())
                && v.Direzione.Trim() == direction.Trim().ToUpper()
                && v.FlgVirtual)
                .FirstOrDefault();
        }

        public override Ts400InterfacceCanaliValori Add(Ts400InterfacceCanaliValori entity)
        {
            var channels = GetVirtualValues(entity.IdInterfaccia).Result;
            int maxId = 0;
            if (channels.Count() > 0)
            {
                bool isParsing = channels.Max(e => int.TryParse(e.IdCanale, out maxId));
                if (!isParsing)
                    return null;
            }
            entity.IdCanale = (channels.Count() > 0) ? (++maxId).ToString() : (65).ToString();
            entity.FlgVirtual = true;
            return base.Add(entity);
        }

        public override bool Update(Ts400InterfacceCanaliValori entity)
        {
            entity.FlgVirtual = true;
            entity.Recno = CheckVirtualValue(entity.IdInterfaccia, entity.IdCanale, entity.Direzione).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400InterfacceCanaliValori entity)
        {
            entity.Recno = CheckVirtualValue(entity.IdInterfaccia, entity.IdCanale, entity.Direzione).Recno;
            return base.Delete(entity);
        }
    }
}
