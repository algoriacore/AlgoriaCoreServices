using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using AlgoriaCore.Application.Managers.Languages.Dto;
using AlgoriaCore.Application.Managers.Roles.Dto;
using System.Collections.Generic;

namespace AlgoriaCore.Application.Managers.Tenants.Dto
{
    public class CreateTenantConfigDto
    {
        public List<SettingConfig> Settings { get; set; }
        public List<MailTemplateDto> Emails { get; set; }
        public List<LanguageDto> Languages { get; set; }
        public List<RolDto> Roles { get; set; }
    }

    public class SettingConfig
    {
        public string SettingKey { get; set; }
        public string Value { get; set; }
    }
}
