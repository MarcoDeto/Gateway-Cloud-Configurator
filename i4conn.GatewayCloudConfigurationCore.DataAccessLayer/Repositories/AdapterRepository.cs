using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Repositories
{
    public class AdapterRepository : BaseRepository<Ts400Interfacce>, IAdapterRepository
    {
        private readonly ILogger<AdapterRepository> _logger;
        private readonly ConnContext _context;
        public AdapterRepository(ConnContext context, ILogger<AdapterRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("ctor");
        }

        public async Task<List<Ts400Interfacce>> GetAdaptersByGateway(string gatewayId)
        {
            var query = from i in _context.Ts400Interfacces
                  join g in _context.Ts400InterfacceGruppis
                  on i.IdGruppoInterfacce equals g.IdGruppoInterfacce
                  where g.IdGateway == gatewayId
                  select i;

            return await query.ToListAsync();
        }

        public async Task<List<Ts400Interfacce>> GetAdatptersByGroup(string groupId)
        {
            return await _context.Ts400Interfacces
                .Where(i => i.IdGruppoInterfacce == groupId)
                .ToListAsync();
        }

        public async Task<List<Ts400Interfacce>> GetAvailableAdaptersByGroup(string groupId)
        {
            string type = GetTypeByGroup(groupId);
            return await _context.Ts400Interfacces
                .Where(i => i.InterfacciaContapezzi == type && i.IdGruppoInterfacce != groupId)
                .ToListAsync();
        }

        public string GetTypeByGroup(string groupId)
        {
            var list = _context.Ts400Interfacces
                .AsNoTracking()
                .Where(i => i.IdGruppoInterfacce == groupId)
                .ToList();
            return (list.Count() != 0) ? 
                (!string.IsNullOrEmpty(list[0].InterfacciaContapezzi)) ? list[0].InterfacciaContapezzi.Trim() : null : null;
        }

        public async Task<Ts400Interfacce> GetById(string id)
        {
            return await _context.Ts400Interfacces
                .Where(i => i.IdInterfaccia == id)
                .FirstOrDefaultAsync();
        }

        public Ts400Interfacce CheckInterface(string id)
        {
            return _context.Ts400Interfacces
                .AsNoTracking()
                .Where(i => i.IdInterfaccia != null && i.IdInterfaccia == id)
                .FirstOrDefault();
        }

        public override Ts400Interfacce Add(Ts400Interfacce entity)
        {
            var interfaces = GetAll().Result;
            int maxId = 0;
            if (interfaces.Count() > 0)
            {
                foreach (var type in interfaces)
                {
                    int.TryParse(type.IdInterfaccia, out int currentId);
                    if (currentId > maxId)
                        maxId = currentId;
                }
            }
            entity.IdInterfaccia = (++maxId).ToString().PadLeft(3, '0');
            return base.Add(entity);
        }

        public override bool Update(Ts400Interfacce entity)
        {
            entity.Recno = CheckInterface(entity.IdInterfaccia).Recno;
            _context.Ts400Interfacces.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Save();
            //return base.Update(entity);
        }

        public override bool Delete(Ts400Interfacce entity)
        {
            entity.Recno = CheckInterface(entity.IdInterfaccia).Recno;
            return base.Delete(entity);
        }

        public int CountDevicesByGateway(string gatewayId)
        {
            return (from i in _context.Ts400Interfacces
                    join g in _context.Ts400InterfacceGruppis
                    on i.IdGruppoInterfacce equals g.IdGruppoInterfacce
                    where g.IdGateway.Trim().ToUpper() == gatewayId.Trim().ToUpper() && i.IdDispositivo != 0
                    select i).Sum(d => d.IdDispositivo);
        }

        public async Task<bool> DeleteGroup(string groupId)
        {
            var interfaces = await GetAdatptersByGroup(groupId);
            interfaces.ForEach(i => i.IdGruppoInterfacce = string.Empty);
            return Save();
        }

        public bool RemoveByGroup(string groupId)
        {
            var paramGroup = new SqlParameter("@GROUP_ID", groupId.Trim().ToUpper());
            _context.Database.ExecuteSqlRaw("delete from TS400_INTERFACCE where ID_GRUPPO_INTERFACCE = @GROUP_ID", paramGroup);
            return Save();
        }
    }
}
