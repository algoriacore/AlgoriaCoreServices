﻿using AlgoriaCore.Application.Interfaces;
using FluentValidation;

namespace AlgoriaCore.Application.QueriesAndCommands.Settings.Host
{
    public class HostSettingsSendTestEmailCommandValidator : AbstractValidator<HostSettingsSendTestEmailCommand>
    {
        private readonly ICoreServices _coreServices;

        public HostSettingsSendTestEmailCommandValidator(ICoreServices coreServices)
        {
            _coreServices = coreServices;

            string labelRequiredField = L("RequiredField");
            string labelMaxLength = L("FieldMaxLength");
            string labelInvalidMailAddress = L("FieldInvalidEmailAddress");

            string labelTrySentTo = L("Host.Settings.Mail.TrySentTo");

            RuleFor(x => x.EmailAddress).NotEmpty().WithMessage(string.Format(labelRequiredField, labelTrySentTo))
            .MaximumLength(256).WithMessage(string.Format(labelMaxLength, labelTrySentTo, 256))
            .EmailAddress().WithMessage(string.Format(labelInvalidMailAddress, labelTrySentTo));
        }

        private string L(string name)
        {
            return _coreServices.AppLocalizationProvider.L(name);
        }
    }
}
