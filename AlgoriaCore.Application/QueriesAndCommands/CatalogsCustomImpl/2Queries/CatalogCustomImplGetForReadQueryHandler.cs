using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom;
using AlgoriaCore.Application.QueriesAndCommands.Questionnaires;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetForReadQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomImplGetForReadQuery, CatalogCustomImplForReadResponse>
    {
        private readonly IMediator _mediator;
        private readonly CatalogCustomImplManager _manager;

        public CatalogCustomImplGetForReadQueryHandler(
            ICoreServices coreServices,
            IMediator mediator,
            CatalogCustomImplManager manager) : base(coreServices)
        {
            _mediator = mediator;
            _manager = manager;
        }

        public async Task<CatalogCustomImplForReadResponse> Handle(CatalogCustomImplGetForReadQuery request, CancellationToken cancellationToken)
        {
            CatalogCustomImplForReadResponse response;

            await _manager.SetCatalogCustom(request.Catalog);

            BsonDocument dto = await _manager.GetCatalogCustomImplAsync(request.Id);

            response = new CatalogCustomImplForReadResponse()
            {
                Id = request.Id,
                Data = _manager.ParseToDictionary(dto)
            };

            response.CatalogCustom = await _mediator.Send(new CatalogCustomGetByIdQuery { Id = request.Catalog });
            response.Questionnaire = await _mediator.Send(new QuestionnaireGetByIdQuery { Id = response.CatalogCustom.Questionnaire });

            return response;
        }
    }
}
