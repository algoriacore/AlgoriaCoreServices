using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.Managers.Base;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using AlgoriaCore.Application.Managers.Questionnaires.Dto;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Entities.MongoDb;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Pagination;
using AlgoriaPersistence.Interfaces.Interfaces;
using Autofac;
using Autofac.Core.Lifetime;
using FluentValidation.Results;
using lizzie;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Questionnaires
{
    public class QuestionnaireManager : BaseManager
    {
        private readonly IRepositoryMongoDb<Questionnaire> _repository;
        private readonly IRepository<User, long> _repositoryUser;

        private readonly ILifetimeScope _lifetimeScope;

        #region Questionnaire

        public QuestionnaireManager(
            IRepositoryMongoDb<Questionnaire> repository,
            IRepository<User, long> repositoryUser,
            ILifetimeScope lifetimeScope)
        {
            _repository = repository;
            _repositoryUser = repositoryUser;

            _lifetimeScope = lifetimeScope;
        }

        public async Task<PagedResultDto<QuestionnaireDto>> GetQuestionnaireListAsync(QuestionnaireListFilterDto dto)
        {
            var query = GetQuestionnaireListQuery(dto);

            var count = await query.CountAsync();
            var ll = await query
                .OrderBy(dto.Sorting.IsNullOrEmpty() ? "Name" : dto.Sorting)
                .PageBy(dto)
                .ToListAsync();

            return new PagedResultDto<QuestionnaireDto>(count, ll);
        }

        public async Task<List<ComboboxItemDto>> GetQuestionnaireComboAsync()
        {
            return await GetQuestionnaireComboAsync(new QuestionnaireComboFilterDto() { IsActive = true });
        }

        public async Task<List<ComboboxItemDto>> GetQuestionnaireComboAsync(QuestionnaireComboFilterDto dto)
        {
            var query = _repository.GetAll()
                .WhereIf(dto.IsActive != null, p => p.IsActive == dto.IsActive)
                .WhereIf(!dto.Filter.IsNullOrWhiteSpace(), p => p.Name.Contains(dto.Filter))
                .OrderBy(p => p.Name)
                .Select(p => new ComboboxItemDto
                {
                    Value = p.Id.ToString(),
                    Label = p.Name
                });

            return await query.ToListAsync();
        }

        public async Task<QuestionnaireDto> GetQuestionnaireAsync(string id, bool throwExceptionIfNotFound = true)
        {
            var query = GetQuestionnaireQuery();

            QuestionnaireDto dto = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (throwExceptionIfNotFound && dto == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Questionnaires.Questionnaire"), id));
            }

            return dto;
        }

        public async Task<List<QuestionnaireDto>> GetQuestionnaireByNameListAsync(string name)
        {
            var query = GetQuestionnaireQuery();

            return await query.Where(p => p.Name.ToUpper() == name.ToUpper()).ToListAsync();
        }

        public async Task<string> CreateQuestionnaireAsync(QuestionnaireDto dto)
        {
            await ValidateQuestionnaireAsync(dto);

            var entity = new Questionnaire();

            entity.Name = dto.Name;
            entity.CustomCode = dto.CustomCode;
            entity.CreationDateTime = DateTime.UtcNow;
            entity.UserCreator = _repositoryUser.GetAll().Where(p => p.Id == SessionContext.UserId)
                .Select(p => (p.Name + " " + p.Lastname + " " + p.SecondLastname).Trim()).First();
            entity.IsActive = dto.IsActive;

            AssignQuestionnaireSection(entity, dto);

            entity.Id = await _repository.InsertAndGetIdAsync(entity);

            return entity.Id;
        }

        public async Task UpdateQuestionnaireAsync(QuestionnaireDto dto)
        {
            await ValidateQuestionnaireAsync(dto);

            var entity = await _repository.FirstOrDefaultAsync(dto.Id);

            if (entity == null)
            {
                throw new EntityNotFoundException(string.Format(L("EntityNotFoundExceptionMessage"), L("Questionnaires.Questionnaire"), dto.Id));
            }

            entity.Name = dto.Name;
            entity.CustomCode = dto.CustomCode;
            entity.IsActive = dto.IsActive;

            AssignQuestionnaireSection(entity, dto);

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteQuestionnaireAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task ValidateBsonDocument(BsonDocument dto, string questionnaire, string customCode)
        {
            QuestionnaireDto questionnaireDto = await GetQuestionnaireAsync(questionnaire);

            ValidateBsonDocument(dto, questionnaireDto.Sections.SelectMany(p => p.Fields).ToList(), customCode);
        }

        public void ValidateBsonDocument(BsonDocument dto, List<QuestionnaireFieldDto> fields, string customCode)
        {
            List<ValidationFailure> failures = new List<ValidationFailure>();
            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelInvalidFormat = L("FieldInvalidFormat");
            string labelMinimumValue = L("FieldMinimumValue");
            string labelMaximumValue = L("FieldMaximumValue");
            string fieldData;

            foreach (QuestionnaireFieldDto field in fields)
            {
                fieldData = dto[field.FieldName].RawValue == null ? null : dto[field.FieldName].RawValue.ToString();

                if (field.IsRequired && fieldData.IsNullOrWhiteSpace())
                {
                    failures.Add(new ValidationFailure(field.FieldName, string.Format(labelRequiredField, field.Name)));
                }
                else if (!field.MustHaveOptions && field.FieldType == QuestionnaireFieldType.Text && fieldData != null)
                {
                    if (fieldData.Length > field.FieldSize)
                    {
                        failures.Add(new ValidationFailure(field.FieldName, string.Format(labelMaxLength, field.Name, field.FieldSize)));
                    }

                    if (field.HasKeyFilter && !field.KeyFilter.IsNullOrWhiteSpace()
                        && (field.FieldControl == QuestionnaireFieldControl.InputText || field.FieldControl == QuestionnaireFieldControl.InputMask)
                        && !(new Regex(field.KeyFilter).IsMatch(fieldData)))
                    {
                        failures.Add(new ValidationFailure(field.FieldName, string.Format(labelInvalidFormat, field.Name, field.KeyFilter)));
                    }
                }
                else if (!fieldData.IsNullOrWhiteSpace() && (field.FieldType == QuestionnaireFieldType.Decimal || field.FieldType == QuestionnaireFieldType.Integer
                        || field.FieldType == QuestionnaireFieldType.Currency)
                    && field.CustomProperties != null)
                {
                    if (field.CustomProperties.MinValue.HasValue && decimal.Parse(fieldData) < field.CustomProperties.MinValue.Value)
                    {
                        failures.Add(new ValidationFailure(field.FieldName, string.Format(labelMinimumValue, field.Name, field.CustomProperties.MinValue.Value)));
                    }

                    if (field.CustomProperties.MaxValue.HasValue && decimal.Parse(fieldData) > field.CustomProperties.MaxValue.Value)
                    {
                        failures.Add(new ValidationFailure(field.FieldName, string.Format(labelMaximumValue, field.Name, field.CustomProperties.MaxValue.Value)));
                    }
                }
                else if (field.FieldType == QuestionnaireFieldType.Multivalue)
                {
                    if (field.CustomProperties.MinValue.HasValue && (!dto[field.FieldName].IsBsonArray || dto[field.FieldName].AsBsonArray.Count < field.CustomProperties.MinValue.Value))
                    {
                        failures.Add(new ValidationFailure(field.FieldName, string.Format(labelMinimumValue, field.Name, field.CustomProperties.MinValue.Value)));
                    }

                    if (field.CustomProperties.MaxValue.HasValue && (!dto[field.FieldName].IsBsonArray || dto[field.FieldName].AsBsonArray.Count > field.CustomProperties.MaxValue.Value))
                    {
                        failures.Add(new ValidationFailure(field.FieldName, string.Format(labelMaximumValue, field.Name, field.CustomProperties.MaxValue.Value)));
                    }
                }
            }

            if (!customCode.IsNullOrWhiteSpace())
            {
                CatalogoCustomImplLizzieContext catalogoCustomImplLizzieContext = _lifetimeScope.Resolve<CatalogoCustomImplLizzieContext>();

                catalogoCustomImplLizzieContext.SetDocument(dto);

                var customValidation = LambdaCompiler.Compile(catalogoCustomImplLizzieContext, customCode);                
            }

            if (failures.Count > 0)
            {
                throw new Exceptions.ValidationException(failures, SessionContext, AppLocalizationProvider);
            }
        }

        #region Private Methods

        private IMongoQueryable<QuestionnaireDto> GetQuestionnaireQuery()
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            int fieldTypeBoolean = (int)QuestionnaireFieldType.Boolean;
            int fieldTypeText = (int)QuestionnaireFieldType.Text;
            int fieldTypeMultivalue = (int)QuestionnaireFieldType.Multivalue;
            int fieldTypeDate = (int)QuestionnaireFieldType.Date;
            int fieldTypeDateTime = (int)QuestionnaireFieldType.DateTime;
            int fieldTypeTime = (int)QuestionnaireFieldType.Time;
            int fieldTypeInteger = (int)QuestionnaireFieldType.Integer;
            int fieldTypeDecimal = (int)QuestionnaireFieldType.Decimal;
            int fieldTypeGoogleAddress = (int)QuestionnaireFieldType.GoogleAddress;
            int fieldTypeCatalogCustom = (int)QuestionnaireFieldType.CatalogCustom;
            int fieldTypeUser = (int)QuestionnaireFieldType.User;

            string fieldTypeBooleanDesc = L("QuestionnaireFields.QuestionnaireField.TypeBoolean");
            string fieldTypeTextDesc = L("QuestionnaireFields.QuestionnaireField.TypeText");
            string fieldTypeMultivalueDesc = L("QuestionnaireFields.QuestionnaireField.TypeMultivalue");
            string fieldTypeDateDesc = L("QuestionnaireFields.QuestionnaireField.TypeDate");
            string fieldTypeDateTimeDesc = L("QuestionnaireFields.QuestionnaireField.TypeDateTime");
            string fieldTypeTimeDesc = L("QuestionnaireFields.QuestionnaireField.TypeTime");
            string fieldTypeIntegerDesc = L("QuestionnaireFields.QuestionnaireField.TypeInteger");
            string fieldTypeDecimalDesc = L("QuestionnaireFields.QuestionnaireField.TypeDecimal");
            string fieldTypeGoogleAddressDesc = L("QuestionnaireFields.QuestionnaireField.TypeGoogleAddress");
            string fieldTypeCatalogCustomDesc = L("QuestionnaireFields.QuestionnaireField.TypeCatalogCustom");
            string fieldTypeUserDesc = L("QuestionnaireFields.QuestionnaireField.TypeUser");

            int fieldControlInputText = (int)QuestionnaireFieldControl.InputText;
            int fieldControlDropDown = (int)QuestionnaireFieldControl.DropDown;
            int fieldControlListbox = (int)QuestionnaireFieldControl.Listbox;
            int fieldControlRadioButton = (int)QuestionnaireFieldControl.RadioButton;
            int fieldControlInputSwitch = (int)QuestionnaireFieldControl.InputSwitch;
            int fieldControlInputMask = (int)QuestionnaireFieldControl.InputMask;
            int fieldControlInputTextArea = (int)QuestionnaireFieldControl.InputTextArea;
            int fieldControlListboxMultivalue = (int)QuestionnaireFieldControl.ListboxMultivalue;
            int fieldControlCheckbox = (int)QuestionnaireFieldControl.Checkbox;
            int fieldControlMultiselect = (int)QuestionnaireFieldControl.Multiselect;
            int fieldControlCalendarBasic = (int)QuestionnaireFieldControl.CalendarBasic;
            int fieldControlCalendarTime = (int)QuestionnaireFieldControl.CalendarTime;
            int fieldControlCalendarTimeOnly = (int)QuestionnaireFieldControl.CalendarTimeOnly;
            int fieldControlSpinner = (int)QuestionnaireFieldControl.Spinner;
            int fieldControlSpinnerFormatInput = (int)QuestionnaireFieldControl.SpinnerFormatInput;
            int fieldControlTextNumber = (int)QuestionnaireFieldControl.TextNumber;
            int fieldControlGoogleAddress = (int)QuestionnaireFieldControl.GoogleAddress;
            int fieldControlAutocomplete = (int)QuestionnaireFieldControl.Autocomplete;
            int fieldControlAutocompleteDynamic = (int)QuestionnaireFieldControl.AutocompleteDynamic;

            string fieldControlInputTextDesc = L("QuestionnaireFields.QuestionnaireField.ControlInputText");
            string fieldControlDropDownDesc = L("QuestionnaireFields.QuestionnaireField.ControlDropDown");
            string fieldControlListboxDesc = L("QuestionnaireFields.QuestionnaireField.ControlListbox");
            string fieldControlRadioButtonDesc = L("QuestionnaireFields.QuestionnaireField.ControlRadioButton");
            string fieldControlInputSwitchDesc = L("QuestionnaireFields.QuestionnaireField.ControlInputSwitch");
            string fieldControlInputMaskDesc = L("QuestionnaireFields.QuestionnaireField.ControlInputMask");
            string fieldControlInputTextAreaDesc = L("QuestionnaireFields.QuestionnaireField.ControlInputTextArea");
            string fieldControlListboxMultivalueDesc = L("QuestionnaireFields.QuestionnaireField.ControlListboxMultivalue");
            string fieldControlCheckboxDesc = L("QuestionnaireFields.QuestionnaireField.ControlCheckbox");
            string fieldControlMultiselectDesc = L("QuestionnaireFields.QuestionnaireField.ControlMultiselect");
            string fieldControlCalendarBasicDesc = L("QuestionnaireFields.QuestionnaireField.ControlCalendarBasic");
            string fieldControlCalendarTimeDesc = L("QuestionnaireFields.QuestionnaireField.ControlCalendarTime");
            string fieldControlCalendarTimeOnlyDesc = L("QuestionnaireFields.QuestionnaireField.ControlCalendarTimeOnly");
            string fieldControlSpinnerDesc = L("QuestionnaireFields.QuestionnaireField.ControlSpinner");
            string fieldControlSpinnerFormatInputDesc = L("QuestionnaireFields.QuestionnaireField.ControlSpinnerFormatInput");
            string fieldControlTextNumberDesc = L("QuestionnaireFields.QuestionnaireField.ControlTextNumber");
            string fieldControlGoogleAddressDesc = L("QuestionnaireFields.QuestionnaireField.ControlGoogleAddress");
            string fieldControlAutocompleteDesc = L("QuestionnaireFields.QuestionnaireField.ControlAutocomplete");
            string fieldControlAutocompleteDynamicDesc = L("QuestionnaireFields.QuestionnaireField.ControlAutocompleteDynamic");

            var query = from entity in _repository.GetAll()
                        select new QuestionnaireDto
                        {
                            Id = entity.Id,
                            CreationDateTime = entity.CreationDateTime,
                            Name = entity.Name,
                            UserCreator = entity.UserCreator,
                            CustomCode = entity.CustomCode,
                            IsActive = entity.IsActive,
                            IsActiveDesc = entity.IsActive ? yesLabel : noLabel,
                            Sections = (List<QuestionnaireSectionDto>)entity.Sections.Select(p => new QuestionnaireSectionDto {
                                IconAF = p.IconAF,
                                Name = p.Name,
                                Order = p.Order,
                                Fields = (List<QuestionnaireFieldDto>)p.Fields.Select(q => new QuestionnaireFieldDto {
                                    Order = q.Order,
                                    FieldControl = (QuestionnaireFieldControl)q.FieldControl,
                                    FieldControlDesc = q.FieldControl == fieldControlInputText ? fieldControlInputTextDesc
                                        : q.FieldControl == fieldControlDropDown ? fieldControlDropDownDesc
                                        : q.FieldControl == fieldControlListbox ? fieldControlListboxDesc
                                        : q.FieldControl == fieldControlRadioButton ? fieldControlRadioButtonDesc
                                        : q.FieldControl == fieldControlInputSwitch ? fieldControlInputSwitchDesc
                                        : q.FieldControl == fieldControlInputMask ? fieldControlInputMaskDesc
                                        : q.FieldControl == fieldControlInputTextArea ? fieldControlInputTextAreaDesc
                                        : q.FieldControl == fieldControlListboxMultivalue ? fieldControlListboxMultivalueDesc
                                        : q.FieldControl == fieldControlCheckbox ? fieldControlCheckboxDesc
                                        : q.FieldControl == fieldControlMultiselect ? fieldControlMultiselectDesc
                                        : q.FieldControl == fieldControlCalendarBasic ? fieldControlCalendarBasicDesc
                                        : q.FieldControl == fieldControlCalendarTime ? fieldControlCalendarTimeDesc
                                        : q.FieldControl == fieldControlCalendarTimeOnly ? fieldControlCalendarTimeOnlyDesc
                                        : q.FieldControl == fieldControlSpinner ? fieldControlSpinnerDesc
                                        : q.FieldControl == fieldControlSpinnerFormatInput ? fieldControlSpinnerFormatInputDesc
                                        : q.FieldControl == fieldControlTextNumber ? fieldControlTextNumberDesc
                                        : q.FieldControl == fieldControlGoogleAddress ? fieldControlGoogleAddressDesc
                                        : q.FieldControl == fieldControlAutocomplete ? fieldControlAutocompleteDesc
                                        : q.FieldControl == fieldControlAutocompleteDynamic ? fieldControlAutocompleteDynamicDesc
                                        : null,
                                    FieldName = q.FieldName,
                                    FieldSize = q.FieldSize,
                                    FieldType = (QuestionnaireFieldType)q.FieldType,
                                    FieldTypeDesc = q.FieldType == fieldTypeBoolean ? fieldTypeBooleanDesc
                                        : q.FieldType == fieldTypeText ? fieldTypeTextDesc
                                        : q.FieldType == fieldTypeMultivalue ? fieldTypeMultivalueDesc
                                        : q.FieldType == fieldTypeDate ? fieldTypeDateDesc
                                        : q.FieldType == fieldTypeDateTime ? fieldTypeDateTimeDesc
                                        : q.FieldType == fieldTypeTime ? fieldTypeTimeDesc
                                        : q.FieldType == fieldTypeInteger ? fieldTypeIntegerDesc
                                        : q.FieldType == fieldTypeDecimal ? fieldTypeDecimalDesc
                                        : q.FieldType == fieldTypeGoogleAddress ? fieldTypeGoogleAddressDesc
                                        : q.FieldType == fieldTypeCatalogCustom ? fieldTypeCatalogCustomDesc
                                        : q.FieldType == fieldTypeUser ? fieldTypeUserDesc
                                        : null,
                                    HasKeyFilter = q.HasKeyFilter,
                                    InputMask = q.InputMask,
                                    IsRequired = q.IsRequired,
                                    IsRequiredDesc = q.IsRequired == true ? yesLabel : noLabel,
                                    KeyFilter = q.KeyFilter,
                                    Name = q.Name,
                                    Options = (List<QuestionnaireFieldOptionDto>)q.Options.Select(r => new QuestionnaireFieldOptionDto {
                                        Description = r.Description,
                                        Value = r.Value
                                    }),
                                    MustHaveOptions = q.FieldControl == fieldControlCheckbox
                                     || q.FieldControl == fieldControlDropDown
                                     || q.FieldControl == fieldControlListbox
                                     || q.FieldControl == fieldControlListboxMultivalue
                                     || q.FieldControl == fieldControlMultiselect
                                     || q.FieldControl == fieldControlRadioButton,
                                    CatalogCustom = q.FieldType == fieldTypeCatalogCustom && q.CatalogCustom != null ? new QuestionnaireCatalogCustomDto {
                                        CatalogCustom = q.CatalogCustom.idCatalogCustom.ToString(),
                                        FieldName = q.CatalogCustom.FieldName
                                    } : null,
                                    CustomProperties = q.CustomProperties == null ? null : new QuestionnaireCustomPropertiesDto
                                    {
                                        Currency = q.CustomProperties.Currency,
                                        Locale = q.CustomProperties.Locale,
                                        MaxValue = q.CustomProperties.MaxValue,
                                        MinValue = q.CustomProperties.MinValue,
                                        UseGrouping = q.CustomProperties.UseGrouping == true
                                    }
                                })
                            })
                        };

            return query;
        }

        private IMongoQueryable<QuestionnaireDto> GetQuestionnaireListQuery(QuestionnaireListFilterDto dto)
        {
            string yesLabel = L("Yes");
            string noLabel = L("No");

            var query = from entity in _repository.GetAll()
                        select new QuestionnaireDto
                        {
                            Id = entity.Id,
                            CreationDateTime = entity.CreationDateTime,
                            Name = entity.Name,
                            UserCreator = entity.UserCreator,
                            IsActive = entity.IsActive,
                            IsActiveDesc = entity.IsActive == true ? yesLabel : noLabel
                        };

            string filter = dto.Filter.IsNullOrEmpty() ? null : dto.Filter.ToUpper();

            query = query
                .WhereIf(
                    filter != null,
                    p => p.Id.Contains(filter)
                    || p.Name.ToUpper().Contains(filter)
                    || p.UserCreator.ToUpper().Contains(filter)
                    || p.IsActiveDesc.ToUpper().Contains(filter)
                );

            return query;
        }

        private async Task ValidateQuestionnaireAsync(QuestionnaireDto dto)
        {
            List<QuestionnaireDto> list = await GetQuestionnaireByNameListAsync(dto.Name);

            if (list.Count > 1 || (list.Count == 1 && list.ElementAt(0).Id != dto.Id))
            {
                throw new EntityDuplicatedException(L("Questionnaires.Questionnaire.DuplicatedKey"));
            }
        }

        private void AssignQuestionnaireSection(Questionnaire entity, QuestionnaireDto dto)
        {
            entity.Sections = dto.Sections.Select(p => new Section
            {
                IconAF = p.IconAF,
                Name = p.Name,
                Order = p.Order,
                Fields = p.Fields.Select(q => new Field
                {
                    Name = q.Name,
                    FieldName = q.Name.ToVariableName(),
                    FieldSize = q.FieldSize,
                    Order = q.Order,
                    FieldControl = (int)q.FieldControl,
                    FieldType = (int)q.FieldType,
                    HasKeyFilter = q.HasKeyFilter,
                    InputMask = q.InputMask,
                    IsRequired = q.IsRequired,
                    KeyFilter = q.KeyFilter,
                    Options = q.Options.Select(r => new FieldOption
                    {
                        Description = r.Description,
                        Value = r.Value
                    }).ToList(),
                    CatalogCustom = q.CatalogCustom == null ? null : new CatalogCustomRelation {
                        idCatalogCustom = q.CatalogCustom.CatalogCustom,
                        FieldName = q.CatalogCustom.FieldName
                    },
                    CustomProperties = q.CustomProperties == null ? null : new CustomProperties {
                        Currency = q.CustomProperties.Currency,
                        Locale = q.CustomProperties.Locale,
                        MaxValue = q.CustomProperties.MaxValue,
                        MinValue = q.CustomProperties.MinValue,
                        UseGrouping = q.CustomProperties.UseGrouping
                    }
                }).ToList()
            }).ToList();
        }

        #endregion

        #endregion

        #region Questionnaire Field

        public async Task<List<ComboboxItemDto>> GetQuestionnaireFieldComboAsync(QuestionnaireFieldComboFilterDto dto)
        {
            string questionnaire = dto.Questionnaire.IsNullOrWhiteSpace() ? null : dto.Questionnaire;

            var query = from Q in _repository.GetAll()
                        .WhereIf(questionnaire != null, p => p.Id == questionnaire)
                        from S in Q.Sections
                        from F in S.Fields
                        orderby F.Name
                        select new ComboboxItemDto
                        {
                            Value = F.FieldName,
                            Label = F.Name
                        };

            return await query.ToListAsync();
        }

        #endregion
    }
}
