﻿using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.Managers.Roles.Dto;
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

namespace AlgoriaCore.Application.QueriesAndCommands.Roles
{
    public class RoleExportQueryHandler : BaseCoreClass, IRequestHandler<RoleExportQuery, FileDto>
    {
        private readonly RoleManager _manager;

        private readonly IExcelService _excelService;

        public RoleExportQueryHandler(ICoreServices coreServices, RoleManager manager, IExcelService excelService) : base(coreServices)
        {
            _manager = manager;

            _excelService = excelService;
        }

        public async Task<FileDto> Handle(RoleExportQuery request, CancellationToken cancellationToken)
        {
            var filterDto = new RoleListFilterDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                IsPaged = request.IsPaged
            };

            var pagedResultDto = await _manager.GetRolesListAsync(filterDto);

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

            var file = _excelService.ExportView(L("Roles"), "ViewRoles", ll, columns, GetViewFilters(request));

            return new FileDto
            {
                FileName = file.FileName,
                FileToken = file.FileToken,
                FileType = file.FileType,
                FileBase64 = Convert.ToBase64String(file.FileArray)
            };
        }

        private List<IViewFilter> GetViewFilters(RoleExportQuery query)
        {
            List<IViewFilter> filters = new List<IViewFilter>();

            filters.Add(new ViewFilter { Name = nameof(query.Filter), Title = L("SearchDots"), Value = query.Filter });

            return filters;
        }
    }
}
