using System;
using MediatR;
using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailConfigs;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailConfigs.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMailConfigs
{
    public class MailServiceMailConfigGetForEditQueryHandler : BaseCoreClass, IRequestHandler<MailServiceMailConfigGetForEditQuery, MailServiceMailConfigForEditResponse>
    {
        private readonly MailServiceMailConfigManager _manager;
        public MailServiceMailConfigGetForEditQueryHandler(ICoreServices coreServices, MailServiceMailConfigManager manager) : base(coreServices)
        {
            _manager = manager;
        }
        public async Task<MailServiceMailConfigForEditResponse> Handle(MailServiceMailConfigGetForEditQuery request, CancellationToken cancellationToken)
        {
            MailServiceMailConfigDto dto = await _manager.GetMailServiceMailConfigAsync(request.Id);

            MailServiceMailConfigForEditResponse response = null;

            if (dto != null)
            {
                response = new MailServiceMailConfigForEditResponse()
                {
                    MailServiceMail = dto.MailServiceMail,
                    MailServiceMailDesc = dto.MailServiceMailDesc,
                    Sender = dto.Sender,
                    SenderDisplay = dto.SenderDisplay,
                    Smpthost = dto.Smpthost,
                    Smptport = dto.Smptport,
                    IsSsl = dto.IsSsl,
                    IsSslDesc = dto.IsSslDesc,
                    UseDefaultCredential = dto.UseDefaultCredential,
                    UseDefaultCredentialDesc = dto.UseDefaultCredentialDesc,
                    Domain = dto.Domain,
                    MailUser = dto.MailUser,
                    MailPassword = dto.MailPassword,
                    Id = dto.Id,
                };
            }


            return response;
        }
    }
}

