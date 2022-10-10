using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;
using System.Text;

namespace AlgoriaCore.Application.BaseClases
{
    public abstract class BaseCoreClass
    {
		private readonly ICoreServices _coreServices;

        protected BaseCoreClass(ICoreServices coreServices)
        {
            _coreServices = coreServices;
        }

        public IAppSession SessionContext => _coreServices.sessionContext;
        public IClock Clock => _coreServices.Clock;
        public ICoreLogger CoreLogger => _coreServices.CoreLogger;

        public string L(string key)
        {
            return _coreServices.AppLocalizationProvider.L(key);
        }

        public string L(string key, params string[] parameters)
        {
            StringBuilder sb = new StringBuilder(L(key) ?? string.Empty);
            var i = 0;
			foreach (var p in parameters)
			{
				string parsed = string.Format("{0}", i);

				sb.Replace("{" + parsed + "}", p);
				i++;
			}

            return sb.ToString();
        }
    }
}
