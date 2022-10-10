using AlgoriaCore.Application.Extensiones;
using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Application.Managers.ChangeLogs.Dto;
using AlgoriaCore.Application.Managers.Emails.Templates.Dto;
using AlgoriaCore.Domain.Email;
using AlgoriaCore.Domain.Interfaces.Authorization;
using AlgoriaCore.Domain.Interfaces.Date;
using AlgoriaCore.Domain.Interfaces.Email;
using AlgoriaCore.Domain.Interfaces.Logger;
using AlgoriaCore.Domain.Session;
using AlgoriaCore.Extensions;
using AlgoriaPersistence.Interfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.Managers.Base
{
    public abstract class BaseManager : IBaseManager
	{
		public IUnitOfWork CurrentUnitOfWork { get; set; }
		public IAppSession SessionContext { get; set; }
        public ICoreLogger CoreLogger { get; set; }
        public IClock Clock { get; set; }
		public IEmailService EmailService { get; set; }
		public IPermissionProvider PermissionsProvider { get; set; }
		public IAppLocalizationProvider AppLocalizationProvider { get; set; }
		public IChangeLogService ChangeLogService { get; set; }

        public string L(string key)
		{
			return AppLocalizationProvider.L(key);
		}

		public async Task<long> LogChange(ChangeLogType changeLogType, string key, string table, string detail)
		{
			return await ChangeLogService.LogChange(changeLogType, key, table, detail);
		}

		public void SendEmail(MailTemplateDto templateDto, EmailAddress sendTo, Dictionary<string, string> dict = null)
		{
			SendEmail(templateDto, new List<EmailAddress>() { sendTo }, dict);
		}

		public void SendEmail(MailTemplateDto templateDto, List<EmailAddress> emailList, Dictionary<string, string> dict = null)
		{
			EmailMessage message = new EmailMessage();
			emailList.ForEach(m => message.ToAddresses.Add(m));

            SetEmailMessageAddressesInList(message.ToAddresses, templateDto.SendTo);
            SetEmailMessageAddressesInList(message.CcAddresses, templateDto.CopyTo);
            SetEmailMessageAddressesInList(message.BccAddresses, templateDto.BlindCopyTo);

			var sB = new StringBuilder(string.Empty);
			sB.AppendLine(templateDto.Header ?? string.Empty);

			if (dict == null)
			{
				sB.AppendLine(templateDto.Body != null ? templateDto.Body : string.Empty);
			}
			else
			{
				sB.AppendLine(templateDto.Body != null ? templateDto.Body.BulkReplace(dict) : string.Empty);
			}

			sB.AppendLine(templateDto.Footer ?? string.Empty);

			message.Subject = templateDto.Subject ?? string.Empty;

			// Estraer las imágenes en "base64" que hay en el mensaje y enviarlas también como archivos adjuntos
			// para que puedan ser visualizadas en algunos proveedores de correo, como gmail
			// message.
			message.Content = sB.ToString();

			EmailService.Send(message);
		}

		#region Auxiliar methods for Logging

		protected void LogStringProperty(StringBuilder sb, string previousProp, string newProp, string labelEntry)
		{
			if (newProp != previousProp)
			{
				sb.AppendFormat("{0}: {1} => {2}\n", labelEntry, previousProp ?? string.Empty, newProp ?? string.Empty);
			}
		}

		protected void LogBoolProperty(StringBuilder sb, bool previousProp, bool newProp, string labelEntry)
		{
			if (newProp != previousProp)
			{
				sb.AppendFormat("{0}: {1} => {2}\n", labelEntry, previousProp ? "{{Yes}}" : "{{No}}", newProp ? "{{Yes}}" : "{{No}}");
			}
		}

        protected void LogIntProperty(StringBuilder sb, int? previousProp, int? newProp, string labelEntry, string format = "###,##0")
        {
            if (newProp != previousProp)
            {
                sb.AppendFormat("{0}: {1} => {2}\n", labelEntry
                        , previousProp == null ? string.Empty : previousProp.Value.ToString(format)
                        , newProp == null ? string.Empty : newProp.Value.ToString(format));
            }
        }

        protected void LogDecimalProperty(StringBuilder sb, decimal? previousProp, decimal? newProp, string labelEntry, string format = "###,##0.00")
        {
            if (newProp != previousProp)
            {
                sb.AppendFormat("{0}: {1} => {2}\n", labelEntry
                        , previousProp == null ? string.Empty : previousProp.Value.ToString(format)
                        , newProp == null ? string.Empty : newProp.Value.ToString(format));
            }
        }

        protected void LogDateTimeProperty(StringBuilder sb, DateTime? previousProp, DateTime? newProp, string labelEntry)
        {
            if (newProp != previousProp)
            {
                sb.AppendFormat("{0}: {1} => {2}\n", labelEntry
                    , previousProp == null ? string.Empty : previousProp.GetDateTimeForLogChange()
                    , newProp == null ? string.Empty : newProp.GetDateTimeForLogChange());
            }
        }

        protected void LogDateProperty(StringBuilder sb, DateTime? previousProp, DateTime? newProp, string labelEntry)
        {
            if (newProp != previousProp)
            {
                sb.AppendFormat("{0}: {1} => {2}\n", labelEntry
                    , previousProp == null ? string.Empty : previousProp.GetDateISOForLogChange()
                    , newProp == null ? string.Empty : newProp.GetDateISOForLogChange());
            }
        }

        protected void LogTimeProperty(StringBuilder sb, DateTime? previousProp, DateTime? newProp, string labelEntry)
        {
            if (newProp != previousProp)
            {
                sb.AppendFormat("{0}: {1} => {2}\n", labelEntry
                    , previousProp == null ? string.Empty : previousProp.GetTimeISOForLogChange()
                    , newProp == null ? string.Empty : newProp.GetTimeISOForLogChange());
            }
        }

        protected void LogTimeProperty(StringBuilder sb, TimeSpan? previousProp, TimeSpan? newProp, string labelEntry)
        {
            if (newProp != previousProp)
            {
                sb.AppendFormat("{0}: {1} => {2}\n", labelEntry
                    , previousProp == null ? string.Empty : previousProp.Value.ToString(@"hh\:mm")
                    , newProp == null ? string.Empty : newProp.Value.ToString(@"hh\:mm"));
            }
        }

        private void SetEmailMessageAddressesInList(List<IEmailAddress> list, string addressesStr)
        {
            if (!addressesStr.IsNullOrEmpty())
            {
                string addressStr3;

                foreach (string addressStr in addressesStr.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    foreach (string addressStr2 in addressStr.Split(";", StringSplitOptions.RemoveEmptyEntries))
                    {
                        addressStr3 = addressStr2.Trim();

                        if (!list.Exists(p => p.Address.Trim() == addressStr3))
                        {
                            list.Add(new EmailAddress { Address = addressStr3 });
                        }
                    }
                }
            }
        }

        #endregion
    }
}
