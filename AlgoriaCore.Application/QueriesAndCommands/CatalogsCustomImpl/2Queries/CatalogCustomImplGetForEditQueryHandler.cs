using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustomImpl;
using AlgoriaCore.Application.Managers.Questionnaires;
using AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom;
using AlgoriaCore.Application.QueriesAndCommands.Questionnaires;
using AlgoriaCore.Extensions;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustomImpl
{
    public class CatalogCustomImplGetForEditQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomImplGetForEditQuery, CatalogCustomImplForEditResponse>
    {
        private readonly IMediator _mediator;
        private readonly CatalogCustomImplManager _manager;

        public CatalogCustomImplGetForEditQueryHandler(
            ICoreServices coreServices,
            IMediator mediator,
            CatalogCustomImplManager manager) : base(coreServices)
        {
            _mediator = mediator;
            _manager = manager;
        }

        public async Task<CatalogCustomImplForEditResponse> Handle(CatalogCustomImplGetForEditQuery request, CancellationToken cancellationToken)
        {
            CatalogCustomImplForEditResponse response;

            if (request.Id.IsNullOrWhiteSpace())
            {
                response = new CatalogCustomImplForEditResponse();
            }
            else
            {
                await _manager.SetCatalogCustom(request.Catalog);

                BsonDocument dto = await _manager.GetCatalogCustomImplAsync(request.Id);

                response = new CatalogCustomImplForEditResponse()
                {
                    Id = request.Id,
                    Data = _manager.ParseToDictionary(dto)
                };
            }

            response.CatalogCustom = await _mediator.Send(new CatalogCustomGetByIdQuery() { Id = request.Catalog }, cancellationToken);
            response.Questionnaire = await _mediator.Send(new QuestionnaireGetByIdQuery() { Id = response.CatalogCustom.Questionnaire }, cancellationToken);

            return response;
        }
    }
}
