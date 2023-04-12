using AlgoriaCore.Application.Authorization;
using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Chat;
using AlgoriaCore.Application.Configuration;
using AlgoriaCore.Application.Date;
using AlgoriaCore.Application.Exceptions;
using AlgoriaCore.Application.Folders;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.Chat.ChatMessages;
using AlgoriaCore.Application.Managers.Chat.Friendships;
using AlgoriaCore.Application.Pipeline;
using AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Domain.Interfaces.Email;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.Exceptions;
using AlgoriaCore.Domain.Interfaces.FileStorage;
using AlgoriaCore.Domain.Interfaces.Folder;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Interfaces.MultiTenancy;
using AlgoriaCore.Domain.Interfaces.Token;
using AlgoriaCore.Domain.MultiTenancy;
using AlgoriaCore.Domain.Session;
using AlgoriaCore.Extensions.Reflection;
using AlgoriaCore.WebAPI.Middleware;
using AlgoriaCore.WebUI.Chat.SignalR;
using AlgoriaCore.WebUI.Controllers;
using AlgoriaCore.WebUI.Filters;
using AlgoriaCore.WebUI.Helpers;
using AlgoriaInfrastructure.Email;
using AlgoriaInfrastructure.Excel;
using AlgoriaInfrastructure.FileStorage;
using AlgoriaInfrastructure.Logger;
using AlgoriaPersistence;
using AlgoriaPersistence.Interfaces.Interfaces;
using AlgoriaPersistence.Repositories;
using AlgoriaPersistence.SqlExecuter;
using AlgoriaPersistence.UOW;
using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore5.WebAPI.Lambda
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            // In ASP.NET Core 3.0 `env` will be an IWebHostEnvironment, not IHostingEnvironment.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; private set; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddTransient(typeof(IRepository<,>), typeof(BaseRepository<,>));

            // Add MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddValidatorsAsTransient(typeof(RoleGetByIdValidator).Assembly);

            services.AddTransient(typeof(ICoreServices), typeof(CoreServices));

            //SCOPED services (Session and UnitOfWork)
            //Session management (User Id, Tenant Id, etc.)
            services.AddScoped<IAppSession, SessionContext>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IClock), typeof(Clock));
            services.AddScoped(typeof(ISqlExecuter), typeof(SqlExecuter));

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = appSettings.ValidateIssuer,
                    ValidateAudience = appSettings.ValidateAudience,
                    ValidateLifetime = appSettings.ValidateLifetime,
                    ValidateIssuerSigningKey = appSettings.ValidateIssuerSigningKey,

                    ValidIssuer = appSettings.ValidIssuer,
                    ValidAudience = appSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.IssuerSigningKey))
                };

                // PZ 20190218: https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-2.2
                // We have to hook the OnMessageReceived event in order to
                // allow the JWT authentication handler to read the access
                // token from the query string when a WebSocket or 
                // Server-Sent Events request comes in.
                options.Events = GetEvents();

            });

            switch (appSettings.DatabaseType)
            {
                case DatabaseType.MySql:
                    // Add DbContext using MySQL Server Provider
                    //services.AddDbContext<AlgoriaCoreDbContext>(options =>
                    //    options.UseMySql(Configuration.GetConnectionString("MySqlAlgoriaCoreDatabase"),
                    //    ServerVersion.AutoDetect(Configuration.GetConnectionString("MySqlAlgoriaCoreDatabase")),
                    //    mySqlOptions => mySqlOptions.CommandTimeout(appSettings.DatabaseCommandTimeout)));
                    break;
                default:
                    // Add DbContext using SQL Server Provider
                    services.AddDbContext<AlgoriaCoreDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("AlgoriaCoreDatabase"),
                        sqlServerOptions => sqlServerOptions.CommandTimeout(appSettings.DatabaseCommandTimeout)));
                    break;
            }

            // Mail services
            // Implements TestEmailService when TEST mode
            var emailSettingsSection = Configuration.GetSection("Email");
            services.Configure<EmailOptions>(emailSettingsSection);

            var emailOptions = emailSettingsSection.Get<EmailOptions>();

            if (emailOptions.SendMethod == EmailSendMethod.Grpc)
            {
                services.AddScoped<ITokenService, TokenGrpcService>();
                services.AddScoped<IEmailService, EmailGrpcService>();
            }
            else
            {
                services.AddTransient<IEmailService, EmailService>();
            }


            // File storage services
            var fileStorageSettingsSection = Configuration.GetSection("FileStorage");
            services.Configure<FileStorageOptions>(fileStorageSettingsSection);
            var fileStorageOptions = fileStorageSettingsSection.Get<FileStorageOptions>();

            if (fileStorageOptions.StorageMethod == FileStorageMethod.S3)
            {
                services.AddScoped<IFileStorageService, FileStorageS3Service>();
            }
            else if(fileStorageOptions.StorageMethod == FileStorageMethod.Normal)
            {
                services.AddTransient<IFileStorageService, FileStorageLocalService>();
            }

            //NLog service
            services.AddTransient<ICoreLogger, CoreNLogger>();


            //Excel service
            services.AddTransient<IExcelService, EpPlusExcelService>();

            //Authorization provider service
            services.AddSingleton<IAppAuthorizationProvider, AppAuthorizationProvider>();
            //Localization (languages) services
            services.AddScoped<IAppLocalizationProvider, AppLocalizationProvider>();
            //Change log service (Log record changes)
            services.AddTransient<IChangeLogService, ChangeLogService>();
            //Web Log folder service
            services.AddSingleton<IAppFolders, AppFolders>();
            //Exceptions service (helper for throwing some custom exceptions)
            services.AddTransient<IExceptionService, ExceptionService>();
            //Cache Language Service
            services.AddSingleton<ICacheLanguageService, CacheLanguageService>();
            //Cache Language Service XMLs
            services.AddSingleton<ICacheLanguageXmlService, CacheLanguageXmlService>();
            // MultiTenancy Config
            services.AddSingleton<IMultiTenancyConfig, MultiTenancyConfig>();

            services.AddSingleton<IChatUserStateWatcher, ChatUserStateWatcher>();
            services.AddSingleton<IOnlineClientManager, OnlineClientManager>();
            services.AddTransient<IChatMessageManager, ChatMessageManager>();
            services.AddTransient<IChatCommunicator, SignalRChatCommunicator>();
            services.AddSingleton<IFriendshipManager, FriendshipManager>();

            services.AddSignalR();

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(SessionFilter));
                    options.Filters.Add(typeof(ValidateModelAttribute));
                    options.Filters.Add(typeof(AlgoriaCoreAuthorizationFilterAttribute));

                    options.EnableEndpointRouting = false;
                }
                 )
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
                .AddApplicationPart(typeof(BaseController).Assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .ConfigureApiBehaviorOptions(setup =>
                {
                    setup.SuppressModelStateInvalidFilter = true;
                });

            // Con ayuda de: https://github.com/RSuter/NSwag/wiki/AspNetCore-Middleware
            services.AddSwaggerDocument(document =>
            {
                document.PostProcess = v1 =>
                {
                    v1.Info.Title = "Algoria Core Lambda 210105";
                    v1.Info.Version = "1.0.0";
                    v1.Info.Description = "DatabaseType: " + (appSettings.DatabaseType == DatabaseType.MySql ? "MySql" : "SQL Server");
                };
            });

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //configure auto fac here
            builder.RegisterMediatR(typeof(RoleGetByIdQuery).GetTypeInfo().Assembly);
            builder.RegisterMediatR(typeof(RequestLogger<>).GetTypeInfo().Assembly);
            builder.AddManagers(typeof(IBaseManager).Assembly);

        }

        private JwtBearerEvents GetEvents()
        {
            return new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;

                    if (path.StartsWithSegments("/signalr"))
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        System.Diagnostics.Debug.WriteLine("OnMessageReceived: " + context.Token);
                    }

                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context => {

                    var path = context.HttpContext.Request.Path;
                    if (path.StartsWithSegments("/signalr"))
                    {
                        System.Diagnostics.Debug.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                    }

                    return Task.CompletedTask;
                },

                OnTokenValidated = context => {
                    var path = context.HttpContext.Request.Path;
                    if (path.StartsWithSegments("/signalr"))
                    {
                        System.Diagnostics.Debug.WriteLine("OnTokenValidated: " + context.SecurityToken);
                    }

                    return Task.CompletedTask;
                }
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SetAppFolders(app, env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); 

            app.UseStaticFiles();

            app.UseOpenApi();

            var chatUserStateWatcher = app.ApplicationServices.GetService<IChatUserStateWatcher>();
            chatUserStateWatcher.Initialize();

            // For correct functionality in IIS virtual application
            app.UseSwaggerUi3(config => config.TransformToExternalPath = (internalUiRoute, request) =>
            {
                if (internalUiRoute.StartsWith("/") && !internalUiRoute.StartsWith(request.PathBase))
                {
                    return request.PathBase + internalUiRoute;
                }
                else
                {
                    return internalUiRoute;
                }
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void SetAppFolders(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IConfigurationSection configurationSection = Configuration.GetSection("AppFolders");
            AppFolders appFolders = (AppFolders)app.ApplicationServices.GetService<IAppFolders>();

            appFolders.TempFileDownloadFolder = configurationSection.GetValue<string>("Temp");
            appFolders.WebLogsFolder = configurationSection.GetValue<string>("Logs");
            appFolders.TemplatesFolder = Path.Combine(env.ContentRootPath, string.Format("Content{0}templates", Path.DirectorySeparatorChar));

            try
            {
                DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.WebLogsFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.TemplatesFolder);
            }
            catch
            {
                // No se debe reportar el error en caso de que no se pueda crear la carpeta de archivos temporales
            }
        }
    }
}
