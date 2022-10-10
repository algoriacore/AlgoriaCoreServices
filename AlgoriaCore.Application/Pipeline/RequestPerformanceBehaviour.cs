using AlgoriaCore.Application.Configuration;
using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Domain.Attributes;
using AlgoriaCore.Domain.Exceptions.Abstracts;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;
using AlgoriaPersistence.Interfaces.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Pipeline
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly IAppSession _session;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoreLogger _coreLogger;
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly IMongoDBContext _context;

        private readonly MongoDbOptions _mongoDbOptions;

        public RequestPerformanceBehaviour(IAppSession session, 
            IUnitOfWork unitOfWork,
            ICoreLogger coreLogger,
            IMongoUnitOfWork mongoUnitOfWork,
            IOptions<MongoDbOptions> mongoDbOptions,
            IMongoDBContext context
            )
        {
            _timer = new Stopwatch();
            _session = session;
            _unitOfWork = unitOfWork;
            _coreLogger = coreLogger;
            _mongoUnitOfWork = mongoUnitOfWork;
            _mongoDbOptions = mongoDbOptions.Value;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();
            TResponse response = default(TResponse);

            var name = typeof(TRequest).Name;
            var auditableAttribute = typeof(TRequest).GetCustomAttributes(typeof(AuditableAttribute), true).Select(t => ((AuditableAttribute)t)).FirstOrDefault();
            
            var isMongoTransactional = _mongoDbOptions.IsEnabled && _context.IsActive && _mongoDbOptions.IsTransactional
            && typeof(TRequest).GetCustomAttributes(typeof(MongoTransactionalAttribute), true).Select(t => ((MongoTransactionalAttribute)t)).Any();
            
            var dict = new Dictionary<string, string>();
            dict.Add("ServiceName", typeof(TRequest).FullName);
            dict.Add("MethodName", typeof(TRequest).Name);
            dict.Add("Parameters", JsonConvert.SerializeObject(request));
            dict.Add("ExecutionDuration", null);
            dict.Add("Exception", null);
            dict.Add("Severity", ((int)LogLevel.Information).ToString()); // Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5

            try
            {
                // Inicia la transacción
                _unitOfWork.Begin();

                if (isMongoTransactional)
                {
                    _mongoUnitOfWork.Begin();
                }

                response = await next();

                if (isMongoTransactional)
                {
                    _mongoUnitOfWork.Commit();
                }

                // Se confirma la transacción
                _unitOfWork.Commit();
            }
            catch (AlgoriaCoreException ae)
            {
                if (isMongoTransactional)
                {
                    _mongoUnitOfWork.Rollback();
                }

                // Ocurrió un error. Se deshace la transacción.
                _unitOfWork.Rollback();

                dict["ExecutionDuration"] = _timer.ElapsedMilliseconds.ToString();
                dict["Exception"] = ae.ToString();
                dict["Severity"] = ((int)LogLevel.Error).ToString();

                if (ae is AlgoriaWarningException)
                {
                    dict["Severity"] = ((int)LogLevel.Warning).ToString();
                    _coreLogger.LogWarning(ae, ae.Message, dict);
                }
                else if (ae is AlgoriaErrorException)
                {
                    dict["Severity"] = ((int)LogLevel.Error).ToString();
                    _coreLogger.LogError(ae, ae.Message, dict);
                }
                else if (ae is AlgoriaCriticalException)
                {
                    dict["Severity"] = ((int)LogLevel.Critical).ToString();
                    dict["EmailTo"] = "fbeltran@algoria.com.mx";
                    dict["EmailBody"] = "Administrador ha ocurrido un error grave. " + ae.Message;
                    _coreLogger.LogCritical(ae, ae.Message, dict);
                }
                else
                {
                    _coreLogger.LogError(ae, string.Format("LOG: ERROR: {0} ({1} milliseconds) user {2}", name, _timer.ElapsedMilliseconds, _session.UserName), dict);
                }

                throw;
            }
            catch (Exception e)
            {
                if (isMongoTransactional)
                {
                    _mongoUnitOfWork.Rollback();
                }

                // Ocurrió un error. Se deshace la transacción.
                _unitOfWork.Rollback();

                dict["ExecutionDuration"] = _timer.ElapsedMilliseconds.ToString();
                dict["Exception"] = e.ToString();
                dict["Severity"] = ((int)LogLevel.Error).ToString();

                _coreLogger.LogError(e, string.Format("LOG: ERROR: {0} ({1} milliseconds) user {2}", name, _timer.ElapsedMilliseconds, _session.UserName), dict);

                throw;
            }

            _timer.Stop();

            if (auditableAttribute == null || auditableAttribute.IsAuditable)
            {
                dict["ExecutionDuration"] = _timer.ElapsedMilliseconds.ToString();


                if (_timer.ElapsedMilliseconds > 500)
                {
                    dict["Severity"] = ((int)LogLevel.Warning).ToString();
                    _coreLogger.LogWarning(string.Format("Long Running Request: {0} ({1} milliseconds) user {2}", name, _timer.ElapsedMilliseconds, _session.UserName), dict);
                }
                else
                {
                    _coreLogger.LogInformation(string.Format("LOG: Everything OK: {0} ({1} milliseconds) user {2}", name, _timer.ElapsedMilliseconds, _session.UserName), dict);
                }
            }

            return response;
        }
    }
}
