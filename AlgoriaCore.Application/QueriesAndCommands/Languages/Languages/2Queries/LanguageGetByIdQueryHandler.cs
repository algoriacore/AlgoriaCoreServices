using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageGetByIdQueryHandler : BaseCoreClass, IRequestHandler<LanguageGetByIdQuery, LanguageResponse>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageGetByIdQueryHandler(ICoreServices coreServices
        , LanguageManager managerLanguage)
                                : base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<LanguageResponse> Handle(LanguageGetByIdQuery request, CancellationToken cancellationToken)
        {
            LanguageResponse response = null;
            LanguageDto dto = await _managerLanguage.GetLanguageAsync(request.Id);

            response = new LanguageResponse()
            {
                Id = dto.Id,
                Name = dto.Name,
                DisplayName = dto.DisplayName,
                IsActive = dto.IsActive
            };

            return response;
        }
    }
}
