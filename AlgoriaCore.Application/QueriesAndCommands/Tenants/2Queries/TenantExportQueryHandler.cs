using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Domain.Excel;
using AlgoriaCore.Domain.Interfaces.Excel;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Tenants
{
    public class TenantExportQueryHandler : BaseCoreClass, IRequestHandler<TenantExportQuery, FileDto>
    {
        private readonly TenantManager _manager;

        private readonly IExcelService _excelService;

        public TenantExportQueryHandler(ICoreServices coreServices, TenantManager manager, IExcelService excelService) : base(coreServices)
        {
            _manager = manager;

            _excelService = excelService;
        }

        public async Task<FileDto> Handle(TenantExportQuery request, CancellationToken cancellationToken)
        {
            var filter = new PageListByDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                IsPaged = request.IsPaged
            };
            var pagedResultDto = await _manager.GetTenantsListAsync(filter);

            List<ExpandoObject> ll = new List<ExpandoObject>();
            dynamic l;

            foreach (var item in pagedResultDto.Items)
            {
                l = new ExpandoObject();

                l.Id = item.Id;
                l.Name = item.Name;
                l.TenancyName = item.TenancyName;
                l.LargeName = item.LargeName;
                l.IsActiveDesc = item.IsActiveDesc;

                ll.Add(l);
            }

            List<IViewColumn> columns = JsonConvert.DeserializeObject<List<ViewColumn>>(request.ViewColumnsConfigJSON)
                .Cast<IViewColumn>().ToList();

            var file = _excelService.ExportView(L("Tenants"), "ViewTenants", ll, columns, GetViewFilters(request));

            return new FileDto
            {
                FileName = file.FileName,
                FileToken = file.FileToken,
                FileType = file.FileType,
                FileBase64 = Convert.ToBase64String(file.FileArray)
            };
        }

        private List<IViewFilter> GetViewFilters(TenantExportQuery query)
        {
            List<IViewFilter> filters = new List<IViewFilter>();

            filters.Add(new ViewFilter { Name = nameof(query.Filter), Title = L("SearchDots"), Value = query.Filter });

            return filters;
        }
    }
}
