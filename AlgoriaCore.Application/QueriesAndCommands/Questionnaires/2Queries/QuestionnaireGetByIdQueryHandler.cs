using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Questionnaires;
using AlgoriaCore.Application.Managers.Questionnaires.Dto;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireGetByIdQueryHandler : BaseCoreClass, IRequestHandler<QuestionnaireGetByIdQuery, QuestionnaireResponse>
    {
        private readonly QuestionnaireManager _manager;

        public QuestionnaireGetByIdQueryHandler(ICoreServices coreServices, QuestionnaireManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<QuestionnaireResponse> Handle(QuestionnaireGetByIdQuery request, CancellationToken cancellationToken)
        {
            QuestionnaireResponse response = null;
            QuestionnaireDto dto = await _manager.GetQuestionnaireAsync(request.Id);

            response = new QuestionnaireResponse()
            {
                Id = dto.Id,
                Name = dto.Name,
                CreationDateTime = dto.CreationDateTime,
                CustomCode = dto.CustomCode,
                UserCreator = dto.UserCreator,
                IsActive = dto.IsActive,
                Sections = dto.Sections.Select(p => new QuestionnaireSectionResponse {
                    IconAF = p.IconAF,
                    Name = p.Name,
                    Order = p.Order,
                    Fields = p.Fields.Select(q => new QuestionnaireFieldResponse
                    {
                        Order = q.Order,
                        FieldControl = q.FieldControl,
                        FieldControlDesc = q.FieldControlDesc,
                        FieldName = q.FieldName,
                        FieldSize = q.FieldSize,
                        FieldType = q.FieldType,
                        FieldTypeDesc = q.FieldTypeDesc,
                        HasKeyFilter = q.HasKeyFilter,
                        InputMask = q.InputMask,
                        IsRequired = q.IsRequired,
                        IsRequiredDesc = q.IsRequiredDesc,
                        KeyFilter = q.KeyFilter,
                        Name = q.Name,
                        Options = q.Options.Select(r => new QuestionnaireFieldOptionResponse
                        {
                            Description = r.Description,
                            Value = r.Value
                        }).ToList(),
                        MustHaveOptions = q.MustHaveOptions,
                        CatalogCustom = q.CatalogCustom == null ? null : new QuestionnaireCatalogCustomResponse
                        {
                            CatalogCustom = q.CatalogCustom.CatalogCustom,
                            CatalogCustomDesc = q.CatalogCustom.CatalogCustomDesc,
                            FieldName = q.CatalogCustom.FieldName
                        },
                        CustomProperties = q.CustomProperties == null ? null : new QuestionnaireCustomPropertiesResponse
                        {
                            Currency = q.CustomProperties.Currency,
                            Locale = q.CustomProperties.Locale,
                            MaxValue = q.CustomProperties.MaxValue,
                            MinValue = q.CustomProperties.MinValue,
                            UseGrouping = q.CustomProperties.UseGrouping
                        }
                    }).ToList()
                }).ToList()
            };

            return response;
        }
    }
}
