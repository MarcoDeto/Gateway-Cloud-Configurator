using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Enums;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Services
{
    public class FirmwareService : BaseService, IFirmwareService
    {
        private readonly ConnContext _context;
        private readonly ILogger<BaseService> _logger;
        public FirmwareService(ConnContext context, ILogger<BaseService> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        //public List<Firmware> All(string path)
        //{
        //    //C:\i4conn\Server\
        //    //string i4ConnDir = GetI4ConnCloudDir();
        //    //i4ConnDir = @"C:\i4conn\Server\";
        //    //var pathFirmware = $@"{i4ConnDir}Firmware\";
        //    if (string.IsNullOrEmpty(path)) return null;

        //    List<KeyValue> repositories = GetRepositories(path, Components.Firmwares);

        //    var firmwares = new List<Firmware> { new Firmware() };

        //    if (repositories.Count == 0)
        //    {
        //        firmwares = null;
        //        return firmwares;
        //    }
            
        //    firmwares.First().Path = path;

        //    firmwares.First().Firmwares = repositories;

        //    return firmwares;
        //}

        public List<FirmwareDto> All(string path, params string[] types)
        {
            //C:\i4conn\Server\
            //string i4ConnDir = GetI4ConnCloudDir();
            //i4ConnDir = @"C:\i4conn\Server\";
            //var pathFirmware = $@"{i4ConnDir}Firmware\";
            if (string.IsNullOrEmpty(path)) return null;

            List<KeyValue> repositories = GetRepositories(path, Components.Firmwares);

            var result = new List<FirmwareDto>();

            if (repositories.Count == 0)
            {
                result = null;
                return result;
            }

            var query = from firmware in repositories
                    where (types.Length != 0) ? types.Contains(firmware.Name) : true
                    group firmware by firmware.Name;
            
            foreach (IGrouping<string, KeyValue> firmwareGroup in query)
            {
                var resultVersions = new List<string>();
                firmwareGroup.ToList().ForEach(v => resultVersions.Add(v.Value));
                result.Add(new FirmwareDto
                {
                    Name = firmwareGroup.Key,
                    Versions = resultVersions
                });
            }

            return result;
        }

        public async Task<List<FirmwareDto>> AllAsync(string path, params string[] types)
        {
            return await Task.Run(() => All(path, types));
        }
    }
}
