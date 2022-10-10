using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Domain.Interfaces.MultiTenancy;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._3Commands
{
    public class UserResetPasswordCommandValidator : AbstractValidator<UserResetPasswordCommand>
    {
        private readonly ICoreServices _coreServices;

        public UserResetPasswordCommandValidator(ICoreServices coreServices, IMultiTenancyConfig multiTenancyConfig)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");

            string labelTenancyName = L("ResetPasswrod.TenancyName");
            string labelUserName = L("ResetPasswrod.UserName");

            // Todas las reglas de validación del Login
            // RuleFor(x => x.TenancyName).NotEmpty().When(p => multiTenancyConfig.IsEnabled()).WithMessage(string.Format(labelRequiredField, labelTenancyName));
            RuleFor(x => x.UserName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelUserName));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
