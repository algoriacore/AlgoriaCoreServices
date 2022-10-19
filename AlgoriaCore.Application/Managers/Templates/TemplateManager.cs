using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.BinaryObjects;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.Templates.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using ScriptBuilder.Structure;
using ScriptBuilder.Structure.Fields.Dates;
using ScriptBuilder.Structure.Fields.Numerics;
using ScriptBuilder.Structure.Fields.Texts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Templates
{
    public class TemplateManager : BaseManager
    {
        private readonly IRepository<Template, long> _repository;
        private readonly IRepository<TemplateSection, long> _repositoryTemplateSection;
        private readonly IRepository<TemplateField, long> _repositoryTemplateField;
        private readonly IRepository<TemplateFieldOption, long> _repositoryTemplateFieldOption;
        private readonly IRepository<TemplateFieldRelation, long> _repositoryTemplateFieldRelation;
        private readonly IRepository<TemplateQuery, long> _repositoryTemplateQuery;
        private readonly IRepository<TemplateToDoStatus, long> _repositoryTemplateToDoStatus;
        private readonly IRepository<ToDoActivity, long> _repositoryToDoActivity;
        private readonly IRepository<TemplateDefaultUserReader, long> _repositoryTemplateDefaultUserReader;
        private readonly IRepository<TemplateDefaultUserEditor, long> _repositoryTemplateDefaultUserEditor;
        private readonly IRepository<TemplateDefaultOUReader, long> _repositoryTemplateDefaultOUReader;
        private readonly IRepository<TemplateDefaultOUEditor, long> _repositoryTemplateDefaultOUEditor;

        private readonly ISqlExecuter _sqlExecuter;

        private readonly BinaryObjectManager _managerBinaryObject;

        private readonly string activityTableName = "ToDoActivity";
        private readonly string activityStatusTableName = "TemplateToDoStatus";
        private readonly string prefixActivityFieldName = "ACT";
        public readonly string fieldNameForActivity = "ToDoActivity";

        public TemplateManager(
            IRepository<Template, long> repository,
            IRepository<TemplateSection, long> repositoryTemplateSection,
            IRepository<TemplateField, long> repositoryTemplateField,
            IRepository<TemplateFieldOption, long> repositoryTemplateFieldOption,
            IRepository<TemplateFieldRelation, long> repositoryTemplateFieldRelation,
            IRepository<TemplateQuery, long> repositoryTemplateQuery,
            IRepository<TemplateToDoStatus, long> repositoryTemplateToDoStatus,
            IRepository<ToDoActivity, long> repositoryToDoActivity,
            IRepository<TemplateDefaultUserReader, long> repositoryTemplateDefaultUserReader,
            IRepository<TemplateDefaultUserEditor, long> repositoryTemplateDefaultUserEditor,
            IRepository<TemplateDefaultOUReader, long> repositoryTemplateDefaultOUReader,
            IRepository<TemplateDefaultOUEditor, long> repositoryTemplateDefaultOUEditor,
            BinaryObjectManager managerBinaryObject,
            ISqlExecuter sqlExecuter
        )
        {
            _repository = repository;
            _repositoryTemplateSection = repositoryTemplateSection;
            _repositoryTemplateField = repositoryTemplateField;
            _repositoryTemplateFieldOption = repositoryTemplateFieldOption;
            _repositoryTemplateFieldRelation = repositoryTemplateFieldRelation;
            _repositoryTemplateQuery = repositoryTemplateQuery;
            _repositoryTemplateToDoStatus = repositoryTemplateToDoStatus;
            _repositoryToDoActivity = repositoryToDoActivity;
            _repositoryTemplateDefaultUserReader = repositoryTemplateDefaultUserReader;
            _repositoryTemplateDefaultUserEditor = repositoryTemplateDefaultUserEditor;
            _repositoryTemplateDefaultOUReader = repositoryTemplateDefaultOUReader;
            _repositoryTemplateDefaultOUEditor = repositoryTemplateDefaultOUEditor;

            _managerBinaryObject = managerBinaryObject;
            _sqlExecuter = sqlExecuter;
        }

        #region TEMPLATES

        public async Task<PagedResultDto<TemplateDto>> GetTemplateListAsync(TemplateListFilterDto dto)
        {
            var query = GetTemplateListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "NameSingular" : dto.Sorting)
                .PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<TemplateDto>(count, ll);
        }

        public async Task<List<TemplateDto>> GetTemplateNoPagedListAsync()
        {
            var query = GetTemplateQuery();

            return await query.ToListAsync();
        }

        public async Task<List<TemplateDto>> GetTemplateReadyListAsync()
        {
            return await Task.Run(() =>
            {
                return GetTemplateReadyList();
            });
        }

        public List<TemplateDto> GetTemplateReadyList()
        {
            var query = GetTemplateQuery();

            return query.Where(p => p.IsActive && p.IsTableGenerated).ToList();
        }

        public async Task<List<ComboboxItemDto>> GetTemplateComboAsync()
        {
            return await GetTemplateComboAsync(new TemplateComboFilterDto() { IsActive = true });
        }

        public async Task<List<ComboboxItemDto>> GetTemplateComboAsync(TemplateComboFilterDto dto)
        {
            var query = _repository.GetAll()
                .WhereIf(dto.IsActive != null, p => p.IsActive == dto.IsActive)
                .WhereIf(!dto.Filter.IsNullOrWhiteSpace(), p => p.NameSingular.Contains(dto.Filter))
                .OrderBy(p => p.NameSingular)
                .Select(p => new ComboboxItemDto
                {
                    Value = p.Id.ToString(),
                    Label = p.NameSingular
                });

            return await query.ToListAsync();
        }

        public async Task<TemplateDto> GetTemplateAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetTemplateQuery();

            TemplateDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Templates.Template"), id));
            }

            return dto;
        }

        public async Task<List<TemplateDto>> GetTemplateByTableNameListAsync(string tableName)
        {
            var query = GetTemplateQuery();

            return await query.Where(p => p.TableName == tableName).ToListAsync();
        }

        public async Task<long> CreateTemplateAsync(TemplateDto dto)
        {
            dto.TableName = CalculateTableName(dto.NameSingular);

            await ValidateTemplateAsync(dto);

            var entity = new Template();

            entity.RGBColor = dto.RGBColor;
            entity.NameSingular = dto.NameSingular;
            entity.NamePlural = dto.NamePlural;
            entity.Description = dto.Description;
            entity.TableName = dto.TableName;
            entity.IsTableGenerated = false;
            entity.HasChatRoom = dto.HasChatRoom;
            entity.IsActivity = dto.IsActivity;
            entity.HasSecurity = dto.HasSecurity;
            entity.IsActive = dto.IsActive;

            if (dto.IconBytes != null && dto.IconBytes.Length > 0)
            {
                entity.Icon = await _managerBinaryObject.CreateAsync(dto.IconBytes);
            }

            entity.Id = _repository.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeTemplate(await GetTemplateAsync(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task UpdateTemplateAsync(TemplateDto dto)
        {
            TemplateDto previousDto = await GetTemplateAsync(dto.Id.Value);

            dto.TableName = previousDto.IsTableGenerated ? previousDto.TableName : CalculateTableName(dto.NameSingular);

            await ValidateTemplateAsync(dto);

            var entity = await _repository.FirstOrDefaultAsync(dto.Id.Value);

            entity.RGBColor = dto.RGBColor;
            entity.NameSingular = dto.NameSingular;
            entity.NamePlural = dto.NamePlural;
            entity.Description = dto.Description;
            entity.HasChatRoom = dto.HasChatRoom;
            entity.IsActivity = dto.IsActivity;
            entity.HasSecurity = dto.HasSecurity;
            entity.IsActive = dto.IsActive;

            if (entity.IsTableGenerated != true)
            {
                entity.TableName = dto.TableName;
            }

            if (dto.IconBytes != null && dto.IconBytes.Length > 0)
            {
                if (entity.Icon == null)
                {
                    entity.Icon = await _managerBinaryObject.CreateAsync(dto.IconBytes);
                }
                else
                {
                    await _managerBinaryObject.UpdateAsync(entity.Icon.Value, dto.IconBytes);
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            TemplateDto newTemplateDto = await GetTemplateAsync(entity.Id);

            if (previousDto.IsActivity != dto.IsActivity || previousDto.HasSecurity != dto.HasSecurity)
            {
                await GenerateTemplateQueries(newTemplateDto);
            }

            await LogChangeTemplate(newTemplateDto, previousDto, ChangeLogType.Update);
        }

        public async Task DeleteTemplateAsync(long id)
        {
            TemplateDto previousDto = await GetTemplateAsync(id);

            await DeleteTemplateFieldOptionByTemplateAsync(id);
            await DeleteTemplateFieldByTemplateAsync(id);
            await DeleteTemplateSectionByTemplateAsync(id);
            await _repository.DeleteAsync(id);

            await LogChangeTemplate(null, previousDto, ChangeLogType.Delete);
        }

        public async Task GenerateDbTableAsync(long template)
        {
            TemplateDto dto = await GetTemplateAsync(template);
            List<TemplateFieldDto> templateFields = await GetTemplateFieldByTemplateListAsync(template);
            List<TemplateFieldDto> templateFieldsDeleted = await GetTemplateFieldDeletedByTemplateListAsync(template);
            TemplateDto templateRelation;

            // VALIDAR QUE TODOS LOS CAMPOS TEMPLATES RELATIONS EXISTA SU CAMPO EN TABLA
            List<long> templateFieldsRelations = templateFields.Where(p => p.FieldType == TemplateFieldType.Template && p.TemplateFieldRelationTemplateField.HasValue)
            .Select(p => p.TemplateFieldRelationTemplateField.Value).Distinct().ToList();
            byte? statusProccesed = (byte?) TemplateFieldStatus.Processed;

            if (templateFieldsRelations.Count > 0
                && (await _repositoryTemplateField.CountAsync(p => templateFieldsRelations.Contains(p.Id) && p.Status == statusProccesed)) != templateFieldsRelations.Count)
            {
                throw new AlgoriaCoreGeneralException(L("Templates.Template.GenerateTableFieldRelationsNoAvailables"));
            }

            // VALIDAR QUE TODOS LOS CAMPOS A ELIMINAR DE BD NO SEAN RELACIONES YA GENERADAS A CAMPOS DE OTRA PLANTILLA
            List<long> idsToDelete = templateFieldsDeleted.Select(p => p.Id.Value).ToList();

            if (idsToDelete.Count > 0
                && (await _repositoryTemplateField.CountAsync(p => idsToDelete.Contains(p.TemplateFieldRelationTemplateFieldNavigation.TemplateFieldRelation1)
                && p.Status == statusProccesed)) > 0)
            {
                throw new AlgoriaCoreGeneralException(L("Templates.Template.GenerateTableFieldRelationsNoAvailables"));
            }

            ScriptBuilder.Structure.ScriptBuilder sb = new ScriptBuilder.Structure.ScriptBuilder();

            TableDefinition tb = sb.AddTableDefinition(dto.TableName);

            if (!dto.IsTableGenerated) {
                tb.AsNewTable();
                tb.AddField<BigIntFieldDefinition>("Id").AsIdentity(1, 1).AsPrimaryKey().AsNotNull();
                tb.AddField<IntFieldDefinition>("TenantId").AsForeignKey("Tenant", "Id");
                tb.AddField<BigIntFieldDefinition>("ToDoActivity").AsForeignKey("ToDoActivity", "Id");

                AddDbTablesOthersAsNewToDefinition(sb, dto);

                await MarkTemplateAsTableGeneratedAsync(template);
            }

            foreach (TemplateFieldDto templateFieldDeleted in templateFieldsDeleted)
            {
                if (templateFieldDeleted.FieldType != TemplateFieldType.Multivalue) {
                    if (!templateFieldDeleted.TemplateFieldRelationTemplateTableName.IsNullOrWhiteSpace() &&
                        !templateFieldDeleted.TemplateFieldRelationTemplateFieldName.IsNullOrWhiteSpace())
                    {

                        tb.DropForeignKey(templateFieldDeleted.FieldName, templateFieldDeleted.TemplateFieldRelationTemplateTableName,
                            templateFieldDeleted.TemplateFieldRelationTemplateFieldName);
                    }

                    tb.DropField(templateFieldDeleted.FieldName);
                    
                    await ChangeTemplateFieldStatusAsync(templateFieldDeleted.Id.Value, TemplateFieldStatus.Processed);
                }
            }

            foreach (TemplateFieldDto templateField in templateFields.Where(p => p.Status == TemplateFieldStatus.New))
            {
                if (!templateField.MustHaveOptions)
                {
                    switch (templateField.FieldType)
                    {
                        case TemplateFieldType.Boolean:
                            tb.AddField<BitFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.Text:
                            tb.AddField<VarCharFieldDefinition>(templateField.FieldName).WithLength(templateField.FieldSize.Value);
                            break;
                        case TemplateFieldType.Multivalue:
                            // SE CUBRE CON LA TABLA DE OPCIONES
                            break;
                        case TemplateFieldType.Date:
                            tb.AddField<DateFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.Time:
                            tb.AddField<TimeFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.DateTime:
                            tb.AddField<DateTimeFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.Integer:
                            tb.AddField<IntFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.Decimal:
                            tb.AddField<DecimalFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.GoogleAddress:
                            tb.AddField<VarCharFieldDefinition>(templateField.FieldName).WithLength(500);
                            break;
                        case TemplateFieldType.Template:
                            templateRelation = await GetTemplateAsync(templateField.TemplateFieldRelationTemplate.Value);
                            tb.AddField<BigIntFieldDefinition>(templateField.FieldName).AsForeignKey(templateRelation.TableName, "Id");
                            break;
                        case TemplateFieldType.User:
                            tb.AddField<BigIntFieldDefinition>(templateField.FieldName);
                            break;
                    }
                }

                await ChangeTemplateFieldStatusAsync(templateField.Id.Value, TemplateFieldStatus.Processed);
            }

            foreach (TemplateFieldDto templateField in templateFields.Where(p => p.Status == TemplateFieldStatus.Modified))
            {
                if (!templateField.MustHaveOptions)
                {
                    switch (templateField.FieldType)
                    {
                        case TemplateFieldType.Boolean:
                            tb.ChangeField<BitFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.Text:
                            tb.ChangeField<VarCharFieldDefinition>(templateField.FieldName).WithLength(templateField.FieldSize.Value);
                            break;
                        case TemplateFieldType.Multivalue:
                            // SE CUBRE CON LA TABLA DE OPCIONES
                            break;
                        case TemplateFieldType.Date:
                            tb.ChangeField<DateFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.Time:
                            tb.ChangeField<TimeFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.DateTime:
                            tb.ChangeField<DateTimeFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.Integer:
                            tb.ChangeField<IntFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.Decimal:
                            tb.ChangeField<DecimalFieldDefinition>(templateField.FieldName);
                            break;
                        case TemplateFieldType.GoogleAddress:
                            tb.ChangeField<VarCharFieldDefinition>(templateField.FieldName).WithLength(500);
                            break;
                        case TemplateFieldType.Template:
                            templateRelation = await GetTemplateAsync(templateField.TemplateFieldRelationTemplate.Value);
                            tb.ChangeField<BigIntFieldDefinition>(templateField.FieldName).AsForeignKey(templateRelation.TableName, "Id");
                            break;
                        case TemplateFieldType.User:
                            tb.ChangeField<BigIntFieldDefinition>(templateField.FieldName);
                            break;
                    }
                }

                await ChangeTemplateFieldStatusAsync(templateField.Id.Value, TemplateFieldStatus.Processed);
            }

            await GenerateTemplateQueries(dto);

            string script = sb.GenerateScripts();

            if (!script.IsNullOrWhiteSpace())
            {
                await _sqlExecuter.ExecuteSqlCommandAsync(script);
            }
        }

        public async Task<bool> IsTableGeneratedAsync(long template) {
            return (await _repository.CountAsync(p => p.Id == template && p.IsTableGenerated == true)) > 0;
        }

        public async Task MarkTemplateAsTableGeneratedAsync(long template)
        {
            var entity = await _repository.FirstOrDefaultAsync(template);

            entity.IsTableGenerated = true;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public string GetTemplateQuerySecurity(string tableName, long? user = null)
        {
            string query = ("INNER JOIN (" +
                    "SELECT Parent, Max([Level]) UserMaxSecutiryLevel, Max(IsExecutor) UserIsExecutor " +
                    "FROM (" +
                        "(SELECT Parent, 1 [Level], 0 IsExecutor FROM {{TABLENAME}}_UserReader WHERE [User] = {{CURRENTUSER}}) " +
                        "UNION (SELECT Parent, 2 [Level], IsExecutor FROM {{TABLENAME}}_UserEditor WHERE [User] = {{CURRENTUSER}}) " +
                        "UNION (SELECT {{TABLENAME}}_OUReader.Parent, 1 [Level], 0 IsExecutor FROM {{TABLENAME}}_OUReader " +
                            "INNER JOIN OUUsersSecurity ON {{TABLENAME}}_OUReader.OrgUnit = OUUsersSecurity.Id AND OUUsersSecurity.[User] = {{CURRENTUSER}}) " +
                        "UNION (SELECT {{TABLENAME}}_OUEditor.Parent, 2 [Level], IsExecutor FROM {{TABLENAME}}_OUEditor " +
                            "INNER JOIN OUUsersSecurity ON {{TABLENAME}}_OUEditor.OrgUnit = OUUsersSecurity.Id AND OUUsersSecurity.[User] = {{CURRENTUSER}})" +
                    ") UNIONS " +
                    "GROUP BY Parent" +
                ") SECMEMBER ON {{TABLENAME}}.Id = SECMEMBER.Parent").Replace("{{TABLENAME}}", tableName);

            if (user != null)
            {
                query = query.Replace("{{CURRENTUSER}}", user.Value.ToString());
            }

            return query;
        }

        #region Private Methods

        private IQueryable<TemplateDto> GetTemplateQuery()
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            var query = (from entity in _repository.GetAll()
                         select new TemplateDto
                         {
                             Id = entity.Id,
                             RGBColor = entity.RGBColor,
                             NameSingular = entity.NameSingular,
                             NamePlural = entity.NamePlural,
                             Description = entity.Description,
                             Icon = entity.Icon,
                             TableName = entity.TableName,
                             IsTableGenerated = entity.IsTableGenerated == true,
                             IsTableGeneratedDesc = entity.IsTableGenerated == true ? yesLabel : noLabel,
                             HasChatRoom = entity.HasChatRoom == true,
                             HasChatRoomDesc = entity.HasChatRoom == true ? yesLabel : noLabel,
                             IsActivity = entity.IsActivity == true,
                             IsActivityDesc = entity.IsActivity == true ? yesLabel : noLabel,
                             HasSecurity = entity.HasSecurity == true,
                             HasSecurityDesc = entity.HasSecurity == true ? yesLabel : noLabel,
                             IsActive = entity.IsActive == true,
                             IsActiveDesc = entity.IsActive == true ? yesLabel : noLabel
                         });

            return query;
        }

        private IQueryable<TemplateDto> GetTemplateListQuery(TemplateListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetTemplateQuery()
                .WhereIf(
                    filter != null,
                    p => p.NameSingular.ToUpper().Contains(filter)
                    || p.NamePlural.ToUpper().Contains(filter)
                    || p.Description.ToUpper().Contains(filter)
                    || p.TableName.ToUpper().Contains(filter)
                    || p.IsTableGeneratedDesc.ToUpper().Contains(filter)
                    || p.HasChatRoomDesc.ToUpper().Contains(filter)
                    || p.IsActiveDesc.ToUpper().Contains(filter)
                );

            return query;
        }

        private async Task ValidateTemplateAsync(TemplateDto dto)
        {
            List<TemplateDto> list;

            using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.SoftDelete))
            {
                list = await GetTemplateByTableNameListAsync(dto.TableName);
            }

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("Templates.Template.DuplicatedTableName"));
            }

        }

        private async Task<long> LogChangeTemplate(TemplateDto newDto, TemplateDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new TemplateDto(); }
            if (previousDto == null) { previousDto = new TemplateDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
				LogStringProperty(sb, previousDto.RGBColor, newDto.RGBColor, "{{Templates.Template.RGBColor}}");
				LogStringProperty(sb, previousDto.NameSingular, newDto.NameSingular, "{{Templates.Template.NameSingular}}");
				LogStringProperty(sb, previousDto.NamePlural, newDto.NamePlural, "{{Templates.Template.NamePlural}}");
				LogStringProperty(sb, previousDto.Description, newDto.Description, "{{Templates.Template.Description}}");
				LogStringProperty(sb, previousDto.TableName, newDto.TableName, "{{Templates.Template.TableName}}");
				LogStringProperty(sb, previousDto.RGBColor, newDto.RGBColor, "{{Templates.Template.RGBColor}}");
				LogStringProperty(sb, previousDto.RGBColor, newDto.RGBColor, "{{Templates.Template.RGBColor}}");
				LogStringProperty(sb, previousDto.RGBColor, newDto.RGBColor, "{{Templates.Template.RGBColor}}");
				LogStringProperty(sb, previousDto.RGBColor, newDto.RGBColor, "{{Templates.Template.RGBColor}}");
                LogBoolProperty(sb, previousDto.HasChatRoom, newDto.HasChatRoom, "{{Templates.Template.HasChatRoom}}");
                LogBoolProperty(sb, previousDto.IsActivity, newDto.IsActivity, "{{Templates.Template.IsActivity}}");
                LogBoolProperty(sb, previousDto.HasSecurity, newDto.HasSecurity, "{{Templates.Template.HasSecurity}}");
                LogBoolProperty(sb, previousDto.IsTableGenerated, newDto.IsTableGenerated, "{{IsActive}}");
				LogBoolProperty(sb, previousDto.IsActive, newDto.IsActive, "{{IsActive}}");
            }
            
            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "Template", sb.ToString());
        }

        private string CalculateTableName(string name) {
            int? tenantId = CurrentUnitOfWork.GetTenantId();

            return "UDT_" + (tenantId.HasValue ? tenantId.ToString() + "_" : "") + name.ToVariableName();
        }

        private static void AddDbTablesOthersAsNewToDefinition(ScriptBuilder.Structure.ScriptBuilder sb, TemplateDto dto)
        {
            TableDefinition tb = sb.AddTableDefinition(dto.TableName + "_OPT").AsNewTable();

            tb.AddField<BigIntFieldDefinition>("Id").AsIdentity(1, 1).AsPrimaryKey().AsNotNull();
            tb.AddField<BigIntFieldDefinition>("Parent").AsForeignKey(dto.TableName, "Id");
            tb.AddField<BigIntFieldDefinition>("TemplateField").AsForeignKey("TemplateField", "Id");
            tb.AddField<IntFieldDefinition>("Value");
            tb.AddField<VarCharFieldDefinition>("Description").WithLength(100);

            tb = sb.AddTableDefinition(dto.TableName + "_UserReader").AsNewTable();

            tb.AddField<BigIntFieldDefinition>("Id").AsIdentity(1, 1).AsPrimaryKey().AsNotNull();
            tb.AddField<BigIntFieldDefinition>("Parent").AsForeignKey(dto.TableName, "Id");
            tb.AddField<BigIntFieldDefinition>("User").AsForeignKey("User", "Id");

            tb = sb.AddTableDefinition(dto.TableName + "_UserEditor").AsNewTable();

            tb.AddField<BigIntFieldDefinition>("Id").AsIdentity(1, 1).AsPrimaryKey().AsNotNull();
            tb.AddField<BigIntFieldDefinition>("Parent").AsForeignKey(dto.TableName, "Id");
            tb.AddField<BigIntFieldDefinition>("User").AsForeignKey("User", "Id");
            tb.AddField<BitFieldDefinition>("IsExecutor");

            tb = sb.AddTableDefinition(dto.TableName + "_OUReader").AsNewTable();

            tb.AddField<BigIntFieldDefinition>("Id").AsIdentity(1, 1).AsPrimaryKey().AsNotNull();
            tb.AddField<BigIntFieldDefinition>("Parent").AsForeignKey(dto.TableName, "Id");
            tb.AddField<BigIntFieldDefinition>("OrgUnit").AsForeignKey("OrgUnit", "Id");

            tb = sb.AddTableDefinition(dto.TableName + "_OUEditor").AsNewTable();

            tb.AddField<BigIntFieldDefinition>("Id").AsIdentity(1, 1).AsPrimaryKey().AsNotNull();
            tb.AddField<BigIntFieldDefinition>("Parent").AsForeignKey(dto.TableName, "Id");
            tb.AddField<BigIntFieldDefinition>("OrgUnit").AsForeignKey("OrgUnit", "Id");
            tb.AddField<BitFieldDefinition>("IsExecutor");
        }

        private async Task GenerateTemplateQueries(TemplateDto templateDto) {
            int? tenantId = CurrentUnitOfWork.GetTenantId();
            List<TemplateFieldDto> templateFields = await GetTemplateFieldByTemplateListAsync(templateDto.Id.Value, true);
            List<TemplateFieldDto> templateFieldsForView = templateFields.FindAll(p => p.ShowOnGrid);
            string query = "SELECT * FROM (SELECT {0} FROM {1} WHERE {2}.TenantId {3}) WRAPPER";

            List<string> parameters = new List<string>();

            parameters.Add(GetTemplateQueryColumns(templateDto, templateFieldsForView));
            parameters.Add(GetTemplateQueryFrom(templateDto, templateFieldsForView));
            parameters.Add(templateDto.TableName);
            parameters.Add(tenantId.HasValue ? "= " + tenantId.Value : "IS NULL");

            await CreateOrUpdateTemplateQuery(templateDto, TemplateQueryType.View, string.Format(query, parameters.ToArray()));
            await CreateOrUpdateTemplateQuery(templateDto, TemplateQueryType.ViewFilters, GetTemplateQueryWhere(templateDto, templateFieldsForView));
            await CreateOrUpdateTemplateQuery(templateDto, TemplateQueryType.Insert, GetTemplateQueryInsert(templateDto, templateFields));
            await CreateOrUpdateTemplateQuery(templateDto, TemplateQueryType.Read, GetTemplateQueryRead(tenantId, templateDto, templateFields));
            await CreateOrUpdateTemplateQuery(templateDto, TemplateQueryType.Update, GetTemplateQueryUpdate(templateDto, templateFields));
        }

        private string GetTemplateQueryColumns(TemplateDto templateDto, List<TemplateFieldDto> templateFields, bool isRead = false)
        {
            List<string> columns = new List<string>();
            string fieldNameForActivityUserCreator;
            string userTable;

            if (templateDto.IsActivity)
            {
                columns.Add(string.Format("{0}.{1} {2}", activityTableName, "Id", GetTemplateFieldNameForActivity("Id")));

                if (!isRead)
                {
                    fieldNameForActivityUserCreator = GetTemplateFieldNameForActivity("UserCreator");
                    userTable = fieldNameForActivityUserCreator + "_User";

                    columns.Add(string.Format("{0}.{1} {2}", activityTableName, "CreationTime", GetTemplateFieldNameForActivity("CreationTime")));
                    columns.Add(string.Format("{0}.{1} {2}", activityTableName, "UserCreator", fieldNameForActivityUserCreator));
                    columns.Add(string.Format("{0} {1}",
                            "TRIM(TRIM(" + userTable + ".Name + ' ' + " + userTable + ".Lastname) + " + "' ' + " + userTable + ".SecondLastname)",
                            fieldNameForActivityUserCreator + "_DESC"));
                    columns.Add(string.Format("{0}.{1} {2}", activityTableName, "Description", GetTemplateFieldNameForActivity("Description")));
                    columns.Add(string.Format("{0}.{1} {2}", activityTableName, "Status", GetTemplateFieldNameForActivity("Status")));
                    columns.Add(string.Format("{0}.{1} {2}", activityStatusTableName, "Name", GetTemplateFieldNameForActivity("Status") + "_DESC"));
                    columns.Add(string.Format("{0}.{1} {2}", activityTableName, "InitialPlannedDate", GetTemplateFieldNameForActivity("InitialPlannedDate")));
                    columns.Add(string.Format("{0}.{1} {2}", activityTableName, "FinalPlannedDate", GetTemplateFieldNameForActivity("FinalPlannedDate")));
                    columns.Add(string.Format("{0}.{1} {2}", activityTableName, "InitialRealDate", GetTemplateFieldNameForActivity("InitialRealDate")));
                    columns.Add(string.Format("{0}.{1} {2}", activityTableName, "FinalRealDate", GetTemplateFieldNameForActivity("FinalRealDate")));
                    columns.Add(string.Format("{0}.{1} {2}", activityTableName, "IsOnTime", GetTemplateFieldNameForActivity("IsOnTime")));
                }
            }

            foreach (TemplateFieldDto templateField in templateFields)
            {
                switch (templateField.FieldType)
                {
                    case TemplateFieldType.Boolean:
                        columns.Add(string.Format("{0}.{1}", templateDto.TableName, templateField.FieldName));
                        columns.Add(string.Format("CASE {0}.{1} WHEN 1 THEN '{2}' ELSE '{3}' END AS {4}_DESC", templateDto.TableName,
                            templateField.FieldName, L("Yes"), L("No"), templateField.FieldName));
                        break;
                    case TemplateFieldType.Multivalue:
                        columns.Add(string.Format("(SELECT STRING_AGG(Value, ', ') FROM {0}_OPT WHERE Parent = {1}.Id AND TemplateField = {2}) AS {3}", templateDto.TableName,
                            templateDto.TableName, templateField.Id, templateField.FieldName));
                        columns.Add(string.Format("(SELECT STRING_AGG(Description, ', ') FROM {0}_OPT WHERE Parent = {1}.Id AND TemplateField = {2}) AS {3}_DESC", templateDto.TableName,
                            templateDto.TableName, templateField.Id, templateField.FieldName));
                        break;
                    case TemplateFieldType.Template:
                        columns.Add(string.Format("{0}.{1}", templateDto.TableName, templateField.FieldName));
                        columns.Add(string.Format("{0}.{1} {2}", templateField.TemplateFieldRelationTemplateTableName,
                            templateField.TemplateFieldRelationTemplateFieldName, templateField.FieldName + "_DESC"));
                        break;
                    case TemplateFieldType.User:
                        userTable = templateField.FieldName + "_User";
                        columns.Add(string.Format("{0}.{1}", templateDto.TableName, templateField.FieldName));
                        columns.Add(string.Format("{0} {1}",
                            "TRIM(TRIM(" + userTable + ".Name + ' ' + " + userTable + ".Lastname) + " + "' ' + " + userTable + ".SecondLastname)",
                            templateField.FieldName + "_DESC"
                        ));
                        break;
                    default:
                        if (templateField.MustHaveOptions)
                        {
                            columns.Add(string.Format("(SELECT TOP 1 Value FROM {0}_OPT WHERE Parent = {1}.Id AND TemplateField = {2} ORDER BY Id) AS {3}", templateDto.TableName,
                                templateDto.TableName, templateField.Id, templateField.FieldName));
                            columns.Add(string.Format("(SELECT TOP 1 Description FROM {0}_OPT WHERE Parent = {1}.Id AND TemplateField = {2} ORDER BY Id) AS {3}_DESC", templateDto.TableName,
                                templateDto.TableName, templateField.Id, templateField.FieldName));
                        }
                        else
                        {
                            columns.Add(string.Format("{0}.{1}", templateDto.TableName, templateField.FieldName));
                        }
                        break;
                }
            }

            if (templateDto.HasSecurity && !isRead)
            {
                columns.Add("SECMEMBER.UserMaxSecutiryLevel");
                columns.Add("SECMEMBER.UserIsExecutor");
            }

            return templateDto.TableName + ".Id, " + templateDto.TableName + ".TenantId"
            + (columns.Count > 0 ? ", " + string.Join(", ", columns) : string.Empty);
        }

        private string GetTemplateQueryFrom(TemplateDto templateDto, List<TemplateFieldDto> templateFields, bool isRead = false)
        {
            List<string> joins = new List<string>();
            string fieldNameForActivityUserCreator;
            string userTable;

            if (templateDto.IsActivity) {
                joins.Add(string.Format("LEFT JOIN {0} ON {1}.{2} = {3}.Id", activityTableName,
                    templateDto.TableName, "ToDoActivity", activityTableName));

                if (!isRead)
                {
                    fieldNameForActivityUserCreator = GetTemplateFieldNameForActivity("UserCreator");
                    userTable = fieldNameForActivityUserCreator + "_User";

                    joins.Add(string.Format("LEFT JOIN {0} ON {1}.{2} = {3}.Id", activityStatusTableName,
                        activityTableName, "Status", activityStatusTableName));
                    joins.Add(string.Format("LEFT JOIN [User] {0} ON {1}.{2} = {3}.Id", userTable, activityTableName, "UserCreator", userTable));
                }
            }

            foreach (TemplateFieldDto templateField in templateFields.Where(p => p.FieldType == TemplateFieldType.Template))
            {
                joins.Add(string.Format("LEFT JOIN {0} ON {1}.{2} = {3}.Id", templateField.TemplateFieldRelationTemplateTableName,
                    templateDto.TableName, templateField.FieldName, templateField.TemplateFieldRelationTemplateTableName));
            }

            foreach (TemplateFieldDto templateField in templateFields.Where(p => p.FieldType == TemplateFieldType.User))
            {
                userTable = templateField.FieldName + "_User";
                joins.Add(string.Format("LEFT JOIN [User] {0} ON {1}.{2} = {3}.Id", userTable, templateDto.TableName, templateField.FieldName, userTable));
            }

            if (templateDto.HasSecurity && !isRead) {
                joins.Add(GetTemplateQuerySecurity(templateDto.TableName));
            }

            return templateDto.TableName + (joins.Count > 0 ? " " + string.Join(" ", joins) : string.Empty);
        }

        private string GetTemplateQueryWhere(TemplateDto templateDto, List<TemplateFieldDto> templateFields)
        {
            List<string> conditions = new List<string>();

            if (templateDto.IsActivity) {
                conditions.Add(string.Format("{0} LIKE '%' + {1} + '%'", GetTemplateFieldNameForActivity("Description"), "'{{FILTER}}'"));
                conditions.Add(string.Format("{0} LIKE '%' + {1} + '%'", GetTemplateFieldNameForActivity("CreationTime"), "'{{FILTER}}'"));
                conditions.Add(string.Format("{0} LIKE '%' + {1} + '%'", GetTemplateFieldNameForActivity("FinalPlannedDate"), "'{{FILTER}}'"));
                conditions.Add(string.Format("{0} LIKE '%' + {1} + '%'", GetTemplateFieldNameForActivity("FinalRealDate"), "'{{FILTER}}'"));
                conditions.Add(string.Format("{0} LIKE '%' + {1} + '%'", GetTemplateFieldNameForActivity("Status"), "'{{FILTER}}'"));
                conditions.Add(string.Format("{0} LIKE '%' + {1} + '%'", GetTemplateFieldNameForActivity("Status") + "_DESC", "'{{FILTER}}'"));
            }

            foreach (TemplateFieldDto templateField in templateFields)
            {
                switch (templateField.FieldType)
                {
                    case TemplateFieldType.Boolean:
                    case TemplateFieldType.Template:
                    case TemplateFieldType.User:
                        conditions.Add(string.Format("{0} LIKE '%' + {1} + '%'", templateField.FieldName + "_DESC", "'{{FILTER}}'"));
                        break;
                    default:
                        if (templateField.MustHaveOptions)
                        {
                            conditions.Add(string.Format("{0} LIKE '%' + {1} + '%'", templateField.FieldName + "_DESC", "'{{FILTER}}'"));
                        } else 
                        {
                            conditions.Add(string.Format("{0} LIKE '%' + {1} + '%'", templateField.FieldName, "'{{FILTER}}'"));
                        }
                        break;
                }
            }

            return "WHERE (Id LIKE '%' + '{{FILTER}}' + '%'" + (conditions.Count > 0 ? " OR " + string.Join(" OR ", conditions) + ")" : string.Empty);
        }

        private string GetTemplateQueryInsert(TemplateDto templateDto, List<TemplateFieldDto> templateFields)
        {
            List<string> fieldNames = new List<string>();
            List<string> paramNames = new List<string>();

            foreach (TemplateFieldDto templateField in templateFields)
            {
                if (!templateField.MustHaveOptions)
                {
                    fieldNames.Add(templateField.FieldName);
                    paramNames.Add("@" + templateField.FieldName);
                }
            }

            string queryInsert;

            if (fieldNames.Count > 0)
            {
                queryInsert = string.Format("INSERT INTO {0} (TenantId, {1}) VALUES ({2}, {3})",
                    templateDto.TableName,
                    string.Join(", ", fieldNames),
                    CurrentUnitOfWork.GetTenantId() == null ? "NULL" : CurrentUnitOfWork.GetTenantId().ToString(),
                    string.Join(", ", paramNames));
            } else 
            {
                queryInsert = string.Format("INSERT INTO {0} (TenantId) VALUES ({1})",
                    templateDto.TableName,
                    CurrentUnitOfWork.GetTenantId() == null ? "NULL" : CurrentUnitOfWork.GetTenantId().ToString());
            }

            return queryInsert + "; SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS Id";
        }

        private string GetTemplateQueryRead(int? tenantId, TemplateDto templateDto, List<TemplateFieldDto> templateFields)
        {
            string query = "SELECT {0} FROM {1} WHERE {2}.TenantId {3} AND {4}.Id = @Id";

            List<string> parameters = new List<string>();

            parameters.Add(GetTemplateQueryColumns(templateDto, templateFields, true));
            parameters.Add(GetTemplateQueryFrom(templateDto, templateFields, true));
            parameters.Add(templateDto.TableName);
            parameters.Add(tenantId.HasValue ? "= " + tenantId.Value : "IS NULL");
            parameters.Add(templateDto.TableName);

            return string.Format(query, parameters.ToArray());
        }

        private string GetTemplateQueryUpdate(TemplateDto templateDto, List<TemplateFieldDto> templateFields)
        {
            List<string> pairsFielNameParams = new List<string>();

            foreach (TemplateFieldDto templateField in templateFields)
            {
                if (!templateField.MustHaveOptions)
                {
                    pairsFielNameParams.Add(string.Format("{0} = {1}", templateField.FieldName, "@" + templateField.FieldName));
                }
            }

            string queryInsert = string.Format("UPDATE {0} SET {1} WHERE TenantId {2} AND Id = @Id",
                templateDto.TableName,
                string.Join(", ", pairsFielNameParams),
                CurrentUnitOfWork.GetTenantId() == null ? "IS NULL" : "= " + CurrentUnitOfWork.GetTenantId().ToString());

            return queryInsert;
        }

        #endregion

        #endregion

        #region TEMPLATE SECTIONS

        public async Task<PagedResultDto<TemplateSectionDto>> GetTemplateSectionListAsync(TemplateSectionListFilterDto dto)
        {
            var query = GetTemplateSectionListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "Order" : dto.Sorting)
                .PageByIf(dto.IsPaged, dto)
                .ToListAsync();

            return new PagedResultDto<TemplateSectionDto>(count, ll);
        }

        public async Task<TemplateSectionDto> GetTemplateSectionAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetTemplateSectionQuery();
            TemplateSectionDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

			if (dto == null && throwExceptionIfNotFound)
			{
				throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("TemplateSections.TemplateSection"), id));
			}

            return dto;
        }

        public async Task<List<TemplateSectionDto>> GetTemplateSectionByTemplateListAsync(long template)
        {
            var query = GetTemplateSectionQuery();

            return await query.Where(p => p.Template == template).ToListAsync();
        }

        public async Task<List<TemplateSectionDto>> GetTemplateSectionByTemplateAndNameListAsync(long template, string name)
        {
            var query = GetTemplateSectionQuery();

            return await query.Where(p => p.Template == template && p.Name == name).ToListAsync();
        }

        public async Task<List<TemplateSectionDto>> GetTemplateSectionByTemplateAndOrderListAsync(long template, short order)
        {
            var query = GetTemplateSectionQuery();

            return await query.Where(p => p.Template == template && p.Order == order).ToListAsync();
        }

        public async Task<long> CreateTemplateSectionAsync(TemplateSectionDto dto)
        {
            await ValidateTemplateSectionAsync(dto);

            var entity = new TemplateSection();

            entity.Template = dto.Template;
            entity.Name = dto.Name;
            entity.Order = dto.Order;
            entity.IconAF = dto.IconAF;

            entity.Id = _repositoryTemplateSection.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeTemplateSection(await GetTemplateSectionAsync(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task UpdateTemplateSectionAsync(TemplateSectionDto dto)
        {
            await ValidateTemplateSectionAsync(dto);

            TemplateSectionDto previousDto = await GetTemplateSectionAsync(dto.Id.Value);

            var entity = await _repositoryTemplateSection.FirstOrDefaultAsync(dto.Id.Value);

            entity.Template = dto.Template;
            entity.Name = dto.Name;
            entity.Order = dto.Order;
            entity.IconAF = dto.IconAF;

            await CurrentUnitOfWork.SaveChangesAsync();

            await LogChangeTemplateSection(await GetTemplateSectionAsync(entity.Id), previousDto, ChangeLogType.Update);
        }

        public async Task DeleteTemplateSectionAsync(long id)
        {
            TemplateSectionDto previousDto = await GetTemplateSectionAsync(id);

            await DeleteTemplateFieldOptionByTemplateSectionAsync(id);
            await DeleteTemplateFieldByTemplateSectionAsync(id);
            await _repositoryTemplateSection.DeleteAsync(id);

            await LogChangeTemplateSection(null, previousDto, ChangeLogType.Delete);
        }

        public async Task DeleteTemplateSectionByTemplateAsync(long template)
        {
            await _repositoryTemplateSection.DeleteAsync(p => p.Template == template);
        }

        #region Private Methods

        private IQueryable<TemplateSectionDto> GetTemplateSectionQuery()
        {
            var query = (from entity in _repositoryTemplateSection.GetAll()
                         select new TemplateSectionDto
                         {
                             Id = entity.Id,
                             Template = entity.Template,
                             TemplateDesc = entity.TemplateNavigation.NameSingular,
                             Name = entity.Name,
                             Order = entity.Order,
                             IconAF = entity.IconAF
                         });

            return query;
        }

        private IQueryable<TemplateSectionDto> GetTemplateSectionListQuery(TemplateSectionListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetTemplateSectionQuery()
                .WhereIf(dto.Template != null,
                    p => p.Template == dto.Template
                ).WhereIf(
                    filter != null,
                    p => p.Name.Contains(filter)
                    || p.TemplateDesc.Contains(filter)
                    || p.Order.ToString().Contains(filter)
                );

            return query;
        }

        private async Task ValidateTemplateSectionAsync(TemplateSectionDto dto)
        {
            List<TemplateSectionDto> list = await GetTemplateSectionByTemplateAndNameListAsync(dto.Template.Value, dto.Name);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("TemplateSections.TemplateSection.DuplicatedName"));
            }

            list = await GetTemplateSectionByTemplateAndOrderListAsync(dto.Template.Value, dto.Order.Value);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("TemplateSections.TemplateSection.DuplicatedOrder"));
            }
        }

        private async Task<long> LogChangeTemplateSection(TemplateSectionDto newDto, TemplateSectionDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new TemplateSectionDto(); }
            if (previousDto == null) { previousDto = new TemplateSectionDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
				LogStringProperty(sb, previousDto.Name, newDto.Name, "{{TemplateSections.TemplateSection.Name}}");
				LogStringProperty(sb, previousDto.Order == null ? string.Empty : previousDto.Order.Value.ToString(), newDto.Order == null ? string.Empty : newDto.Order.Value.ToString(), "{{TemplateSections.TemplateSection.Order}}");
				LogStringProperty(sb, previousDto.IconAF, newDto.IconAF, "{{TemplateSections.TemplateSection.IconAF}}");
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "TemplateSection", sb.ToString());
        }

        #endregion

        #endregion

        #region TEMPLATE FIELDS

        public async Task<PagedResultDto<TemplateFieldDto>> GetTemplateFieldListAsync(TemplateFieldListFilterDto dto)
        {
            var query = GetTemplateFieldListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "TemplateSectionOrder, Order" : dto.Sorting)
                .PageByIf(dto.IsPaged, dto)
                .ToListAsync();

            return new PagedResultDto<TemplateFieldDto>(count, ll);
        }

        public async Task<List<ComboboxItemDto>> GetTemplateFieldComboAsync()
        {
            return await GetTemplateFieldComboAsync(new TemplateFieldComboFilterDto());
        }

        public async Task<List<ComboboxItemDto>> GetTemplateFieldComboAsync(TemplateFieldComboFilterDto dto)
        {
            var query = _repositoryTemplateField.GetAll()
                .WhereIf(dto.Template != null, p => p.TemplateSectionNavigation.Template == dto.Template)
                .OrderBy(p => p.Order)
                .Select(p => new ComboboxItemDto
                {
                    Value = p.Id.ToString(),
                    Label = p.Name
                });

            return await query.ToListAsync();
        }

        public async Task<TemplateFieldDto> GetTemplateFieldAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetTemplateFieldQuery(true);
            TemplateFieldDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (dto == null && throwExceptionIfNotFound)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("TemplateFields.TemplateField"), id));
            }

            return dto;
        }

        public async Task<List<TemplateFieldDto>> GetTemplateFieldByTemplateListAsync(long template, bool onlyProcessed = false)
        {
            var query = GetTemplateFieldQuery(true, onlyProcessed);

            return await query.Where(p => p.Template == template).OrderBy(p => p.Order).ToListAsync();
        }

        public async Task<List<TemplateFieldDto>> GetTemplateFieldDeletedByTemplateListAsync(long template)
        {
            List<TemplateFieldDto> list;

            using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.SoftDelete)) {
                var query = GetTemplateFieldQuery();

                list = await query.Where(p => p.Template == template && p.Status == TemplateFieldStatus.Deleted).OrderBy(p => p.Order).ToListAsync();
            }

            return list;
        }

        public async Task<List<TemplateFieldDto>> GetTemplateFieldByTemplateAndFieldNameListAsync(long template, string fieldName)
        {
            var query = GetTemplateFieldQuery();

            return await query.Where(p => p.Template == template && p.FieldName == fieldName).ToListAsync();
        }

        public async Task<List<TemplateFieldDto>> GetTemplateFieldByTemplateSectionAndOrderListAsync(long templateSection, short order)
        {
            var query = GetTemplateFieldQuery();

            return await query.Where(p => p.TemplateSection == templateSection && p.Order == order).ToListAsync();
        }

        public async Task<long> CreateTemplateFieldAsync(TemplateFieldDto dto)
        {
            dto.FieldName = dto.Name.ToVariableName();

            await ValidateTemplateFieldAsync(dto);

            var entity = new TemplateField();

            entity.TemplateSection = dto.TemplateSection;
            entity.Status = (byte?)TemplateFieldStatus.New;
            entity.Name = dto.Name;
            entity.FieldName = dto.FieldName;
            entity.FieldType = (short?)dto.FieldType;
            entity.FieldSize = dto.FieldSize;
            entity.FieldControl = (short?)dto.FieldControl;
            entity.InputMask = dto.InputMask;
            entity.HasKeyFilter = dto.HasKeyFilter;
            entity.KeyFilter = dto.HasKeyFilter? dto.KeyFilter: null;
            entity.IsRequired = dto.IsRequired;
            entity.ShowOnGrid = dto.ShowOnGrid;
            entity.Order = dto.Order;
            entity.InheritSecurity = dto.InheritSecurity;

            entity.Id = _repositoryTemplateField.InsertAndGetId(entity);

            if (dto.FieldType == TemplateFieldType.Template) {
                entity.TemplateFieldRelationTemplateFieldNavigation = new TemplateFieldRelation() 
                { TenantId = CurrentUnitOfWork.GetTenantId(), TemplateFieldRelation1 = dto.TemplateFieldRelationTemplateField.Value };
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            if (MustFieldControlHaveOptions(dto.FieldControl.Value))
            {
                CreateTemplateFieldOptions(entity.Id, dto.Options);
            }

            await LogChangeTemplateField(await GetTemplateFieldAsync(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task UpdateTemplateFieldAsync(TemplateFieldDto dto)
        {
            TemplateFieldDto previousDto = await GetTemplateFieldAsync(dto.Id.Value);

            if (previousDto.Status == TemplateFieldStatus.New)
            {
                dto.FieldName = dto.Name.ToVariableName();
            }

            await ValidateTemplateFieldAsync(dto);

            var entity = await _repositoryTemplateField.GetAllIncluding(p => p.TemplateFieldRelationTemplateFieldNavigation).FirstOrDefaultAsync(p => p.Id == dto.Id.Value);

            entity.TemplateSection = dto.TemplateSection;
            entity.Name = dto.Name;
            entity.FieldType = (short?)dto.FieldType;
            entity.FieldSize = dto.FieldSize;
            entity.FieldControl = (short?)dto.FieldControl;
            entity.InputMask = dto.InputMask;
            entity.HasKeyFilter = dto.HasKeyFilter;
            entity.KeyFilter = dto.HasKeyFilter ? dto.KeyFilter : null;
            entity.IsRequired = dto.IsRequired;
            entity.ShowOnGrid = dto.ShowOnGrid;
            entity.Order = dto.Order;
            entity.InheritSecurity = dto.InheritSecurity;

            if (previousDto.Status == TemplateFieldStatus.New) {
                entity.FieldName = dto.FieldName;
            } else if (previousDto.Status == TemplateFieldStatus.Processed && (previousDto.FieldType != dto.FieldType
                || previousDto.FieldSize != dto.FieldSize)) {
                entity.Status = (byte?)TemplateFieldStatus.Modified;
            }

            if (dto.FieldType == TemplateFieldType.Template)
            {
                if (entity.TemplateFieldRelationTemplateFieldNavigation == null)
                {
                    entity.TemplateFieldRelationTemplateFieldNavigation = new TemplateFieldRelation()
                    { TenantId = CurrentUnitOfWork.GetTenantId(), TemplateFieldRelation1 = dto.TemplateFieldRelationTemplateField.Value };
                } else {
                    entity.TemplateFieldRelationTemplateFieldNavigation.TemplateFieldRelation1 = dto.TemplateFieldRelationTemplateField.Value;
                }
            } else {
                if (entity.TemplateFieldRelationTemplateFieldNavigation != null)
                {
                    await _repositoryTemplateFieldRelation.DeleteAsync(entity.TemplateFieldRelationTemplateFieldNavigation);
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            await DeleteTemplateFieldOptionByTemplateFieldAsync(dto.Id.Value);

            if (MustFieldControlHaveOptions(dto.FieldControl.Value))
            {
                CreateTemplateFieldOptions(entity.Id, dto.Options);
            }

            await LogChangeTemplateField(await GetTemplateFieldAsync(entity.Id), previousDto, ChangeLogType.Update);
        }

        public async Task DeleteTemplateFieldAsync(long id)
        {
            if (await IsTemplateFieldRelationAsync(id)) {
                throw new AlgoriaCoreGeneralException(L("TemplateFields.TemplateField.IsRelation"));
            }

            TemplateFieldDto previousDto = await GetTemplateFieldAsync(id);
            var entity = await _repositoryTemplateField.FirstOrDefaultAsync(id);

            if (previousDto.Status == TemplateFieldStatus.Modified || previousDto.Status == TemplateFieldStatus.Processed)
            {
                entity.Status = (byte?)TemplateFieldStatus.Deleted;
            }

            await DeleteTemplateFieldOptionByTemplateSectionAsync(id);
            await _repositoryTemplateField.DeleteAsync(entity);

            await LogChangeTemplateField(null, previousDto, ChangeLogType.Delete);
        }

        public async Task DeleteTemplateFieldByTemplateSectionAsync(long templateSection)
        {
            await _repositoryTemplateField.DeleteAsync(p => p.TemplateSection == templateSection);
        }

        public async Task DeleteTemplateFieldByTemplateAsync(long template)
        {
            await _repositoryTemplateField.DeleteAsync(p => p.TemplateSectionNavigation.Template == template);
        }

        public async Task<bool> IsTemplateFieldRelationAsync(long templateField) {
            return (await _repositoryTemplateFieldRelation.CountAsync(p => p.TemplateFieldRelation1 == templateField)) > 0;
        }

        public async Task ChangeTemplateFieldStatusAsync(long templateField, TemplateFieldStatus newStatus)
        {
            using (CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.SoftDelete))
            {
                var entity = await _repositoryTemplateField.FirstOrDefaultAsync(templateField);

                entity.Status = (byte?)newStatus;

                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        public string GetTemplateFieldNameForActivity(string columnName)
        {
            return prefixActivityFieldName + "_" + columnName;
        }

        public async Task<short> GetTemplateFieldNextOrderByTemplateSection(long templateSection)
        {
            short? maxOrder = await _repositoryTemplateField.GetAll().Where(p => p.TemplateSection == templateSection).MaxAsync(p => p.Order);

            return (short) (maxOrder.HasValue ? maxOrder.Value + 1 : 1);
        }

        #region Private Methods

        private IQueryable<TemplateFieldDto> GetTemplateFieldQuery(bool withOptions = false, bool onlyProcessed = false)
		{
			string yesLabel = L("Yes");
			string noLabel = L("No");

            byte fieldStatusProcessed = (byte)TemplateFieldStatus.Processed;

            Dictionary<byte, string> dicFieldStatus = new Dictionary<byte, string>();

            dicFieldStatus.Add((byte)TemplateFieldStatus.New, L("TemplateFields.TemplateField.StatusNew"));
            dicFieldStatus.Add(fieldStatusProcessed, L("TemplateFields.TemplateField.StatusProcessed"));
            dicFieldStatus.Add((byte)TemplateFieldStatus.Modified, L("TemplateFields.TemplateField.StatusModified"));
            dicFieldStatus.Add((byte)TemplateFieldStatus.Deleted, L("TemplateFields.TemplateField.StatusDeleted"));

            Dictionary<short, string> dicFieldTypes = new Dictionary<short, string>();

            dicFieldTypes.Add((short)TemplateFieldType.Boolean, L("TemplateFields.TemplateField.TypeBoolean"));
            dicFieldTypes.Add((short)TemplateFieldType.Text, L("TemplateFields.TemplateField.TypeText"));
            dicFieldTypes.Add((short)TemplateFieldType.Multivalue, L("TemplateFields.TemplateField.TypeMultivalue"));
            dicFieldTypes.Add((short)TemplateFieldType.Date, L("TemplateFields.TemplateField.TypeDate"));
            dicFieldTypes.Add((short)TemplateFieldType.DateTime, L("TemplateFields.TemplateField.TypeDateTime"));
            dicFieldTypes.Add((short)TemplateFieldType.Time, L("TemplateFields.TemplateField.TypeTime"));
            dicFieldTypes.Add((short)TemplateFieldType.Integer, L("TemplateFields.TemplateField.TypeInteger"));
            dicFieldTypes.Add((short)TemplateFieldType.Decimal, L("TemplateFields.TemplateField.TypeDecimal"));
            dicFieldTypes.Add((short)TemplateFieldType.GoogleAddress, L("TemplateFields.TemplateField.TypeGoogleAddress"));
            dicFieldTypes.Add((short)TemplateFieldType.Template, L("TemplateFields.TemplateField.TypeTemplate"));
            dicFieldTypes.Add((short)TemplateFieldType.User, L("TemplateFields.TemplateField.TypeUser"));

            Dictionary<short, string> dicFieldControls = new Dictionary<short, string>();

            dicFieldControls.Add((short)TemplateFieldControl.InputText, L("TemplateFields.TemplateField.ControlInputText"));
            dicFieldControls.Add((short)TemplateFieldControl.DropDown, L("TemplateFields.TemplateField.ControlDropDown"));
            dicFieldControls.Add((short)TemplateFieldControl.Listbox, L("TemplateFields.TemplateField.ControlListbox"));
            dicFieldControls.Add((short)TemplateFieldControl.RadioButton, L("TemplateFields.TemplateField.ControlRadioButton"));
            dicFieldControls.Add((short)TemplateFieldControl.InputSwitch, L("TemplateFields.TemplateField.ControlInputSwitch"));
            dicFieldControls.Add((short)TemplateFieldControl.InputMask, L("TemplateFields.TemplateField.ControlInputMask"));
            dicFieldControls.Add((short)TemplateFieldControl.InputTextArea, L("TemplateFields.TemplateField.ControlInputTextArea"));
            dicFieldControls.Add((short)TemplateFieldControl.ListboxMultivalue, L("TemplateFields.TemplateField.ControlListboxMultivalue"));
            dicFieldControls.Add((short)TemplateFieldControl.Checkbox, L("TemplateFields.TemplateField.ControlCheckbox"));
            dicFieldControls.Add((short)TemplateFieldControl.Multiselect, L("TemplateFields.TemplateField.ControlMultiselect"));
            dicFieldControls.Add((short)TemplateFieldControl.CalendarBasic, L("TemplateFields.TemplateField.ControlCalendarBasic"));
            dicFieldControls.Add((short)TemplateFieldControl.CalendarTime, L("TemplateFields.TemplateField.ControlCalendarTime"));
            dicFieldControls.Add((short)TemplateFieldControl.CalendarTimeOnly, L("TemplateFields.TemplateField.ControlCalendarTimeOnly"));
            dicFieldControls.Add((short)TemplateFieldControl.Spinner, L("TemplateFields.TemplateField.ControlSpinner"));
            dicFieldControls.Add((short)TemplateFieldControl.SpinnerFormatInput, L("TemplateFields.TemplateField.ControlSpinnerFormatInput"));
            dicFieldControls.Add((short)TemplateFieldControl.TextNumber, L("TemplateFields.TemplateField.ControlTextNumber"));
            dicFieldControls.Add((short)TemplateFieldControl.GoogleAddress, L("TemplateFields.TemplateField.ControlGoogleAddress"));
            dicFieldControls.Add((short)TemplateFieldControl.Autocomplete, L("TemplateFields.TemplateField.ControlAutocomplete"));
            dicFieldControls.Add((short)TemplateFieldControl.AutocompleteDynamic, L("TemplateFields.TemplateField.ControlAutocompleteDynamic"));

            List<short> mustFieldsControlHaveOptions = new List<short>();

            mustFieldsControlHaveOptions.Add((short) TemplateFieldControl.Checkbox);
            mustFieldsControlHaveOptions.Add((short)TemplateFieldControl.DropDown);
            mustFieldsControlHaveOptions.Add((short)TemplateFieldControl.Listbox);
            mustFieldsControlHaveOptions.Add((short)TemplateFieldControl.ListboxMultivalue);
            mustFieldsControlHaveOptions.Add((short)TemplateFieldControl.Multiselect);
            mustFieldsControlHaveOptions.Add((short)TemplateFieldControl.RadioButton);

            var query = (from entity in _repositoryTemplateField.GetAll()
						 join TS in _repositoryTemplateSection.GetAll() on new { entity.TenantId, entity.TemplateSection } equals new { TS.TenantId, TemplateSection = (long?)TS.Id } into TSJoined
						 from TS in TSJoined.DefaultIfEmpty()
						 join T in _repository.GetAll() on new { TS.TenantId, TS.Template } equals new { T.TenantId, Template = (long?)T.Id } into TJoined
						 from T in TJoined.DefaultIfEmpty()
                         where !onlyProcessed || entity.Status == fieldStatusProcessed
                         select new TemplateFieldDto
						 {
							 Id = entity.Id,
							 TemplateSection = entity.TemplateSection,
							 TemplateSectionDesc = TS.Name,
							 TemplateSectionIconAF = TS.IconAF,
							 TemplateSectionOrder = TS.Order,
							 Template = TS.Template,
							 TemplateDesc = T.NameSingular,
                             TemplateTableName = T.TableName,
                             Name = entity.Name,
							 FieldName = entity.FieldName,
							 FieldType = (TemplateFieldType?)entity.FieldType,
							 FieldTypeDesc = entity.FieldType == null ? null : dicFieldTypes[entity.FieldType.Value],
                             FieldSize = entity.FieldSize,
							 FieldControl = (TemplateFieldControl?)entity.FieldControl,
							 FieldControlDesc = entity.FieldControl == null ? null : dicFieldControls[entity.FieldControl.Value],
							 InputMask = entity.InputMask,
							 HasKeyFilter = entity.HasKeyFilter == true,
							 HasKeyFilterDesc = entity.HasKeyFilter == true ? yesLabel : noLabel,
							 KeyFilter = entity.KeyFilter,
							 Status = (TemplateFieldStatus?)entity.Status,
							 StatusDesc = entity.Status == null ? null : dicFieldStatus[entity.Status.Value],
                             IsRequired = entity.IsRequired == true,
                             IsRequiredDesc = entity.IsRequired == true ? yesLabel : noLabel,
                             ShowOnGrid = entity.ShowOnGrid == true,
							 ShowOnGridDesc = entity.ShowOnGrid == true ? yesLabel : noLabel,
							 Order = entity.Order,
                             InheritSecurity = entity.InheritSecurity == true,
                             InheritSecurityDesc = entity.InheritSecurity == true ? yesLabel : noLabel,
                             TemplateFieldRelationTemplate = entity.TemplateFieldRelationTemplateFieldNavigation.TemplateFieldRelation1Navigation.TemplateSectionNavigation.Template,
                             TemplateFieldRelationTemplateDesc = entity.TemplateFieldRelationTemplateFieldNavigation.TemplateFieldRelation1Navigation.TemplateSectionNavigation.TemplateNavigation.NameSingular,
                             TemplateFieldRelationTemplateTableName = entity.TemplateFieldRelationTemplateFieldNavigation.TemplateFieldRelation1Navigation.TemplateSectionNavigation.TemplateNavigation.TableName,
                             TemplateFieldRelationTemplateField = entity.TemplateFieldRelationTemplateFieldNavigation.TemplateFieldRelation1Navigation.Id,
							 TemplateFieldRelationTemplateFieldDesc = entity.TemplateFieldRelationTemplateFieldNavigation.TemplateFieldRelation1Navigation.Name,
                             TemplateFieldRelationTemplateFieldName = entity.TemplateFieldRelationTemplateFieldNavigation.TemplateFieldRelation1Navigation.FieldName,
                             Options = withOptions ? entity.TemplateFieldOption.Select(p => new TemplateFieldOptionDto() {
                                Id = p.Id,
                                TemplateField = p.TemplateField,
                                Value = p.value,
                                Description = p.Description
                             }).ToList() : new List<TemplateFieldOptionDto>(),
                             MustHaveOptions = mustFieldsControlHaveOptions.Contains((short) entity.FieldControl)
                         });

			return query;
		}

        private IQueryable<TemplateFieldDto> GetTemplateFieldListQuery(TemplateFieldListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetTemplateFieldQuery(false, dto.OnlyProcessed)
                .WhereIf(dto.Template != null,
                    p => p.Template == dto.Template
                ).WhereIf(dto.TemplateSection != null,
                    p => p.TemplateSection == dto.TemplateSection
                ).WhereIf(
                    filter != null,
                    p => p.Name.Contains(filter)
                    || p.FieldName.Contains(filter)
                    || p.FieldTypeDesc.Contains(filter)
                    || p.FieldSize.ToString().Contains(filter)
                    || p.FieldControlDesc.Contains(filter)
                    || p.Order.ToString().Contains(filter)
                    || p.StatusDesc.Contains(filter)
                );

            return query;
        }

        private async Task ValidateTemplateFieldAsync(TemplateFieldDto dto)
        {
            TemplateSectionDto section = await GetTemplateSectionAsync(dto.TemplateSection.Value);
            TemplateDto templateDto = await GetTemplateAsync(section.Template.Value);

            List<TemplateFieldDto> list;

            if (!templateDto.IsTableGenerated)
            {
                list = await GetTemplateFieldByTemplateAndFieldNameListAsync(section.Template.Value, dto.FieldName);

                if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
                {
                    throw new EntityDuplicatedException(L("TemplateFields.TemplateField.DuplicatedFieldName"));
                }

                ValidateTemplateFieldForActivity(dto);
            }

            list = await GetTemplateFieldByTemplateSectionAndOrderListAsync(dto.TemplateSection.Value, dto.Order.Value);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("TemplateFields.TemplateField.DuplicatedOrder"));
            }
        }

        private void ValidateTemplateFieldForActivity(TemplateFieldDto dto) {
            List<string> list = _repositoryToDoActivity.GetColumnNames();
            list.Add("ToDoActivity");

            foreach (string columnName in list) {
                if (GetTemplateFieldNameForActivity(columnName).ToUpper() == dto.FieldName.ToUpper()) {
                    throw new EntityDuplicatedException(L("TemplateFields.TemplateField.ReservedActivityFieldName"));
                }
            }
        }

        private async Task<long> LogChangeTemplateField(TemplateFieldDto newDto, TemplateFieldDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new TemplateFieldDto(); }
            if (previousDto == null) { previousDto = new TemplateFieldDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
				LogStringProperty(sb, previousDto.TemplateSectionDesc, newDto.TemplateSectionDesc, "{{TemplateFields.TemplateField.TemplateSection}}");
				LogStringProperty(sb, previousDto.Name, newDto.Name, "{{TemplateFields.TemplateField.Name}}");
				LogStringProperty(sb, previousDto.FieldName, newDto.FieldName, "{{TemplateFields.TemplateField.FieldName}}");
				LogStringProperty(sb, previousDto.FieldTypeDesc, newDto.FieldTypeDesc, "{{TemplateFields.TemplateField.Type}}");
				LogStringProperty(sb, previousDto.FieldSize == null ? string.Empty : previousDto.FieldSize.Value.ToString(), newDto.FieldSize == null ? string.Empty : newDto.FieldSize.Value.ToString(), "{{TemplateFields.TemplateField.Size}}");
				LogStringProperty(sb, previousDto.FieldControlDesc, newDto.FieldControlDesc, "{{TemplateFields.TemplateField.Control}}");
				LogStringProperty(sb, previousDto.InputMask, newDto.InputMask, "{{TemplateFields.TemplateField.InputMask}}");
				LogBoolProperty(sb, previousDto.HasKeyFilter, newDto.HasKeyFilter, "{{TemplateFields.TemplateField.HasKeyFilter}}");
				LogStringProperty(sb, previousDto.KeyFilter, newDto.KeyFilter, "{{TemplateFields.TemplateField.KeyFilter}}");
				LogStringProperty(sb, previousDto.StatusDesc, newDto.StatusDesc, "{{TemplateFields.TemplateField.Status}}");
                LogBoolProperty(sb, previousDto.IsRequired, newDto.IsRequired, "{{TemplateFields.TemplateField.IsRequired}}");
                LogBoolProperty(sb, previousDto.ShowOnGrid, newDto.ShowOnGrid, "{{TemplateFields.TemplateField.ShowOnGrid}}");
				LogStringProperty(sb, previousDto.Order == null ? string.Empty : previousDto.Order.Value.ToString(), newDto.Order == null ? string.Empty : newDto.Order.Value.ToString(), "{{TemplateFields.TemplateField.Order}}");
                LogBoolProperty(sb, previousDto.InheritSecurity, newDto.InheritSecurity, "{{TemplateFields.TemplateField.InheritSecurity}}");
                LogStringProperty(sb, 
                    string.Join(", " , previousDto.Options.Select(p => string.Format("{0}({1})", p.Description, p.Value))),
                    string.Join(", ", newDto.Options.Select(p => string.Format("{0}({1})", p.Description, p.Value))),
                    "{{TemplateFields.TemplateField.Options}}"
                );
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "TemplateField", sb.ToString());
        }

        private bool MustFieldControlHaveOptions(TemplateFieldControl fieldControl)
        {
            return fieldControl == TemplateFieldControl.Checkbox
            || fieldControl == TemplateFieldControl.DropDown
            || fieldControl == TemplateFieldControl.Listbox
            || fieldControl == TemplateFieldControl.ListboxMultivalue
            || fieldControl == TemplateFieldControl.Multiselect
            || fieldControl == TemplateFieldControl.RadioButton;
        }

        #endregion

        #endregion

        #region TEMPLATE FIELD OPTIONS

        public long CreateTemplateFieldOption(TemplateFieldOptionDto dto)
        {
            var entity = new TemplateFieldOption();

            entity.TemplateField = dto.TemplateField;
            entity.value = dto.Value;
            entity.Description = dto.Description;

            return _repositoryTemplateFieldOption.InsertAndGetId(entity);
        }

        public void CreateTemplateFieldOptions(long templateField, List<TemplateFieldOptionDto> list)
        {
            foreach (TemplateFieldOptionDto optionDto in list)
            {
                optionDto.TemplateField = templateField;

                CreateTemplateFieldOption(optionDto);
            }
        }

        public async Task DeleteTemplateFieldOptionByTemplateFieldAsync(long templateField)
        {
            await _repositoryTemplateFieldOption.DeleteAsync(p => p.TemplateField == templateField);
        }

        public async Task DeleteTemplateFieldOptionByTemplateSectionAsync(long templateSection)
        {
            await _repositoryTemplateFieldOption.DeleteAsync(p => p.TemplateFieldNavigation.TemplateSection == templateSection);
        }

        public async Task DeleteTemplateFieldOptionByTemplateAsync(long template)
        {
            await _repositoryTemplateFieldOption.DeleteAsync(p => p.TemplateFieldNavigation.TemplateSectionNavigation.Template == template);
        }

        #region Private Methods

        private IQueryable<TemplateFieldOptionDto> GetTemplateFieldOptionQuery()
        {
            var query = (from entidad in _repositoryTemplateFieldOption.GetAll()
                         select new TemplateFieldOptionDto
                         {
                             Id = entidad.Id,
                             TemplateField = entidad.TemplateField,
                             Value = entidad.value,
                             Description = entidad.Description
                         }
                         );

            return query;
        }

        #endregion

        #endregion

        #region TEMPLATE QUERIES

        public async Task<TemplateQueryDto> GetTemplateQueryByTemplateAndTypeAsync(long template, TemplateQueryType type)
        {
            return await GetTemplateQueryQuery().FirstOrDefaultAsync(p => p.Template == template && p.QueryType == type);
        }

        private async Task<long> CreateTemplateQueryAsync(TemplateQueryDto dto)
        {
            var entity = new TemplateQuery();

            entity.Template = dto.Template;
            entity.QueryType = (byte?) dto.QueryType;
            entity.Query = dto.Query;

            entity.Id = _repositoryTemplateQuery.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateTemplateQueryAsync(TemplateQueryDto dto)
        {
            var entity = await _repositoryTemplateQuery.FirstOrDefaultAsync(dto.Id.Value);

            entity.Template = dto.Template;
            entity.QueryType = (byte?) dto.QueryType;
            entity.Query = dto.Query;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        private async Task CreateOrUpdateTemplateQuery(TemplateDto templateDto, TemplateQueryType templateQueryType, string query)
        {
            TemplateQueryDto templateQueryDto = await GetTemplateQueryByTemplateAndTypeAsync(templateDto.Id.Value, templateQueryType);

            if (templateQueryDto == null)
            {
                await CreateTemplateQueryAsync(new TemplateQueryDto()
                {
                    Template = templateDto.Id.Value,
                    QueryType = templateQueryType,
                    Query = query
                });
            }
            else
            {
                templateQueryDto.Query = query;
                await UpdateTemplateQueryAsync(templateQueryDto);
            }
        }

        #region Private Methods

        private IQueryable<TemplateQueryDto> GetTemplateQueryQuery()
        {
            var query = (from entity in _repositoryTemplateQuery.GetAll()
                         select new TemplateQueryDto
                         {
                             Id = entity.Id,
                             Template = entity.Template.Value,
                             QueryType = (TemplateQueryType) entity.QueryType.Value,
                             Query = entity.Query
                         });

            return query;
        }

        #endregion

        #endregion

        #region ACTIVITY STATUS

        public async Task<PagedResultDto<TemplateToDoStatusDto>> GetTemplateToDoStatusListAsync(TemplateToDoStatusListFilterDto dto)
        {
            var query = GetTemplateToDoStatusListQuery(dto);
            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "Type, Name" : dto.Sorting)
                .PageByIf(dto.IsPaged, dto)
                .ToListAsync();

            return new PagedResultDto<TemplateToDoStatusDto>(count, ll);
        }

        public async Task<List<ComboboxItemDto>> GetTemplateToDoStatusComboAsync(TemplateToDoStatusComboFilterDto dto)
        {
            var query = _repositoryTemplateToDoStatus.GetAll()
                .Where(p => p.Template == dto.Template)
                .WhereIf(dto.IsActive != null, p => p.IsActive == dto.IsActive)
                .OrderBy("Type, Name")
                .Select(p => new ComboboxItemDto
                {
                    Value = p.Id.ToString(),
                    Label = p.Name
                });

            return await query.ToListAsync();
        }

        public async Task<TemplateToDoStatusDto> GetTemplateToDoStatusAsync(long id, bool throwExceptionIfNotFound = true)
        {
            var query = GetTemplateToDoStatusQuery();
            TemplateToDoStatusDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (dto == null && throwExceptionIfNotFound)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("TemplateToDoStatus.TemplateToDoStatus"), id));
            }

            return dto;
        }

        public async Task<List<TemplateToDoStatusDto>> GetTemplateToDoStatusByTemplateAndNameListAsync(long template, string name)
        {
            var query = GetTemplateToDoStatusQuery();

            return await query.Where(p => p.Template == template && p.Name == name).ToListAsync();
        }

        public async Task<List<TemplateToDoStatusDto>> GetTemplateToDoStatusByTemplateAndTypeAndDefaultListAsync(long template, TemplateToDoStatusType type)
        {
            var query = GetTemplateToDoStatusQuery();

            return await query.Where(p => p.Template == template && p.Type == type && p.IsDefault).ToListAsync();
        }

        public async Task<long> CreateTemplateToDoStatusAsync(TemplateToDoStatusDto dto)
        {
            await ValidateTemplateToDoStatusAsync(dto);

            var entity = new TemplateToDoStatus();

            entity.Template = dto.Template;
            entity.Type = (byte?)dto.Type;
            entity.Name = dto.Name;
            entity.IsDefault = false;
            entity.IsActive = dto.IsActive;

            entity.Id = _repositoryTemplateToDoStatus.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            if (dto.IsDefault)
            {
                await MarkTemplateToDoStatusAsDefaultAsync(entity.Id, dto.Template, dto.Type);
            }

            await LogChangeTemplateToDoStatus(await GetTemplateToDoStatusAsync(entity.Id), null, ChangeLogType.Create);

            return entity.Id;
        }

        public async Task UpdateTemplateToDoStatusAsync(TemplateToDoStatusDto dto)
        {
            TemplateToDoStatusDto previousDto = await GetTemplateToDoStatusAsync(dto.Id.Value);

            await ValidateTemplateToDoStatusAsync(dto);

            var entity = await _repositoryTemplateToDoStatus.GetAll().FirstOrDefaultAsync(p => p.Id == dto.Id.Value);

            entity.Type = (byte?)dto.Type;
            entity.Name = dto.Name;
            entity.IsDefault = false;
            entity.IsActive = dto.IsActive;

            await CurrentUnitOfWork.SaveChangesAsync();

            if (dto.IsDefault)
            {
                await MarkTemplateToDoStatusAsDefaultAsync(entity.Id, previousDto.Template, dto.Type);
            }

            await LogChangeTemplateToDoStatus(await GetTemplateToDoStatusAsync(entity.Id), previousDto, ChangeLogType.Update);
        }

        public async Task DeleteTemplateToDoStatusAsync(long id)
        {
            TemplateToDoStatusDto previousDto = await GetTemplateToDoStatusAsync(id);

            await _repositoryTemplateToDoStatus.DeleteAsync(id);

            await LogChangeTemplateToDoStatus(null, previousDto, ChangeLogType.Delete);
        }

        #region Private Methods

        private IQueryable<TemplateToDoStatusDto> GetTemplateToDoStatusQuery()
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            byte toDoStatusTypePending = (byte)TemplateToDoStatusType.Pending;
            byte toDoStatusTypeInRevision = (byte)TemplateToDoStatusType.InRevision;
            byte toDoStatusTypeReturned = (byte)TemplateToDoStatusType.Returned;
            byte toDoStatusTypeRejected = (byte)TemplateToDoStatusType.Rejected;
            byte toDoStatusTypeClosed = (byte)TemplateToDoStatusType.Closed;
            byte toDoStatusTypeCanceled = (byte)TemplateToDoStatusType.Canceled;

            Dictionary<byte, string> dicToDoStatusTypeDesc = new Dictionary<byte, string>();

            dicToDoStatusTypeDesc.Add((byte)TemplateToDoStatusType.Pending, L("TemplateToDoStatus.TemplateToDoStatus.Type.Pending"));
            dicToDoStatusTypeDesc.Add((byte)TemplateToDoStatusType.InRevision, L("TemplateToDoStatus.TemplateToDoStatus.Type.InRevision"));
            dicToDoStatusTypeDesc.Add((byte)TemplateToDoStatusType.Returned, L("TemplateToDoStatus.TemplateToDoStatus.Type.Returned"));
            dicToDoStatusTypeDesc.Add((byte)TemplateToDoStatusType.Rejected, L("TemplateToDoStatus.TemplateToDoStatus.Type.Rejected"));
            dicToDoStatusTypeDesc.Add((byte)TemplateToDoStatusType.Closed, L("TemplateToDoStatus.TemplateToDoStatus.Type.Closed"));
            dicToDoStatusTypeDesc.Add((byte)TemplateToDoStatusType.Canceled, L("TemplateToDoStatus.TemplateToDoStatus.Type.Canceled"));

            var query = (from entity in _repositoryTemplateToDoStatus.GetAll()
                         select new TemplateToDoStatusDto
                         {
                             Id = entity.Id,
                             Template = entity.Template,
                             TemplateDesc = entity.TemplateNavigation.Description,
                             Type = (TemplateToDoStatusType) entity.Type,
                             TypeDesc = entity.Type == toDoStatusTypePending ? dicToDoStatusTypeDesc[toDoStatusTypePending] 
                             : entity.Type == toDoStatusTypeInRevision ? dicToDoStatusTypeDesc[toDoStatusTypeInRevision]
                             : entity.Type == toDoStatusTypeReturned ? dicToDoStatusTypeDesc[toDoStatusTypeReturned]
                             : entity.Type == toDoStatusTypeRejected ? dicToDoStatusTypeDesc[toDoStatusTypeRejected]
                             : entity.Type == toDoStatusTypeClosed ? dicToDoStatusTypeDesc[toDoStatusTypeClosed]
                             : entity.Type == toDoStatusTypeCanceled ? dicToDoStatusTypeDesc[toDoStatusTypeCanceled]
                             : null,
                             Name = entity.Name,
                             IsDefault = entity.IsDefault == true,
                             IsDefaultDesc = entity.IsDefault == true ? yesLabel : noLabel,
                             IsActive = entity.IsActive == true,
                             IsActiveDesc = entity.IsActive == true ? yesLabel : noLabel
                         });

            return query;
        }

        private IQueryable<TemplateToDoStatusDto> GetTemplateToDoStatusListQuery(TemplateToDoStatusListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            var query = GetTemplateToDoStatusQuery()
                .WhereIf(dto.Template != null,
                    p => p.Template == dto.Template
                ).WhereIf(
                    filter != null,
                    p => p.Name.Contains(filter)
                    || p.TypeDesc.Contains(filter)
                    || p.IsDefaultDesc.Contains(filter)
                    || p.IsActiveDesc.ToString().Contains(filter)
                );

            return query;
        }

        private async Task ValidateTemplateToDoStatusAsync(TemplateToDoStatusDto dto)
        {
            List<TemplateToDoStatusDto> list;

            list = await GetTemplateToDoStatusByTemplateAndNameListAsync(dto.Template, dto.Name);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("TemplateToDoStatus.TemplateToDoStatus.DuplicatedName"));
            }
        }

        public async Task MarkTemplateToDoStatusAsDefaultAsync(long id, long template, TemplateToDoStatusType type)
        {
            List<TemplateToDoStatus> list = await _repositoryTemplateToDoStatus.GetAllListAsync(p => p.Template == template && p.Type == (byte)type && p.IsDefault == true);
           
            foreach(TemplateToDoStatus item in list) {
                item.IsDefault = false;

                await _repositoryTemplateToDoStatus.UpdateAsync(item);
            }

            TemplateToDoStatus entity = await _repositoryTemplateToDoStatus.GetAsync(id);

            entity.IsDefault = true;

            await _repositoryTemplateToDoStatus.UpdateAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        private async Task<long> LogChangeTemplateToDoStatus(TemplateToDoStatusDto newDto, TemplateToDoStatusDto previousDto, ChangeLogType changeLogType)
        {
            if (newDto == null) { newDto = new TemplateToDoStatusDto(); }
            if (previousDto == null) { previousDto = new TemplateToDoStatusDto(); }

            StringBuilder sb = new StringBuilder("");

            if (changeLogType == ChangeLogType.Create || changeLogType == ChangeLogType.Update)
            {
                LogStringProperty(sb, previousDto.TypeDesc, newDto.TypeDesc, "{{TemplateToDoStatus.TemplateToDoStatus.Type}}");
                LogStringProperty(sb, previousDto.Name, newDto.Name, "{{TemplateToDoStatus.TemplateToDoStatus.Name}}");
                LogBoolProperty(sb, previousDto.IsDefault, newDto.IsDefault, "{{TemplateToDoStatus.TemplateToDoStatus.IsDefault}}");
                LogBoolProperty(sb, previousDto.IsActive, newDto.IsActive, "{{IsActive}}");
            }

            return await LogChange(changeLogType, (newDto.Id ?? previousDto.Id).ToString(), "TemplateToDoStatus", sb.ToString());
        }

        #endregion

        #endregion

        #region SECURITY

        public async Task<PagedResultDto<TemplateSecurityMemberDto>> GetTemplateSecurityMemberListAsync(TemplateSecurityMemberListFilterDto dto)
        {
            string query = GetTemplateSecurityMemberListQueryString(dto);
            int count = await _sqlExecuter.ExecuteSqlQueryScalar<int>(string.Format("SELECT COUNT(*) FROM ({0}) WRAPPERCOUNT", query));
            Dictionary<string, object> aParameters = new Dictionary<string, object>();

            aParameters.Add("@strSelectTabla", query);
            aParameters.Add("@numeropagina", dto.PageNumber > 0 ? dto.PageNumber - 1 : 0);
            aParameters.Add("@porpagina", dto.PageSize);
            aParameters.Add("@orderby", dto.Sorting);

            List<TemplateSecurityMemberDto> list = await _sqlExecuter.SqlStoredProcedure<TemplateSecurityMemberDto>("spr_obtienedatosporseccion", aParameters);

            return new PagedResultDto<TemplateSecurityMemberDto>(count, list);
        }

        public async Task<List<TemplateSecurityMemberDto>> GetTemplateSecurityMemberListAsync(long template, SecurityMemberType? type = null, SecurityMemberLevel? level = null)
        {
            return await _sqlExecuter.SqlQuery<TemplateSecurityMemberDto>(GetTemplateSecurityMemberQueryString(template, type, level));
        }

        public async Task<TemplateSecurityMemberDto> GetTemplateSecurityMemberAsync(
            long id, 
            SecurityMemberType type,
            SecurityMemberLevel level,
            bool throwExceptionIfNotFound = true
        )
        {
            var query = GetTemplateSecurityMemberQuery(type, level);
            TemplateSecurityMemberDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (dto == null && throwExceptionIfNotFound)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("TemplateSecurityMember.TemplateSecurityMember"), id));
            }

            return dto;
        }

        public async Task<List<TemplateSecurityMemberDto>> GetTemplateSecurityMemberListAsync(
            long template, 
            long member,
            SecurityMemberType type,
            SecurityMemberLevel level
        )
        {
            var query = GetTemplateSecurityMemberQuery(type, level);

            return await query.Where(p => p.Template == template && p.Member == member).ToListAsync();
        }

        public async Task<long> CreateTemplateSecurityMemberAsync(TemplateSecurityMemberDto dto)
        {
            await ValidateTemplateSecurityMemberAsync(dto);

            if (dto.Type == SecurityMemberType.User)
            {
                if (dto.Level == SecurityMemberLevel.Reader)
                {
                    return await CreateTemplateDefaultUserReaderAsync(dto);
                }
                else if (dto.Level == SecurityMemberLevel.Editor)
                {
                    return await CreateTemplateDefaultUserEditorAsync(dto);
                }
            } else if (dto.Type == SecurityMemberType.OrgUnit)
            {
                if (dto.Level == SecurityMemberLevel.Reader)
                {
                    return await CreateTemplateDefaultOUReaderAsync(dto);
                }
                else if (dto.Level == SecurityMemberLevel.Editor)
                {
                    return await CreateTemplateDefaultOUEditorAsync(dto);
                }
            }

            throw new EntityNotFoundException(L("TemplateSecurity.MissingData"));
        }

        public async Task DeleteTemplateSecurityMemberAsync(long id, SecurityMemberType type, SecurityMemberLevel level)
        {
            if (type == SecurityMemberType.User)
            {
                if (level == SecurityMemberLevel.Reader)
                {
                    await _repositoryTemplateDefaultUserReader.DeleteAsync(id);
                }
                else if (level == SecurityMemberLevel.Editor)
                {
                    await _repositoryTemplateDefaultUserEditor.DeleteAsync(id);
                }
            }
            else if (type == SecurityMemberType.OrgUnit)
            {
                if (level == SecurityMemberLevel.Reader)
                {
                    await _repositoryTemplateDefaultOUReader.DeleteAsync(id);
                }
                else if (level == SecurityMemberLevel.Editor)
                {
                    await _repositoryTemplateDefaultOUEditor.DeleteAsync(id);
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #region Private Methods

        private IQueryable<TemplateSecurityMemberDto> GetTemplateSecurityMemberQuery(SecurityMemberType type, SecurityMemberLevel level)
        {
            IQueryable<TemplateSecurityMemberDto> query = null;
            string yesLabel = L("Yes");
            string noLabel = L("No");

            string typeUserDesc = L("TemplateSecurity.Type.User");
            string typeOrgUnitDesc = L("TemplateSecurity.Type.OrgUnit");

            string levelReaderDesc = L("TemplateSecurity.Level.Reader");
            string levelEditorDesc = L("TemplateSecurity.Level.Editor");

            if (type == SecurityMemberType.User)
            {
                if (level == SecurityMemberLevel.Reader)
                {
                    query = (
                            from entity in _repositoryTemplateDefaultUserReader.GetAll()
                            select new TemplateSecurityMemberDto()
                            {
                                Id = entity.Id,
                                Template = entity.Template,
                                Type = SecurityMemberType.User,
                                TypeDesc = typeUserDesc,
                                Member = entity.User,
                                MemberDesc = ((entity.UserNavigation.Name + " " + entity.UserNavigation.Lastname).Trim() + " " + entity.UserNavigation.SecondLastname).Trim(),
                                Level = SecurityMemberLevel.Reader,
                                LevelDesc = levelReaderDesc,
                                IsExecutor = false,
                                IsExecutorDesc = noLabel
                            }
                        );
                }
                else if (level == SecurityMemberLevel.Editor)
                {
                    query = (
                            from entity in _repositoryTemplateDefaultUserEditor.GetAll()
                            select new TemplateSecurityMemberDto()
                            {
                                Id = entity.Id,
                                Template = entity.Template,
                                Type = SecurityMemberType.User,
                                TypeDesc = typeUserDesc,
                                Member = entity.User,
                                MemberDesc = ((entity.UserNavigation.Name + " " + entity.UserNavigation.Lastname).Trim() + " " + entity.UserNavigation.SecondLastname).Trim(),
                                Level = SecurityMemberLevel.Editor,
                                LevelDesc = levelEditorDesc,
                                IsExecutor = entity.IsExecutor,
                                IsExecutorDesc = entity.IsExecutor ? yesLabel : noLabel
                            }
                        );
                }
            }
            else if (type == SecurityMemberType.OrgUnit)
            {
                if (level == SecurityMemberLevel.Reader)
                {
                    query = (
                            from entity in _repositoryTemplateDefaultOUReader.GetAll()
                            select new TemplateSecurityMemberDto()
                            {
                                Id = entity.Id,
                                Template = entity.Template,
                                Type = SecurityMemberType.OrgUnit,
                                TypeDesc = typeOrgUnitDesc,
                                Member = entity.OrgUnit,
                                MemberDesc = entity.OrgUnitNavigation.Name,
                                Level = SecurityMemberLevel.Reader,
                                LevelDesc = levelReaderDesc,
                                IsExecutor = false,
                                IsExecutorDesc = noLabel
                            }
                        );
                }
                else if (level == SecurityMemberLevel.Editor)
                {
                    query = (
                            from entity in _repositoryTemplateDefaultOUEditor.GetAll()
                            select new TemplateSecurityMemberDto()
                            {
                                Id = entity.Id,
                                Template = entity.Template,
                                Type = SecurityMemberType.OrgUnit,
                                TypeDesc = typeOrgUnitDesc,
                                Member = entity.OrgUnit,
                                MemberDesc = entity.OrgUnitNavigation.Name,
                                Level = SecurityMemberLevel.Editor,
                                LevelDesc = levelEditorDesc,
                                IsExecutor = entity.IsExecutor,
                                IsExecutorDesc = entity.IsExecutor ? yesLabel : noLabel
                            }
                        );
                }
            }

            return query;
        }

        private string GetTemplateSecurityMemberQueryString(long template, SecurityMemberType? type = null, SecurityMemberLevel? level = null)
        {
            string query = string.Empty;

            string yesLabel = L("Yes");
            string noLabel = L("No");

            string typeUserDesc = L("TemplateSecurity.Type.User");
            string typeOrgUnitDesc = L("TemplateSecurity.Type.OrgUnit");

            string levelReaderDesc = L("TemplateSecurity.Level.Reader");
            string levelEditorDesc = L("TemplateSecurity.Level.Editor");

            string queryUserReader = string.Format("SELECT TD.Id, TD.Template, 1 Type, '{0}' TypeDesc, TD.[User] Member" +
                ", TRIM(TRIM(U.Name + ' ' + U.Lastname) + ' ' + U.SecondLastname) MemberDesc" +
                ", 1 Level, '{1}' LevelDesc, 0 IsExecutor, '{2}' IsExecutorDesc" +
                " FROM TemplateDefaultUserReader TD LEFT JOIN [User] U ON TD.[User] = U.Id" +
                " WHERE TD.Template = {3}",
                typeUserDesc, levelReaderDesc, noLabel, template);

            string queryOrgUnitReader = string.Format("SELECT TD.Id, TD.Template, 2 Type, '{0}' TypeDesc, TD.OrgUnit Member" +
                ", OU.Name MemberDesc, 1 Level, '{1}' LevelDesc, 0 IsExecutor, '{2}' IsExecutorDesc" +
                " FROM TemplateDefaultOUReader TD LEFT JOIN OrgUnit OU ON TD.OrgUnit = OU.Id" +
                " WHERE TD.Template = {3} AND (OU.IsDeleted IS NULL OR OU.IsDeleted = 0)",
                typeOrgUnitDesc, levelReaderDesc, noLabel, template);

            string queryUserEditor = string.Format("SELECT TD.Id, TD.Template, 1 Type, '{0}' TypeDesc, TD.[User] Member" +
                ", TRIM(TRIM(U.Name + ' ' + U.Lastname) + ' ' + U.SecondLastname) MemberDesc" +
                ", 2 Level, '{1}' LevelDesc, TD.IsExecutor, CASE TD.IsExecutor WHEN 1 THEN '{2}' ELSE '{3}' END IsExecutorDesc" +
                " FROM TemplateDefaultUserEditor TD LEFT JOIN [User] U ON TD.[User] = U.Id" +
                " WHERE TD.Template = {4}",
                typeUserDesc, levelEditorDesc, yesLabel, noLabel, template);

            string queryOrgUnitEditor = string.Format("SELECT TD.Id, TD.Template, 2 Type, '{0}' TypeDesc, TD.OrgUnit Member" +
                ", OU.Name MemberDesc, 2 Level, '{1}' LevelDesc, TD.IsExecutor, CASE TD.IsExecutor WHEN 1 THEN '{2}' ELSE '{3}' END IsExecutorDesc" +
                " FROM TemplateDefaultOUEditor TD LEFT JOIN OrgUnit OU ON TD.OrgUnit = OU.Id" +
                " WHERE TD.Template = {4} AND (OU.IsDeleted IS NULL OR OU.IsDeleted = 0)",
                typeOrgUnitDesc, levelEditorDesc, yesLabel, noLabel, template);

            if (type == null && level == null) {
                query = string.Format("({0}) UNION ({1}) UNION ({2}) UNION ({3})", queryUserReader, queryOrgUnitReader, queryUserEditor, queryOrgUnitEditor);
            } else {
                if (type == SecurityMemberType.User)
                {
                    if (level == SecurityMemberLevel.Reader)
                    {
                        query = queryUserReader;
                    }
                    else if (level == SecurityMemberLevel.Editor)
                    {
                        query = queryUserEditor;
                    }
                }
                else if (type == SecurityMemberType.OrgUnit)
                {
                    if (level == SecurityMemberLevel.Reader)
                    {
                        query = queryOrgUnitReader;
                    }
                    else if (level == SecurityMemberLevel.Editor)
                    {
                        query = queryOrgUnitEditor;
                    }
                }
            }

            return query;
        }

        private string GetTemplateSecurityMemberListQueryString(TemplateSecurityMemberListFilterDto dto)
        {
            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();
            string query = GetTemplateSecurityMemberQueryString(dto.Template);
            List<string> queryWhereConditions = new List<string>();

            if (dto.Type.HasValue) {
                queryWhereConditions.Add(string.Format("Type = {0}", (byte)dto.Type));
            }

            if (dto.Level.HasValue)
            {
                queryWhereConditions.Add(string.Format("Level = {0}", (byte)dto.Level));
            }

            if (filter.IsNullOrWhiteSpace()) {
                if (queryWhereConditions.Count > 0)
                {
                    query = string.Format("SELECT * FROM ({0}) WRAPPER WHERE {1}", query, string.Join(" AND ", queryWhereConditions));
                }
            } else {
                query = string.Format(@"SELECT * FROM ({0}) WRAPPER WHERE (Id LIKE '%[FILTER]%' OR TypeDesc LIKE '%[FILTER]%'" +
                " OR MemberDesc LIKE '%[FILTER]%' OR LevelDesc LIKE '%[FILTER]%' OR IsExecutorDesc LIKE '%[FILTER]%')", query)
                .Replace("[FILTER]", filter);

                if (queryWhereConditions.Count > 0)
                {
                    query += string.Format(" AND {0}", string.Join(" AND ", queryWhereConditions));
                }
            }

            return query;
        }

        private async Task ValidateTemplateSecurityMemberAsync(TemplateSecurityMemberDto dto)
        {
            List<TemplateSecurityMemberDto> list;

            list = await GetTemplateSecurityMemberListAsync(dto.Template, dto.Member, dto.Type, dto.Level);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("TemplateSecurity.DuplicatedName"));
            }
        }

        public async Task<long> CreateTemplateDefaultUserReaderAsync(TemplateSecurityMemberDto dto)
        {
            var entity = new TemplateDefaultUserReader();
            
            entity.Template = dto.Template;
            entity.User = dto.Member;

            entity.Id = _repositoryTemplateDefaultUserReader.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<long> CreateTemplateDefaultOUReaderAsync(TemplateSecurityMemberDto dto)
        {
            var entity = new TemplateDefaultOUReader();

            entity.Template = dto.Template;
            entity.OrgUnit = dto.Member;

            entity.Id = _repositoryTemplateDefaultOUReader.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<long> CreateTemplateDefaultUserEditorAsync(TemplateSecurityMemberDto dto)
        {
            var entity = new TemplateDefaultUserEditor();

            entity.Template = dto.Template;
            entity.User = dto.Member;
            entity.IsExecutor = dto.IsExecutor;

            entity.Id = _repositoryTemplateDefaultUserEditor.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<long> CreateTemplateDefaultOUEditorAsync(TemplateSecurityMemberDto dto)
        {
            var entity = new TemplateDefaultOUEditor();

            entity.Template = dto.Template;
            entity.OrgUnit = dto.Member;
            entity.IsExecutor = dto.IsExecutor;

            entity.Id = _repositoryTemplateDefaultOUEditor.InsertAndGetId(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            return entity.Id;
        }

        #endregion

        #endregion
    }
}
