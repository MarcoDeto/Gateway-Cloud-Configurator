using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class InfoMsg<T> : InfoMsg
    {
        public T Content { get; set; }
        public InfoMsg() { }
        public InfoMsg(T content, int statusCode, string message) : base (statusCode, message)
        {
            Content = content;
        }
    }

    public class InfoMsg
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public InfoMsg() { }
        public InfoMsg(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }
    }
}
