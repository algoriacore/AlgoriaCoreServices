using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Users._1Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserGetListQueryHandler : BaseCoreClass, IRequestHandler<UserGetListQuery, PagedResultDto<UserListResponse>>
    {
        private readonly UserManager _userManager;

        public UserGetListQueryHandler(ICoreServices coreServices, UserManager userManager) : base(coreServices)
        {
            _userManager = userManager;
        }

        public async Task<PagedResultDto<UserListResponse>> Handle(UserGetListQuery request, CancellationToken cancellationToken)
        {
            var filter = new PageListByDto {
                Filter = request.Filter,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Sorting = request.Sorting
            };

            PagedResultDto<UserDto> pagedResultDto;

            if (SessionContext.TenantId == null && request.Tenant != null)
            {
                using (_userManager.CurrentUnitOfWork.SetTenantId(request.Tenant))
                {
                    pagedResultDto = await _userManager.GetUsersAsync(filter);
                }
            } else {
                pagedResultDto = await _userManager.GetUsersAsync(filter);
            }

            var ll = new List<UserListResponse>();

            foreach (var item in pagedResultDto.Items)
            {
                ll.Add(new UserListResponse {
                    Id = item.Id,
                    Login = item.Login,
                    Name = item.Name,
                    LastName = item.LastName,
                    SecondLastName = item.SecondLastName,
                    FullName = item.FullName,
                    EmailAddress = item.EmailAddress,
                    IsEmailConfirmed = item.IsEmailConfirmed,
                    UserLocked = item.UserLocked,
                    UserLockedDesc = item.UserLockedDesc,
                    IsActive = item.IsActive,
                    IsActiveDesc = item.IsActiveDesc,
                    CreationTime = item.CreationTime
                });
            }

            return new PagedResultDto<UserListResponse>(pagedResultDto.TotalCount, ll);
        }
    }
}
