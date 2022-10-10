namespace AlgoriaCore.Domain.Interfaces.Exceptions
{
    public interface IExceptionService
    {
        ///<exception cref = "TenantRegistrationDuplicatedTenancyName" ></exception>
        void ThrowTenantRegistrationDuplicatedTenancyName(string tenancyName);

        ///<exception cref = "NoValidUserException" ></exception>
        void ThrowNoValidUserException(string userName);
    }
}
