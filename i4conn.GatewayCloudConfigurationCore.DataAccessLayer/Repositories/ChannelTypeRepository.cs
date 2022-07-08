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
    public class ChannelTypeRepository : BaseRepository<Ts400TipiCanale>, IChannelTypeRepository
    {
        private readonly ILogger<ChannelTypeRepository> _logger;
        private readonly ConnContext _context;
        public ChannelTypeRepository(ConnContext context, ILogger<ChannelTypeRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public Ts400TipiCanale CheckChannelType(string type)
        {
            return _context.Ts400TipiCanales
                .AsNoTracking()
                .Where(ct => ct.TipoCanale.Trim().ToUpper() == type.Trim().ToUpper())
                .FirstOrDefault();
        }

        public async Task<Ts400TipiCanale> GetByType(string type)
        {
            return await _context.Ts400TipiCanales
                .Where(ct => ct.TipoCanale.Trim().ToUpper() == type.Trim().ToUpper())
                .FirstOrDefaultAsync();
        }

        public override bool Update(Ts400TipiCanale entity)
        {
            entity.Recno = CheckChannelType(entity.TipoCanale).Recno;
            return base.Update(entity);
        }

        public override bool Delete(Ts400TipiCanale entity)
        {
            entity.Recno = CheckChannelType(entity.TipoCanale).Recno;
            return base.Delete(entity);
        }
    }
}
