using i4conn.GatewayCloudConfigurationCore.Api.Controllers;
using i4conn.GatewayCloudConfigurationCore.Api.Extensions;
using i4conn.GatewayCloudConfigurationCore.Api.Helpers;
using i4conn.GatewayCloudConfigurationCore.Api.Interfaces;
using i4conn.GatewayCloudConfigurationCore.Api.Middlewares;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Helpers;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Repositories;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Services;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using i4conn.GatewayCloudConfigurationCore.Persistence.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleInjector;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.Api
{
    public class Startup
    {
        private Container container = new Container();
        public Startup(IConfiguration configuration)
        {
            container.Options.ResolveUnregisteredConcreteTypes = false;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvcCore();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true
                };
            });

            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore()
                    .AddControllerActivation();
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "i4conn.GatewayCloudConfigurationCore.Api", Version = "v1" });
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            InitializeContainer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Configure called");
            app.UseSimpleInjector(container);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "i4conn.GatewayCloudConfigurationCore.Api v1"));
            }

            app.CustomExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
#if DEBUG
            container.Verify();
#endif
        }

        private void InitializeContainer()
        {
            container.Register(() =>
            {
                var optionsBuilder =
                    new DbContextOptionsBuilder<ConnContext>()
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .AddInterceptors(new StringTrimmerInterceptor())
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    .EnableSensitiveDataLogging(true);
                return new ConnContext(optionsBuilder.Options);
            }, Lifestyle.Scoped);
            container.Register(typeof(IBaseRepository<>), typeof(BaseRepository<>), Lifestyle.Scoped);
            container.Register(typeof(IControllerHelper<,,>), typeof(ControllerHelper<,,>), Lifestyle.Scoped);
            container.Register<IUserRepository, UserRepository>(Lifestyle.Scoped);
            container.Register<IGatewayRepository, GatewayRepository>(Lifestyle.Scoped);
            container.Register<IAdapterRepository, AdapterRepository>(Lifestyle.Scoped);
            container.Register<IFirmwareService, FirmwareService>(Lifestyle.Scoped);
            container.Register<IGroupInterfaceRepository, GroupInterfaceRepository>(Lifestyle.Scoped);
            container.Register<IEntityRepository, EntityRepository>(Lifestyle.Scoped);
            container.Register<IChannelInterfaceRepository, ChannelInterfaceRepository>(Lifestyle.Scoped);
            container.Register<ITypeEntityRepository, TypeEntityRepository>(Lifestyle.Scoped);
            container.Register<IChannelTypeRepository, ChannelTypeRepository>(Lifestyle.Scoped);
            container.Register<IEntityParamRegistryRepository, EntityParamRegistryRepository>(Lifestyle.Scoped);
            container.Register<IEntityParamValueRepository, EntityParamValueRepository>(Lifestyle.Scoped);
            container.Register<ICIValuesRepository, CIValuesRepository>(Lifestyle.Scoped);
            container.Register<ICIVariablesRepository, CIVariablesRepository>(Lifestyle.Scoped);
            container.Register<ICIAssociatesRepository, CIAssociatesRepository>(Lifestyle.Scoped);
            container.Register<ICIVirtualRepository, CIVirtualRepository>(Lifestyle.Scoped);
            container.Register<ICIRegistryRepository, CIRegistryRepository>(Lifestyle.Scoped);
        }
    }
}
