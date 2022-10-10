using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Questionnaires;
using AlgoriaCore.Application.Managers.Questionnaires.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Questionnaires
{
    public class QuestionnaireGetListQueryHandler : BaseCoreClass, IRequestHandler<QuestionnaireGetListQuery, PagedResultDto<QuestionnaireForListResponse>>
    {
        private readonly QuestionnaireManager _manager;

        public QuestionnaireGetListQueryHandler(
            ICoreServices coreServices,
            QuestionnaireManager manager) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<QuestionnaireForListResponse>> Handle(QuestionnaireGetListQuery request, CancellationToken cancellationToken)
        {
            QuestionnaireListFilterDto filterDto = new QuestionnaireListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            PagedResultDto<QuestionnaireDto> pagedResultDto = await _manager.GetQuestionnaireListAsync(filterDto);
            List<QuestionnaireForListResponse> ll = new List<QuestionnaireForListResponse>();

            foreach (QuestionnaireDto dto in pagedResultDto.Items)
            {
                ll.Add(new QuestionnaireForListResponse()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    CreationDateTime = dto.CreationDateTime,
                    UserCreator = dto.UserCreator,
                    IsActive = dto.IsActive,
                    IsActiveDesc = dto.IsActiveDesc
                });
            }

            return new PagedResultDto<QuestionnaireForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
