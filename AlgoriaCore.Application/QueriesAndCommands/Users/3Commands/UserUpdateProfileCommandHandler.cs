using AlgoriaCore.Application.BaseClases;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.BinaryObjects;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.SettingsClient;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.Managers.Users.Dto;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserUpdateProfileCommandHandler : BaseCoreClass, IRequestHandler<UserUpdateProfileCommand, long>
    {
        private readonly UserManager _userManager;
        private readonly BinaryObjectManager _binaryManager;
        private readonly SettingClientManager _managerSettingClient;
        private readonly SettingManager _managerSetting;

        public UserUpdateProfileCommandHandler(ICoreServices coreServices, 
            UserManager userManager,
            BinaryObjectManager binaryManager,
            SettingClientManager managerSettingClient,
            SettingManager managerSetting
        ) : base(coreServices)
        {
            _userManager = userManager;
            _binaryManager = binaryManager;
            _managerSettingClient = managerSettingClient;
            _managerSetting = managerSetting;
        }

        public async Task<long> Handle(UserUpdateProfileCommand request, CancellationToken cancellationToken)
        {
            UserDto uDto = new UserDto();
            uDto.Name = request.Name;
            uDto.LastName = request.LastName;
            uDto.SecondLastName = request.SecondLastName;
            uDto.EmailAddress = request.EmailAddress;
            uDto.PhoneNumber = request.PhoneNumber;

            if (request.NewPassword != null && request.NewPassword.Trim() != string.Empty)
            {
                // Validar la complejidad de la contraseña
                await _userManager.ValidatePasswordComplexityAsync(request.NewPassword);

                UserDto currentSavedUser = await _userManager.GetUserById(SessionContext.UserId.Value);
                PasswordHasher<UserDto> p = new PasswordHasher<UserDto>();
                var v = p.VerifyHashedPassword(currentSavedUser, currentSavedUser.Password, request.CurrentPassword);

                if (!(v == PasswordVerificationResult.Success || v == PasswordVerificationResult.SuccessRehashNeeded))
                {
                    throw new AlgoriaCoreGeneralException("La contraseña actual es incorrecta");
                }

                uDto.Password = request.NewPassword;
            }

            if (request.PictureName != null && request.PictureName.Trim() != null)
            {
                var file = await _binaryManager.GetTempFileAsync(request.PictureName);
                if (file != null && file.FileArray != null)
                {
                    var guid = await _binaryManager.CreateAsync(file.FileArray);
                    uDto.ProfilePictureId = guid;
                }
            }

            var resp = await _userManager.UpdateUserProfileAsync(uDto);

            _managerSetting.ChangeSettingByUser(AppSettings.General.LanguageDefault, request.Language?.ToString());

            if (!request.ClientType.IsNullOrWhiteSpace() && SessionContext.UserId.HasValue)
            {
                await _managerSettingClient.ChangeSettingClient(request.ClientType, SessionContext.UserId.Value, request.Preferences);
            }

            return resp;
        }
    }
}
