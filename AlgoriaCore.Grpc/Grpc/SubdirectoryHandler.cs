using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Grpc
{
    /// <summary>
    /// Un controlador de delegación que agregará un subdirectorio al URI de las solicitudes de gRPC 
    /// </summary>
    public class SubdirectoryHandler : DelegatingHandler
    {
        private readonly string _subdirectory;

        public SubdirectoryHandler(HttpMessageHandler innerHandler, string subdirectory) : base(innerHandler)
        {
            _subdirectory = subdirectory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var url = request.RequestUri.AbsoluteUri.Replace(request.RequestUri.AbsolutePath, _subdirectory);
            url += request.RequestUri.AbsolutePath;
            request.RequestUri = new Uri(url, UriKind.Absolute);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
