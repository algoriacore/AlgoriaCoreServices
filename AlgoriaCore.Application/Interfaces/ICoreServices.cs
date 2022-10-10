using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;

namespace AlgoriaCore.Application.Interfaces
{
    //Todos los servicios deben ser "Scoped"
    public interface ICoreServices
    {
        IAppSession sessionContext { get; set; }
        IAppLocalizationProvider AppLocalizationProvider { get; set; }
        IClock Clock { get; set; }
        ICoreLogger CoreLogger { get; set; }
    }
}
