using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextGetByIdQueryHandler : BaseCoreClass, IRequestHandler<LanguageTextGetByIdQuery, LanguageTextResponse>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageTextGetByIdQueryHandler(ICoreServices coreServices
        , LanguageManager managerLanguage)
                                : base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<LanguageTextResponse> Handle(LanguageTextGetByIdQuery request, CancellationToken cancellationToken)
        {
            LanguageTextResponse response = null;
            LanguageTextDto dto = await _managerLanguage.GetLanguageTextAsync(request.Id);

            if (dto != null)
            {
                response = new LanguageTextResponse()
                {
                    Id = dto.Id,
                    LanguageId = dto.LanguageId,
                    Key = dto.Key,
                    Value = dto.Value
                };
            }

            return response;
        }
    }
}
