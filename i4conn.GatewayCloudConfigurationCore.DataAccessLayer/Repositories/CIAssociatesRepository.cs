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
    public class CIAssociatesRepository : BaseRepository<Ts400InterfacceCanaliAssociati>, ICIAssociatesRepository
    {
        private readonly ILogger<CIAssociatesRepository> _logger;
        private readonly ConnContext _context;

        public CIAssociatesRepository(ConnContext context, ILogger<CIAssociatesRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Ts400InterfacceCanaliAssociati CheckAssociateChannel(
            string interfaceId,
            string virtualChId,
            string channelId,
            string direction)
        {
            return _context.Ts400InterfacceCanaliAssociatis
                .AsNoTracking()
                .Where(c => c.IdInterfaccia == interfaceId.Trim() && c.IdCanaleVirtuale == virtualChId.Trim()
                && c.IdCanale == channelId.Trim() && c.Direzione.Trim() == direction.Trim().ToUpper())
                .FirstOrDefault();
        }

        public async Task<List<Ts400InterfacceCanaliAnagr>> GetAssociableChannels(string typeInterface, string direction)
        {
            return await _context.Ts400InterfacceCanaliAnagrs
                .Where(c => c.TipologiaInterfaccia.Trim() == typeInterface.Trim().ToUpper()
                && c.Direzione.Trim() == direction.Trim().ToUpper())
                .ToListAsync();
        }

        public async Task<Ts400InterfacceCanaliAssociati> GetAssociateChannel(
            string interfaceId,
            string virtualChId,
            string channelId,
            string direction)
        {
            return await _context.Ts400InterfacceCanaliAssociatis
                .Where(c => c.IdInterfaccia == interfaceId.Trim() && c.IdCanaleVirtuale == virtualChId.Trim()
                && c.IdCanale == channelId.Trim() && c.Direzione.Trim() == direction.Trim().ToUpper())
                .FirstOrDefaultAsync();
        }

        public async Task<List<Ts400InterfacceCanaliAssociati>> GetAssociateChannels(string interfaceId, string virtualChId)
        {
            return await _context.Ts400InterfacceCanaliAssociatis
                .Where(c => c.IdInterfaccia == interfaceId.Trim() && c.IdCanaleVirtuale == virtualChId.Trim())
                .ToListAsync();
        }

        public override bool Update(Ts400InterfacceCanaliAssociati entity)
        {
            entity.Recno = CheckAssociateChannel(
                entity.IdInterfaccia,
                entity.IdCanaleVirtuale,
                entity.IdCanale,
                entity.Direzione).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400InterfacceCanaliAssociati entity)
        {
            entity.Recno = CheckAssociateChannel(
                entity.IdInterfaccia,
                entity.IdCanaleVirtuale,
                entity.IdCanale,
                entity.Direzione).Recno;
            return base.Delete(entity);
        }
    }
}
