using AlgoriaCore.Application.Localization;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.Tenants;
using AlgoriaCore.Application.Managers.Tenants.Dto;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Extensions;
using AlgoriaPersistence.Interfaces.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries
{
    public class UserLoginMicrosoftQueryHandler : IRequestHandler<UserLoginMicrosoftQuery, SessionLoginResponse>
	{
		private readonly UserManager _userManager;
		private readonly SettingManager _settingManager;
		private readonly TenantManager _managerTenant;
		private readonly IClock Clock;
		private readonly IAppLocalizationProvider _appLocalizationProvider;

		public UserLoginMicrosoftQueryHandler(UserManager userManager,
									SettingManager settingManager,
									TenantManager managerTenant,
									IClock clock,
									IAppLocalizationProvider appLocalizationProvider)
		{
			_userManager = userManager;
			_settingManager = settingManager;
			_managerTenant = managerTenant;

			Clock = clock;
			_appLocalizationProvider = appLocalizationProvider;
		}

		public async Task<SessionLoginResponse> Handle(UserLoginMicrosoftQuery request, CancellationToken cancellationToken)
		{
			UserDto userDto = await GetUserDto(request.UserName, request.TenancyName);

            // Genera información necesaria para la sesión
            var validationResult = new SessionLoginResponse();
            validationResult.TenantId = userDto.TenantId;
            validationResult.TenancyName = userDto.TenancyName;
            validationResult.UserId = userDto.Id;
            validationResult.UserName = request.UserName;
            validationResult.FirstName = userDto.Name;
            validationResult.LastName = userDto.LastName;
            validationResult.SecondLastName = userDto.SecondLastName;
            validationResult.EMail = userDto.EmailAddress;

            // Se actualiza la información de último login
            using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    await _userManager.UpdateLoginInfo(userDto.Id, 0);
                }
            }

            return validationResult;
        }

        private async Task<UserDto> GetUserDto(string userName, string tenancyName)
		{
			UserDto userDto = null;

			using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
			{
				using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
				{
					userDto = await _userManager.GetUserByUserLoginAndTenancyNameAsync(userName, tenancyName);
				}
			}

			if (userDto == null)
			{
				throw new WrongUserNameOrPasswordException(_appLocalizationProvider.L("Login.FailLoginMessage"));
			}

			if (userDto.IsActive != true)
			{
				throw new WrongUserNameOrPasswordException(_appLocalizationProvider.L("Login.FailLoginMessage"));
			}

			return userDto;
		}

		private async Task UpdateAccesFailedCount(long userId, int accesFailedCount)
		{
			using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
			{
				using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
				{
					await _userManager.UpdateLoginInfo(userId, accesFailedCount);
					_userManager.CurrentUnitOfWork.Commit();
				}
			}
		}

		private async Task UpdateUserLocked(UserDto userDto, int tiempoATranscurrir, string requestUserName)
		{
			// Revisar que haya transcurrido el tiempo configurado
			if (Clock.Now.AddSeconds(-1 * tiempoATranscurrir) >= userDto.LastLoginTime)
			{
				// Se actualiza la información de último login
				using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
				{
					using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
					{
						await _userManager.UpdateLoginInfo(userDto.Id, 0);
						await _userManager.UnlockUserAsync(userDto.Id, false);

						userDto.UserLocked = false;
						userDto.AccesFailedCount = 0;
					}
				}
			}
			else
			{
				// Se actualiza la información de último login
				using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
				{
					using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
					{
						// SE actualiza la información de login sin modificar la cantidad de accesos fallidos, 
						// pero si actualiza la fecha del último intento
						await _userManager.UpdateLoginInfo(userDto.Id, userDto.AccesFailedCount);

						if (userDto.UserLocked != true)
						{
							await _userManager.LockUserAsync(userDto.Id);
						}
					}
				}

				_userManager.CurrentUnitOfWork.Commit();

				throw new UserLockedException(string.Format(_appLocalizationProvider.L("Login.UserLocked"), requestUserName));
			}
		}

		private async Task<SessionLoginResponse> DoValidationsOnPasswordSuccess(PasswordHasher<UserDto> p, 
            PasswordVerificationResult v, UserDto userDto, string requestUserName, 
            string requestPassword, bool activarVigenciaContraseña, int diasVigenciaContrasela)
		{
			SessionLoginResponse validationResult = null;
			
			//Se verifica la bandera de cambio de password hasta que el usuario se autenticó
			if (userDto.ChangePassword == true)
			{
				throw new UserMustChangePasswordException(_appLocalizationProvider.L("Login.UserMustChangePassword", requestUserName));
			}

			// Verificar si tiene activado el periodo de vigencia de la contraseña y si ese periodo no ha transcurrido
			if (activarVigenciaContraseña)
			{
				using (_userManager.CurrentUnitOfWork.SetTenantId(userDto.TenantId))
				{
					var pList = await _userManager.GetPasswordHistoryList(userDto.Id);
					var mFecha = pList.Max(m => m.Date);

					if (mFecha.HasValue && Clock.Now.Date.AddDays(-1 * diasVigenciaContrasela) >= mFecha.Value.Date)
					{
						// Ya caducó la contraseña de usuario.
						// Debe cambiarla.
						throw new UserMustChangePasswordException(_appLocalizationProvider.L("Login.PasswordExpired", requestUserName));
					}
				}
			}

			if (v == PasswordVerificationResult.SuccessRehashNeeded)
			{
				using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
				{
					using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
					{
						userDto.Password = p.HashPassword(userDto, requestPassword);
						await _userManager.UpdateUserAsync(userDto, null);
					}
				}
			}

			//Genera información necesaria para la sesión
			validationResult = new SessionLoginResponse();
			validationResult.TenantId = userDto.TenantId;
			validationResult.TenancyName = userDto.TenancyName;
			validationResult.UserId = userDto.Id;
			validationResult.UserName = requestUserName;
			validationResult.FirstName = userDto.Name;
			validationResult.LastName = userDto.LastName;
			validationResult.SecondLastName = userDto.SecondLastName;
			validationResult.EMail = userDto.EmailAddress;

			// Se actualiza la información de último login
			using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
			{
				using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
				{
					await _userManager.UpdateLoginInfo(userDto.Id, 0);
				}
			}

			return validationResult;
		}
	}
}
