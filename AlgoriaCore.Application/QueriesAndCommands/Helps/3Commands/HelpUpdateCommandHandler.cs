using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Helps;
using AlgoriaCore.Application.Managers.Helps.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpUpdateCommandHandler : BaseCoreClass, IRequestHandler<HelpUpdateCommand, long>
    {
        private readonly HelpManager _managerHelp;

        public HelpUpdateCommandHandler(ICoreServices coreServices
        , HelpManager managerHelp): base(coreServices)
        {
            _managerHelp = managerHelp;
        }

        public async Task<long> Handle(HelpUpdateCommand request, CancellationToken cancellationToken)
        {
            HelpDto dto = new HelpDto()
            {
                Id = request.Id,
                Language = request.Language,
                Key = request.Key,
                DisplayName = request.DisplayName,
                IsActive = request.IsActive,
                Body = request.Body
            };

            await _managerHelp.UpdateHelpAsync(dto);

            return dto.Id.Value;
        }
    }
}
