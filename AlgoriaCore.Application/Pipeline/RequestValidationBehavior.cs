using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Session;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Pipeline
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IAppSession _session;
        private readonly IAppLocalizationProvider _appLocalizationProvider;

        public RequestValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            IAppSession session,
            IAppLocalizationProvider appLocalizationProvider)
        {
            _validators = validators;
            _session = session;
            _appLocalizationProvider = appLocalizationProvider;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);

            if(_validators.Any())
            {
                var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

                if (failures.Count != 0)
                {
                    throw new Exceptions.ValidationException(failures, _session, _appLocalizationProvider);
                }
            }

            return next();
        }
    }
}
