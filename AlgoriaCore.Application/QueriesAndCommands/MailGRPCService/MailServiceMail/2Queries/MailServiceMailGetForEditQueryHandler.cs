using System;
using MediatR;
using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMails.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailStatuss;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailBodys;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailStatuss.Dto;
using AlgoriaCore.Application.Managers.MailGRPCService.MailServiceMailBodys.Dto;

namespace AlgoriaCore.Application.QueriesAndCommands.MailGRPCService.MailServiceMails
{
    public class MailServiceMailGetForEditQueryHandler : BaseCoreClass, IRequestHandler<MailServiceMailGetForEditQuery, MailServiceMailForEditResponse>
    {
        private readonly MailServiceMailManager _manager;
        private readonly MailServiceMailStatusManager _managerStatus;
        private readonly MailServiceMailBodyManager _managerBody;

        public MailServiceMailGetForEditQueryHandler(ICoreServices coreServices,
            MailServiceMailManager manager,
            MailServiceMailStatusManager managerStatus,
            MailServiceMailBodyManager managerBody

        ) : base(coreServices)
        {
            _manager = manager;
            _managerStatus = managerStatus;
            _managerBody = managerBody;
        }
        public async Task<MailServiceMailForEditResponse> Handle(MailServiceMailGetForEditQuery request, CancellationToken cancellationToken)
        {
            MailServiceMailForEditResponse response;
            MailServiceMailDto dto;
            MailServiceMailStatusDto statusDto;
            MailServiceMailBodyDto bodyDto;

            if (SessionContext.TenantId.HasValue)
            {
                dto = await _manager.GetMailServiceMailAsync(request.Id);
                statusDto = await _managerStatus.GetMailServiceMailStatusAsync(dto.Id);
                bodyDto = await _managerBody.GetMailServiceMailBodyAsync(dto.Id);
            }
            else
            {
                dto = await _manager.GetMailServiceMailByHostAsync(request.Id);
                statusDto = await _managerStatus.GetMailServiceMailStatusByHostAsync(dto.Id);
                bodyDto = await _managerBody.GetMailServiceMailBodyByHostAsync(dto.Id);
            }

            response = new MailServiceMailForEditResponse()
            {
                Id = dto.Id,
                MailServiceRequest = dto.MailServiceRequest,
                MailServiceRequestDate = dto.MailServiceRequestDate,
                IsLocalConfig = dto.IsLocalConfig,
                IsLocalConfigDesc = dto.IsLocalConfigDesc,
                Sendto = dto.Sendto,
                CopyTo = dto.CopyTo,
                BlindCopyTo = dto.BlindCopyTo,
                Subject = dto.Subject,
                MailServiceMailStatus = statusDto,
                MailServiceMailBody = bodyDto
            };

            return response;
        }
    }
}

