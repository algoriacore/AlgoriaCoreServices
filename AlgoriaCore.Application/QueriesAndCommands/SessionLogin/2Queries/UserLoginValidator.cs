using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries
{
    public class UserLoginValidator : AbstractValidator<UserLoginQuery>
    {
        private readonly ICoreServices _coreServices;

        public UserLoginValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMinLength = L("FieldMinLength");
            
            string labelUserName = L("Login.UserName");
            string labelPassword = L("Login.Password");

            //Todas las reglas de validación del Login
            RuleFor(x => x.UserName).NotEmpty().WithMessage(string.Format(labelRequiredField, labelUserName));
            RuleFor(x => x.Password).MinimumLength(5).WithMessage(string.Format(labelMinLength, labelPassword, 5))
            .NotEmpty().WithMessage(string.Format(labelRequiredField, labelPassword));

        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
