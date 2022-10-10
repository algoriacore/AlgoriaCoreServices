using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Files._2Queries
{
    public class GetTempFile : IRequest<GetFileResponse>
    {
        public string FileName { get; set; }
    }
}
