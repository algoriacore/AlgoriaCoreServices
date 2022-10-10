using AlgoriaCore.Application.Localization;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using AlgoriaCore.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries
{
    public class UserLoginTokenQueryHandler : IRequestHandler<UserLoginTokenQuery, SessionLoginResponse>
    {
		private readonly UserManager _userManager;
		private readonly IAppLocalizationProvider _appLocalizationProvider;

		public UserLoginTokenQueryHandler(
			UserManager userManager,
			IAppLocalizationProvider appLocalizationProvider)
		{
			_userManager = userManager;
			_appLocalizationProvider = appLocalizationProvider;
		}

		public async Task<SessionLoginResponse> Handle(UserLoginTokenQuery request, CancellationToken cancellationToken)
		{
			using (_userManager.CurrentUnitOfWork.SetTenantId(request.TenantId ?? 0))
			{
				UserDto userDto = await _userManager.GetUserById(request.UserId, false);

				if (userDto == null)
				{
					throw new WrongUserNameOrPasswordException(_appLocalizationProvider.L("Login.FailLoginMessage"));
				}

				if (userDto.IsActive != true)
				{
					throw new WrongUserNameOrPasswordException(_appLocalizationProvider.L("Login.FailLoginMessage"));
				}

				//Genera información necesaria para la sesión
				var validationResult = new SessionLoginResponse();
				validationResult.TenantId = userDto.TenantId;
				validationResult.TenancyName = userDto.TenancyName;
				validationResult.UserId = userDto.Id;
				validationResult.UserName = userDto.Login;
				validationResult.FirstName = userDto.Name;
				validationResult.LastName = userDto.LastName;
				validationResult.SecondLastName = userDto.SecondLastName;
				validationResult.EMail = userDto.EmailAddress;

				return validationResult;
			}
		}
	}
}
