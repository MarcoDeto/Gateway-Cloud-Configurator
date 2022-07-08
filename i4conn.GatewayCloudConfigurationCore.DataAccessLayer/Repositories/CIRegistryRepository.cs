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
    public class CIRegistryRepository : BaseRepository<Ts400InterfacceCanaliAnagr>, ICIRegistryRepository
    {
        private readonly ILogger<CIRegistryRepository> _logger;
        private readonly ConnContext _context;

        public CIRegistryRepository(ConnContext context, ILogger<CIRegistryRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public Ts400InterfacceCanaliAnagr CheckRegistry(string channelId, string typeInterface, string direction)
        {
            return _context.Ts400InterfacceCanaliAnagrs
                .AsNoTracking()
                .Where(c => c.IdCanale.Trim() == channelId.Trim()
                && c.TipoCanale.Trim() == typeInterface.Trim().ToUpper()
                && c.Direzione.Trim() == direction.Trim().ToUpper())
                .FirstOrDefault();
        }

        public async Task<List<Ts400InterfacceCanaliAnagr>> GetAssociateRealChannels(string typeInterface, string direction)
        {
            return await _context.Ts400InterfacceCanaliAnagrs
                .Where(c => c.TipoCanale.Trim() == typeInterface.Trim().ToUpper()
                && c.Direzione.Trim() == direction.Trim().ToUpper())
                .ToListAsync();
        }
    }
}
