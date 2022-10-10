using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Helps;
using AlgoriaCore.Application.Managers.Helps.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpGetByIdQueryHandler : BaseCoreClass, IRequestHandler<HelpGetByIdQuery, HelpResponse>
    {
        private readonly HelpManager _manager;

        public HelpGetByIdQueryHandler(ICoreServices coreServices, HelpManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<HelpResponse> Handle(HelpGetByIdQuery request, CancellationToken cancellationToken)
        {
            HelpResponse response = null;
            HelpDto dto = await _manager.GetHelpAsync(request.Id);

            response = new HelpResponse()
            {
                Id = dto.Id,
                Language = dto.Language,
                LanguageDesc = dto.LanguageDesc,
                Key = dto.Key,
                DisplayName = dto.DisplayName,
                Body = dto.Body,
                IsActive = dto.IsActive
            };

            return response;
        }
    }
}
