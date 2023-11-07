using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Helps;
using AlgoriaCore.Application.Managers.Helps.Dto;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using AlgoriaCore.Domain.Entities.MongoDb;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpGetForEditQueryHandler : BaseCoreClass, IRequestHandler<HelpGetForEditQuery, HelpForEditResponse>
    {
        private readonly HelpManager _manager;
        private readonly LanguageManager _managerLanguage;

        public HelpGetForEditQueryHandler(
            ICoreServices coreServices,
            HelpManager manager,
            LanguageManager managerLanguage) : base(coreServices)
        {
            _manager = manager;
            _managerLanguage = managerLanguage;
        }

        public async Task<HelpForEditResponse> Handle(HelpGetForEditQuery request, CancellationToken cancellationToken)
        {
            HelpForEditResponse response;
            List<ComboboxItemDto> languageComboDto = await _managerLanguage.GetLanguageCombo();

            if (request.Id.HasValue)
            {
                HelpDto dto = await _manager.GetHelpAsync(request.Id.Value);

                response = new HelpForEditResponse()
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
            else
            {
                response = new HelpForEditResponse();
                LanguageDto languageDto = await _managerLanguage.GetLanguageDefaultAsync(true);

                if (languageDto != null && languageDto.TenantId != null && languageDto.TenantId == SessionContext.TenantId)
                {
                    response.Language = languageDto.Id;
                    response.LanguageDesc = languageDto.DisplayName;
                }
            }

            response.LanguageCombo = languageComboDto.Select(p => new ComboboxItemDto(p.Value, p.Label)).ToList();

            if (response.Language != null && !response.LanguageCombo.Exists(p => p.Value == response.Language.ToString()))
            {
                response.LanguageCombo.Add(new ComboboxItemDto(response.Language.ToString(), response.LanguageDesc));
                response.LanguageCombo = response.LanguageCombo.OrderBy(p => p.Label).ToList();
            }

            response.KeyCombo.Add(new ComboboxItemDto("DASHBOARD", "DASHBOARD"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.ROLE.VIEW", "ADMINISTRATION.ROLE.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.ROLE.FORM", "ADMINISTRATION.ROLE.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.LANGUAGE.VIEW", "ADMINISTRATION.LANGUAGE.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.LANGUAGE.FORM", "ADMINISTRATION.LANGUAGE.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.LANGUAGETEXT.VIEW", "ADMINISTRATION.LANGUAGETEXT.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.USER.VIEW", "ADMINISTRATION.USER.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.USER.FORM", "ADMINISTRATION.USER.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.AUDITLOG.VIEW", "ADMINISTRATION.AUDITLOG.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.SETTING.FORM", "ADMINISTRATION.SETTING.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.MAINTENANCE.VIEW", "ADMINISTRATION.MAINTENANCE.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.EMAILGROUP.VIEW", "ADMINISTRATION.EMAILGROUP.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.EMAILGROUP.FORM", "ADMINISTRATION.EMAILGROUP.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.EMAILTEMPLATE.VIEW", "ADMINISTRATION.EMAILTEMPLATE.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.EMAILTEMPLATE.FORM", "ADMINISTRATION.EMAILTEMPLATE.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.HELP.VIEW", "ADMINISTRATION.HELP.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.HELP.FORM", "ADMINISTRATION.HELP.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.ORGUNIT.VIEW", "ADMINISTRATION.ORGUNIT.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("ADMINISTRATION.ORGUNIT.FORM", "ADMINISTRATION.ORGUNIT.FORM"));

            response.KeyCombo.Add(new ComboboxItemDto("EXAMPLES.DATETIME.VIEW", "EXAMPLES.DATETIME.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("EXAMPLES.DATETIME.FORM", "EXAMPLES.DATETIME.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("EXAMPLES.CHAT.FORM", "EXAMPLES.CHAT.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("EXAMPLES.LOG.FORM", "EXAMPLES.LOG.FORM"));

            response.KeyCombo.Add(new ComboboxItemDto("MAILSERVICEMAIL.MAILSERVICEMAIL.FORM", "CATALOGS.MAILSERVICEMAIL.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("MAILSERVICEMAIL.MAILSERVICEMAIL.VIEW", "CATALOGS.MAILSERVICEMAIL.VIEW"));

            response.KeyCombo.Add(new ComboboxItemDto("QUESTIONNAIRES.QUESTIONNAIRES.VIEW", "QUESTIONNAIRES.QUESTIONNAIRES.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("QUESTIONNAIRES.QUESTIONNAIRES.FORM", "QUESTIONNAIRES.QUESTIONNAIRES.FORM"));
            response.KeyCombo.Add(new ComboboxItemDto("CATALOGSCUSTOM.CATALOGSCUSTOM.VIEW", "CATALOGSCUSTOM.CATALOGSCUSTOM.VIEW"));
            response.KeyCombo.Add(new ComboboxItemDto("CATALOGSCUSTOM.CATALOGSCUSTOM.FORM", "CATALOGSCUSTOM.CATALOGSCUSTOM.FORM"));

            if (SessionContext.TenantId == null)
            {
                response.KeyCombo.Add(new ComboboxItemDto("INSTANCE.VIEW", "INSTANCE.VIEW"));
                response.KeyCombo.Add(new ComboboxItemDto("INSTANCE.FORM", "INSTANCE.FORM"));
            }

            response.KeyCombo = response.KeyCombo.OrderBy(p => p.Label).ToList();

            return response;
        }
    }
}
