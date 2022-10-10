using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using MediatR;

namespace AlgoriaCore.Application.QueriesAndCommands.Files._3Commands
{
    public class UploadTempFile : IRequest<FileUploadResponse>
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public int Length { get; set; }
        public byte[] FileArray { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }
    }
}
