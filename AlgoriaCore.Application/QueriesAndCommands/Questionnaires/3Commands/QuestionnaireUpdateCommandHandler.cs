using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Questionnaires;
using AlgoriaCore.Application.Managers.Questionnaires.Dto;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireUpdateCommandHandler : BaseCoreClass, IRequestHandler<QuestionnaireUpdateCommand, string>
    {
        private readonly QuestionnaireManager _manager;

        public QuestionnaireUpdateCommandHandler(
            ICoreServices coreServices,
            QuestionnaireManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<string> Handle(QuestionnaireUpdateCommand request, CancellationToken cancellationToken)
        {
            await _manager.UpdateQuestionnaireAsync(new QuestionnaireDto
            {
                Id = request.Id,
                Name = request.Name,
                CustomCode = request.CustomCode,
                IsActive = request.IsActive,
                Sections = request.Sections.Select(p => new QuestionnaireSectionDto
                {
                    IconAF = p.IconAF,
                    Order = p.Order,
                    Name = p.Name,
                    Fields = p.Fields.Select(q => new QuestionnaireFieldDto
                    {
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
                        CatalogCustom = q.CatalogCustom == null ? null : new QuestionnaireCatalogCustomDto {
                            CatalogCustom = q.CatalogCustom.CatalogCustom,
                            FieldName = q.CatalogCustom.FieldName
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
            });

            return request.Id;
        }
    }
}
