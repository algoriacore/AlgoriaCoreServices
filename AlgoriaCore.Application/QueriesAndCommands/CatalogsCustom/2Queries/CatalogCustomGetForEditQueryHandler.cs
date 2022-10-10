using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using AlgoriaCore.Application.Managers.Questionnaires;
using AlgoriaCore.Extensions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomGetForEditQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomGetForEditQuery, CatalogCustomForEditResponse>
    {
        private readonly CatalogCustomManager _manager;
        private readonly QuestionnaireManager _managerQuestionnaire;

        public CatalogCustomGetForEditQueryHandler(
            ICoreServices coreServices,
            CatalogCustomManager manager,
            QuestionnaireManager managerQuestionnaire) : base(coreServices)
        {
            _manager = manager;
            _managerQuestionnaire = managerQuestionnaire;
        }

        public async Task<CatalogCustomForEditResponse> Handle(CatalogCustomGetForEditQuery request, CancellationToken cancellationToken)
        {
            CatalogCustomForEditResponse response;
   
            if (request.Id.IsNullOrWhiteSpace())
            {
                response = new CatalogCustomForEditResponse();
            }
            else
            {
                CatalogCustomDto dto = await _manager.GetCatalogCustomAsync(request.Id);

                response = new CatalogCustomForEditResponse()
                {
                    Id = dto.Id,
                    NameSingular = dto.NameSingular,
                    NamePlural = dto.NamePlural,
                    IsCollectionGenerated = dto.IsCollectionGenerated,
                    CollectionName = dto.CollectionName,
                    Description = dto.Description,
                    CreationDateTime = dto.CreationDateTime,
                    Questionnaire = dto.Questionnaire,
                    QuestionnaireDesc = dto.QuestionnaireDesc,
                    UserCreator = dto.UserCreator,
                    IsActive = dto.IsActive,
                    FieldNames = dto.FieldNames
                };
            }

            response.QuestionnaireCombo = await _managerQuestionnaire.GetQuestionnaireComboAsync();

            if (!response.Questionnaire.IsNullOrWhiteSpace() && !response.QuestionnaireCombo.Exists(p => p.Value == response.Questionnaire))
            {
                response.QuestionnaireCombo.Add(new ComboboxItemDto(response.Questionnaire.ToString(), response.Questionnaire));
                response.QuestionnaireCombo = response.QuestionnaireCombo.OrderBy(p => p.Label).ToList();
            }

            return response;
        }
    }
}
