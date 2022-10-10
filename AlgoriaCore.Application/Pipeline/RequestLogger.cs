using AlgoriaCore.Domain.Session;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Pipeline
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly IAppSession _session;

        public RequestLogger(ILogger<TRequest> logger, IAppSession session)
        {
            _logger = logger;
            _session = session;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            var json = JsonConvert.SerializeObject(request);
            _logger.LogWarning("LOG PreProcessor: Request -> {Name} {@Request} User {UserName}", name, request, _session.UserName);
            _logger.LogWarning("LOG JSON -> {json}  User {UserName}", json, _session.UserName);

            return Task.CompletedTask;
        }
    }
}
