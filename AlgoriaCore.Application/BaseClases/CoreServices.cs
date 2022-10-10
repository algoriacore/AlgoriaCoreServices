using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;

namespace AlgoriaCore.Application.BaseClases
{
    public class CoreServices : ICoreServices
    {
        public IAppSession sessionContext { get; set; }
        public IAppLocalizationProvider AppLocalizationProvider { get; set; }
        public IClock Clock { get; set; }
        public ICoreLogger CoreLogger { get; set; }

        public CoreServices(IAppSession _sessionContext, 
            IAppLocalizationProvider _appLocalizationProvider, 
            IClock _clock,
            ICoreLogger _coreLogger)
        {
            sessionContext = _sessionContext;
            AppLocalizationProvider = _appLocalizationProvider;
            Clock = _clock;
            CoreLogger = _coreLogger;
        }

    }
}
