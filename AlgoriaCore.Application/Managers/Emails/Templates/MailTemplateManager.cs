using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.Emails.Groups.Dto;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Domain.Email;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.EFExtensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Emails.Templates
{
    public class MailTemplateManager : BaseManager
    {
        private readonly IRepository<mailgroup, long> _repMailGroup;
        private readonly IRepository<mailgrouptxt, long> _repMailGroupTxt;
        private readonly IRepository<mailtemplate, long> _repMailTemplate;

        private readonly SettingManager _managerSetting;

        public MailTemplateManager(
            IRepository<mailgroup, long> repMailGroup,
            IRepository<mailgrouptxt, long> repMailGroupTxt,
            IRepository<mailtemplate, long> repMailTemplate,
            SettingManager managerSetting)
        {
            _repMailGroup = repMailGroup;
            _repMailGroupTxt = repMailGroupTxt;
            _repMailTemplate = repMailTemplate;

            _managerSetting = managerSetting;
        }

        public async Task<PagedResultDto<MailTemplateDto>> GetMailTemplateListAsync(MailTemplateListFilterDto input)
        {
            var query = GetMailTemplateListQuery(input);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(input.Sorting ?? "DisplayName")
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<MailTemplateDto>(count, ll);
        }

        public async Task<List<MailTemplateDto>> GetMailTemplateList(long mailGroup)
        {
            var input = new MailTemplateListFilterDto();
            input.MailGroup = mailGroup;
            input.IsIncludeBody = true;
            input.PageSize = 999;
            input.PageNumber = 1;

            var query = GetMailTemplateListQuery(input);

            return await query.ToListAsync();
        }

        public async Task<long> CreateMailTemplateAsync(MailTemplateDto dto)
        {
            var entity = new mailtemplate();

            entity.TenantId = dto.TenantId ?? (SessionContext.TenantId ?? CurrentUnitOfWork.GetTenantId());
            entity.mailgroup = dto.MailGroup;
            entity.mailkey = dto.MailKey;
            entity.DisplayName = dto.DisplayName;
            entity.SendTo = dto.SendTo;
            entity.CopyTo = dto.CopyTo;
            entity.BlindCopyTo = dto.BlindCopyTo;
            entity.Subject = dto.Subject;
            entity.Body = dto.Body;
            entity.IsActive = dto.IsActive;

            _repMailTemplate.Insert(entity);
            CurrentUnitOfWork.SaveChanges();

            await LogChange(await GetMailTemplate(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task<long> UpdateMailTemplateAsync(MailTemplateDto dto)
        {
            var entity = await _repMailTemplate.FirstOrDefaultAsync(m => m.Id == dto.Id.Value);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("EmailTemplates"), dto.Id));
            }

            var previousDto = await GetMailTemplate(dto.Id.Value);

            entity.mailkey = dto.MailKey;
            entity.DisplayName = dto.DisplayName;
            entity.SendTo = dto.SendTo;
            entity.CopyTo = dto.CopyTo;
            entity.BlindCopyTo = dto.BlindCopyTo;
            entity.Subject = dto.Subject;
            entity.Body = dto.Body;
            entity.IsActive = dto.IsActive;

            CurrentUnitOfWork.SaveChanges();

            await LogChange(await GetMailTemplate(entity.Id), previousDto, ChangeLogType.Update);

            return entity.Id;
        }

        public async Task<MailTemplateDto> GetMailTemplate(long id, bool throwExceptionIfNotFound = true)
        {
            var entity = await _repMailTemplate.FirstOrDefaultAsync(m => m.Id == id);

            if (throwExceptionIfNotFound && entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("EmailTemplates"), id));
            }

            var dto = new MailTemplateDto();

            dto.Id = entity.Id;
            dto.TenantId = entity.TenantId;
            dto.MailGroup = entity.mailgroup;
            dto.MailKey = entity.mailkey;
            dto.DisplayName = entity.DisplayName;
            dto.SendTo = entity.SendTo;
            dto.CopyTo = entity.CopyTo;
            dto.BlindCopyTo = entity.BlindCopyTo;
            dto.Subject = entity.Subject;
            dto.Body = entity.Body;
            dto.IsActive = entity.IsActive;

            return dto;
        }

        public async Task<MailTemplateDto> GetMailTemplateCurrentByMailKey(string mailKey)
        {
            MailTemplateDto dto = null;
            MailGroupDto groupDto = null;

            var settingDto = await _managerSetting.GetSettingValueAsync(AppSettings.General.MailGroup);
            var mailGroupId = !settingDto.IsNullOrEmpty() ? long.Parse(settingDto) : 0;

            groupDto = await GetMailGroup(mailGroupId);
            dto = (await GetMailTemplateList(mailGroupId)).FirstOrDefault(m => m.MailKey == mailKey);

            if (dto == null && CurrentUnitOfWork.GetTenantId().HasValue)
            {
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    settingDto = await _managerSetting.GetSettingValueByHostAsync(AppSettings.General.MailGroup);
                    mailGroupId = !settingDto.IsNullOrEmpty() ? long.Parse(settingDto) : 0;

                    groupDto = await GetMailGroup(mailGroupId);
                    dto = (await GetMailTemplateList(mailGroupId)).FirstOrDefault(m => m.MailKey == mailKey);
                }
            }

            if (dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("EmailTemplates"), mailKey));
            }

            dto.Body = dto.Body ?? string.Empty;
            dto.Header = groupDto.Header ?? string.Empty;
            dto.Footer = groupDto.Footer ?? string.Empty;

            return dto;
        }

        public async Task SendTestMailAsync(MailTemplateSendTestDto dto)
        {
            var mailGroupDto = await GetMailGroup(dto.MailGroup);

            if (mailGroupDto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("EmailGroups"), dto.MailGroup));
            }

            var sB = new StringBuilder("");
            sB.AppendLine(mailGroupDto.Header ?? "");
            sB.AppendLine(dto.Body ?? "");
            sB.AppendLine(mailGroupDto.Footer ?? "");

            // Enviar correo
            var mailTemplateDto = new MailTemplateDto
            {
                Header = mailGroupDto.Header,
                Footer = mailGroupDto.Footer,
                Body = dto.Body,
                Subject = dto.Subject
            };

            var emailAddress = new EmailAddress { Address = dto.Email, Name = string.Empty };

            var dict = new Dictionary<string, string>();

            SendEmail(mailTemplateDto, emailAddress, dict);
        }

        public static List<string> GetMailTemplateBodyParamList(string mailKey)
        {
            var lista = new List<string>();

            switch (mailKey)
            {
                case EmailKeys.TenantRegistration:
                    lista.AddRange(new string[]
                    {
                        EmailVariables.TenantRegistration.TenantShortName.ToParam(),
                        EmailVariables.TenantRegistration.TenantName.ToParam(),
                        EmailVariables.TenantRegistration.Name.ToParam(),
                        EmailVariables.TenantRegistration.LastName.ToParam(),
                        EmailVariables.TenantRegistration.SecondLastName.ToParam(),
                        EmailVariables.TenantRegistration.Email.ToParam(),
                        EmailVariables.TenantRegistration.ConfirmationCode.ToParam(),
                        EmailVariables.TenantRegistration.ActivationUrl.ToParam()
                    });
                    break;
                case EmailKeys.NewUser:
                    lista.AddRange(new string[]
                    {
                        EmailVariables.NewUser.FullName.ToParam(),
                        EmailVariables.NewUser.UserName.ToParam(),
                        EmailVariables.NewUser.Password.ToParam()
                    });
                    break;
                case EmailKeys.UserReset:
                    lista.AddRange(new string[]
                    {
                        EmailVariables.ResetPassword.FullName.ToParam(),
                        EmailVariables.ResetPassword.UserName.ToParam(),
                        EmailVariables.ResetPassword.ResetUrl.ToParam()
                    });
                    break;
                case EmailKeys.UserUnblock:
                    lista.AddRange(new string[]
                    {
                        EmailVariables.UnblockUser.FullName.ToParam()
                    });
                    break;
                case EmailKeys.UserModification:
                    lista.AddRange(new string[]
                    {
                        EmailVariables.ModifyUser.FullName.ToParam()
                    });
                    break;
                case EmailKeys.ChatTaggedUser:
                    lista.AddRange(new string[]
                    {
                        EmailVariables.TagChatUser.ChatRoom.ToParam(),
                        EmailVariables.TagChatUser.ChatRoomDescription.ToParam(),
                        EmailVariables.TagChatUser.TaggedUser.ToParam(),
                        EmailVariables.TagChatUser.TaggedBy.ToParam(),
                        EmailVariables.TagChatUser.TaggedDateTime.ToParam(),
                        EmailVariables.TagChatUser.Comment.ToParam()
                    });
                    break;
            }

            return lista;
        }

        public async Task<List<ComboboxItemDto>> GetMailTemplateMailKeyAvailableList(long mailGroup)
        {
            var lista = GetMailTemplateMailKeyList();

            List<MailTemplateDto> plantillasGrupo = null;

            if (SessionContext.TenantId.HasValue)
            {
                plantillasGrupo = await GetMailTemplateList(mailGroup);
            }
            else
            {
                using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
                {
                    using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                    {
                        plantillasGrupo = await GetMailTemplateList(mailGroup);
                    }
                }
            }

            foreach (var p in plantillasGrupo)
            {
                var t = lista.FirstOrDefault(m => m.Value == p.MailKey);

                if (t != null)
                {
                    lista.Remove(t);
                }
            }

            return lista;
        }

        public List<ComboboxItemDto> GetMailTemplateMailKeyList()
        {
            var lista = new List<ComboboxItemDto>();

            lista.Add(new ComboboxItemDto { Value = EmailKeys.NewUser, Label = L("MailKey.NewUser") });
            lista.Add(new ComboboxItemDto { Value = EmailKeys.UserReset, Label = L("MailKey.UserReset") });
            lista.Add(new ComboboxItemDto { Value = EmailKeys.UserUnblock, Label = L("MailKey.UserUnblock") });
            lista.Add(new ComboboxItemDto { Value = EmailKeys.UserModification, Label = L("MailKey.UserModification") });
            lista.Add(new ComboboxItemDto { Value = EmailKeys.ChatTaggedUser, Label = L("MailKey.ChatTaggedUser") });

            if (!SessionContext.TenantId.HasValue)
            {
                lista.Add(new ComboboxItemDto { Value = EmailKeys.TenantRegistration, Label = L("MailKey.TenantRegistration") });
            }

            return lista;
        }

        #region Métodos privados

        private IQueryable<MailTemplateDto> GetMailTemplateQuery(bool isIncludeCuerpo = false)
        {
            var tenantRegistrationDesc = L("MailKey.TenantRegistration");
            var newUserDesc = L("MailKey.NewUser");
            var userResetDesc = L("MailKey.UserReset");
            var userUnblockDesc = L("MailKey.UserUnblock");
            var userModificationDesc = L("MailKey.UserModification");
            var chatTaggedUserDesc = L("MailKey.ChatTaggedUser");

            var query = (from mt in _repMailTemplate.GetAll()
                         select new MailTemplateDto
                         {
                             Id = mt.Id,
                             TenantId = mt.TenantId,
                             MailGroup = mt.mailgroup,
                             MailKey = mt.mailkey,
                             MailKeyDesc = mt.mailkey == EmailKeys.TenantRegistration ? tenantRegistrationDesc
                             : mt.mailkey == EmailKeys.NewUser ? newUserDesc
                             : mt.mailkey == EmailKeys.UserReset ? userResetDesc
                             : mt.mailkey == EmailKeys.UserUnblock ? userUnblockDesc
                             : mt.mailkey == EmailKeys.UserModification ? userModificationDesc
                             : mt.mailkey == EmailKeys.ChatTaggedUser ? chatTaggedUserDesc
                             : null,
                             DisplayName = mt.DisplayName,
                             SendTo = mt.SendTo,
                             CopyTo = mt.CopyTo,
                             BlindCopyTo = mt.BlindCopyTo,
                             Subject = mt.Subject,
                             Body = isIncludeCuerpo ? mt.Body : null,
                             IsActive = mt.IsActive == true
                         });

            return query;
        }

        private IQueryable<MailTemplateDto> GetMailTemplateListQuery(MailTemplateListFilterDto input)
        {
            string filter = input.Filter.IsNullOrEmpty() ? null : input.Filter.ToUpper();

            var query = GetMailTemplateQuery(input.IsIncludeBody)
                .Where(p => p.MailGroup == input.MailGroup)
                .WhereIf(
                    !filter.IsNullOrEmpty(),
                    p => p.Id.ToString().Contains(filter)
                    || p.DisplayName.ToUpper().Contains(filter)
                    || p.MailKeyDesc.ToUpper().Contains(filter)
                );

            return query;
        }

        private async Task<long> LogChange(MailTemplateDto newDto, MailTemplateDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new MailTemplateDto(); }
            if (previousDto == null) { previousDto = new MailTemplateDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
                LogStringProperty(sb, previousDto.MailKey, newDto.MailKey, "{{EmailTemplates.MailKeyForm}}");
                LogStringProperty(sb, previousDto.DisplayName, newDto.DisplayName, "{{EmailTemplates.DisplayNameForm}}");
                LogStringProperty(sb, previousDto.SendTo, newDto.SendTo, "{{EmailTemplates.SendToForm}}");
                LogStringProperty(sb, previousDto.CopyTo, newDto.CopyTo, "{{EmailTemplates.CopyToForm}}");
                LogStringProperty(sb, previousDto.BlindCopyTo, newDto.BlindCopyTo, "{{EmailTemplates.BlindCopyToForm}}");
                LogStringProperty(sb, previousDto.Subject, newDto.Subject, "{{EmailTemplates.SubjectForm}}");

                if (newDto.IsActive != previousDto.IsActive)
                {
                    sb.AppendFormat("{0}: {1} => {2}\n", "{{Status}}", previousDto.IsActive == true ? "{{Active}}" : "{{Inactive}}", newDto.IsActive == true ? "{{Active}}" : "{{Inactive}}");
                }
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "Mailtemplate", sb.ToString());
        }

        #endregion

        #region Mail Group

        #region Métodos privados

        private async Task<MailGroupDto> GetMailGroup(long mailGroup)
        {
            var tipoEncabezado = Convert.ToByte(MailGroupTxtType.Header);
            var tipoPie = Convert.ToByte(MailGroupTxtType.Footer);

            var query = (from eg in _repMailGroup.GetAll()
                         join enc in _repMailGroupTxt.GetAll() on new { id = (long?)eg.Id, tipo = (byte?)tipoEncabezado } equals new { id = enc.mailgroup, tipo = enc.type } into encJoin
                         from enc in encJoin.DefaultIfEmpty()
                         join pie in _repMailGroupTxt.GetAll() on new { id = (long?)eg.Id, tipo = (byte?)tipoPie } equals new { id = pie.mailgroup, tipo = pie.type } into pieJoin
                         from pie in pieJoin.DefaultIfEmpty()
                         select new MailGroupDto
                         {
                             Id = eg.Id,
                             DisplayName = eg.DisplayName,
                             Header = enc.body,
                             Footer = pie.body,
                             TenantId = eg.TenantId
                         });

            return await query.FirstOrDefaultAsync(m => m.Id == mailGroup);
        }

        #endregion

        #endregion
    }

}
