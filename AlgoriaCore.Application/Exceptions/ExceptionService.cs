using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.Exceptions;

namespace AlgoriaCore.Application.Exceptions
{
    public class ExceptionService: IExceptionService
    {
        private readonly IAppLocalizationProvider _appLocalizationProvider;

        public ExceptionService(IAppLocalizationProvider appLocalizationProvider)
        {
            _appLocalizationProvider = appLocalizationProvider;
        }

        public void ThrowTenantRegistrationDuplicatedTenancyName(string tenancyName)
        {
            throw new TenantRegistrationDuplicatedTenancyNameException(string.Format(L("Register.Tenant.TenantDuplicatedTenancyName"), tenancyName));
        }

        public void ThrowNoValidUserException(string userName)
        {
            throw new NoValidUserException(string.Format(L("Users.NotValidExceptionMessage"), userName));
        }

        public string L(string key)
        {
            return _appLocalizationProvider.L(key);
        }
    }
}
