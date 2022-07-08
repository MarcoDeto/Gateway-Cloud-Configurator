using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.Api.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
            _logger.LogDebug("ctor");
        }
        public async Task InvokeAsync(HttpContext httpContext, IWebHostEnvironment env)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occur: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex, env);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env)
        {
            var stackTrace = String.Empty;

            var status = (int)HttpStatusCode.InternalServerError;
            var message = exception.Message;
            if (env.IsEnvironment("Development"))
                stackTrace = exception.StackTrace;

            var result = JsonSerializer.Serialize(new InfoMsg
            {
                StatusCode = status,
                Message = (env.IsDevelopment()) ? "error: " + message + " - stack trace: " + stackTrace : "Server error"
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }
}
