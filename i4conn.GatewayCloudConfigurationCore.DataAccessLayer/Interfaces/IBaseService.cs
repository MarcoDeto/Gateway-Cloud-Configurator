using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Enums;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models;
using System.Collections.Generic;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces
{
    public interface IBaseService
    {
        string GetI4ConnCloudDir();
        List<KeyValue> GetRepositories(string path, Components component);
        InfoMsg<List<KeyValue>> GetVersions(string path, Components component, string library = "");
    }
}