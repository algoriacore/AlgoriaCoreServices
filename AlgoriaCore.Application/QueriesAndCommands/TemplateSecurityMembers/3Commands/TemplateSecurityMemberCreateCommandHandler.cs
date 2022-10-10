using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateSecurityMembers
{
    public class TemplateSecurityMemberCreateCommandHandler : BaseCoreClass, IRequestHandler<TemplateSecurityMemberCreateCommand, long>
    {
        private readonly TemplateManager _manager;

        public TemplateSecurityMemberCreateCommandHandler(ICoreServices coreServices, TemplateManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(TemplateSecurityMemberCreateCommand request, CancellationToken cancellationToken)
        {
            TemplateSecurityMemberDto dto = new TemplateSecurityMemberDto()
            {
                Template = request.Template,
                Type = request.Type,
                Member = request.Member,
                Level = request.Level,
                IsExecutor = request.IsExecutor
            };

            return await _manager.CreateTemplateSecurityMemberAsync(dto);
        }
    }
}
