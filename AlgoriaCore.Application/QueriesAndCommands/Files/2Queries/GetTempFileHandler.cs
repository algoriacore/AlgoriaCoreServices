using AlgoriaCore.Application.Managers.BinaryObjects;
using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Files._2Queries
{
    public class GetTempFileHandler : IRequestHandler<GetTempFile, GetFileResponse>
    {
        private readonly BinaryObjectManager _binaryObject;

        public GetTempFileHandler(BinaryObjectManager binaryObject)
        {
            _binaryObject = binaryObject;
        }

        public async Task<GetFileResponse> Handle(GetTempFile request, CancellationToken cancellationToken)
        {
            var fu = await _binaryObject.GetTempFileAsync(request.FileName);

            return new GetFileResponse
            {
                ContentType = fu.ContentType,
                FileArray = fu.FileArray,
                FileName = fu.FileName,
                FileSize = fu.Length
            };
        }
    }
}
