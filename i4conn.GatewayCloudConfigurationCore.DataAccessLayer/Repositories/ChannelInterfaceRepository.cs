using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Linq;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using Microsoft.Extensions.Logging;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Helpers;
using Newtonsoft.Json;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Repositories
{
    public class ChannelInterfaceRepository : IChannelInterfaceRepository
    {
        private readonly ILogger<ChannelInterfaceRepository> _logger;
        private readonly ConnContext _context;

        public ChannelInterfaceRepository(ConnContext context, ILogger<ChannelInterfaceRepository> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("ctor");
        }

        public async Task<bool> ConfirmChannelInterfaces(string interfaceId, string username)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, username);
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@cID_INTERFACCIA",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 3,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = interfaceId.Trim()
                        },
                        new SqlParameter() {
                            ParameterName = "@cUTENTE",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 10,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = username.Trim()
                        }};
                _logger.LogTrace("Params: ", param);
                await _context
                    .Database
                    .ExecuteSqlRawAsync("exec ConfirmChannelInterfaces @cID_INTERFACCIA, @cUTENTE", param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred at {LoggerHelper.GetActualMethodName()}");
            }
            
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool> InitChannelDetail(string interfaceId, string typeInterface, string username)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, typeInterface, username);
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@cID_INTERFACCIA",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 3,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = interfaceId.Trim()
                        },
                        new SqlParameter() {
                            ParameterName = "@tipologiaInterfaccia",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 10,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = typeInterface.Trim().ToUpper()
                        },
                        new SqlParameter() {
                            ParameterName = "@cUTENTE",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 10,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = username.Trim()
                        }};
                _logger.LogTrace("Params: ", param);
                await _context
                    .Database
                    .ExecuteSqlRawAsync(
                        "exec InitAP_DETTAGLIO_CANALI @cID_INTERFACCIA, @tipologiaInterfaccia, @cUTENTE", param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred at {LoggerHelper.GetActualMethodName()}");
            }
            
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool> InitChannelVariable(string interfaceId, string typeInterface, string username)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            LoggerHelper.CheckArgument(interfaceId, typeInterface, username);
            try
            {
                var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@cID_INTERFACCIA",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 3,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = interfaceId.Trim()
                        },
                        new SqlParameter() {
                            ParameterName = "@tipologiaInterfaccia",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 10,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = typeInterface.Trim().ToUpper()
                        },
                        new SqlParameter() {
                            ParameterName = "@cUTENTE",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 10,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = username.Trim()
                        }};
                _logger.LogTrace("Params: ", param);
                await _context
                    .Database
                    .ExecuteSqlRawAsync(
                        "exec InitAP_VARIABILI_CANALI @cID_INTERFACCIA, @tipologiaInterfaccia, @cUTENTE",
                        param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred at {LoggerHelper.GetActualMethodName()}");
            }
           
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<IONumberResponse> TypeInterfacesInputOutputNumber(string typeInterface)
        {
            var list = await _context.Ts400InterfacceCanaliAnagrs
                .Where(t => t.TipologiaInterfaccia.Trim().ToUpper() == typeInterface.Trim().ToUpper())
                .ToListAsync();

            return new IONumberResponse
            {
                InputNumber = list.FindAll(e => e.Direzione.Trim().ToUpper() == "INPUT").Count(),
                OutputNumber = list.FindAll(e => e.Direzione.Trim().ToUpper() == "OUTPUT").Count()
            };
        }
    }
}
