using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
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

namespace AlgoriaCore.Application.QueriesAndCommands.Languages
{
    public class LanguageExportPDFQueryHandler : BaseCoreClass, IRequestHandler<LanguageExportPDFQuery, FileDto>
    {
        private readonly LanguageManager _manager;

        private readonly IPDFService _pdfService;

        public LanguageExportPDFQueryHandler(ICoreServices coreServices, LanguageManager manager, IPDFService pdfService) : base(coreServices)
        {
            _manager = manager;

            _pdfService = pdfService;
        }

        public async Task<FileDto> Handle(LanguageExportPDFQuery request, CancellationToken cancellationToken)
        {
            var filterDto = new LanguageListFilterDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                IsPaged = request.IsPaged
            };

            var pagedResultDto = await _manager.GetLanguageListAsync(filterDto);

            List<ExpandoObject> ll = new List<ExpandoObject>();
            dynamic l;

            foreach (var item in pagedResultDto.Items)
            {
                l = new ExpandoObject();

                l.Id = item.Id;
                l.Name = item.Name;
                l.DisplayName = item.DisplayName;
                l.IsActive = item.IsActive;
                l.IsActiveDesc = item.IsActiveDesc;

                ll.Add(l);
            }

            List<IViewColumn> columns = JsonConvert.DeserializeObject<List<ViewColumn>>(request.ViewColumnsConfigJSON)
                .Cast<IViewColumn>().ToList();

            byte[] bytes = await _pdfService.ExportView(L("Languages"), ll, columns, GetViewFilters(request));
            return new FileDto
            {
                FileName = "ViewLanguages.pdf",
                FileType = "PDF",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }

        private List<IViewFilter> GetViewFilters(LanguageExportPDFQuery query)
        {
            List<IViewFilter> filters = new List<IViewFilter>();

            filters.Add(new ViewFilter { Name = nameof(query.Filter), Title = L("SearchDots"), Value = query.Filter });

            return filters;
        }
    }
}
