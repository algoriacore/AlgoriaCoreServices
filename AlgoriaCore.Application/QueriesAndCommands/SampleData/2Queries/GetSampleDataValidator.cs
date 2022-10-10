using AlgoriaCore.Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;

namespace AlgoriaCore.Application.QueriesAndCommands.SampleData
{
    public class GetSampleDataValidator : AbstractValidator<GetSampleDataQuery>
    {
        private readonly ILogger _logger;
        private readonly ICoreServices _coreServices;

        public GetSampleDataValidator(ILogger<GetSampleDataValidator> logger, ICoreServices coreServices)
        {
            
            _logger = logger;
            _coreServices = coreServices;

            RuleFor(x => x.Id).Must(HaveValidID).WithMessage("El ID '12345' No es válido");
            RuleFor(x => x.Id).Length(5).WithMessage("El tamaño no es el correcto").NotEmpty().WithMessage("No debe estar vacío");

        }

        private bool HaveValidID(GetSampleDataQuery model, String Id)
        {
            _logger.LogWarning("LOG VALIDANDO: {Id} user {UserName}", model.Id, _coreServices.sessionContext.UserName);

            if (Id.Equals("12345"))
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
    }
}
