using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages;
using AlgoriaCore.Application.Managers.Languages.Dto;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.Managers.Roles.Dto;
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

namespace AlgoriaCore.Application.QueriesAndCommands.Languages
{
    public class LanguageExportCSVQueryHandler : BaseCoreClass, IRequestHandler<LanguageExportCSVQuery, FileDto>
    {
        private readonly LanguageManager _manager;

        private readonly ICSVService _csvService;

        public LanguageExportCSVQueryHandler(ICoreServices coreServices, LanguageManager manager, ICSVService csvService) : base(coreServices)
        {
            _manager = manager;

            _csvService = csvService;
        }

        public async Task<FileDto> Handle(LanguageExportCSVQuery request, CancellationToken cancellationToken)
        {
            LanguageListFilterDto filterDto = new LanguageListFilterDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                IsPaged = request.IsPaged
            };

            PagedResultDto<LanguageDto> pagedResultDto = await _manager.GetLanguageListAsync(filterDto);

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

            byte[] bytes = _csvService.ExportView(ll, columns);

            return new FileDto
            {
                FileName = "ViewLanguages.csv",
                FileType = "CSV",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }
    }
}
