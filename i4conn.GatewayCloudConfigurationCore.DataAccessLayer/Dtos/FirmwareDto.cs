using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class FirmwareDto
    {
        public string Name { get; set; }
        public List<string> Versions { get; set; }
    }
}
