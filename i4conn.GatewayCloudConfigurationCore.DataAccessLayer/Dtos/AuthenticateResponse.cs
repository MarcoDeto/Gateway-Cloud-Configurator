using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
