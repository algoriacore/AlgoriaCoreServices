using System;
using MediatR;
using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailAttachs;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailAttachs.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailAttachs
{
    public class MailServiceMailAttachGetFileQueryHandler : BaseCoreClass, IRequestHandler<MailServiceMailAttachGetFileQuery, MailServiceMailAttachForEditResponse>
    {
        private readonly MailServiceMailAttachManager _manager;
        public MailServiceMailAttachGetFileQueryHandler(ICoreServices coreServices, MailServiceMailAttachManager manager) : base(coreServices)
        {
            _manager = manager;
        }
        public async Task<MailServiceMailAttachForEditResponse> Handle(MailServiceMailAttachGetFileQuery request, CancellationToken cancellationToken)
        {
            MailServiceMailAttachDto dto = await _manager.GetMailServiceMailAttachAsync(request.Id);

            MailServiceMailAttachForEditResponse response = new MailServiceMailAttachForEditResponse()
            {
                Id = dto.Id,
                MailServiceMailBody = dto.MailServiceMailBody,
                ContenType = dto.ContenType,
                FileName = dto.FileName,
                Base64File = Convert.ToBase64String(dto.BinaryFile, 0, dto.BinaryFile.Length)
            };

            return response;
        }
    }
}

