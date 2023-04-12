using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Templates;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Domain.Email;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using AlgoriaPersistence.Interfaces.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserCreateCommandHandler : BaseCoreClass, IRequestHandler<UserCreateCommand, long>
    {
        private readonly UserManager _userManager;
        private readonly ILogger _logger;
        private readonly RoleManager _roleManager;
        private readonly MailTemplateManager _mailTemplateManager;

        public UserCreateCommandHandler(ICoreServices coreServices,
            UserManager userManager,
            ILogger<UserCreateCommandHandler> logger,
            RoleManager roleManager,
            MailTemplateManager mailTemplateManager) : base(coreServices)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _mailTemplateManager = mailTemplateManager;
        }

        public async Task<long> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            // Validar que el correo no exista entre los usuarios del Tenant actual

            UserDto exist = null;

            // Se valida que no exista ya registrado esa dirección de correo
            exist = await _userManager.GetUserByEmailAsync(request.EmailAddress);
            if (exist != null)
            {
                throw new AlgoriaCoreGeneralException(L("Users.DuplicatedEmail"));
            }

            using (var uow = _userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.SoftDelete))
            {
                exist = await _userManager.GetUserByUserName(request.UserName);
            }

            if (exist != null)
            {
                if (exist.IsDeleted == true)
                {
                    throw new AlgoriaCoreGeneralException(L("Register.User.NotAvailableUserName", request.UserName));
                }
                else
                {
                    throw new AlgoriaCoreGeneralException(L("Register.User.DuplicatedUserName", request.UserName));
                }
            }

            var dto = new UserDto();

            dto.Login = request.UserName;

            if (request.SetRandomPassword == true)
            {
                request.Password = Guid.NewGuid().ToString("N").Truncate(16);
            }
            else if (!request.Password.IsNullOrEmpty())
            {
                await _userManager.ValidatePasswordComplexityAsync(request.Password);
            }


            PasswordHasher<UserDto> p = new PasswordHasher<UserDto>();
            dto.Password = p.HashPassword(dto, request.Password);

            dto.Name = request.Name;
            dto.LastName = request.LastName;
            dto.SecondLastName = request.SecondLastName;
            dto.EmailAddress = request.EmailAddress;
            dto.IsEmailConfirmed = false;
            dto.PhoneNumber = request.PhoneNumber;
            dto.IsPhoneNumberConfirmed = false;
            dto.ChangePassword = request.ShouldChangePasswordOnNextLogin;
            dto.AccesFailedCount = 0;
            dto.UserLocked = false;
            dto.IsLockoutEnabled = false;
            dto.IsActive = request.IsActive == true;
            dto.IsDeleted = false;

            // agregar roles
            request.AssignedRoleNames = request.AssignedRoleNames ?? new List<string>();
            var rolDtoList = await _roleManager.GetRolesFromNamesByValidating(request.AssignedRoleNames);

            dto.Id = await _userManager.CreateUserAsync(dto, rolDtoList);

            await _userManager.ReplaceRolesAsync(dto.Id, rolDtoList);

            // enviar correo de nuevo usuario
            try
            {
                var emailTemplateDto = await _mailTemplateManager.GetMailTemplateCurrentByMailKey(EmailKeys.NewUser);
                _userManager.SendNewUserEmailAsync(dto, request.Password, emailTemplateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LOG SendNewUserEmailAsync: User {UserId} - {UserName}", SessionContext.UserId, SessionContext.UserName);
            }

            return dto.Id;
        }
    }
}
