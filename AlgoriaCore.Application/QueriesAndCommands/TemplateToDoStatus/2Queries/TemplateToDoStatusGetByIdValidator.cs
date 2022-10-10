﻿using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.TemplateToDoStatus
{
    public class TemplateToDoStatusGetByIdValidator : AbstractValidator<TemplateToDoStatusGetByIdQuery>
    {
        private readonly ICoreServices _coreServices;

        public TemplateToDoStatusGetByIdValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelGreaterThan = L("FieldGreaterThan");

            string labelId = L("Id");

            RuleFor(x => x.Id).GreaterThan(0).WithMessage(string.Format(labelGreaterThan, labelId, 0));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
