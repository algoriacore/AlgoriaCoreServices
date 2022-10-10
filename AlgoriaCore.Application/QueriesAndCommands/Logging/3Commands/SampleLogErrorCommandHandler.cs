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
    public class SampleLogErrorCommandHandler : BaseCoreClass, IRequestHandler<SampleLogErrorCommand, bool>
    {

        public SampleLogErrorCommandHandler(ICoreServices coreServices) : base(coreServices)
        {
        }

        public async Task<bool> Handle(SampleLogErrorCommand request, CancellationToken cancellationToken)
        {
            var ex = new AlgoriaCoreGeneralException(L("Examples.Log.ErrorExceptionMessage"));
            var dict = new Dictionary<string, string>();

            dict.Add("ServiceName", GetType().FullName);
            dict.Add("MethodName", "Handle");
            dict.Add("Parameters", JsonConvert.SerializeObject(request));
            dict.Add("Exception", ex.ToString());
            dict.Add("Severity", ((int)LogLevel.Error).ToString()); // Trace = 0, Debug = 1, Information = 2, Warning = 3, Error = 4, Critical = 5

            CoreLogger.LogError(ex, request.Message, dict);

            return await Task.FromResult(true);
        }
    }
}
