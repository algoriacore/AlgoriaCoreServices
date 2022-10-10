using AlgoriaCore.Extensions;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System;
using System.Net.Http;

namespace AlgoriaCore.Grpc.Helpers
{
    public static class GrpcHelper
    {
        public static GrpcChannel GetGrpcChannel(Uri uri, Version httpVersion)
        {
            GrpcWebHandler handler;
            string urlBase;

            if (uri.AbsolutePath.IsNullOrWhiteSpace())
            {
                handler = new GrpcWebHandler();
                urlBase = uri.AbsoluteUri;
            }
            else
            {
                var subdirectoryHandler = new SubdirectoryHandler(new HttpClientHandler(), uri.AbsolutePath);
                handler = new GrpcWebHandler(subdirectoryHandler);
                urlBase = uri.AbsoluteUri.Substring(0, uri.AbsoluteUri.Length - uri.AbsolutePath.Length);
            }

            handler.HttpVersion = httpVersion;

            return GrpcChannel.ForAddress(urlBase, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });
        }
    }
}
