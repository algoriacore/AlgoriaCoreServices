﻿using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
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

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserExportQueryHandler : BaseCoreClass, IRequestHandler<UserExportQuery, FileDto>
    {
        private readonly UserManager _userManager;

        private readonly IExcelService _excelService;

        public UserExportQueryHandler(ICoreServices coreServices, UserManager userManager, IExcelService excelService) : base(coreServices)
        {
            _userManager = userManager;

            _excelService = excelService;
        }

        public async Task<FileDto> Handle(UserExportQuery request, CancellationToken cancellationToken)
        {
            var filter = new PageListByDto
            {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting,
                IsPaged = request.IsPaged
            };

            PagedResultDto<UserDto> pagedResultDto;

            if (SessionContext.TenantId == null && request.Tenant != null)
            {
                using (_userManager.CurrentUnitOfWork.SetTenantId(request.Tenant))
                {
                    pagedResultDto = await _userManager.GetUsersAsync(filter);
                }
            }
            else
            {
                pagedResultDto = await _userManager.GetUsersAsync(filter);
            }


            List<ExpandoObject> ll = new List<ExpandoObject>();
            dynamic l;

            foreach (var item in pagedResultDto.Items)
            {
                l = new ExpandoObject();

                l.Id = item.Id;
                l.Name = item.Name;
                l.LastName = item.LastName;
                l.SecondLastName = item.SecondLastName;
                l.Login = item.Login;
                l.FullName = item.FullName;
                l.EmailAddress = item.EmailAddress;
                l.IsActiveDesc = item.IsActiveDesc;
                l.UserLockedDesc = item.UserLockedDesc;

                ll.Add(l);
            }

            List<IViewColumn> columns = JsonConvert.DeserializeObject<List<ViewColumn>>(request.ViewColumnsConfigJSON)
                .Cast<IViewColumn>().ToList();

            var file = _excelService.ExportView(L("Users"), "ViewUsers", ll, columns, GetViewFilters(request));

            return new FileDto
            {
                FileName = file.FileName,
                FileToken = file.FileToken,
                FileType = file.FileType,
                FileBase64 = Convert.ToBase64String(file.FileArray)
            };
        }

        private List<IViewFilter> GetViewFilters(UserExportQuery query)
        {
            List<IViewFilter> filters = new List<IViewFilter>();

            filters.Add(new ViewFilter { Name = nameof(query.Filter), Title = L("SearchDots"), Value = query.Filter });

            return filters;
        }
    }
}
