using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using AlgoriaCore.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries
{
    public class UserImpersonalizeQueryHandler : BaseCoreClass, IRequestHandler<UserImpersonalizeQuery, SessionLoginResponse>
    {
        private readonly UserManager _userManager;

        public UserImpersonalizeQueryHandler(ICoreServices coreServices
            , UserManager userManager
        ) : base(coreServices)
        {
            _userManager = userManager;
        }

        public async Task<SessionLoginResponse> Handle(UserImpersonalizeQuery request, CancellationToken cancellationToken)
        {
            SessionLoginResponse validationResult = null;
            UserDto userDto = null;

            if (SessionContext.TenantId != null && SessionContext.TenantId != request.Tenant)
            {
                throw new WrongUserNameOrPasswordException(L("Impersonalize.NotPermited"));
            }

            using (_userManager.CurrentUnitOfWork.SetTenantId(request.Tenant))
            {
                userDto = await _userManager.GetUserById(request.User);
            }

            if (userDto == null)
            {
                throw new WrongUserNameOrPasswordException(L("Impersonalize.UserNotFound"));
            }

            if (userDto.IsActive != true)
            {
                throw new WrongUserNameOrPasswordException(L("Impersonalize.UserInactivated"));
            }

            //Genera información necesaria para la sesión
            validationResult = new SessionLoginResponse();
            validationResult.TenantId = userDto.TenantId;
            validationResult.TenancyName = userDto.TenancyName;
            validationResult.UserId = userDto.Id;
            validationResult.UserName = userDto.Login;
            validationResult.FirstName = userDto.Name;
            validationResult.LastName = userDto.LastName;
            validationResult.SecondLastName = userDto.SecondLastName;
            validationResult.EMail = userDto.EmailAddress;
            validationResult.IsImpersonalized = true;

            if (SessionContext.ImpersonalizerUserId == null)
            {
                validationResult.ImpersonalizerUserId = SessionContext.UserId;
            } else {
                validationResult.ImpersonalizerUserId = SessionContext.ImpersonalizerUserId;
            }

            return validationResult;
        }
    }
}
