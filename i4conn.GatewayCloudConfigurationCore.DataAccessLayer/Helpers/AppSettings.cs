using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int Expiration { get; set; }
    }
}
