using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetForReadValidator : AbstractValidator<CatalogCustomImplGetForReadQuery>
    {
        private readonly ICoreServices _coreServices;

        public CatalogCustomImplGetForReadValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelCatalog = L("CatalogsCustom.CatalogCustom");
            string labelId = L("Id");

            RuleFor(x => x.Catalog).NotEmpty().WithMessage(string.Format(labelRequiredField, labelCatalog));
            RuleFor(x => x.Id).NotEmpty().WithMessage(string.Format(labelRequiredField, labelId));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
