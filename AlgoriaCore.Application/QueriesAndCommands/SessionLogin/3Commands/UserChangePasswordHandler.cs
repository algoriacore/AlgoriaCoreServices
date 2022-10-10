using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._3Commands;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.Exceptions;
using AlgoriaPersistence.Interfaces.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserChangePasswordHandler : BaseCoreClass, IRequestHandler<UserChangePasswordCommand, long>
    {
        private readonly UserManager _userManager;
        private readonly IExceptionService _exceptionService;

        public UserChangePasswordHandler(ICoreServices coreServices
        , UserManager userManager,
        IExceptionService exceptionService) : base(coreServices)
        {
            _userManager = userManager;
            _exceptionService = exceptionService;
        }

        public async Task<long> Handle(UserChangePasswordCommand request, CancellationToken cancellationToken)
        {
            long resp = 0;
            UserDto userDto = null;

            using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    userDto = await _userManager.GetUserByUserLoginAndTenancyNameAsync(request.UserName, request.TenancyName);
                }
            }

            if (userDto == null || userDto.IsActive != true)
            {
                _exceptionService.ThrowNoValidUserException(request.UserName);
            }
            else
            {

                //Verificar la contraseña
                PasswordHasher<UserDto> p = new PasswordHasher<UserDto>();
                var v = p.VerifyHashedPassword(userDto, userDto.Password, request.CurrentPassword);

                if (v == PasswordVerificationResult.Success || v == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    // Validar la complejidad del nuevo password
                    using (var s = _userManager.CurrentUnitOfWork.SetTenantId(userDto.TenantId))
                    {
                        await _userManager.ValidatePasswordComplexityAsync(request.NewPassword);
                    }

                    resp = await _userManager.ChangePasswordAsync(userDto.Id, request.NewPassword);

                    // Guardar el historial de contraseñas
                    await _userManager.SavePasswordHistoryAsync(userDto.Id, userDto.Password);
                }
                else
                {
                    _exceptionService.ThrowNoValidUserException(request.UserName);
                }
            }

            return resp;
        }
    }
}
