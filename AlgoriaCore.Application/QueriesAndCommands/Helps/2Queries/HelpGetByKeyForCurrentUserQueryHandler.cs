using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Helps;
using AlgoriaCore.Application.Managers.Helps.Dto;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpGetByKeyForCurrentUserQueryHandler : BaseCoreClass, IRequestHandler<HelpGetByKeyForCurrentUserQuery, HelpResponse>
    {
        private readonly HelpManager _managerHelp;
        private readonly LanguageManager _managerLanguage;

        public HelpGetByKeyForCurrentUserQueryHandler(ICoreServices coreServices
        , HelpManager managerHelp
        , LanguageManager managerLanguage
        ) : base(coreServices)
        {
            _managerHelp = managerHelp;
            _managerLanguage = managerLanguage;
        }

        public async Task<HelpResponse> Handle(HelpGetByKeyForCurrentUserQuery request, CancellationToken cancellationToken)
        {
            HelpResponse response = null;
            LanguageDto languageDto = await _managerLanguage.GetLanguageDefaultAsync();

            if (languageDto != null)
            {
                HelpDto dto = await _managerHelp.GetHelpByLanguageAndKeyAsync(languageDto.Id.Value, request.Key);

				if (dto == null && languageDto.TenantId != null)
				{
					using (_managerLanguage.CurrentUnitOfWork.SetTenantId(null))
					{
						languageDto = await _managerLanguage.GetLanguageByNameAsync(languageDto.Name);

						if (languageDto != null)
						{
							dto = await _managerHelp.GetHelpByLanguageAndKeyAsync(languageDto.Id.Value, request.Key);
						}
					}
				}

                if (dto != null)
                {
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
                }
            }

            return response;
        }
    }
}
