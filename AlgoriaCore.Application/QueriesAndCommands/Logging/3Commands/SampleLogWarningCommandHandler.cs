using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Logging._3Commands
{
    public class SampleLogWarningCommandHandler : BaseCoreClass, IRequestHandler<SampleLogWarningCommand, bool>
    {

        public SampleLogWarningCommandHandler(ICoreServices coreServices) : base(coreServices)
        {
        }

        public async Task<bool> Handle(SampleLogWarningCommand request, CancellationToken cancellationToken)
        {
            var dict = new Dictionary<string, string>();

            dict.Add("ServiceName", GetType().FullName);
            dict.Add("MethodName", "Handle");
            dict.Add("Parameters", JsonConvert.SerializeObject(request));
            dict.Add("Severity", ((int)LogLevel.Warning).ToString()); // Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5

            CoreLogger.LogWarning(request.Message, dict);

            return await Task.FromResult(true);
        }
    }
}
