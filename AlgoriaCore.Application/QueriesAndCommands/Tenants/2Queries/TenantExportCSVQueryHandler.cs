using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Domain.Excel;
using AlgoriaCore.Domain.Interfaces.CSV;
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
    public class TenantExportCSVQueryHandler : BaseCoreClass, IRequestHandler<TenantExportCSVQuery, FileDto>
    {
        private readonly TenantManager _manager;

        private readonly ICSVService _csvService;

        public TenantExportCSVQueryHandler(ICoreServices coreServices, TenantManager manager, ICSVService csvService) : base(coreServices)
        {
            _manager = manager;

            _csvService = csvService;
        }

        public async Task<FileDto> Handle(TenantExportCSVQuery request, CancellationToken cancellationToken)
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

            byte[] bytes = _csvService.ExportView(ll, columns);

            return new FileDto
            {
                FileName = "ViewTenants.csv",
                FileType = "CSV",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }
    }
}
