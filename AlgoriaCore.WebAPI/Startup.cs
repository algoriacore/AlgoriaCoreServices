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
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using AlgoriaCore.Application.Managers.Chat.ChatMessages;
using AlgoriaCore.Application.Managers.Chat.Friendships;
using AlgoriaCore.Application.Pipeline;
using AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.Interfaces.CSV;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Domain.Interfaces.Email;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.Exceptions;
using AlgoriaCore.Domain.Interfaces.FileStorage;
using AlgoriaCore.Domain.Interfaces.Folder;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Interfaces.MultiTenancy;
using AlgoriaCore.Domain.Interfaces.PDF;
using AlgoriaCore.Domain.Interfaces.Token;
using AlgoriaCore.Domain.MultiTenancy;
using AlgoriaCore.Domain.Session;
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
using Autofac.Extensions.DependencyInjection;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI
{
    public class Startup
    {
        //FB To log at starupt phase
        //public Startup(IConfiguration configuration)
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método se llama en tiempo de ejecución. Usa este método para agregar servicios al contenedor.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddTransient(typeof(IRepository<,>), typeof(BaseRepository<,>));
            services.AddTransient(typeof(IRepositoryMongoDb<>), typeof(RepositoryMongoDb<>));

            // Agregar MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddValidatorsAsTransient(typeof(RoleGetByIdValidator).Assembly);
            
            services.AddTransient(typeof(ICoreServices), typeof(CoreServices));

            // Servicios SCOPED (Session y UnitOfWork)
            // Administración de sesiones (User Id, Tenant Id, etc.)
            services.AddScoped<IAppSession, SessionContext>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IMongoUnitOfWork), typeof(MongoUnitOfWork));
            services.AddScoped(typeof(IClock), typeof(Clock));
            services.AddScoped(typeof(ISqlExecuter), typeof(SqlExecuter));

            // Configurar objetos de configuración fuertemente tipeados
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Configurar autenticación jwt
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
                // Tenemos que enganchar el evento OnMessageReceived para poder
                // permitir que el controlador de autenticación JWT lea el token
                // de acceso de la cadena de consulta cuando un WebSocket o
                // solicitud de envetos enviados por el servidor entra.
                options.Events = GetEvents();

			});

            // Bases de datos
            switch (appSettings.DatabaseType)
            {
                case DatabaseType.MySql:
                    // Agregar DbContext usando MySQL Server Provider
                    //services.AddDbContext<AlgoriaCoreDbContext>(options =>
                    //    options.UseMySql(Configuration.GetConnectionString("MySqlAlgoriaCoreDatabase"),
                    //    ServerVersion.AutoDetect(Configuration.GetConnectionString("MySqlAlgoriaCoreDatabase")),
                    //    mySqlOptions => mySqlOptions.CommandTimeout(appSettings.DatabaseCommandTimeout)));
                    break;
                case DatabaseType.Postgres:
					// Agregar DbContext usando SQL Server Provider
					services.AddDbContext<AlgoriaCoreDbContext>(options =>
						options.UseNpgsql(Configuration.GetConnectionString("PostgresAlgoriaCoreDatabase"),
						sqlServerOptions => sqlServerOptions.CommandTimeout(appSettings.DatabaseCommandTimeout)));

					// La siguiente línea evita el error:
					// Cannot write DateTime with Kind=UTC to PostgreSQL type 'timestamp without time zone'
					AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
					break;
                default:
                    // Agregar DbContext usando SQL Server Provider
                    services.AddDbContext<AlgoriaCoreDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("AlgoriaCoreDatabase"),
                        sqlServerOptions => sqlServerOptions.CommandTimeout(appSettings.DatabaseCommandTimeout));
                        options.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
                    });
                    break;
            }

            services.Configure<MongoDbOptions>(Configuration.GetSection("MongoDbSettings"));
            services.AddSingleton<IMongoDBContext, MongoDBContext>();

            // Servicios de correo electrónico
            // Implementa TestEmailService con modo TEST
            var emailSettingsSection = Configuration.GetSection("Email");
            services.Configure<EmailOptions>(emailSettingsSection);

            var emailOptions = emailSettingsSection.Get<EmailOptions>();

            if (emailOptions.SendMethod == EmailSendMethod.Grpc)
            {
                services.AddScoped<ITokenService, TokenGrpcService>();
                services.AddScoped<IEmailService, EmailGrpcService>();
            } else
            {
                services.AddTransient<IEmailService, EmailService>();
            }

            // Servicios de almacenamiento de archivos
            var fileStorageSettingsSection = Configuration.GetSection("FileStorage");
            services.Configure<FileStorageOptions>(fileStorageSettingsSection);
            var fileStorageOptions = fileStorageSettingsSection.Get<FileStorageOptions>();

            if (fileStorageOptions.StorageMethod == FileStorageMethod.S3)
            {
                services.AddScoped<IFileStorageService, FileStorageS3Service>();
            }
            else if (fileStorageOptions.StorageMethod == FileStorageMethod.Normal)
            {
                services.AddTransient<IFileStorageService, FileStorageLocalService>();
            }

            //Servicio NLog
            services.AddTransient<ICoreLogger, CoreNLogger>();
            //Servicio Excel
            services.AddTransient<IExcelService, EpPlusExcelService>();
            //Servicio CSV
            services.AddTransient<ICSVService, EpPlusCSVService>();
            //Servicio PDF
            services.AddTransient<IPDFService, QuestPDFService>();

            //Servicio proveedor de autorización
            services.AddSingleton<IAppAuthorizationProvider, AppAuthorizationProvider>();
            //Servicios de lenguajes (idiomas)
            services.AddScoped<IAppLocalizationProvider, AppLocalizationProvider>();
            //Servicio de registro de cambios (registro los cambios en un registro)
            services.AddTransient<IChangeLogService, ChangeLogService>();
            //Servicios de carpetas Web Log
            services.AddSingleton<IAppFolders, AppFolders>();
            //Servicios de exceptiones (utilidad para lanzar alunas exceptiones personalizadas)
            services.AddTransient<IExceptionService, ExceptionService>();
            //Servicio del caché de idiomas
            services.AddSingleton<ICacheLanguageService, CacheLanguageService>();
            //Servicio del caché de idiomas XML
            services.AddSingleton<ICacheLanguageXmlService, CacheLanguageXmlService>();
            // Configuración de múltiples tenants
            services.AddSingleton<IMultiTenancyConfig, MultiTenancyConfig>();

            services.AddSingleton<IChatUserStateWatcher, ChatUserStateWatcher>();
            services.AddSingleton<IOnlineClientManager, OnlineClientManager>();
            services.AddTransient<IChatMessageManager, ChatMessageManager>();
            services.AddTransient<IChatCommunicator, SignalRChatCommunicator>();
            services.AddSingleton<IFriendshipManager, FriendshipManager>();
            services.AddTransient<CatalogoCustomImplLizzieContext>();

            services.AddSignalR();

            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

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
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddApplicationPart(typeof(BaseController).Assembly)
                .ConfigureApiBehaviorOptions(setup =>
                {
                    setup.SuppressModelStateInvalidFilter = true;
                });

            // Con ayuda de: https://github.com/RSuter/NSwag/wiki/AspNetCore-Middleware
            services.AddSwaggerDocument(document =>
            {
                document.PostProcess = v1 =>
                {
                    v1.Info.Title = "Algoria Core 250618";
                    v1.Info.Version = "1.0.0";
                    v1.Info.Description = "DatabaseType: " + (appSettings.DatabaseType == DatabaseType.MySql ? "MySql" : "SQL Server");
                };
            });

            //Configurar Autofac IoC
            // Crear el constrctor del contenedor.
            var builder = new ContainerBuilder();

            builder.RegisterMediatR(MediatRConfigurationBuilder.Create(typeof(RoleGetByIdQuery).GetTypeInfo().Assembly).WithAllOpenGenericHandlerTypesRegistered().Build());
            builder.RegisterMediatR(MediatRConfigurationBuilder.Create(typeof(RequestLogger<>).GetTypeInfo().Assembly).WithAllOpenGenericHandlerTypesRegistered().Build());

            builder.AddManagers(typeof(IBaseManager).Assembly);

            builder.Populate(services);

            var container = builder.Build();

            //Regresar Autofac IoC
            return new AutofacServiceProvider(container);
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

        // Este método se llama en tiempo de ejecución. Usa este´método para configurar la tubería de peticiones HTTP.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SetAppFolders(app, env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // El valor predeterminado HSTS es 30 días. Podrías quere cambiarlo para ambientes de producción, ver https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();

            app.UseCors(builder => 
            {
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                //.WithOrigins("http://localhost:4200")
                .WithOrigins(string.IsNullOrWhiteSpace(appSettings.CORSOrigins) ? Array.Empty<string>() : appSettings.CORSOrigins.Split(",").Select(p => p.Trim()).ToArray())
                .SetIsOriginAllowedToAllowWildcardSubdomains();
            });

            app.UseStaticFiles();
            app.UseOpenApi();

            // Para un correcto funcionamiento en la aplicación virtual de IIS 
            app.UseSwaggerUi(config => config.TransformToExternalPath = (internalUiRoute, request) =>
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
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(options =>
            {
                options.MapHub<ChatHub>("/signalr");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var chatUserStateWatcher = app.ApplicationServices.GetService<IChatUserStateWatcher>();
            chatUserStateWatcher.Initialize();

            app.ApplicationServices.GetService<IMongoDBContext>().CheckConnection();
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
