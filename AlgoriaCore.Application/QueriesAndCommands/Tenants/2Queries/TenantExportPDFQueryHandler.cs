using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Domain.Excel;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.PDF;
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
    public class TenantExportPDFQueryHandler : BaseCoreClass, IRequestHandler<TenantExportPDFQuery, FileDto>
    {
        private readonly TenantManager _manager;

        private readonly IPDFService _pdfService;

        public TenantExportPDFQueryHandler(ICoreServices coreServices, TenantManager manager, IPDFService pdfService) : base(coreServices)
        {
            _manager = manager;

            _pdfService = pdfService;
        }

        public async Task<FileDto> Handle(TenantExportPDFQuery request, CancellationToken cancellationToken)
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

            byte[] bytes = await _pdfService.ExportView(L("Tenants"), ll, columns, GetViewFilters(request));

            CoreLogger.LogError("PDF Bytes: " + bytes == null ? "NULL" : bytes.Length.ToString());

            return new FileDto
            {
                FileName = "ViewTenants.pdf",
                FileType = "PDF",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }

        private List<IViewFilter> GetViewFilters(TenantExportPDFQuery query)
        {
            List<IViewFilter> filters = new List<IViewFilter>();

            filters.Add(new ViewFilter { Name = nameof(query.Filter), Title = L("SearchDots"), Value = query.Filter });

            return filters;
        }
    }
}
