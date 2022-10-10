using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageUpdateCommandHandler : BaseCoreClass, IRequestHandler<LanguageUpdateCommand, int>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageUpdateCommandHandler(ICoreServices coreServices
        , LanguageManager managerLanguage): base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<int> Handle(LanguageUpdateCommand request, CancellationToken cancellationToken)
        {
            LanguageDto dto = new LanguageDto()
            {
                Id = request.Id,
                Name = request.Name,
                DisplayName = request.DisplayName,
                IsActive = request.IsActive
            };

            await _managerLanguage.UpdateLanguageAsync(dto);

            return dto.Id.Value;
        }
    }
}
