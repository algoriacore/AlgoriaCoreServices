using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Domain.Excel;
using AlgoriaCore.Domain.Excel.Users;
using AlgoriaCore.Domain.Interfaces.Excel;
using AlgoriaCore.Domain.Interfaces.Excel.Users;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            List<IUserExcel> ll = new List<IUserExcel>();

            foreach (var item in pagedResultDto.Items)
            {
                ll.Add(new UserExcel
                {
                    Id = item.Id,
                    Login = item.Login,
                    FullName = item.FullName,
                    EmailAddress = item.EmailAddress,
                    IsActiveDesc = item.IsActiveDesc,
                    UserLockedDesc = item.UserLockedDesc
                });
            }

            IUserFilterExcel userFilterExcel = new UserFilterExcel { Filter = request.Filter };
            List<IViewColumn> columns = JsonConvert.DeserializeObject<List<ViewColumn>>(request.ViewColumnsConfigJSON)
                .Cast<IViewColumn>().ToList();

            var file = _excelService.ExportViewUsersToBinary(userFilterExcel, ll, columns);

            return new FileDto
            {
                FileName = file.FileName,
                FileToken = file.FileToken,
                FileType = file.FileType,
                FileBase64 = Convert.ToBase64String(file.FileArray)
            };
        }
    }
}
