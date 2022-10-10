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
    public class QuestionnaireCreateCommandHandler : BaseCoreClass, IRequestHandler<QuestionnaireCreateCommand, string>
    {
        private readonly QuestionnaireManager _manager;

        public QuestionnaireCreateCommandHandler(ICoreServices coreServices, QuestionnaireManager manager): base(coreServices)
        {
            _manager = manager;
        }

        public async Task<string> Handle(QuestionnaireCreateCommand request, CancellationToken cancellationToken)
        {
            QuestionnaireDto dto = new QuestionnaireDto()
            {
                Name = request.Name,
                CustomCode = request.CustomCode,
                IsActive = request.IsActive,
                Sections = request.Sections.Select(p => new QuestionnaireSectionDto {
                    IconAF =  p.IconAF,
                    Order = p.Order,
                    Name = p.Name,
                    Fields = p.Fields.Select(q => new QuestionnaireFieldDto {
                        Name = q.Name,
                        FieldSize = q.FieldSize,
                        Order = q.Order,
                        FieldControl = q.FieldControl,
                        FieldName = q.FieldName,
                        FieldType = q.FieldType,
                        HasKeyFilter = q.HasKeyFilter,
                        InputMask = q.InputMask,
                        IsRequired = q.IsRequired,
                        KeyFilter = q.KeyFilter,
                        Options = q.Options.Select(r => new QuestionnaireFieldOptionDto
                        {
                            Description = r.Description,
                            Value = r.Value
                        }).ToList(),
                        CatalogCustom = q.CatalogCustom == null ? null : new QuestionnaireCatalogCustomDto
                        {
                            CatalogCustom = q.CatalogCustom.CatalogCustom,
                            FieldName = q.FieldName
                        },
                        CustomProperties = q.CustomProperties == null ? null : new QuestionnaireCustomPropertiesDto
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

            return await _manager.CreateQuestionnaireAsync(dto);
        }
    }
}
