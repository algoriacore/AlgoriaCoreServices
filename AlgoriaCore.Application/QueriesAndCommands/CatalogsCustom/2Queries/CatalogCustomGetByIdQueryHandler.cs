using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomGetByIdQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomGetByIdQuery, CatalogCustomResponse>
    {
        private readonly CatalogCustomManager _manager;

        public CatalogCustomGetByIdQueryHandler(ICoreServices coreServices, CatalogCustomManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<CatalogCustomResponse> Handle(CatalogCustomGetByIdQuery request, CancellationToken cancellationToken)
        {
            CatalogCustomResponse response = null;
            CatalogCustomDto dto = await _manager.GetCatalogCustomAsync(request.Id);

            response = new CatalogCustomResponse()
            {
                Id = dto.Id,
                CollectionName = dto.CollectionName,
                NameSingular = dto.NameSingular,
                NamePlural = dto.NamePlural,
                Description = dto.Description,
                IsCollectionGenerated = dto.IsCollectionGenerated,
                CreationDateTime = dto.CreationDateTime,
                UserCreator = dto.UserCreator,
                IsActive = dto.IsActive,
                Questionnaire = dto.Questionnaire,
                FieldNames = dto.FieldNames

            };

            return response;
        }
    }
}
