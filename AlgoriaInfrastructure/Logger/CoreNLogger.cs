using AlgoriaCore.Application.Configuration;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using NLog.AWS.Logger;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace AlgoriaInfrastructure.Logger
{
    public class CoreNLogger : ICoreLogger
    {
        private readonly IUnitOfWork _currentUnitOfWork;
        private readonly IAppSession _session;
        private readonly IClock _iclock;
        private readonly FileStorageOptions _logsOptions;

        public CoreNLogger(
            IUnitOfWork currentUnitOfWork,
            IAppSession session,
            IClock iclock,
            IOptions<FileStorageOptions> logsOptions
        )
        {
            _currentUnitOfWork = currentUnitOfWork;
            _session = session;
            _iclock = iclock;
            _logsOptions = logsOptions.Value;
        }

        public void LogCritical(string message, object parameters)
        {
            Log(null, LogLevel.Fatal, message, null, parameters);
        }

        public void LogCritical(string message, Dictionary<string, string> args = null)
        {
            Log(null, LogLevel.Fatal, message, args);
        }

        public void LogCritical(Exception exception, string message, Dictionary<string, string> args = null)
        {
            Log(exception, LogLevel.Fatal, message, args);
        }


        public void LogDebug(string message, object parameters)
        {
            Log(null, LogLevel.Debug, message, null, parameters);
        }

        public void LogDebug(string message, Dictionary<string, string> args = null)
        {
            Log(null, LogLevel.Debug, message, args);
        }

        public void LogDebug(Exception exception, string message, Dictionary<string, string> args = null)
        {
            Log(exception, LogLevel.Debug, message, args);
        }


        public void LogError(string message, object parameters)
        {
            Log(null, LogLevel.Error, message, null, parameters);
        }

        public void LogError(string message, Dictionary<string, string> args = null)
        {
            Log(null, LogLevel.Error, message, args);
        }

        public void LogError(Exception exception, string message, Dictionary<string, string> args = null)
        {
            Log(exception, LogLevel.Error, message, args);
        }


        public void LogInformation(string message, object parameters)
        {
            Log(null, LogLevel.Info, message, null, parameters);
        }

        public void LogInformation(string message, Dictionary<string, string> args = null)
        {
            Log(null, LogLevel.Info, message, args);
        }

        public void LogInformation(Exception exception, string message, Dictionary<string, string> args = null)
        {
            Log(exception, LogLevel.Info, message, args);
        }


        public void LogTrace(string message, object parameters)
        {
            Log(null, LogLevel.Trace, message, null, parameters);
        }

        public void LogTrace(string message, Dictionary<string, string> args = null)
        {
            Log(null, LogLevel.Trace, message, args);
        }

        public void LogTrace(Exception exception, string message, Dictionary<string, string> args = null)
        {
            Log(exception, LogLevel.Trace, message, args);
        }


        public void LogWarning(string message, object parameters)
        {
            Log(null, LogLevel.Warn, message, null, parameters);
        }

        public void LogWarning(string message, Dictionary<string, string> args = null)
        {
            Log(null, LogLevel.Warn, message, args);
        }

        public void LogWarning(Exception exception, string message, Dictionary<string, string> args = null)
        {
            Log(exception, LogLevel.Warn, message, args);
        }

        public virtual void Log(Exception exception, LogLevel logLevel, string message, Dictionary<string, string> args, object parameters = null)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            NLog.LogEventInfo logEventInfo = new NLog.LogEventInfo(logLevel, "", message);
            logEventInfo.Exception = exception;

            #region AWS LOGS
            if (_logsOptions.StorageMethod == FileStorageMethod.S3)
            {
                var config = new LoggingConfiguration();
                string accessKey = _logsOptions.CloudWatch.Id;
                string secretKey = _logsOptions.CloudWatch.SecretKey;
                var awsTarget = new AWSTarget()
                {
                    LogGroup = _logsOptions.CloudWatch.LogGroup,
                    Region = _logsOptions.CloudWatch.Region,
                    Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey)

                };
                config.AddTarget("aws", awsTarget);
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, awsTarget));
                LogManager.Configuration = config;
            }
            #endregion

            var newDict = GetDictionary(args);

            newDict["Parameters"] = parameters == null ? newDict["Parameters"] : JsonConvert.SerializeObject(parameters);
            newDict["CustomData"] = message == null ? newDict["CustomData"] : message;
            newDict["Exception"] = exception == null ? newDict["Exception"] : exception.ToString();

            foreach (var arg in newDict)
            {
                logEventInfo.Properties.Add(arg.Key, arg.Value);
            }

            logger.Log(logEventInfo);
        }

        public Dictionary<string, string> GetDictionary(Dictionary<string, string> args)
        {
            var dict = new Dictionary<string, string>();

            int? tenantId = _currentUnitOfWork.GetTenantId();

            if (!tenantId.HasValue)
            {
                tenantId = _session.TenantId;
            }

            dict.Add("TenantId", tenantId.HasValue ? tenantId.ToString() : null);
            dict.Add("UserName", _session.UserName);
            dict.Add("UserId", _session.UserId.HasValue ? _session.UserId.ToString() : null);
            dict.Add("ImpersonalizerUserId", _session.ImpersonalizerUserId.HasValue ? _session.ImpersonalizerUserId.ToString() : null);
            dict.Add("ServiceName", null);
            dict.Add("MethodName", null);
            dict.Add("Parameters", null);
            dict.Add("ExecutionDatetime", _iclock.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            dict.Add("ExecutionDuration", null);
            dict.Add("ClientIpAddress", null);
            dict.Add("ClientName", null);
            dict.Add("BrowserInfo", null);
            dict.Add("Exception", null);
            dict.Add("CustomData", null);
            dict.Add("Severity", null); // Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5

            if (args == null || !args.ContainsKey("ServiceName") || !args.ContainsKey("MethodName"))
            {
                StackTrace stackTrace = new StackTrace();
                StackFrame stackFrame = stackTrace.GetFrame(3);
                MethodBase methodBase = stackFrame.GetMethod();
                Type type = methodBase.ReflectedType;
                string name = type.Name.StartsWith("<") ? type.Name.Substring(1, type.Name.IndexOf(">") - 1) : type.Name;

                dict["ServiceName"] = type.DeclaringType.FullName;
                dict["MethodName"] = name;
            }

            if (args != null)
            {
                foreach (var arg in args)
                {
                    if (dict.ContainsKey(arg.Key))
                    {
                        dict[arg.Key] = arg.Value;
                    }
                    else
                    {
                        dict.Add(arg.Key, arg.Value);
                    }
                }
            }

            return dict;
        }

    }
}
