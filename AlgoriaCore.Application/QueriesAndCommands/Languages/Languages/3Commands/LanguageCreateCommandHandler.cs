using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageCreateCommandHandler : BaseCoreClass, IRequestHandler<LanguageCreateCommand, int>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageCreateCommandHandler(ICoreServices coreServices
        , LanguageManager managerLanguage): base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<int> Handle(LanguageCreateCommand request, CancellationToken cancellationToken)
        {
            LanguageDto dto = new LanguageDto()
            {
                Name = request.Name,
                DisplayName = request.DisplayName,
                IsActive = request.IsActive
            };

            return await _managerLanguage.CreateLanguageAsync(dto);
        }
    }
}
