using System;
using System.Collections.Generic;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class PagingResponse<T>
    {
        public T Content { get; set; }
        public int Count { get; set; }
    }
}
