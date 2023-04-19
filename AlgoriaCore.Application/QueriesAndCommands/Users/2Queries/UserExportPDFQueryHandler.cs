using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
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

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserExportPDFQueryHandler : BaseCoreClass, IRequestHandler<UserExportPDFQuery, FileDto>
    {
        private readonly UserManager _userManager;

        private readonly IPDFService _pdfService;

        public UserExportPDFQueryHandler(ICoreServices coreServices, UserManager userManager, IPDFService pdfService) : base(coreServices)
        {
            _userManager = userManager;

            _pdfService = pdfService;
        }

        public async Task<FileDto> Handle(UserExportPDFQuery request, CancellationToken cancellationToken)
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

            byte[] bytes = await _pdfService.ExportView(L("Users"), ll, columns, GetViewFilters(request));
            // byte[] bytes = await _pdfService.ExportView(L("Users"), new List<ExpandoObject>(), columns, GetViewFilters(request));
            // byte[] bytes = await _pdfService.ExportView(L("Users"), GetFakedList(), columns, GetViewFilters(request));

            return new FileDto
            {
                FileName = "ViewUsers.pdf",
                FileType = "PDF",
                FileBase64 = Convert.ToBase64String(bytes)
            };
        }

        private List<IViewFilter> GetViewFilters(UserExportPDFQuery query)
        {
            List<IViewFilter> filters = new List<IViewFilter>();

            filters.Add(new ViewFilter { Name = nameof(query.Filter), Title = L("SearchDots"), Value = query.Filter });

            return filters;
        }

        private List<ExpandoObject> GetFakedList()
        {
            List<ExpandoObject> ll = new List<ExpandoObject>();
            dynamic l;

            for(var i = 1; i <= 3500; i++)
            {
                l = new ExpandoObject();

                l.Id = i;
                l.Name = "Fernando " + i;
                l.LastName = "Castro " + i;
                l.SecondLastName = "Medina " + i;
                l.Login = "fcastro" + i;
                l.FullName = l.Name + " " + l.LastName + " " + l.SecondLastName;
                l.EmailAddress = "fcastro" + i + "@algoria.com.mx";
                l.IsActiveDesc = "Sí";
                l.UserLockedDesc = "No";

                ll.Add(l);
            }

            return ll;
        }
    }
}
