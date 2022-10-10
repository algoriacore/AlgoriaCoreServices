using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.Emails.Groups.Dto;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Emails.Groups
{
    public class MailGroupManager : BaseManager
    {
        private readonly IRepository<mailgroup, long> _repMailGroup;
        private readonly IRepository<mailgrouptxt, long> _repMailGroupTxt;
        private readonly IRepository<mailtemplate, long> _repMailTemplate;

        private readonly SettingManager _managerSetting;

        public MailGroupManager(
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

        #region Mail Group

        public async Task<PagedResultDto<MailGroupDto>> GetMailGroupListAsync(PageListByDto input)
        {
            var query = GetMailGroupQuery(input);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            if (SessionContext.TenantId.HasValue)
            {
                await CheckMailGroup(ll, SessionContext.TenantId);
            }
            else
            {
                var temaHost = ll.FirstOrDefault(m => m.TenantId == null);

                if (temaHost != null)
                {
                    await CheckMailGroup(ll, null);
                }

                var grupos = ll.Where(m => m.TenantId != null).GroupBy(m => m.TenantId).Select(n => n.FirstOrDefault()).ToList();

                foreach (var g in grupos)
                {
                    await CheckMailGroup(ll, g.TenantId.Value);
                }
            }

            return new PagedResultDto<MailGroupDto>(count, ll);
        }

        public async Task<List<ComboboxItemDto>> GetMailGroupForComboboxAsync()
        {
            var ll = await GetMailGroupQuery(false).ToListAsync();

            return new List<ComboboxItemDto>(
                ll.Select(e => new ComboboxItemDto(e.Id.ToString(), e.DisplayName)).OrderBy(order => order.Label).ToList()
                );
        }

        public async Task<MailGroupDto> GetMailGroupAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetMailGroupQuery(true);

            var dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("EmailGroups"), id));
            }

            return dto;
        }

        public async Task<MailGroupDto> GetMailGroupWithTemplatesAsync(long id)
        {
            var dto = await GetMailGroupAsync(id);
            dto.TemplateList = GetMailTemplateList(id);

            return dto;
        }

        public async Task<long> CreateMailGroupAsync(MailGroupDto dto)
        {
            var entity = new mailgroup();

            entity.DisplayName = dto.DisplayName;

            _repMailGroup.Insert(entity);
            CurrentUnitOfWork.SaveChanges();

            CreateMailGroupTxt(new MailGroupTxtDto { MailGroup = entity.Id, Type = MailGroupTxtType.Header, Body = dto.Header ?? string.Empty });
            CreateMailGroupTxt(new MailGroupTxtDto { MailGroup = entity.Id, Type = MailGroupTxtType.Footer, Body = dto.Footer ?? string.Empty });

            await LogChange(await GetMailGroupAsync(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task<long> UpdateMailGroupAsync(MailGroupDto dto)
        {
            var entity = _repMailGroup.FirstOrDefault(m => m.Id == dto.Id);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("EmailGroups"), dto.Id));
            }

            var previousDto = await GetMailGroupAsync(dto.Id.Value);

            entity.DisplayName = dto.DisplayName;
            CurrentUnitOfWork.SaveChanges();

            CreateOrUpdateMailGroupTxtByGroupAndType(entity.Id, MailGroupTxtType.Header, dto.Header);
            CreateOrUpdateMailGroupTxtByGroupAndType(entity.Id, MailGroupTxtType.Footer, dto.Footer);

            await LogChange(await GetMailGroupAsync(entity.Id), previousDto, ChangeLogType.Update);

            return entity.Id;
        }

        public async Task<long> CopyMailGroupAsync(MailGroupDto dto)
        {
            var dtoCopy = await GetMailGroupWithTemplatesAsync(dto.Id.Value);

            dtoCopy.DisplayName = dto.DisplayName;

            dtoCopy.Id = await CreateMailGroupAsync(dtoCopy);

            // Copiar plantillas
            foreach (var template in dtoCopy.TemplateList)
            {
                template.MailGroup = dtoCopy.Id;
                template.TenantId = dtoCopy.TenantId;

                CreateMailTemplate(template);
            }

            return dtoCopy.Id.Value;
        }

        public async Task CheckMailGroup(long id)
        {
            var entity = await _repMailGroup.FirstOrDefaultAsync(m => m.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("EmailGroups"), id));
            }

            if (SessionContext.TenantId.HasValue || entity.TenantId.HasValue)
            {
                using (CurrentUnitOfWork.SetTenantId(entity.TenantId))
                {
                    _managerSetting.ChangeSetting(AppSettings.General.MailGroup, entity.Id.ToString());
                }
            }
            else
            {
                _managerSetting.ChangeSettingByHost(AppSettings.General.MailGroup, entity.Id.ToString());
            }
        }

        public async Task UnCheckMailGroup(long id)
        {
            var entity = await _repMailGroup.FirstOrDefaultAsync(m => m.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("EmailGroups"), id));
            }

            if (SessionContext.TenantId.HasValue || entity.TenantId.HasValue)
            {
                using (CurrentUnitOfWork.SetTenantId(entity.TenantId))
                {
                    _managerSetting.ChangeSetting(AppSettings.General.MailGroup, null);
                }
            }
            else
            {
                _managerSetting.ChangeSettingByHost(AppSettings.General.MailGroup, null);
            }
        }

        #region Métodos privados

        private IQueryable<MailGroupDto> GetMailGroupQuery(bool isIncludeHeaderFoot = false)
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
                             Header = isIncludeHeaderFoot ? enc.body : null,
                             Footer = isIncludeHeaderFoot ? pie.body : null,
                             TenantId = eg.TenantId
                         });

            return query;
        }

        private IQueryable<MailGroupDto> GetMailGroupQuery(PageListByDto input)
        {
            string filter = input.Filter.IsNullOrEmpty() ? null : input.Filter.ToUpper();

            var query = GetMailGroupQuery(false)
                .WhereIf(
                    !filter.IsNullOrEmpty(),
                    p => p.DisplayName.ToUpper().Contains(filter)
                    || p.Id.ToString().ToUpper().Contains(filter)
                );

            return query;
        }

        private async Task CheckMailGroup(List<MailGroupDto> ll, int? tenantId)
        {
            long? groupId = await GetMailGroupFromSettings(tenantId);

            if (groupId.HasValue)
            {
                var t = ll.FirstOrDefault(m => m.Id == groupId.Value);
                if (t != null)
                {
                    t.IsSelected = true;
                }
            }
        }

        private async Task<long?> GetMailGroupFromSettings(long? tenantId)
        {
            string resp = null;
            if (tenantId.HasValue)
            {
                resp = await _managerSetting.GetSettingValueAsync(AppSettings.General.MailGroup);
            }
            else
            {
                resp = await _managerSetting.GetSettingValueByHostAsync(AppSettings.General.MailGroup);
            }

            if (!resp.IsNullOrEmpty())
            {
                return long.Parse(resp);
            }
            else
            {
                return null;
            }
        }

        private async Task<long> LogChange(MailGroupDto newDto, MailGroupDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new MailGroupDto(); }
            if (previousDto == null) { previousDto = new MailGroupDto(); }

            StringBuilder sb = new StringBuilder("");

			if ((changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update) && newDto.DisplayName != previousDto.DisplayName)
			{
				sb.AppendFormat("{0}: {1} => {2}\n", "{{EmailGroups.DisplayNameForm}}", previousDto.DisplayName ?? string.Empty, newDto.DisplayName ?? string.Empty);
			}

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "Mailgroup", sb.ToString());
        }

        #endregion

        #endregion

        #region Mail Group Txt

        #region Métodos privados

        private long CreateMailGroupTxt(MailGroupTxtDto dto)
        {
            var entity = new mailgrouptxt();

            entity.mailgroup = dto.MailGroup;
            entity.type = Convert.ToByte(dto.Type);
            entity.body = dto.Body;

            _repMailGroupTxt.Insert(entity);
            CurrentUnitOfWork.SaveChanges();

            return entity.Id;
        }

        private void UpdateEmailGroupTxt(MailGroupTxtDto dto)
        {
            var entity = _repMailGroupTxt.Get(dto.Id.Value);

            entity.body = dto.Body;

            CurrentUnitOfWork.SaveChanges();
        }

        private void CreateOrUpdateMailGroupTxtByGroupAndType(long mailGroup, MailGroupTxtType type, string body)
        {
            var dto = GetMailGroupTxtByGroupAndType(mailGroup, type);

            if (dto == null)
            {
                dto = new MailGroupTxtDto { MailGroup = mailGroup, Type = type, Body = body };
                dto.Id = CreateMailGroupTxt(dto);
            }
            else
            {
                dto.Body = body;
                UpdateEmailGroupTxt(dto);
            }
        }

        private MailGroupTxtDto GetMailGroupTxtByGroupAndType(long mailGroup, MailGroupTxtType type)
        {
            MailGroupTxtDto dto = null;
            var typeByte = Convert.ToByte(type);
            var entity = _repMailGroupTxt.FirstOrDefault(m => m.mailgroup == mailGroup && m.type == typeByte);

            if (entity != null)
            {
                dto = new MailGroupTxtDto()
                {
                    Id = entity.Id,
                    MailGroup = entity.mailgroup,
                    Type = (MailGroupTxtType)entity.type,
                    Body = entity.body
                };
            }

            return dto;
        }

        #endregion

        #endregion

        #region Templates

        #region Métodos privados

        private List<MailTemplateDto> GetMailTemplateList(long mailGroup)
        {
            var ll = (from mt in _repMailTemplate.GetAll()
                      where mt.mailgroup == mailGroup
                      select new MailTemplateDto
                      {
                          Id = mt.Id,
                          TenantId = mt.TenantId,
                          MailGroup = mt.mailgroup,
                          MailKey = mt.mailkey,
                          DisplayName = mt.DisplayName,
                          SendTo = mt.SendTo,
                          CopyTo = mt.CopyTo,
                          BlindCopyTo = mt.BlindCopyTo,
                          Subject = mt.Subject,
                          Body = mt.Body,
                          IsActive = mt.IsActive == true
                      }).ToList();

            return ll;
        }

        private void CreateMailTemplate(MailTemplateDto dto)
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
        }

        #endregion

        #endregion
    }
}
