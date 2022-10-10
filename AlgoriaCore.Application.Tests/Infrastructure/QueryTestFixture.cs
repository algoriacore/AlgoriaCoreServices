using AlgoriaCore.Application.Authorization;
using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Date;
using AlgoriaCore.Application.Exceptions;
using AlgoriaCore.Application.Folders;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Pipeline;
using AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Domain.Interfaces.Email;
using AlgoriaCore.Domain.Interfaces.Exceptions;
using AlgoriaCore.Domain.Interfaces.Folder;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Interfaces.MultiTenancy;
using AlgoriaCore.Domain.MultiTenancy;
using AlgoriaCore.Domain.Session;
using AlgoriaInfrastructure.Email;
using AlgoriaInfrastructure.Logger;
using AlgoriaPersistence;
using AlgoriaPersistence.Interfaces.Interfaces;
using AlgoriaPersistence.Repositories;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AlgoriaCore.Application.Tests.Infrastructure
{
    public class QueryTestFixture : IDisposable
    {
        public AlgoriaCoreDbContext Context { get; private set; }
        public IMediator Mediator;
        private readonly IServiceProvider Provider;
        public ICoreLogger Logger;

        public QueryTestFixture()
        {
            Provider = BuildMediator();
            Context = Provider.GetService<AlgoriaCoreDbContext>();
            Mediator = Provider.GetRequiredService<IMediator>();
            Logger = Provider.GetRequiredService<ICoreLogger>();
            AlgoriaCoreInitializer.Initialize(Context);
        }

        public IAppSession GetSessionContext(int? tenantId, long userId)
        {
            User user = Context.User.First(p => p.Id == userId && p.TenantId == tenantId);

            return GetSessionContext(user);
        }

        public IAppSession GetSessionContextHost()
        {
            User user = Context.User.First(p => p.UserLogin == "admin" && p.TenantId == null);

            return GetSessionContext(user);
        }

        public IAppSession GetSessionContextTenantDefault()
        {
            User user = Context.User.First(p => p.UserLogin == "admin" && p.Tenant.TenancyName.ToLower() == "default");

            return GetSessionContext(user);
        }

        public IAppSession GetSessionContext(User user)
        {
            IAppSession SessionContext = Provider.GetService<IAppSession>();
            SessionContext.TenantId = user.TenantId;

            if (user.Tenant != null)
            {
                SessionContext.TenancyName = user.Tenant.TenancyName;
            }

            SessionContext.UserId = user.Id;
            SessionContext.UserName = string.Format("{0} {1} {2}", user.Name, user.Lastname, user.SecondLastname).Trim();

            return SessionContext;
        }

        public UnitOfWork GetUnitOfWork()
        {
            return Provider.GetService<IUnitOfWork>() as UnitOfWork;

        }

		protected virtual void Dispose(bool disposing)
		{
			// Implementación del patrón IDisposable
			AlgoriaCoreContextFactory.Destroy(Context);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private static IServiceProvider BuildMediator()
        {
            IConfiguration Configuration = GetIConfigurationRoot();

            var services = new ServiceCollection();

            services.AddLogging(logging => logging.AddConsole());
            services.AddLogging(logging => logging.AddDebug());
            services.AddLogging(logging => logging.AddEventLog());

            services.AddSingleton(Configuration);
            services.AddTransient(typeof(IRepository<,>), typeof(BaseRepository<,>));

            // Agrega MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddValidatorsAsTransient(typeof(RolGetByIdValidator).Assembly);

            services.AddTransient(typeof(ICoreServices), typeof(CoreServices));

            services.AddScoped<IAppSession, SessionContextTest>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IClock), typeof(Clock));

            // Agrega DbContext usando el proveedor de SQL Server
            services.AddDbContext<AlgoriaCoreDbContext>(options => 
                options.UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            );
			
            services.AddTransient<IEmailService, TestEmailService>();

            services.AddTransient<ICoreLogger, CoreNLogger>();

            //Servicio proveedor de autorización
            services.AddSingleton<IAppAuthorizationProvider, AppAuthorizationProvider>();
            //Servicios de idiomas Localization (idiomas)
            services.AddScoped<IAppLocalizationProvider, AppLocalizationProvider>();
            //Servicio de log de cambios (registar los cambios en los registros)
            services.AddTransient<IChangeLogService, ChangeLogService>();
            //Servicios de carpetas de Web Log
            services.AddSingleton<IAppFolders, AppFolders>();
            //Servicios de exceptiones (utilidad para lanzar alunas exceptiones)
            services.AddTransient<IExceptionService, ExceptionService>();
            //Servicio del caché de idiomas
            services.AddSingleton<ICacheLanguageService, CacheLanguageService>();
            //Servicio del caché  de idiomas en XMLs
            services.AddSingleton<ICacheLanguageXmlService, CacheLanguageXmlService>();
            // Configuración múltiples tenants
            services.AddSingleton<IMultiTenancyConfig, MultiTenancyConfig>();

            //Configuar Autofac IoC
            // Crear el constructor del contenedor.
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterMediatR(typeof(RolGetByIdQuery).GetTypeInfo().Assembly);
            builder.RegisterMediatR(typeof(RequestLogger<>).GetTypeInfo().Assembly);

            builder.AddManagers(typeof(IBaseManager).Assembly);

            var container = builder.Build();

            //Regresar Autofac IoC
            return new AutofacServiceProvider(container);
        }

        private static IConfigurationRoot GetIConfigurationRoot()
        {
            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            basePath = basePath.Replace("file:\\", "").Replace("file:", "");

            string fileName = "appsettings.json";

            if (File.Exists(Path.Combine(basePath, "appsettings.Development.json")))
            {
                fileName = "appsettings.Development.json";
            }

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(fileName, optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }

    [CollectionDefinition("TestsCollection")]
    public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}