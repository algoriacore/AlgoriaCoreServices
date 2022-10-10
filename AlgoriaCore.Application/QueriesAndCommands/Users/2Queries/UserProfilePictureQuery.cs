using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserProfilePictureQuery : IRequest<GetFileResponse>
    {
        public long Id { get; set; }
    }
}
