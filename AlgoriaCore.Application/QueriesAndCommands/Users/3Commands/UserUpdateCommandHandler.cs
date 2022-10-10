using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Emails.Templates;
using AlgoriaCore.Application.Managers.Roles;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Domain.Email;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserUpdateCommandHandler : BaseCoreClass, IRequestHandler<UserUpdateCommand, long>
    {
        private readonly UserManager _userManager;
        private readonly ILogger _logger;
        private readonly RolManager _rolManager;
        private readonly MailTemplateManager _mailTemplateManager;

        public UserUpdateCommandHandler(ICoreServices coreServices,
            UserManager userManager,
            ILogger<UserUpdateCommandHandler> logger,
            RolManager rolManager,
            MailTemplateManager mailTemplateManager) : base(coreServices)
        {
            _userManager = userManager;
            _logger = logger;
            _rolManager = rolManager;
            _mailTemplateManager = mailTemplateManager;
        }

        public async Task<long> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var dto = await _userManager.GetUserById(request.Id);

            if (dto == null)
            {
                throw new AlgoriaCoreGeneralException(L("Users.NotFound"));
            }

            // Validar que el correo no exista entre los usuarios del Tenant actual
            if (request.EmailAddress != dto.EmailAddress)
            {
                // Se valida que no exista ya registrado esa dirección de correo
                var exist = await _userManager.GetUserByEmailAsync(request.EmailAddress);
                if (exist != null)
                {
                    throw new AlgoriaCoreGeneralException(L("Users.DuplicatedEmail"));
                }
            }

            dto.Login = request.UserName;

            if (request.SetRandomPassword == true)
            {
                request.Password = Guid.NewGuid().ToString("N").Truncate(16);
            }
            else if (!request.Password.IsNullOrEmpty())
            {
                await _userManager.ValidatePasswordComplexityAsync(request.Password);
            }

            if (!request.Password.IsNullOrEmpty())
            {
                // Se guarda el password anterior
                string oldPassword = dto.Password;

                await _userManager.ChangePasswordAsync(dto.Id, request.Password);
                var updatedDto = await _userManager.GetUserById(request.Id);
                dto.Password = updatedDto.Password;

                // Guardar el historial de contraseñas
                await _userManager.SavePasswordHistoryAsync(dto.Id, oldPassword);
            }

            dto.Name = request.Name;
            dto.LastName = request.LastName;
            dto.SecondLastName = request.SecondLastName;
            dto.EmailAddress = request.EmailAddress;
            dto.IsEmailConfirmed = false;
            dto.PhoneNumber = request.PhoneNumber;
            dto.IsPhoneNumberConfirmed = false;
            dto.ChangePassword = request.ShouldChangePasswordOnNextLogin;
            dto.UserLocked = false;
            dto.IsLockoutEnabled = false;
            dto.IsActive = request.IsActive == true;
            dto.IsDeleted = false;

            // agregar roles
            request.AssignedRoleNames = request.AssignedRoleNames ?? new List<string>();
            var rolDtoList = await _rolManager.GetRolesFromNamesByValidating(request.AssignedRoleNames);

            await _userManager.UpdateUserAsync(dto, rolDtoList);

            await _userManager.ReplaceRolesAsync(dto.Id, rolDtoList);

            // enviar correo de cambio de contraseña
            if (!request.Password.IsNullOrEmpty())
            {
                try
                {
                    var emailTemplateDto = await _mailTemplateManager.GetMailTemplateCurrentByMailKey(EmailKeys.UserModification);
                    _userManager.SendChangePasswordEmailAsync(dto, emailTemplateDto);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LOG SendChangePasswordEmailAsync: User {UserId} - {UserName}", SessionContext.UserId, SessionContext.UserName);
                }
            }

            return dto.Id;
        }
    }
}
