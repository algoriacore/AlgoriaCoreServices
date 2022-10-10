using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Languages
{
    public class LanguageGetForEditQueryHandler : BaseCoreClass, IRequestHandler<LanguageGetForEditQuery, LanguageForEditResponse>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageGetForEditQueryHandler(ICoreServices coreServices
        , LanguageManager managerLanguage)
                                : base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<LanguageForEditResponse> Handle(LanguageGetForEditQuery request, CancellationToken cancellationToken)
        {
            LanguageForEditResponse response;

            if (request.Id.HasValue) 
            {
                LanguageDto dto = await _managerLanguage.GetLanguageAsync(request.Id.Value);

                response = new LanguageForEditResponse()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    DisplayName = dto.DisplayName,
                    IsActive = dto.IsActive
                };
            } else 
            {
                response = new LanguageForEditResponse();
            }

            return response;
        }
    }
}
