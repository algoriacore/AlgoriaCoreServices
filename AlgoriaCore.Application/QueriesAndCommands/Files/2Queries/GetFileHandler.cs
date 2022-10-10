using AlgoriaCore.Application.Managers.BinaryObjects;
using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using AlgoriaPersistence.Interfaces.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Files._2Queries
{
    public class GetFileHandler : IRequestHandler<GetFile, GetFileResponse>
    {
        private readonly BinaryObjectManager _binaryObject;

        public GetFileHandler(BinaryObjectManager binaryObject)
        {
            _binaryObject = binaryObject;
        }

        public async Task<GetFileResponse> Handle(GetFile request, CancellationToken cancellationToken)
        {
            using (_binaryObject.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (_binaryObject.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    var fu = await _binaryObject.GetFileAsync(new System.Guid(request.UUID));
                    var fileName = await _binaryObject.GetFileNameAsync(new System.Guid(request.UUID));
                    return new GetFileResponse
                    {
                        ContentType = "application/octet-stream",
                        FileArray = fu,
                        FileSize = fu.Length,
                        FileName = fileName
                    };
                }
            }
        }
    }
}
