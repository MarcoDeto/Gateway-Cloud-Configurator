using i4conn.GatewayCloudConfigurationCore.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace i4conn.GatewayCloudConfigurationCore.Api.Extensions
{
    public static class ExtensionMethods
    {
        public static IApplicationBuilder CustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
