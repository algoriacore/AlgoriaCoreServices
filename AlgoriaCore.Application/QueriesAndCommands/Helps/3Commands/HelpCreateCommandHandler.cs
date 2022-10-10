using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Helps;
using AlgoriaCore.Application.Managers.Helps.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpCreateCommandHandler : BaseCoreClass, IRequestHandler<HelpCreateCommand, long>
    {
        private readonly HelpManager _manager;

        public HelpCreateCommandHandler(ICoreServices coreServices, HelpManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<long> Handle(HelpCreateCommand request, CancellationToken cancellationToken)
        {
            HelpDto dto = new HelpDto()
            {
                Language = request.Language,
                Key = request.Key,
                DisplayName = request.DisplayName,
                IsActive = request.IsActive,
                Body = request.Body
            };

            return await _manager.CreateHelpAsync(dto);
        }
    }
}
