using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Logging._3Commands
{
    public class SampleLogCriticalCommandHandler : BaseCoreClass, IRequestHandler<SampleLogCriticalCommand, bool>
    {

        public SampleLogCriticalCommandHandler(ICoreServices coreServices) : base(coreServices)
        {
        }

        public async Task<bool> Handle(SampleLogCriticalCommand request, CancellationToken cancellationToken)
        {
            var ex = new AlgoriaCoreGeneralException(L("Examples.Log.CriticalExceptionMessage"));
            var dict = new Dictionary<string, string>();

            dict.Add("ServiceName", GetType().FullName);
            dict.Add("MethodName", "Handle");
            dict.Add("Parameters", JsonConvert.SerializeObject(request));
            dict.Add("Exception", ex.ToString());
            dict.Add("Severity", ((int)LogLevel.Critical).ToString()); // Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5
			
            dict["EmailTo"] = request.Email;
            dict["EmailBody"] = L("Examples.Log.CriticalEmailBody", request.Message);

            CoreLogger.LogCritical(ex, request.Message, dict);

            return await Task.FromResult(true);
        }
    }
}
