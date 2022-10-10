using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries
{
    public class UserImpersonalizeValidator : AbstractValidator<UserImpersonalizeQuery>
    {
        private readonly ICoreServices _coreServices;

        public UserImpersonalizeValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");

            string labelUserName = L("Impersonalize.User");

            //Todas las reglas de validación del Login
            RuleFor(x => x.User).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelUserName, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
