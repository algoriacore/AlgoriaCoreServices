using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetForEditValidator : AbstractValidator<CatalogCustomImplGetForEditQuery>
    {
        private readonly ICoreServices _coreServices;

        public CatalogCustomImplGetForEditValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelCatalog = L("CatalogsCustom.CatalogCustom");

            RuleFor(x => x.Catalog).NotEmpty().WithMessage(string.Format(labelRequiredField, labelCatalog));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
