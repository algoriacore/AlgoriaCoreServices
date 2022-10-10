using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoriaCore.WebUI.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly IAppLocalizationProvider _L;

        private string L(string key)
        {
            return _L.L(key);
        }

        private readonly ICoreLogger _coreLogger;
        private readonly IAppSession _session;

        public ValidateModelAttribute(ICoreLogger coreLogger, IAppSession session, IAppLocalizationProvider L)
        {
            _coreLogger = coreLogger;
            _session = session;
            _L = L;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var invalidFields = context.ModelState.Where(p => p.Value.ValidationState == ModelValidationState.Invalid);

                StringBuilder builder = new StringBuilder();
                foreach (var invalidField in invalidFields)
                {
                    builder.Append(string.Format(L("InvalidField"), invalidField.Key)).Append("\r\n");
                }

                AlgoriaCoreGeneralException ex = new AlgoriaCoreGeneralException(builder.ToString());

                ControllerActionDescriptor controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var dict = new Dictionary<string, string>();
                dict.Add("ServiceName", controllerActionDescriptor.ControllerTypeInfo.FullName);
                dict.Add("MethodName", controllerActionDescriptor.ActionName);
                dict.Add("Parameters", JsonConvert.SerializeObject(
                    context.ModelState.Where(p => p.Value.ValidationState == ModelValidationState.Invalid)
                    .ToDictionary(p => p.Key, p => p.Value.AttemptedValue)));
                dict["Exception"] = ex.ToString();
                dict.Add("Severity", "4");

                _coreLogger.LogError(ex, string.Format("LOG: ERROR: {0} user {1}", 
                    controllerActionDescriptor.ActionName, _session.UserName), dict);
                throw ex;
            }
        }
    }
}
