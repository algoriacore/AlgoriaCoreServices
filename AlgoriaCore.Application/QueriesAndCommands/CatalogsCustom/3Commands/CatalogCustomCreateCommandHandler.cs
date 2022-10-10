using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomCreateCommandHandler : BaseCoreClass, IRequestHandler<CatalogCustomCreateCommand, string>
    {
        private readonly CatalogCustomManager _manager;

        public CatalogCustomCreateCommandHandler(ICoreServices coreServices, CatalogCustomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<string> Handle(CatalogCustomCreateCommand request, CancellationToken cancellationToken)
        {
            return await _manager.CreateCatalogCustomAsync(new CatalogCustomDto
            {
                Description = request.Description,
                NameSingular = request.NameSingular,
                NamePlural = request.NamePlural,
                Questionnaire = request.Questionnaire,
                IsActive = request.IsActive,
                FieldNames = request.FieldNames
            });
        }
    }
}
