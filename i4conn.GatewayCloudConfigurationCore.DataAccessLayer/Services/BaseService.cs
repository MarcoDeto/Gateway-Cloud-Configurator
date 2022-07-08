using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Enums;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Services
{
    public abstract class BaseService : IBaseService
    {
        private readonly ConnContext _context;
        private readonly ILogger<BaseService> _logger;

        protected BaseService(ConnContext context, ILogger<BaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public string GetI4ConnCloudDir()
        {
            return Environment.GetEnvironmentVariable("I4CONN_CLOUD_DIR");
        }

        private List<string> GetFiles(string path)
        {
            return string.IsNullOrEmpty(path) ? new List<string>() : Directory.GetFiles(path, "*.zip", SearchOption.AllDirectories).ToList();
        }

        private List<KeyValue> GetFileNamesAndVersions(IEnumerable<string> files, Components component)
        {
            List<KeyValue> results = new List<KeyValue>();

            foreach (string file in files)
            {
                List<string> split = file.Split('\\').ToList();
                string[] nameParts = split.Last().Split('-').ToArray();

                string name;
                string version;

                if (nameParts.Length < 2)
                {
                    name = string.Empty;
                    version = string.Empty;
                }
                else if (nameParts.Length == 2)
                {
                    switch (component)
                    {
                        case Components.Firmwares:
                        case Components.Rules:
                            name = nameParts.First().Split('.').Last();
                            break;
                        case Components.Drivers:
                            name = nameParts.First();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(component), component, null);
                    }
                    version = nameParts.Last().Remove(nameParts.Last().Length - 4);
                }
                else
                {
                    version = nameParts.Last().Remove(nameParts.Last().Length - 4);
                    Array.Resize(ref nameParts, nameParts.Length - 1);
                    name = string.Join("-", nameParts);
                }

                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(version))
                    results.Add(new KeyValue
                    {
                        Name = name,
                        Value = version
                    });
            }

            return results;
        }

        public List<KeyValue> GetRepositories(string path, Components component)
        {
            //string i4ConnDir = GetI4ConnCloudDir();
            //string fullDir = $@"{i4ConnDir}{path}";
            string fullDir = $"{path}";
            List<string> files = GetFiles(fullDir);
            return GetFileNamesAndVersions(files, component);
        }

        public InfoMsg<List<KeyValue>> GetVersions(string path, Components component, string library = "")
        {
            InfoMsg<List<KeyValue>> dBridge = new InfoMsg<List<KeyValue>>();

            List<KeyValue> versions = GetRepositories(path, component);
            if (!string.IsNullOrEmpty(library) && !string.IsNullOrWhiteSpace(library))
                dBridge.Content = versions.Where(v => v.Name.Equals(library)).ToList();
            else
                dBridge.Content = versions;

            return dBridge;
        }
    }
}
