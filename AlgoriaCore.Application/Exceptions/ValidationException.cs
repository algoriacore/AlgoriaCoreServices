using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Exceptions.Abstracts;
using AlgoriaCore.Domain.Session;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AlgoriaCore.Application.Exceptions
{
    [Serializable]
    public class ValidationException : AlgoriaErrorException
    {
        public ValidationException(IAppLocalizationProvider appLocalizationProvider)
            : base(AlgoriaCoreExceptionErrorCodes.GeneralValidationException, appLocalizationProvider.L("ExceptionFilter.ValidationException.Title"))
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(List<ValidationFailure> failures, IAppSession session, IAppLocalizationProvider appLocalizationProvider)
            : this(appLocalizationProvider)
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

		// Sin este constructor, la serialización falla
		protected ValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public IDictionary<string, string[]> Failures { get; }
    }
}
