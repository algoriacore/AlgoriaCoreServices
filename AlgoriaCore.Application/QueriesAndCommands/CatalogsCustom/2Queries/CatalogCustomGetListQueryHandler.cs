using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.CatalogsCustom;
using AlgoriaCore.Application.Managers.CatalogsCustom.Dto;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.CatalogsCustom
{
    public class CatalogCustomGetListQueryHandler : BaseCoreClass, IRequestHandler<CatalogCustomGetListQuery, PagedResultDto<CatalogCustomForListResponse>>
    {
        private readonly CatalogCustomManager _manager;

        public CatalogCustomGetListQueryHandler(
            ICoreServices coreServices,
            CatalogCustomManager manager
            ) : base(coreServices)
        {
            _manager = manager;
        }

        public async Task<PagedResultDto<CatalogCustomForListResponse>> Handle(CatalogCustomGetListQuery request, CancellationToken cancellationToken)
        {
            CatalogCustomListFilterDto filterDto = new CatalogCustomListFilterDto()
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            PagedResultDto<CatalogCustomDto> pagedResultDto = await _manager.GetCatalogCustomListAsync(filterDto);
            List<CatalogCustomForListResponse> ll = new List<CatalogCustomForListResponse>();

            foreach (CatalogCustomDto dto in pagedResultDto.Items)
            {
                ll.Add(new CatalogCustomForListResponse()
                {
                    Id = dto.Id,
                    CollectionName = dto.CollectionName,
                    NamePlural = dto.NamePlural,
                    NameSingular = dto.NameSingular,
                    IsCollectionGenerated = dto.IsCollectionGenerated,
                    IsCollectionGeneratedDesc = dto.IsCollectionGeneratedDesc,
                    CreationDateTime = dto.CreationDateTime,
                    UserCreator = dto.UserCreator,
                    IsActive = dto.IsActive,
                    IsActiveDesc = dto.IsActiveDesc,
                    Questionnaire = dto.Questionnaire
                });
            }

            return new PagedResultDto<CatalogCustomForListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
