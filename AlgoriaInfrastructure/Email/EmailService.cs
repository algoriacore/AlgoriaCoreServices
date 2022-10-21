using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.Email;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using Autofac;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AlgoriaInfrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly ILifetimeScope _lifeTimeScope;

        public EmailService(ILifetimeScope lifeTimeScope)
        {
            _lifeTimeScope = lifeTimeScope;
        }

        public void Send(IEmailMessage message)
        {
            var mX = new MimeMessage();
            mX.To.AddRange(message.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            mX.Cc.AddRange(message.CcAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            mX.Bcc.AddRange(message.BccAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            var _emailConfiguration = GetEmailConfiguration(true);

            mX.From.Add(new MailboxAddress(_emailConfiguration.SmtpSenderName, _emailConfiguration.SmtpSender));
            mX.Subject = message.Subject;

            var builder = new BodyBuilder();

            string newText;
            var lEnts = ParseImageInContent(message.Content, out newText);

            foreach (var l in lEnts)
            {
                builder.LinkedResources.Add(l);
            }

            builder.HtmlBody = newText;

            // Diremos que estmos enviando HTML. Pero hay opciones para texto plano, etc.
            mX.Body = builder.ToMessageBody();

            // Ten cuidado que la clase SmtpClient sea la del Mailkit y no la del framework!
            using (var emailClient = new SmtpClient())
            {
                // El último parámetro aquí es para usar SSL (como deberías!)
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, 
                    _emailConfiguration.UseSSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);

                // Eliminar cualquier funcionalidad OAuth porque no la usaremos
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                if (!_emailConfiguration.UseDefaultCredentials)
                {
                    emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                }

                emailClient.Send(mX);

                emailClient.Disconnect(true);
            }
        }

        private List<MimeEntity> ParseImageInContent(string textContent, out string newContent)
        {
            List<MimeEntity> entidades = new List<MimeEntity>();

            StringBuilder sB = new StringBuilder(textContent);
            var matches = Regex.Matches(sB.ToString(), @"data:[\w\d\/;,+=?¿]{0,}");
            foreach (var m in matches)
            {
                string s = m.ToString().Replace("data:", "");

                var contentType = s.Substring(0, s.IndexOf(";"));
                var name = Guid.NewGuid().ToString().Replace("-", "");
                var extension = contentType.Substring(contentType.IndexOf('/') + 1);
                var fileName = name + "." + extension;

                s = s.Replace(contentType + ";", "").Replace("base64,", "");
                byte[] fileArray = Convert.FromBase64String(s);

                var att = new MimePart(contentType.Split("/")[0], contentType.Split("/")[1]);
                att.Content = new MimeContent(new MemoryStream(fileArray));
                att.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);
                att.ContentTransferEncoding = ContentEncoding.Base64;
                att.FileName = fileName;
                att.ContentId = MimeUtils.GenerateMessageId();

                entidades.Add(att);

                sB = sB.Replace(m.ToString(), string.Format("cid:{0}", att.ContentId));
            }

            newContent = sB.ToString();
            return entidades;
        }

        private EmailServiceConfiguration GetEmailConfiguration(bool throwExceptionIfNotConfigured = false)
        {
            var setting = _lifeTimeScope.Resolve<SettingManager>();

            var smtpServer = setting.GetSettingValueOrHostSettingValue(AppSettings.Mail.Smtp.Host);
            var smtpPort = setting.GetSettingValueOrHostSettingValue(AppSettings.Mail.Smtp.Port);
            var smtpSender = setting.GetSettingValueOrHostSettingValue(AppSettings.Mail.Smtp.SenderDefault);
            var smtpSenderName = setting.GetSettingValueOrHostSettingValue(AppSettings.Mail.Smtp.SenderDefaultDisplayName);
            var useSSL = setting.GetSettingValueOrHostSettingValue(AppSettings.Mail.EnableSSL);

            if (throwExceptionIfNotConfigured && (smtpServer.IsNullOrWhiteSpace() ||
                smtpPort.IsNullOrWhiteSpace() || smtpSender.IsNullOrWhiteSpace() || smtpSenderName.IsNullOrWhiteSpace()))
            {
                throw new AlgoriaCoreGeneralException(setting.L("Settings.Mail.NotConfiguredMessage"));
            }

            EmailServiceConfiguration conf = new EmailServiceConfiguration();

            if (smtpServer != null) conf.SmtpServer = smtpServer;
            if (smtpPort != null) conf.SmtpPort = Int32.Parse(smtpPort);
            if (smtpSender != null) conf.SmtpSender = smtpSender;
            if (smtpSenderName != null) conf.SmtpSenderName = smtpSenderName;

            var smtpUseDefaultCredentials = setting.GetSettingValueOrHostSettingValue(AppSettings.Mail.UseDefaultCredentials);
            conf.UseDefaultCredentials = !smtpUseDefaultCredentials.IsNullOrWhiteSpace() && smtpUseDefaultCredentials.ToLower() == "true";

            if (!conf.UseDefaultCredentials)
            {
                var smtpUserName = setting.GetSettingValueOrHostSettingValue(AppSettings.Mail.UserName);
                var smtpPassword = setting.GetSettingValueOrHostSettingValue(AppSettings.Mail.UserPassword);

                if (smtpUserName != null) conf.SmtpUsername = smtpUserName;
                if (smtpPassword != null) conf.SmtpPassword = smtpPassword;
            }

            if (useSSL != null) conf.UseSSL = useSSL.ToLower() == "true";

            return conf;
        }

        private sealed class EmailServiceConfiguration
        {
            public string SmtpServer { get; set; }
            public int SmtpPort { get; set; }
            public string SmtpSender { get; set; }
            public string SmtpSenderName { get; set; }
            public bool UseDefaultCredentials { get; set; }
            public string SmtpUsername { get; set; }
            public string SmtpPassword { get; set; }
            public bool UseSSL { get; set; }
        }
    }
}
