using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.QueriesAndCommands.Users._1Model;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserGetForEditQueryHandler : BaseCoreClass, IRequestHandler<UserGetForEditQuery, UserForEditResponse>
    {
        private readonly UserManager _userManager;

        public UserGetForEditQueryHandler(ICoreServices coreServices,
            UserManager userManager,
            ILogger<UserGetForEditQueryHandler> logger
        ) : base(coreServices)
        {
            _userManager = userManager;
        }

        public async Task<UserForEditResponse> Handle(UserGetForEditQuery request, CancellationToken cancellationToken)
        {
            var dto = await _userManager.GetUserById(request.Id);

            var resp = new UserForEditResponse
            {
                Id = dto.Id,
                UserName = dto.Login,
                Name = dto.Name,
                LastName = dto.LastName,
                SecondLastName = dto.SecondLastName,
                EmailAddress = dto.EmailAddress,
                PhoneNumber = dto.PhoneNumber,
                IsActive = dto.IsActive,
                ShouldChangePasswordOnNextLogin = dto.ChangePassword
            };

            var userRoleList = await _userManager.GetRoleListByUser(dto.Id);

            resp.RolList = userRoleList.Select(s => new UserRolResponse {
                RoleId = s.RoleId,
                RoleName = s.RoleName,
                RoleDisplayName = s.RoleDisplayName
            }).ToList();

            return resp;
        }
    }
}
