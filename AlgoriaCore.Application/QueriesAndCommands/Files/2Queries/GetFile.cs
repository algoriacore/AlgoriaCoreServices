using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Files._2Queries
{
    public class GetFile : IRequest<GetFileResponse>
    {
        public string UUID { get; set; }
    }
}
