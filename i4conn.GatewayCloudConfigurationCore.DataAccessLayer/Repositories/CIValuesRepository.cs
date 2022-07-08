using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
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
    public class CIValuesRepository : BaseRepository<Ts400InterfacceCanaliValori>, ICIValuesRepository
    {
        private readonly ILogger<CIValuesRepository> _logger;
        private readonly ConnContext _context;

        public CIValuesRepository(ConnContext context, ILogger<CIValuesRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Ts400InterfacceCanaliValori CheckValue(string interfaceId, string channelId, string direction)
        {
            return _context.Ts400InterfacceCanaliValoris
                .AsNoTracking()
                .Where(v => v.IdInterfaccia == interfaceId.Trim()
                && v.IdCanale.Trim() == channelId.Trim()
                && v.Direzione.Trim() == direction.Trim().ToUpper()
                && !v.FlgVirtual).FirstOrDefault();
        }

        public async Task<Ts400InterfacceCanaliValori> GetValue(string interfaceId, string channelId, string direction)
        {
            return await _context.Ts400InterfacceCanaliValoris
                .Where(v => v.IdInterfaccia == interfaceId.Trim()
                && v.IdCanale.Trim() == channelId.Trim()
                && v.Direzione.Trim() == direction.Trim().ToUpper()
                && !v.FlgVirtual).FirstOrDefaultAsync();
        }

        public async Task<List<Ts400InterfacceCanaliValori>> GetValues(string interfaceId)
        {
            return await _context.Ts400InterfacceCanaliValoris
                .Where(v => v.IdInterfaccia.Trim() == interfaceId.Trim() && !v.FlgVirtual)
                .ToListAsync();
        }

        public async Task<List<Ts400InterfacceCanaliValori>> GetValues(string interfaceId, string direction)
        {
            return await _context.Ts400InterfacceCanaliValoris
                .Where(v => v.IdInterfaccia.Trim() == interfaceId.Trim() 
                && v.Direzione.Trim() == direction.Trim().ToUpper()
                && !v.FlgVirtual)
                .ToListAsync();
        }

        public async Task<PagingResponse<List<Ts400InterfacceCanaliValori>>> GetValues(string interfaceId, int skip, int take)
        {
            var res = _context.Ts400InterfacceCanaliValoris
                .Where(v => v.IdInterfaccia.Trim() == interfaceId.Trim() && !v.FlgVirtual);

            return new PagingResponse<List<Ts400InterfacceCanaliValori>>
            {
                Content = await res.Skip(skip).Take(take).ToListAsync(),
                Count = await res.CountAsync()
        };
        }

        public override bool Update(Ts400InterfacceCanaliValori entity)
        {
            var obj = _context.Ts400InterfacceCanaliValoris
                .Where(v => v.IdInterfaccia == entity.IdInterfaccia.Trim()
                && v.IdCanale.Trim() == entity.IdCanale.Trim()
                && v.Direzione.Trim() == entity.Direzione.Trim().ToUpper()
                && !v.FlgVirtual).FirstOrDefault();
            obj.Descrizione = entity.Descrizione;
            return Save();
        }

        public override bool Delete(Ts400InterfacceCanaliValori entity)
        {
            entity.Recno = CheckValue(entity.IdInterfaccia, entity.IdCanale, entity.Direzione).Recno;
            return base.Delete(entity);
        }
    }
}
