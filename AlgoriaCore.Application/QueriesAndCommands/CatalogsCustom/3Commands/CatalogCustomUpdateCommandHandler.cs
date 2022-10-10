using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomUpdateCommandHandler : BaseCoreClass, IRequestHandler<CatalogCustomUpdateCommand, string>
    {
        private readonly CatalogCustomManager _manager;

        public CatalogCustomUpdateCommandHandler(ICoreServices coreServices
        , CatalogCustomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<string> Handle(CatalogCustomUpdateCommand request, CancellationToken cancellationToken)
        {
            await _manager.UpdateCatalogCustomAsync(new CatalogCustomDto
            {
                Id = request.Id,
                Description = request.Description,
                NameSingular = request.NameSingular,
                NamePlural = request.NamePlural,
                Questionnaire = request.Questionnaire,
                IsActive = request.IsActive,
                FieldNames = request.FieldNames
            });

            return request.Id;
        }
    }
}
