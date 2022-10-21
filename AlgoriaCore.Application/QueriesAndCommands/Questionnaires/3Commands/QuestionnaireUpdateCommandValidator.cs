using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Questionnaires.Dto;
using AlgoriaCore.Extensions;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Text.RegularExpressions;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireUpdateCommandValidator : AbstractValidator<QuestionnaireUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public QuestionnaireUpdateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelInvalidFormat = L("FieldInvalidFormat");
            string labelGreaterThan = L("FieldGreaterThan");

            string labelId = L("Id");
            string labelName = L("QuestionnaireFields.QuestionnaireField.Name");

            string labelSectionName = L("QuestionnaireSections.QuestionnaireSection.Name");
            string labelSectionOrder = L("QuestionnaireSections.QuestionnaireSection.Order");

            string labelFieldName = L("QuestionnaireFields.QuestionnaireField.Name");
            string labelFieldOrder = L("QuestionnaireFields.QuestionnaireField.Order");
            string labelFieldType = L("QuestionnaireFields.QuestionnaireField.Type");
            string labelFieldControl = L("QuestionnaireFields.QuestionnaireField.Control");
            string labelFieldSize = L("QuestionnaireFields.QuestionnaireField.Size");
            string labelFieldDecimalPartSize = L("QuestionnaireFields.QuestionnaireField.DecimalPartSize");
            string labelFieldInputMask = L("QuestionnaireFields.QuestionnaireField.InputMask");
            string labelFieldKeyFilter = L("QuestionnaireFields.QuestionnaireField.KeyFilter");
            string labelFieldOptions = L("QuestionnaireFields.QuestionnaireField.Options");

            string labelOptionDescription = L("QuestionnaireFields.QuestionnaireField.Options.Option.Description");

            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(labelRequiredField, labelId));
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(labelRequiredField, labelName));
            RuleFor(x => x.Name).Matches("^.{3,50}$").When(x => !x.Name.IsNullOrWhiteSpace()).WithMessage(string.Format(labelInvalidFormat, labelName, "^.{3,50}$"));

            RuleFor(x => x).Custom((a, b) =>
            {
                int sectionIndex = 0;
                int fieldIndex = 0;
                int optionIndex = 0;

                string sectionPath;
                string fieldPath;
                string optionPath;
                string separator = ": ";

                foreach (QuestionnaireSectionResponse s in a.Sections)
                {
                    sectionIndex++;
                    fieldIndex = 0;
                    sectionPath = string.Format("Sections[{0}]", sectionIndex);

                    if (s.Name.IsNullOrWhiteSpace())
                    {
                        b.AddFailure(new ValidationFailure(sectionPath, sectionPath + separator + string.Format(labelRequiredField, labelSectionName)));
                    }
                    else
                    {
                        if (!Regex.IsMatch(s.Name, "^.{3,50}$"))
                        {
                            b.AddFailure(new ValidationFailure(sectionPath, sectionPath + separator + string.Format(labelInvalidFormat, labelSectionName, "^.{3,50}$")));
                        }

                        if (a.Sections.Count(p => p.Name.ToLower() == s.Name.ToLower()) > 1)
                        {
                            b.AddFailure(new ValidationFailure(sectionPath, sectionPath + separator + L("QuestionnaireSections.QuestionnaireSection.DuplicatedName")));
                        }
                    }

                    if (s.Order <= 0)
                    {
                        b.AddFailure(new ValidationFailure(sectionPath, sectionPath + separator + string.Format(labelGreaterThan, labelSectionOrder, 0)));
                    }
                    else if (a.Sections.Count(p => p.Order == s.Order) > 1)
                    {
                        b.AddFailure(new ValidationFailure(sectionPath, sectionPath + separator + L("QuestionnaireSections.QuestionnaireSection.DuplicatedOrder")));
                    }

                    foreach (QuestionnaireFieldResponse f in s.Fields)
                    {
                        fieldIndex++;
                        optionIndex = 0;
                        fieldPath = sectionPath + string.Format(".Fields[{0}]", fieldIndex);

                        if (f.Name.IsNullOrWhiteSpace())
                        {
                            b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelRequiredField, labelFieldName)));
                        }
                        else
                        {
                            if (!Regex.IsMatch(s.Name, "^.{3,50}$"))
                            {
                                b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelInvalidFormat, labelFieldName, "^.{3,50}$")));
                            }

                            if (a.Sections.SelectMany(p => p.Fields).Count(p => p.Name.ToLower() == f.Name.ToLower()) > 1)
                            {
                                b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + L("QuestionnaireFields.QuestionnaireField.DuplicatedFieldName")));
                            }
                        }

                        if (f.FieldType <= 0)
                        {
                            b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelRequiredField, labelFieldType)));
                        }

                        if (f.FieldControl <= 0)
                        {
                            b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelRequiredField, labelFieldControl)));
                        }

                        if (f.FieldSize == null || f.FieldSize <= 0)
                        {
                            if (f.FieldType == QuestionnaireFieldType.Text)
                            {
                                b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelRequiredField, labelFieldSize)));
                            }
                            else if (f.FieldType == QuestionnaireFieldType.Decimal)
                            {
                                b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelRequiredField, labelFieldDecimalPartSize)));
                            }
                        }

                        if (f.FieldControl == QuestionnaireFieldControl.InputMask && f.InputMask.IsNullOrWhiteSpace())
                        {
                            b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelRequiredField, labelFieldInputMask)));
                        }

                        if (f.HasKeyFilter && f.KeyFilter.IsNullOrWhiteSpace())
                        {
                            b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelRequiredField, labelFieldKeyFilter)));
                        }

                        if (s.Order <= 0)
                        {
                            b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelGreaterThan, labelFieldOrder, 0)));
                        }
                        else if (s.Fields.Count(p => p.Order == f.Order) > 1)
                        {
                            b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + L("QuestionnaireFields.QuestionnaireField.DuplicatedOrder")));
                        }

                        if ((f.Options == null || f.Options.Count <= 0) && MustFieldControlHaveOptions(f.FieldControl))
                        {
                            b.AddFailure(new ValidationFailure(fieldPath, fieldPath + separator + string.Format(labelRequiredField, labelFieldOptions)));
                        }

                        foreach (QuestionnaireFieldOptionResponse o in f.Options)
                        {
                            optionIndex++;
                            optionPath = fieldPath + string.Format(".Options[{0}]", fieldIndex);

                            if (o.Description.IsNullOrWhiteSpace())
                            {
                                b.AddFailure(new ValidationFailure(optionPath, optionPath + separator + string.Format(labelRequiredField, labelOptionDescription)));
                            }
                        }
                    }
                }
            });
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }

        private bool MustFieldControlHaveOptions(QuestionnaireFieldControl fieldControl)
        {
            return fieldControl == QuestionnaireFieldControl.Checkbox
                    || fieldControl == QuestionnaireFieldControl.DropDown
                    || fieldControl == QuestionnaireFieldControl.Listbox
                    || fieldControl == QuestionnaireFieldControl.ListboxMultivalue
                    || fieldControl == QuestionnaireFieldControl.Multiselect
                    || fieldControl == QuestionnaireFieldControl.RadioButton;
        }
    }
}
