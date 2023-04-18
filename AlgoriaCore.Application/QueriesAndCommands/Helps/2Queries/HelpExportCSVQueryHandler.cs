using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Helps;
using AlgoriaCore.Application.Managers.Helps.Dto;
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

namespace AlgoriaCore.Application.QueriesAndCommands.Helps
{
    public class HelpExportCSVQueryHandler : BaseCoreClass, IRequestHandler<HelpExportCSVQuery, FileDto>
    {
        private readonly HelpManager _manager;

        private readonly ICSVService _csvService;

        public HelpExportCSVQueryHandler(ICoreServices coreServices, HelpManager manager, ICSVService csvService) : base(coreServices)
        {
            _manager = manager;

            _csvService = csvService;
        }

        public async Task<FileDto> Handle(HelpExportCSVQuery request, CancellationToken cancellationToken)
        {
            var filterDto = new HelpListFilterDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                IsPaged = request.IsPaged,
                IsIncludeBody = false
            };

            PagedResultDto<HelpDto> pagedResultDto = await _manager.GetHelpListAsync(filterDto);

            List<ExpandoObject> ll = new List<ExpandoObject>();
            dynamic l;

            foreach (var item in pagedResultDto.Items)
            {
                l = new ExpandoObject();

                l.Id = item.Id;
                l.LanguageDesc = item.LanguageDesc;
                l.Key = item.Key;
                l.DisplayName = item.DisplayName;
                l.IsActive = item.IsActive;
                l.IsActiveDesc = item.IsActiveDesc;

                ll.Add(l);
            }

            List<IViewColumn> columns = JsonConvert.DeserializeObject<List<ViewColumn>>(request.ViewColumnsConfigJSON)
                .Cast<IViewColumn>().ToList();

            byte[] bytes = _csvService.ExportView(ll, columns);

            return new FileDto
            {
                FileName = "ViewHelps.csv",
                FileType = "CSV",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }
    }
}
