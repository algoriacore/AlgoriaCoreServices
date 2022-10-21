using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.Processes.Dto;
using AlgoriaCore.Application.Managers.Templates;
using AlgoriaCore.Application.Managers.Templates.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Exceptions.Abstracts;
using AlgoriaCore.Extensions;
using AlgoriaPersistence.Interfaces.Interfaces;
using Autofac;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Processes
{
    public class ProcessManager : BaseManager
    {
        private readonly TemplateManager _managerTemplate;
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<ToDoActivity, long> _repositoryToDoActivity;
        private readonly IRepository<ToDoActExecutor, long> _repositoryToDoActExecutor;
        private readonly IRepository<ToDoActEvaluator, long> _repositoryToDoActEvaluator;
        private readonly IRepository<ToDoTimeSheet, long> _repositoryToDoTimeSheet;

        private TemplateDto _templateDto;

        private readonly ILifetimeScope _lifetimeScope;

        public ProcessManager(
            TemplateManager managerTemplate,
            ISqlExecuter sqlExecuter,
            IRepository<ToDoActivity, long> repositoryToDoActivity,
            IRepository<ToDoActExecutor, long> repositoryToDoActExecutor,
            IRepository<ToDoActEvaluator, long> repositoryToDoActEvaluator,
            IRepository<ToDoTimeSheet, long> repositoryToDoTimeSheet,
            ILifetimeScope lifetimeScope
            )
        {
            _managerTemplate = managerTemplate;
            _sqlExecuter = sqlExecuter;
            _repositoryToDoActivity = repositoryToDoActivity;
            _repositoryToDoActExecutor = repositoryToDoActExecutor;
            _repositoryToDoActEvaluator = repositoryToDoActEvaluator;
            _repositoryToDoTimeSheet = repositoryToDoTimeSheet;

            _lifetimeScope = lifetimeScope;
        }

        public async Task SetTemplate(long template) {
            _templateDto = await _managerTemplate.GetTemplateAsync(template);
        }

        #region PROCESSES

        public async Task<PagedResultDto<Dictionary<string, object>>> GetProcessListAsync(ProcessListFilterDto dto)
        {
            TemplateQueryDto queryView = await _managerTemplate.GetTemplateQueryByTemplateAndTypeAsync(_templateDto.Id.Value, TemplateQueryType.View);
            string query = queryView.Query;

            query = query.Replace("{{CURRENTUSER}}", SessionContext.UserId.Value.ToString());

            if (!dto.Filter.IsNullOrWhiteSpace()) {
                TemplateQueryDto queryViewFilters = await _managerTemplate.GetTemplateQueryByTemplateAndTypeAsync(_templateDto.Id.Value, TemplateQueryType.ViewFilters);
                query += " " + queryViewFilters.Query.Replace("{{FILTER}}", dto.Filter);
            }

            if (_templateDto.IsActivity && dto.ViewType != ProcessViewType.Normal) {
                query += dto.Filter.IsNullOrWhiteSpace() ? " WHERE " : " AND ";

                if (dto.ViewType == ProcessViewType.Own) {
                    query += _managerTemplate.GetTemplateFieldNameForActivity("UserCreator") + " = " + SessionContext.UserId.Value.ToString();
                } else if (dto.ViewType == ProcessViewType.OwnPendings) {
                    query += "UserIsExecutor = 1";
                }
            }

            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@strSelectTabla", query);
            aParameters.Add("@numeropagina", dto.PageNumber > 0 ? dto.PageNumber - 1 : 0);
            aParameters.Add("@porpagina", dto.PageSize);
            aParameters.Add("@orderby", dto.Sorting);

            List<Dictionary<string, object>> list = await _sqlExecuter.SqlStoredProcedureToDictionary("spr_obtienedatosporseccion", aParameters);

            return new PagedResultDto<Dictionary<string, object>>(list.Count > 0 ? int.Parse(list[0]["total"].ToString()) : 0, list);
        }

        public async Task<List<ComboboxItemDto>> GetProcessComboAsync(ProcessComboFilterDto dto)
        {
            List<ComboboxItemDto> combo = new List<ComboboxItemDto>();
            TemplateFieldDto templateFieldDto = await _managerTemplate.GetTemplateFieldAsync(dto.TemplateField);
            TemplateDto templateDto = await _managerTemplate.GetTemplateAsync(templateFieldDto.Template.Value);

            string query = string.Format("SELECT Id AS Value, {0} AS Label FROM {1}", templateFieldDto.FieldName, templateFieldDto.TemplateTableName);

            if (templateDto.HasSecurity) 
            {
                query += " " + _managerTemplate.GetTemplateQuerySecurity(templateDto.TableName, SessionContext.UserId);
            }

            if (!dto.Filter.IsNullOrWhiteSpace()) {
                query += string.Format(" WHERE {0} LIKE '%{1}%'", templateFieldDto.FieldName, dto.Filter);
            }

            query += string.Format(" ORDER BY {0} ASC", templateFieldDto.FieldName);

            List<Dictionary<string, object>> list = await _sqlExecuter.SqlQueryToDictionary(query);

            foreach(Dictionary<string, object> item in list) {
                combo.Add(new ComboboxItemDto(item["Value"].ToString(), item["Label"] == null ? null: item["Label"].ToString()));
            }

            return combo;
        }

        public async Task<ProcessDto> GetProcessAsync(long id, bool throwExceptionIfNotFound = true)
        {
            Dictionary<string, object> data = null;
            TemplateQueryDto query = await _managerTemplate.GetTemplateQueryByTemplateAndTypeAsync(_templateDto.Id.Value, TemplateQueryType.Read);
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Id", id);

            List<Dictionary<string, object>> list = await _sqlExecuter.SqlQueryToDictionary(query.Query, aParameters);

            if (list.Count > 0)
            {
                data = list.First();
            }

            if (throwExceptionIfNotFound && data == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Processes.Process"), id));
            }

            return new ProcessDto() {
                Id = id,
                DataFromServer = data,
                Activity = _templateDto.IsActivity && data != null ? 
                    await GetProcessToDoActivityAsync((long)data[_managerTemplate.GetTemplateFieldNameForActivity("Id")])
                    : null
            };
        }

        public async Task<long> CreateProcessAsync(ProcessDto dto)
        {
            List<TemplateFieldDto> fields = await _managerTemplate.GetTemplateFieldByTemplateListAsync(_templateDto.Id.Value, true);

            ValidateProcess(dto, fields);

            TemplateQueryDto queryView = await _managerTemplate.GetTemplateQueryByTemplateAndTypeAsync(_templateDto.Id.Value, TemplateQueryType.Insert);
            string query = queryView.Query;

            Dictionary<string, object> aParameters = GetParametersForInsert(fields, dto.DataFromClient);
            long id = await _sqlExecuter.ExecuteSqlQueryScalar<long>(query, aParameters);

            await CreateProcessOptionsAsync(id, fields, dto.DataFromClient);

            dto.Id = id;

            if (_templateDto.IsActivity)
            {
                dto.Activity.Key = id;
                dto.Activity.Table = _templateDto.TableName;

                await LinkProcessToDoActivityAsync(id, await CreateProcessToDoActivityAsync(dto.Activity), _templateDto.TableName);
            }

            dto = await GetProcessAsync(id);

            if (_templateDto.HasSecurity) {
                await InheritProcessDefaultSecurityAsync(dto, fields);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeProcessAsync(dto, null, ChangeLogType.Create, fields);

            return id;
        }

        public async Task UpdateProcessAsync(ProcessDto dto)
        {
            List<TemplateFieldDto> fields = await _managerTemplate.GetTemplateFieldByTemplateListAsync(_templateDto.Id.Value, true);

            ValidateProcess(dto, fields);

            TemplateQueryDto queryView = await _managerTemplate.GetTemplateQueryByTemplateAndTypeAsync(_templateDto.Id.Value, TemplateQueryType.Update);
            string query = queryView.Query;

            ProcessDto previousDto = await GetProcessAsync(dto.Id.Value);

            Dictionary<string, object> aParameters = GetParametersForInsert(fields, dto.DataFromClient);
            aParameters.Add("@Id", dto.Id.Value);

            await _sqlExecuter.ExecuteSqlCommandAsync(query, aParameters);

            await DeleteProcessOptionByProcessAsync(dto.Id.Value);
            await CreateProcessOptionsAsync(dto.Id.Value, fields, dto.DataFromClient);

            if (_templateDto.IsActivity) {
                if (previousDto.Activity == null) {
                    dto.Activity.Key = dto.Id.Value;
                    dto.Activity.Table = _templateDto.TableName;
                    await CreateProcessToDoActivityAsync(dto.Activity);
                } else {
                    dto.Activity.Id = previousDto.Activity.Id;
                    await UpdateProcessToDoActivityAsync(dto.Activity);
                }
            }

            dto = await GetProcessAsync(dto.Id.Value);

            if (_templateDto.HasSecurity && _templateDto.IsActivity) {
                await UpdateProcessSecurityAsync(dto);
            }

            await LogChangeProcessAsync(dto, previousDto, ChangeLogType.Update, fields);
        }

        public async Task DeleteProcessAsync(long id)
        {

            ProcessDto previousDto = await GetProcessAsync(id);

            await DeleteProcessOptionByProcessAsync(id);
            await DeleteProcessSecurityMemberByParentAsync(id, SecurityMemberType.User, SecurityMemberLevel.Editor);
            await DeleteProcessSecurityMemberByParentAsync(id, SecurityMemberType.User, SecurityMemberLevel.Reader);
            await DeleteProcessSecurityMemberByParentAsync(id, SecurityMemberType.OrgUnit, SecurityMemberLevel.Editor);
            await DeleteProcessSecurityMemberByParentAsync(id, SecurityMemberType.OrgUnit, SecurityMemberLevel.Reader);

            string query = string.Format("DELETE FROM {0} WHERE Id = @Id", _templateDto.TableName);
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Id", id);

            try
            {
                await _sqlExecuter.ExecuteSqlCommandAsync(query, aParameters);
            }
            catch (Exception)
            {
                throw new EntityHasRelationshipsException(L("Processes.RecordHasRelationshipsMessage"));
            }

            await LogChangeProcessAsync(null, previousDto, ChangeLogType.Delete, null);
        }

        #region Private Methods

        private void ValidateProcess(ProcessDto dto, List<TemplateFieldDto> fields) {
            List<ValidationFailure> failures = new List<ValidationFailure>();
            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string fieldData;

            foreach (TemplateFieldDto field in fields) {
                fieldData = dto.DataFromClient[field.FieldName];

                if (field.IsRequired && fieldData.IsNullOrWhiteSpace()) {
                    failures.Add(new ValidationFailure(field.FieldName, string.Format(labelRequiredField, field.Name)));
                } else if (!field.MustHaveOptions && field.FieldType == TemplateFieldType.Text && fieldData != null && fieldData.Length > field.FieldSize) {
                    failures.Add(new ValidationFailure(field.FieldName, string.Format(labelMaxLength, field.Name, field.FieldSize)));
                }
            }

            if (failures.Count > 0)
            {
                throw new Exceptions.ValidationException(failures, SessionContext, AppLocalizationProvider);
            }
        }

        private static Dictionary<string, object> GetParametersForInsert(List<TemplateFieldDto> fields, Dictionary<string, string> data) {
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            foreach (TemplateFieldDto field in fields)
            {
                aParameters.Add("@" + field.FieldName, data[field.FieldName]);
            }

            return aParameters;
        }

        private async Task CreateProcessOptionsAsync(long parentId, List<TemplateFieldDto> fields, Dictionary<string, string> data) {
            List<TemplateFieldDto> fieldsWithOptions = fields.FindAll(p => p.MustHaveOptions);

            if (fieldsWithOptions.Count > 0)
            {
                string queryInsertOption = string.Format("INSERT INTO {0}_OPT (Parent, TemplateField, Value, Description) VALUES ({1}, @TemplateField, @Value, @Description)",
                    fieldsWithOptions.First().TemplateTableName, parentId);
                TemplateFieldOptionDto templateFieldOptionDto;
                Dictionary<string, object> aParameters;

                foreach (TemplateFieldDto field in fieldsWithOptions)
                {
                    if (!data[field.FieldName].IsNullOrWhiteSpace())
                    {
                        foreach (string value in data[field.FieldName].Split(","))
                        {
                            templateFieldOptionDto = field.Options.Find(p => p.Value == int.Parse(value));

                            aParameters = new Dictionary<string, object>();
                            aParameters.Add("@TemplateField", field.Id);
                            aParameters.Add("@Value", templateFieldOptionDto.Value);
                            aParameters.Add("@Description", templateFieldOptionDto.Description);

                            await _sqlExecuter.ExecuteSqlCommandAsync(queryInsertOption, aParameters);
                        }
                    }
                }
            }
        }

        private async Task DeleteProcessOptionByProcessAsync(long process) {
            string query = string.Format("DELETE FROM {0}_OPT WHERE Parent = @ParentId", _templateDto.TableName);
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@ParentId", process);

            await _sqlExecuter.ExecuteSqlCommandAsync(query, aParameters);
        }

        private async Task<long> LogChangeProcessAsync(ProcessDto newDto, ProcessDto previousDto, ChangeLogType changeLogType, List<TemplateFieldDto> fields)
        {
            if (newDto == null) { 
                newDto = new ProcessDto();
                newDto.Activity = new ToDoActivityDto();
            }

            if (previousDto == null) { 
                previousDto = new ProcessDto();
                previousDto.Activity = new ToDoActivityDto();
            }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
                if (_templateDto.IsActivity) {
                    LogStringProperty(sb, previousDto.Activity.UserCreatorDesc, newDto.Activity.UserCreatorDesc, "{{Processes.Process.Activity.UserCreator}}");
                    LogDateTimeProperty(sb, previousDto.Activity.CreationTime, newDto.Activity.CreationTime, "{{Processes.Process.Activity.CreationTime}}");
                    LogStringProperty(sb, previousDto.Activity.StatusDesc, newDto.Activity.StatusDesc, "{{Processes.Process.Activity.Status}}");
                    LogStringProperty(sb,  previousDto.Activity.Description, newDto.Activity.Description, "{{Processes.Process.Activity.Description}}");
                    LogDateProperty(sb, previousDto.Activity.InitialPlannedDate, newDto.Activity.InitialPlannedDate, "{{Processes.Process.Activity.InitialPlannedDate}}");
                    LogDateProperty(sb, previousDto.Activity.FinalPlannedDate, newDto.Activity.FinalPlannedDate, "{{Processes.Process.Activity.FinalPlannedDate}}");
                    LogDateProperty(sb, previousDto.Activity.InitialRealDate, newDto.Activity.InitialRealDate, "{{Processes.Process.Activity.InitialRealDate}}");
                    LogDateProperty(sb, previousDto.Activity.FinalRealDate, newDto.Activity.FinalRealDate, "{{Processes.Process.Activity.FinalRealDate}}");
                    LogBoolProperty(sb, previousDto.Activity.IsOnTime, newDto.Activity.IsOnTime, "{{Processes.Process.Activity.IsOnTime}}");
                    LogStringProperty(
                        sb, 
                        string.Join(", ", previousDto.Activity.Executor.Select(p => p.UserDesc).OrderBy(p => p)),
                        string.Join(", ", newDto.Activity.Executor.Select(p => p.UserDesc).OrderBy(p => p)),
                        "{{Processes.Process.Activity.Executor}}"
                    );
                    LogStringProperty(
                        sb,
                        string.Join(", ", previousDto.Activity.Evaluator.Select(p => p.UserDesc).OrderBy(p => p)),
                        string.Join(", ", newDto.Activity.Evaluator.Select(p => p.UserDesc).OrderBy(p => p)),
                        "{{Processes.Process.Activity.Evaluator}}"
                    );
                }

                string fieldName;
                object previousValue;
                object newValue;

                foreach (TemplateFieldDto field in fields)
                {
                    previousValue = previousDto.DataFromServer.ContainsKey(field.FieldName) ? previousDto.DataFromServer[field.FieldName] : null;
                    newValue = newDto.DataFromServer.ContainsKey(field.FieldName) ? newDto.DataFromServer[field.FieldName] : null;

                    switch (field.FieldType)
                    {
                        case TemplateFieldType.Boolean:
                            LogBoolProperty(sb,
                                (previousValue != null) && (bool)previousValue,
                                (newValue != null) && (bool)newValue, field.Name);
                            break;
                        case TemplateFieldType.Date:
                            LogDateProperty(sb, (DateTime?)previousValue, (DateTime?)newValue, field.Name);
                            break;
                        case TemplateFieldType.DateTime:
                            LogDateTimeProperty(sb, (DateTime?)previousValue, (DateTime?)newValue, field.Name);
                            break;
                        case TemplateFieldType.Time:
                            LogTimeProperty(sb, (TimeSpan?)previousValue, (TimeSpan?)newValue, field.Name);
                            break;
                        case TemplateFieldType.Multivalue:
                        case TemplateFieldType.Template:
                        case TemplateFieldType.User:
                            fieldName = field.FieldName + "_DESC";
                            previousValue = previousDto.DataFromServer.ContainsKey(fieldName) ? previousDto.DataFromServer[fieldName] : null;
                            newValue = newDto.DataFromServer.ContainsKey(fieldName) ? newDto.DataFromServer[fieldName] : null;

                            LogStringProperty(sb,
                                previousValue == null ? null : previousValue.ToString(),
                                newValue == null ? null : newValue.ToString(), field.Name);
                            break;
                        case TemplateFieldType.Integer:
                            LogIntProperty(sb, (int?)previousValue, (int?)newValue, field.Name);
                            break;
                        case TemplateFieldType.Decimal:
                            LogDecimalProperty(sb, (decimal?)previousValue, (decimal?)newValue, field.Name);
                            break;
                        default:
                            if (field.MustHaveOptions)
                            {
                                fieldName = field.FieldName + "_DESC";
                                previousValue = previousDto.DataFromServer.ContainsKey(fieldName) ? previousDto.DataFromServer[fieldName] : null;
                                newValue = newDto.DataFromServer.ContainsKey(fieldName) ? newDto.DataFromServer[fieldName] : null;

                                LogStringProperty(sb,
                                    previousValue == null ? null : previousValue.ToString(),
                                    newValue == null ? null : newValue.ToString(), field.Name);
                            }
                            else
                            {
                                LogStringProperty(sb,
                                        previousValue == null ? null : previousValue.ToString(),
                                        newValue == null ? null : newValue.ToString(), field.Name);
                            }
                            break;
                    }
                }
            }

            return await LogChange(changeLogType, (newDto.Id?? previousDto.Id).ToString(), _templateDto.TableName, sb.ToString());
        }

        private async Task InheritProcessDefaultSecurityAsync(ProcessDto dto, List<TemplateFieldDto> fields) 
        {
            List<ProcessSecurityMemberDto> processSecurityMembers = new List<ProcessSecurityMemberDto>();
            ProcessSecurityMemberDto processSecurityMemberDto;
            List<TemplateFieldDto> fieldsSecurity = fields.FindAll(p => p.FieldType == TemplateFieldType.Template && p.InheritSecurity);

            if (fieldsSecurity.Count > 0)
            {
                object fieldData;
                long fkId;
                ProcessManager processManagerAux = _lifetimeScope.Resolve<ProcessManager>();
                List<ProcessSecurityMemberDto> processSecurityMembersFromTemplateField;

                foreach (TemplateFieldDto fieldDto in fieldsSecurity)
                {
                    fieldData = dto.DataFromServer[fieldDto.FieldName];

                    if (fieldData != null)
                    {
                        fkId = Convert.ToInt64(fieldData);
                        await processManagerAux.SetTemplate(fieldDto.TemplateFieldRelationTemplate.Value);

                        processSecurityMembersFromTemplateField = await processManagerAux.GetProcessSecurityMemberListAsync(fkId);

                        foreach(ProcessSecurityMemberDto processSecurityMemberFromTemplateFieldDto in processSecurityMembersFromTemplateField)
                        {
                            processSecurityMemberDto = processSecurityMembers.FirstOrDefault(p => p.Type == processSecurityMemberFromTemplateFieldDto.Type 
                                && p.Level == processSecurityMemberFromTemplateFieldDto.Level
                                && p.Member == processSecurityMemberFromTemplateFieldDto.Member);

                            if (processSecurityMemberDto == null) 
                            {
                                processSecurityMembers.Add(new ProcessSecurityMemberDto()
                                {
                                    Parent = dto.Id.Value,
                                    Type = processSecurityMemberFromTemplateFieldDto.Type,
                                    Member = processSecurityMemberFromTemplateFieldDto.Member,
                                    Level = processSecurityMemberFromTemplateFieldDto.Level,
                                    IsExecutor = processSecurityMemberFromTemplateFieldDto.IsExecutor
                                });
                            } else if (processSecurityMemberFromTemplateFieldDto.Level == SecurityMemberLevel.Editor
                                    && processSecurityMemberFromTemplateFieldDto.IsExecutor)
                            {
                                processSecurityMemberDto.IsExecutor = true;
                            }
                        }
                    }
                }
            }

            List<TemplateSecurityMemberDto> list = await _managerTemplate.GetTemplateSecurityMemberListAsync(_templateDto.Id.Value);

            foreach (TemplateSecurityMemberDto templateSecurityMemberDto in list)
            {
                processSecurityMemberDto = processSecurityMembers.FirstOrDefault(p => p.Type == templateSecurityMemberDto.Type 
                    && p.Level == templateSecurityMemberDto.Level && p.Member == templateSecurityMemberDto.Member);

                if (processSecurityMemberDto == null)
                {
                    processSecurityMembers.Add(new ProcessSecurityMemberDto()
                    {
                        Parent = dto.Id.Value,
                        Type = templateSecurityMemberDto.Type,
                        Member = templateSecurityMemberDto.Member,
                        Level = templateSecurityMemberDto.Level,
                        IsExecutor = templateSecurityMemberDto.IsExecutor
                    });
                } else if (templateSecurityMemberDto.Level == SecurityMemberLevel.Editor && templateSecurityMemberDto.IsExecutor)
                {
                    processSecurityMemberDto.IsExecutor = true;
                }
            }

            if (_templateDto.IsActivity)
            {
                List<ProcessSecurityMemberDto> processSecurityMembersFromActivity = await GetProcessSecurityMemberFromActivityAsync(dto.Activity);

                processSecurityMembers.RemoveAll(p => p.Type == SecurityMemberType.User 
                    && processSecurityMembersFromActivity.Exists(x => x.Member == p.Member));

                processSecurityMembers.AddRange(processSecurityMembersFromActivity);
            }

            foreach(ProcessSecurityMemberDto item in processSecurityMembers)
            {
                await CreateProcessSecurityMemberAsync(item);
            }
        }
        
        private async Task<List<ProcessSecurityMemberDto>> GetProcessSecurityMemberFromActivityAsync(ToDoActivityDto dto)
        {
            List<ProcessSecurityMemberDto> list = new List<ProcessSecurityMemberDto>();
            TemplateToDoStatusDto statusDto = await _managerTemplate.GetTemplateToDoStatusAsync(dto.Status);
            ProcessSecurityMemberDto processSecurityMemberDto;
            long parent = Convert.ToInt64(dto.Key);

            foreach (ToDoActivityUserDto executor in dto.Executor)
            {
                processSecurityMemberDto = new ProcessSecurityMemberDto()
                {
                    Parent = parent,
                    Type = SecurityMemberType.User,
                    Member = executor.User,
                    Level = SecurityMemberLevel.Reader,
                    IsExecutor = false
                };

                switch (statusDto.Type) 
                {
                    case TemplateToDoStatusType.Pending:
                    case TemplateToDoStatusType.Returned:
                        {
                            processSecurityMemberDto.Level = SecurityMemberLevel.Editor;
                            processSecurityMemberDto.IsExecutor = true;
                            break;
                        }
                    case TemplateToDoStatusType.InRevision:
                        {
                            processSecurityMemberDto.Level = SecurityMemberLevel.Editor;
                            break;
                        }
                }

                list.Add(processSecurityMemberDto);
            }

            foreach (ToDoActivityUserDto evaluator in dto.Evaluator)
            {
                processSecurityMemberDto = list.FirstOrDefault(p => p.Member == evaluator.User);

                if (processSecurityMemberDto == null)
                {
                    processSecurityMemberDto = new ProcessSecurityMemberDto()
                    {
                        Parent = parent,
                        Type = SecurityMemberType.User,
                        Member = evaluator.User,
                        Level = SecurityMemberLevel.Reader,
                        IsExecutor = false
                    };

                    list.Add(processSecurityMemberDto);
                }

                switch (statusDto.Type)
                {
                    case TemplateToDoStatusType.Pending:
                    case TemplateToDoStatusType.Returned:
                        {
                            processSecurityMemberDto.Level = SecurityMemberLevel.Editor;
                            break;
                        }
                    case TemplateToDoStatusType.InRevision:
                        {
                            processSecurityMemberDto.Level = SecurityMemberLevel.Editor;
                            processSecurityMemberDto.IsExecutor = true;
                            break;
                        }
                }
            }

            return list;
        }

        private async Task UpdateProcessSecurityAsync(ProcessDto dto)
        {
            List<ProcessSecurityMemberDto> activityMembers = await  GetProcessSecurityMemberFromActivityAsync(dto.Activity);
            List<ProcessSecurityMemberDto> currentMembers = await GetProcessSecurityMemberListAsync(dto.Id.Value, SecurityMemberType.User, SecurityMemberLevel.Reader);
            currentMembers.AddRange(await GetProcessSecurityMemberListAsync(dto.Id.Value, SecurityMemberType.User, SecurityMemberLevel.Editor));

            List<ProcessSecurityMemberDto> deletedMembers = currentMembers.FindAll(p =>
                !activityMembers.Exists(x => x.Member == p.Member && x.Type == p.Type && x.Level == p.Level));

            foreach (ProcessSecurityMemberDto deletedMember in deletedMembers)
            {
                await DeleteProcessSecurityMemberAsync(deletedMember.Id.Value, deletedMember.Type, deletedMember.Level);
            }

            ProcessSecurityMemberDto itemAux;

            foreach (ProcessSecurityMemberDto item in activityMembers) {
                itemAux = currentMembers.FirstOrDefault(x => x.Member == item.Member && x.Type == item.Type && x.Level == item.Level);

                if (itemAux == null) {
                    await CreateProcessSecurityMemberAsync(item);
                } else if (itemAux.Level == SecurityMemberLevel.Editor && item.IsExecutor != itemAux.IsExecutor) {
                    item.Id = itemAux.Id;

                    if (item.Type == SecurityMemberType.User) {
                        await UpdateProcessUserEditorAsync(item);
                    } else if (item.Type == SecurityMemberType.OrgUnit) {
                        await UpdateProcessOUEditorAsync(item);
                    }
                }
            }
        }

        #endregion

        #endregion

        #region ACTIVITIES

        public async Task<ToDoActivityDto> GetProcessToDoActivityAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetProcessToDoActivityQuery();

            ToDoActivityDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Processes.Process.Activity"), id));
            }

            return dto;
        }

        public async Task<long> CreateProcessToDoActivityAsync(ToDoActivityDto dto)
        {
            ValidateProcessToDoActivity(dto);

            var entity = new ToDoActivity();

            entity.UserCreator = SessionContext.UserId.Value;
            entity.Status = dto.Status;
            entity.CreationTime = DateTime.UtcNow;
            entity.Description = dto.Description;
            entity.InitialPlannedDate = dto.InitialPlannedDate;
            entity.FinalPlannedDate = dto.FinalPlannedDate;
            entity.InitialRealDate = dto.InitialRealDate;
            entity.FinalRealDate = dto.FinalRealDate;
            entity.IsOnTime = await CalculateProcessToDoActivityIsOnTimeAsync(dto);
            entity.table = dto.Table;
            entity.key = dto.Key;

            foreach(ToDoActivityUserDto executorDto in dto.Executor) {
                entity.ToDoActExecutor.Add(new ToDoActExecutor() { User = executorDto.User });
            }

            foreach (ToDoActivityUserDto evaluatorDto in dto.Evaluator)
            {
                entity.ToDoActEvaluator.Add(new ToDoActEvaluator() { User = evaluatorDto.User });
            }

            entity.Id = _repositoryToDoActivity.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateProcessToDoActivityAsync(ToDoActivityDto dto)
        {
            ValidateProcessToDoActivity(dto);

            await _repositoryToDoActExecutor.DeleteAsync(p => p.ToDoActivity == dto.Id.Value);
            await _repositoryToDoActEvaluator.DeleteAsync(p => p.ToDoActivity == dto.Id.Value);

            var entity = await _repositoryToDoActivity.FirstOrDefaultAsync(dto.Id.Value);

            entity.Status = dto.Status;
            entity.Description = dto.Description;
            entity.InitialPlannedDate = dto.InitialPlannedDate;
            entity.FinalPlannedDate = dto.FinalPlannedDate;
            entity.InitialRealDate = dto.InitialRealDate;
            entity.FinalRealDate = dto.FinalRealDate;
            entity.IsOnTime = await CalculateProcessToDoActivityIsOnTimeAsync(dto);

            foreach (ToDoActivityUserDto executorDto in dto.Executor)
            {
                entity.ToDoActExecutor.Add(new ToDoActExecutor() { User = executorDto.User });
            }

            foreach (ToDoActivityUserDto evaluatorDto in dto.Evaluator)
            {
                entity.ToDoActEvaluator.Add(new ToDoActEvaluator() { User = evaluatorDto.User });
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task ChangeProcessToDoActivityStatusAsync(ToDoActivityDto dto)
        {
            var entity = await _repositoryToDoActivity.FirstOrDefaultAsync(dto.Id.Value);

            entity.Status = dto.Status;
            entity.IsOnTime = await CalculateProcessToDoActivityIsOnTimeAsync(dto);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task LinkProcessToDoActivityAsync(long processId, long activityId, string processTableName)
        {
            string query = string.Format("UPDATE {0} SET {1} = {2} WHERE TenantId {3} AND Id = @Id",
                processTableName,
                _managerTemplate.fieldNameForActivity,
                "@" + _managerTemplate.fieldNameForActivity,
                CurrentUnitOfWork.GetTenantId() == null ? "IS NULL" : "= " + CurrentUnitOfWork.GetTenantId().ToString()
            );

            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Id", processId);
            aParameters.Add("@" + _managerTemplate.fieldNameForActivity, activityId);

            await _sqlExecuter.ExecuteSqlCommandAsync(query, aParameters);
        }

        #region Private Methods

        private IQueryable<ToDoActivityDto> GetProcessToDoActivityQuery()
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            var query = (from entity in _repositoryToDoActivity.GetAll()
                         select new ToDoActivityDto
                         {
                             Id = entity.Id,
                             UserCreator = entity.UserCreator,
                             UserCreatorDesc = ((entity.UserCreatorNavigation.Name + " " + entity.UserCreatorNavigation.Lastname).Trim() + " " + entity.UserCreatorNavigation.SecondLastname).Trim(),
                             Status = entity.Status,
                             StatusDesc = entity.StatusNavigation.Name,
                             CreationTime = entity.CreationTime,
                             Description = entity.Description,
                             InitialPlannedDate = entity.InitialPlannedDate,
                             FinalPlannedDate = entity.FinalPlannedDate,
                             InitialRealDate = entity.InitialRealDate,
                             FinalRealDate = entity.FinalRealDate,
                             IsOnTime = entity.IsOnTime == true,
                             IsOnTimeDesc = entity.IsOnTime == true ? yesLabel : noLabel,
                             Table = entity.table,
                             Key = entity.key,
                             Executor = entity.ToDoActExecutor.Select(p => new ToDoActivityUserDto() { 
                                Id = p.Id,
                                ToDoActivity = p.ToDoActivity,
                                User = p.User,
                                UserDesc = ((p.UserNavigation.Name + " " + p.UserNavigation.Lastname).Trim() + " " + p.UserNavigation.SecondLastname).Trim()
                             }).ToList(),
                             Evaluator = entity.ToDoActEvaluator.Select(p => new ToDoActivityUserDto()
                             {
                                 Id = p.Id,
                                 ToDoActivity = p.ToDoActivity,
                                 User = p.User,
                                 UserDesc = ((p.UserNavigation.Name + " " + p.UserNavigation.Lastname).Trim() + " " + p.UserNavigation.SecondLastname).Trim()
                             }).ToList()
                         });

            return query;
        }

        private void ValidateProcessToDoActivity(ToDoActivityDto dto)
        {
            List<ValidationFailure> failures = new List<ValidationFailure>();
            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelGreaterThan = L("FieldGreaterThan");

            string labelDescription = L("Processes.Process.Activity.Description");
            string labelStatus = L("Processes.Process.Activity.Status");
            string labelFinalPlannedDate = L("Processes.Process.Activity.FinalPlannedDate");
            string labelExecutor = L("Processes.Process.Activity.Executor");
            string labelEvaluator = L("Processes.Process.Activity.Evaluator");

            if (dto.Description.IsNullOrWhiteSpace())
            {
                failures.Add(new ValidationFailure(_managerTemplate.GetTemplateFieldNameForActivity("Description"), string.Format(labelRequiredField, labelDescription)));
            } else if (dto.Description.Length > 100) {
                failures.Add(new ValidationFailure(_managerTemplate.GetTemplateFieldNameForActivity("Description"), string.Format(labelMaxLength, labelDescription, 100)));
            }

            if (dto.Status <= 0)
            {
                failures.Add(new ValidationFailure(_managerTemplate.GetTemplateFieldNameForActivity("Status"), string.Format(labelGreaterThan, labelStatus, 0)));
            }

            if (dto.FinalPlannedDate == null)
            {
                failures.Add(new ValidationFailure(_managerTemplate.GetTemplateFieldNameForActivity("FinalPlannedDate"), string.Format(labelRequiredField, labelFinalPlannedDate)));
            }

            if (dto.Executor == null || dto.Executor.Count <= 0)
            {
                failures.Add(new ValidationFailure(_managerTemplate.GetTemplateFieldNameForActivity("Executor"), string.Format(labelRequiredField, labelExecutor)));
            }

            if (dto.Evaluator == null || dto.Evaluator.Count <= 0)
            {
                failures.Add(new ValidationFailure(_managerTemplate.GetTemplateFieldNameForActivity("Evaluator"), string.Format(labelRequiredField, labelEvaluator)));
            }

            if (failures.Count > 0)
            {
                throw new Exceptions.ValidationException(failures, SessionContext, AppLocalizationProvider);
            }
        }

        private async Task<bool> CalculateProcessToDoActivityIsOnTimeAsync(ToDoActivityDto dto)
        {
            bool isOnTime = false;

            if (dto.FinalPlannedDate != null && dto.FinalRealDate != null && dto.FinalRealDate <= dto.FinalPlannedDate) {
                TemplateToDoStatusDto statusDto = await _managerTemplate.GetTemplateToDoStatusAsync(dto.Status);

                isOnTime = statusDto.Type == TemplateToDoStatusType.Closed;
            }            

            return isOnTime;
        }

        #endregion

        #endregion

        #region TIMESHEETS

        public async Task<ToDoTimeSheetDto> GetToDoTimeSheetAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetToDoTimeSheetQuery();

            ToDoTimeSheetDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Processes.Process.ToDoTimeSheets.ToDoTimeSheet"), id));
            }

            return dto;
        }

        public async Task<long> CreateToDoTimeSheetAsync(ToDoTimeSheetDto dto)
        {
            var entity = new ToDoTimeSheet();

            entity.ToDoActivity = dto.Activity;
            entity.UserCreator = SessionContext.UserId.Value;
            entity.CreationDate = dto.CreationDate;
            entity.CreationTime = DateTime.UtcNow;
            entity.Comments = dto.Comments;
            entity.HoursSpend = dto.HoursSpend;

            entity.Id = _repositoryToDoTimeSheet.InsertAndGetId(entity);

            ToDoActivityDto activityDto = await GetProcessToDoActivityAsync(dto.Activity);

            if (dto.ActivityStatus.HasValue && activityDto.Status != dto.ActivityStatus) {
                activityDto.Status = dto.ActivityStatus.Value;
                await ChangeProcessToDoActivityStatusAsync(activityDto);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateToDoTimeSheetAsync(ToDoTimeSheetDto dto)
        {
            var entity = await _repositoryToDoTimeSheet.FirstOrDefaultAsync(dto.Id.Value);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Processes.Process.ToDoTimeSheets.ToDoTimeSheet"), dto.Id));
            }

            entity.CreationDate = dto.CreationDate;
            entity.Comments = dto.Comments;
            entity.HoursSpend = dto.HoursSpend;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteToDoTimeSheetAsync(long id)
        {
            var entity = await _repositoryToDoTimeSheet.FirstOrDefaultAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Processes.Process.ToDoTimeSheets.ToDoTimeSheet"), id));
            }

            await _repositoryToDoTimeSheet.DeleteAsync(entity);
        }

        #region Private Methods

        private IQueryable<ToDoTimeSheetDto> GetToDoTimeSheetQuery()
        {
            var query = (from entity in _repositoryToDoTimeSheet.GetAll()
                         select new ToDoTimeSheetDto
                         {
                             Id = entity.Id,
                             Activity = entity.ToDoActivity,
                             UserCreator = entity.UserCreator,
                             UserCreatorDesc = ((entity.UserCreatorNavigation.Name + " " + entity.UserCreatorNavigation.Lastname).Trim() + " " + entity.UserCreatorNavigation.SecondLastname).Trim(),
                             CreationDate = entity.CreationDate,
                             CreationTime = entity.CreationTime,
                             Comments = entity.Comments,
                             HoursSpend = entity.HoursSpend.Value
                         });

            return query;
        }

        #endregion

        #endregion

        #region SECURITY

        public async Task<PagedResultDto<ProcessSecurityMemberDto>> GetProcessSecurityMemberListAsync(ProcessSecurityMemberListFilterDto dto)
        {
            string query = GetProcessSecurityMemberListQuery(dto);
            int count = await _sqlExecuter.ExecuteSqlQueryScalar<int>(string.Format("SELECT COUNT(*) FROM ({0}) WRAPPERCOUNT", query));
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@strSelectTabla", query);
            aParameters.Add("@numeropagina", dto.PageNumber > 0 ? dto.PageNumber - 1 : 0);
            aParameters.Add("@porpagina", dto.PageSize);
            aParameters.Add("@orderby", dto.Sorting);

            List<ProcessSecurityMemberDto> list = await _sqlExecuter.SqlStoredProcedure<ProcessSecurityMemberDto>("spr_obtienedatosporseccion", aParameters);

            return new PagedResultDto<ProcessSecurityMemberDto>(count, list);
        }

        public async Task<List<ProcessSecurityMemberDto>> GetProcessSecurityMemberListAsync(long parent, SecurityMemberType? type = null, SecurityMemberLevel? level = null)
        {
            return await _sqlExecuter.SqlQuery<ProcessSecurityMemberDto>(GetProcessSecurityMemberQuery(parent, type, level));
        }

		public async Task<List<ProcessSecurityMemberDto>> GetProcessSecurityMemberListAsync(
	        long template,
	        long parent,
	        long member,
	        SecurityMemberType type,
	        SecurityMemberLevel level)
		{
			var query = GetProcessSecurityMemberQuery(null, type, level);
			query = string.Format("SELECT * FROM ({0}) WRAPPER WHERE Parent = @Parent AND Member = @Member", query);
			Dictionary<string, object> aParameters = new Dictionary<string, object>();

			aParameters.Add("@Parent", parent);
			aParameters.Add("@Member", member);

			return await _sqlExecuter.SqlQuery<ProcessSecurityMemberDto>(query, aParameters);
		}

		public async Task<ProcessSecurityMemberDto> GetProcessSecurityMemberAsync(
            long template,
            long id,
            SecurityMemberType type,
            SecurityMemberLevel level,
            bool throwExceptionIfNotFound = true
        )
        {
            var query = GetProcessSecurityMemberQuery(null, type, level);
            query = string.Format("SELECT * FROM ({0}) WRAPPER WHERE Id = @Id", query);
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Id", id);

            List<ProcessSecurityMemberDto> list = await _sqlExecuter.SqlQuery<ProcessSecurityMemberDto>(query, aParameters);

            ProcessSecurityMemberDto dto = list.FirstOrDefault();

            if (dto == null && throwExceptionIfNotFound)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("ProcessSecurityMember.ProcessSecurityMember"), id));
            }

            return dto;
        }

        public async Task<long> CreateProcessSecurityMemberAsync(ProcessSecurityMemberDto dto, bool validate = true)
        {
            if (validate)
            {
                await ValidateProcessSecurityMemberAsync(dto);
            }

            if (dto.Type == SecurityMemberType.User)
            {
                if (dto.Level == SecurityMemberLevel.Reader)
                {
                    return await CreateProcessUserReaderAsync(dto);
                }
                else if (dto.Level == SecurityMemberLevel.Editor)
                {
                    return await CreateProcessUserEditorAsync(dto);
                }
            }
            else if (dto.Type == SecurityMemberType.OrgUnit)
            {
                if (dto.Level == SecurityMemberLevel.Reader)
                {
                    return await CreateProcessOUReaderAsync(dto);
                }
                else if (dto.Level == SecurityMemberLevel.Editor)
                {
                    return await CreateProcessOUEditorAsync(dto);
                }
            }

            throw new EntityNotFoundException(L("TemplateSecurity.MissingData"));
        }

        public async Task DeleteProcessSecurityMemberAsync(long id, SecurityMemberType type, SecurityMemberLevel level)
        {
            var query = string.Format("DELETE FROM {0} WHERE Id = @Id", GetSecurityMemberTableName(type, level));
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Id", id);

            await _sqlExecuter.ExecuteSqlCommandAsync(query, aParameters);
        }

        public async Task DeleteProcessSecurityMemberByParentAsync(long parent, SecurityMemberType type, SecurityMemberLevel level)
        {
            var query = string.Format("DELETE FROM {0} WHERE Parent = @Parent", GetSecurityMemberTableName(type, level));
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Parent", parent);

            await _sqlExecuter.ExecuteSqlCommandAsync(query, aParameters);
        }

        public async Task<bool> HasProcessReadPermission(long id)
        {
            string query = "SELECT COUNT(Id) " +
                    "FROM (" +
                        "(SELECT Id FROM {{TABLENAME}}_UserReader WHERE Parent = @Parent AND [User] = @User) " +
                        "UNION (SELECT Id FROM {{TABLENAME}}_UserEditor WHERE Parent = @Parent AND [User] = @User) " +
                        "UNION (SELECT {{TABLENAME}}_OUReader.Id FROM {{TABLENAME}}_OUReader " +
                            "INNER JOIN OUUsersSecurity ON {{TABLENAME}}_OUReader.OrgUnit = OUUsersSecurity.Id AND OUUsersSecurity.[User] = @User " +
                            "WHERE Parent = @Parent) " +
                        "UNION (SELECT {{TABLENAME}}_OUEditor.Id FROM {{TABLENAME}}_OUEditor " +
                            "INNER JOIN OUUsersSecurity ON {{TABLENAME}}_OUEditor.OrgUnit = OUUsersSecurity.Id AND OUUsersSecurity.[User] = @User " +
                            "WHERE Parent = @Parent)" +
                    ") UNIONS";

            query = query.Replace("{{TABLENAME}}", _templateDto.TableName);

            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Parent", id);
            aParameters.Add("@User", SessionContext.UserId.Value);

            return (await _sqlExecuter.ExecuteSqlQueryScalar<int>(query, aParameters)) > 0;
        }

        public async Task<bool> HasProcessEditPermission(long id)
        {
            string query = "SELECT COUNT(Id) " +
                    "FROM (" +
                        "(SELECT Id FROM {{TABLENAME}}_UserEditor WHERE Parent = @Parent AND [User] = @User) " +
                        "UNION (SELECT {{TABLENAME}}_OUEditor.Id FROM {{TABLENAME}}_OUEditor " +
                            "INNER JOIN OUUsersSecurity ON {{TABLENAME}}_OUEditor.OrgUnit = OUUsersSecurity.Id AND OUUsersSecurity.[User] = @User " +
                            "WHERE Parent = @Parent)" +
                    ") UNIONS";

            query = query.Replace("{{TABLENAME}}", _templateDto.TableName);

            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Parent", id);
            aParameters.Add("@User", SessionContext.UserId.Value);

            return (await _sqlExecuter.ExecuteSqlQueryScalar<int>(query, aParameters)) > 0;
        }

        public async Task ValidateProcessReadPermission(long id)
        {
            if (_templateDto.HasSecurity && !(await HasProcessReadPermission(id)))
            {
                throw new UserUnauthorizedException();
            }
        }

        public async Task ValidateProcessEditPermission(long id)
        {
            if (_templateDto.HasSecurity && !(await HasProcessEditPermission(id))) {
                throw new UserUnauthorizedException();
            }
        }

        #region Private Methods

        private string GetProcessSecurityMemberQuery(long? parent = null, SecurityMemberType? type = null, SecurityMemberLevel? level = null)
        {
            string query = string.Empty;

            if (type == null && level == null)
            {
                string queryUserReader = GetProcessQueryUserReader(parent);
                string queryOrgUnitReader = GetProcessQueryOrgUnitReader(parent);
                string queryUserEditor = GetProcessQueryUserEditor(parent);
                string queryOrgUnitEditor = GetProcessQueryOrgUnitEditor(parent);

                query = string.Format("({0}) UNION ({1}) UNION ({2}) UNION ({3})", queryUserReader, queryOrgUnitReader, queryUserEditor, queryOrgUnitEditor);
            }
            else
            {
                if (type == SecurityMemberType.User)
                {
                    if (level == SecurityMemberLevel.Reader)
                    {
                        query = GetProcessQueryUserReader(parent);
                    }
                    else if (level == SecurityMemberLevel.Editor)
                    {
                        query = GetProcessQueryUserEditor(parent);
                    }
                }
                else if (type == SecurityMemberType.OrgUnit)
                {
                    if (level == SecurityMemberLevel.Reader)
                    {
                        query = GetProcessQueryOrgUnitReader(parent);
                    }
                    else if (level == SecurityMemberLevel.Editor)
                    {
                        query = GetProcessQueryOrgUnitEditor(parent);
                    }
                }
            }

            return query;
        }

        private string GetProcessSecurityMemberListQuery(ProcessSecurityMemberListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();
            string query = GetProcessSecurityMemberQuery(dto.Parent);
            List<string> queryWhereConditions = new List<string>();

            if (dto.Type.HasValue)
            {
                queryWhereConditions.Add(string.Format("Type = {0}", (byte)dto.Type));
            }

            if (dto.Level.HasValue)
            {
                queryWhereConditions.Add(string.Format("Level = {0}", (byte)dto.Level));
            }

            if (filter.IsNullOrWhiteSpace())
            {
                if (queryWhereConditions.Count > 0)
                {
                    query = string.Format("SELECT * FROM ({0}) WRAPPER WHERE {1}", query, string.Join(" AND ", queryWhereConditions));
                }
            }
            else
            {
                string queryWhere = "WHERE (Id LIKE '%{{FILTER}}%' OR TypeDesc LIKE '%{{FILTER}}%'" +
                " OR MemberDesc LIKE '%{{FILTER}}%' OR LevelDesc LIKE '%{{FILTER}}%' OR IsExecutorDesc LIKE '%{{FILTER}}%')";

                queryWhere = queryWhere.Replace("{{FILTER}}", filter);
                query = string.Format("SELECT * FROM ({0}) WRAPPER {1}", query, queryWhere);

                if (queryWhereConditions.Count > 0)
                {
                    query += string.Format(" AND {0}", string.Join(" AND ", queryWhereConditions));
                }
            }

            return query;
        }

        private string GetProcessQueryUserReader(long? parent = null)
        {
            string noLabel = L("No");
            string typeUserDesc = L("TemplateSecurity.Type.User");
            string levelReaderDesc = L("TemplateSecurity.Level.Reader");

            string query = "SELECT {{TableName}}.Id, {{TableName}}.Parent, 1 Type, '{0}' TypeDesc, {{TableName}}.[User] Member" +
                ", TRIM(TRIM([User].Name + ' ' + [User].Lastname) + ' ' + [User].SecondLastname) MemberDesc" +
                ", 1 Level, '{1}' LevelDesc, 0 IsExecutor, '{2}' IsExecutorDesc" +
                " FROM {{TableName}} INNER JOIN [User] ON {{TableName}}.[User] = [User].Id";

            if (parent.HasValue)
            {
                query += " WHERE {{TableName}}.Parent = " + parent.Value;
            }

            query = query.Replace("{{TableName}}", _templateDto.TableName + "_UserReader");

            return string.Format(query, typeUserDesc, levelReaderDesc, noLabel);
        }

        private string GetProcessQueryUserEditor(long? parent = null)
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");
            string typeUserDesc = L("TemplateSecurity.Type.User");
            string levelEditorDesc = L("TemplateSecurity.Level.Editor");

            string query = "SELECT {{TableName}}.Id, {{TableName}}.Parent, 1 Type, '{0}' TypeDesc, {{TableName}}.[User] Member" +
                ", TRIM(TRIM([User].Name + ' ' + [User].Lastname) + ' ' + [User].SecondLastname) MemberDesc" +
                ", 2 Level, '{1}' LevelDesc, {{TableName}}.IsExecutor, CASE {{TableName}}.IsExecutor WHEN 1 THEN '{2}' ELSE '{3}' END IsExecutorDesc" +
                " FROM {{TableName}} INNER JOIN [User] ON {{TableName}}.[User] = [User].Id";

            if (parent.HasValue) {
                query += " WHERE {{TableName}}.Parent = " + parent.Value;
            }

            query = query.Replace("{{TableName}}", _templateDto.TableName + "_UserEditor");

            return string.Format(query, typeUserDesc, levelEditorDesc, yesLabel, noLabel);
        }

        private string GetProcessQueryOrgUnitReader(long? parent = null)
        {
            string noLabel = L("No");
            string typeOrgUnitDesc = L("TemplateSecurity.Type.OrgUnit");
            string levelReaderDesc = L("TemplateSecurity.Level.Reader");

            string query = "SELECT {{TableName}}.Id, {{TableName}}.Parent, 2 Type, '{0}' TypeDesc, {{TableName}}.OrgUnit Member" +
                ", OrgUnit.Name MemberDesc, 1 Level, '{1}' LevelDesc, 0 IsExecutor, '{2}' IsExecutorDesc" +
                " FROM {{TableName}} INNER JOIN OrgUnit ON {{TableName}}.OrgUnit = OrgUnit.Id AND (OrgUnit.IsDeleted IS NULL OR OrgUnit.IsDeleted = 0)";

            if (parent.HasValue)
            {
                query += " WHERE {{TableName}}.Parent = " + parent.Value;
            }

            query = query.Replace("{{TableName}}", _templateDto.TableName + "_OUReader");

            return string.Format(query, typeOrgUnitDesc, levelReaderDesc, noLabel);
        }

        private string GetProcessQueryOrgUnitEditor(long? parent = null)
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");
            string typeOrgUnitDesc = L("TemplateSecurity.Type.OrgUnit");
            string levelEditorDesc = L("TemplateSecurity.Level.Editor");

            string query = "SELECT {{TableName}}.Id, {{TableName}}.Parent, 2 Type, '{0}' TypeDesc, {{TableName}}.OrgUnit Member" +
                ", OrgUnit.Name MemberDesc, 2 Level, '{1}' LevelDesc, {{TableName}}.IsExecutor, CASE {{TableName}}.IsExecutor WHEN 1 THEN '{2}' ELSE '{3}' END IsExecutorDesc" +
                " FROM {{TableName}} INNER JOIN OrgUnit ON {{TableName}}.OrgUnit = OrgUnit.Id AND (OrgUnit.IsDeleted IS NULL OR OrgUnit.IsDeleted = 0)";

            if (parent.HasValue)
            {
                query += " WHERE {{TableName}}.Parent = " + parent.Value;
            }

            query = query.Replace("{{TableName}}", _templateDto.TableName + "_OUEditor");

            return string.Format(query, typeOrgUnitDesc, levelEditorDesc, yesLabel, noLabel);
        }

        private async Task ValidateProcessSecurityMemberAsync(ProcessSecurityMemberDto dto)
        {
            List<ProcessSecurityMemberDto> list;

            list = await GetProcessSecurityMemberListAsync(_templateDto.Id.Value, dto.Parent, dto.Member, dto.Type, dto.Level);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("TemplateSecurity.DuplicatedName"));
            }
        }

        private async Task<long> CreateProcessUserReaderAsync(ProcessSecurityMemberDto dto)
        {
            var query = string.Format("INSERT {0} ([Parent], [User]) VALUES (@Parent, @Member); SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS Id",
                _templateDto.TableName + "_UserReader");
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Parent", dto.Parent);
            aParameters.Add("@Member", dto.Member);

            return await _sqlExecuter.ExecuteSqlQueryScalar<long>(query, aParameters);
        }

        private async Task<long> CreateProcessOUReaderAsync(ProcessSecurityMemberDto dto)
        {
            var query = string.Format("INSERT {0} ([Parent], [OrgUnit]) VALUES (@Parent, @Member); SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS Id",
                _templateDto.TableName + "_OUReader");
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Parent", dto.Parent);
            aParameters.Add("@Member", dto.Member);

            return await _sqlExecuter.ExecuteSqlQueryScalar<long>(query, aParameters);
        }

        private async Task<long> CreateProcessUserEditorAsync(ProcessSecurityMemberDto dto)
        {
            var query = string.Format("INSERT {0} ([Parent], [User], [IsExecutor]) VALUES (@Parent, @Member, @IsExecutor); SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS Id",
                _templateDto.TableName + "_UserEditor");
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Parent", dto.Parent);
            aParameters.Add("@Member", dto.Member);
            aParameters.Add("@IsExecutor", dto.IsExecutor);

            return await _sqlExecuter.ExecuteSqlQueryScalar<long>(query, aParameters);
        }

        private async Task<long> CreateProcessOUEditorAsync(ProcessSecurityMemberDto dto)
        {
            var query = string.Format("INSERT {0} ([Parent], [OrgUnit], [IsExecutor]) VALUES (@Parent, @Member, @IsExecutor); SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS Id",
                _templateDto.TableName + "_OUEditor");
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Parent", dto.Parent);
            aParameters.Add("@Member", dto.Member);
            aParameters.Add("@IsExecutor", dto.IsExecutor);

            return await _sqlExecuter.ExecuteSqlQueryScalar<long>(query, aParameters);
        }

        private async Task<long> UpdateProcessUserEditorAsync(ProcessSecurityMemberDto dto)
        {
            var query = string.Format("UPDATE {0} SET [IsExecutor] = @IsExecutor WHERE [Id] = @Id;",
                _templateDto.TableName + "_UserEditor");
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Id", dto.Id);
            aParameters.Add("@IsExecutor", dto.IsExecutor);

            return await _sqlExecuter.ExecuteSqlCommandAsync(query, aParameters);
        }

        private async Task<long> UpdateProcessOUEditorAsync(ProcessSecurityMemberDto dto)
        {
            var query = string.Format("UPDATE {0} SET [IsExecutor] = @IsExecutor WHERE [Id] = @Id;",
                _templateDto.TableName + "_OUEditor");
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@Id", dto.Id);
            aParameters.Add("@IsExecutor", dto.IsExecutor);

            return await _sqlExecuter.ExecuteSqlCommandAsync(query, aParameters);
        }

        private string GetSecurityMemberTableName(SecurityMemberType type, SecurityMemberLevel level) {
            string tableNameSufix = string.Empty;

            if (type == SecurityMemberType.User)
            {
                if (level == SecurityMemberLevel.Reader)
                {
                    tableNameSufix = "_UserReader";
                }
                else if (level == SecurityMemberLevel.Editor)
                {
                    tableNameSufix = "_UserEditor";
                }
            }
            else if (type == SecurityMemberType.OrgUnit)
            {
                if (level == SecurityMemberLevel.Reader)
                {
                    tableNameSufix = "_OUReader";
                }
                else if (level == SecurityMemberLevel.Editor)
                {
                    tableNameSufix = "_OUEditor";
                }
            }

            return _templateDto.TableName + tableNameSufix;
        }

        #endregion

        #endregion
    }
}
