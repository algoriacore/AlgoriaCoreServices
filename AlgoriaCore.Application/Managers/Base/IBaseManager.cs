using AlgoriaCore.Domain.Interfaces.Authorization;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Domain.Interfaces.Email;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;
using AlgoriaPersistence.Interfaces.Interfaces;

namespace AlgoriaCore.Application.Managers.Base
{
    public interface IBaseManager
    {
        IUnitOfWork CurrentUnitOfWork { get; set; }
        IClock Clock { get; set; }
        IAppSession SessionContext { get; set; }
        ICoreLogger CoreLogger { get; set; }
        IEmailService EmailService { get; set; }
        IPermissionProvider PermissionsProvider { get; set; }

        string L(string key);
    }
}
