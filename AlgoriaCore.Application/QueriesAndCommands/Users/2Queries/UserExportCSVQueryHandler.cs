using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
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

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserExportCSVQueryHandler : BaseCoreClass, IRequestHandler<UserExportCSVQuery, FileDto>
    {
        private readonly UserManager _userManager;

        private readonly ICSVService _csvService;

        public UserExportCSVQueryHandler(ICoreServices coreServices, UserManager userManager, ICSVService csvService) : base(coreServices)
        {
            _userManager = userManager;

            _csvService = csvService;
        }

        public async Task<FileDto> Handle(UserExportCSVQuery request, CancellationToken cancellationToken)
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

            byte[] bytes = _csvService.ExportViewUsersToBinary(ll, columns);

            return new FileDto
            {
                FileName = "ViewUsers.csv",
                FileType = "CSV",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }
    }
}
