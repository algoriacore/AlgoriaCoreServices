using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Languages.Texts
{
    public class LanguageTextGetForEditQueryHandler : BaseCoreClass, IRequestHandler<LanguageTextGetForEditQuery, LanguageTextForEditResponse>
    {
        private readonly LanguageManager _managerLanguage;

        public LanguageTextGetForEditQueryHandler(ICoreServices coreServices
        , LanguageManager managerLanguage)
                                : base(coreServices)
        {
            _managerLanguage = managerLanguage;
        }

        public async Task<LanguageTextForEditResponse> Handle(LanguageTextGetForEditQuery request, CancellationToken cancellationToken)
        {
            LanguageTextForEditResponse response;

            if (request.Id.HasValue) 
            {
                LanguageTextDto dto = await _managerLanguage.GetLanguageTextAsync(request.Id.Value);

                response = new LanguageTextForEditResponse()
                {
                    Id = dto.Id,
                    LanguageId = dto.LanguageId,
                    Key = dto.Key,
                    Value = dto.Value
                };
            } else 
            {
                response = new LanguageTextForEditResponse();
            }

            return response;
        }
    }
}
