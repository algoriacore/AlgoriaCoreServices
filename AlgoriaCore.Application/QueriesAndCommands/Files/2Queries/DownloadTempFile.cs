using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Files._2Queries
{
    public class DownloadTempFile : IRequest<DownloadTempFileResponse>
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileToken { get; set; }
    }
}
