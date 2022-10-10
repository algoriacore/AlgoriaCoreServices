using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Extensions;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomUpdateCommandValidator : AbstractValidator<CatalogCustomUpdateCommand>
    {
        private readonly ICoreServices _coreServices;

        public CatalogCustomUpdateCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelInvalidFormat = L("FieldInvalidFormat");

            string labelId = L("Id");
            string labelDescription = L("CatalogsCustom.CatalogCustom.Description");
            string labelNameSingular = L("CatalogsCustom.CatalogCustom.NameSingular");
            string labelNamePlural = L("CatalogsCustom.CatalogCustom.NamePlural");

            RuleFor(x => x.Description).NotEmpty().WithMessage(string.Format(labelRequiredField, labelDescription));
            RuleFor(x => x.Description).Matches("^.{3,500}$").When(x => !x.Description.IsNullOrWhiteSpace()).WithMessage(string.Format(labelInvalidFormat, labelDescription, "^.{3,500}$"));
            RuleFor(x => x.NameSingular).NotEmpty().WithMessage(string.Format(labelRequiredField, labelNameSingular));
            RuleFor(x => x.NameSingular).Matches("^.{3,50}$").When(x => !x.NameSingular.IsNullOrWhiteSpace()).WithMessage(string.Format(labelInvalidFormat, labelNameSingular, "^.{3,50}$"));
            RuleFor(x => x.NamePlural).NotEmpty().WithMessage(string.Format(labelRequiredField, labelNamePlural));
            RuleFor(x => x.NamePlural).Matches("^.{3,55}$").When(x => !x.NamePlural.IsNullOrWhiteSpace()).WithMessage(string.Format(labelInvalidFormat, labelNamePlural, "^.{3,55}$"));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
