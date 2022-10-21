using AlgoriaCore.Application.Configuration;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Domain.Interfaces.Email;
using AlgoriaCore.Domain.Interfaces.Token;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using AlgoriaCore.Extensions.Utils;
using AlgoriaCore.Grpc;
using AlgoriaCore.Grpc.Helpers;
using Autofac;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace AlgoriaInfrastructure.Email
{
    public class EmailGrpcService : IEmailService
    {
        private readonly ILifetimeScope _lifeTimeScope;
        private readonly ITokenService _tokenService;
        private readonly EmailOptions _emailOptions;
        private SettingManager _managerSetting;
        private readonly EmailGrpc.EmailGrpcClient client;

        public EmailGrpcService(
            ILifetimeScope lifeTimeScope,
            ITokenService tokenService,
            IOptions<EmailOptions> emailOptions
        )
        {
            _lifeTimeScope = lifeTimeScope;
            _tokenService = tokenService;
            _emailOptions = emailOptions.Value;

            GrpcChannel channel = GrpcHelper.GetGrpcChannel(
                new Uri(emailOptions.Value.Grpc.Url),
                new Version(emailOptions.Value.Grpc.HttpVersion.Major, emailOptions.Value.Grpc.HttpVersion.Minor)
            );
            client = new EmailGrpc.EmailGrpcClient(channel);
        }

        public void Send(IEmailMessage message)
        {
            Metadata headers = new Metadata();
            headers.Add("Authorization", string.Format("Bearer {0}", AsyncUtil.RunSync(() => _tokenService.GetToken())));

            EmailGrpcRequest request = new EmailGrpcRequest();

            request.ToAddresses.AddRange(message.ToAddresses.Select(p => new EmailGrpcAddressRequest { Address = p.Address, Name = p.Name == null ? "" : p.Name }));
            request.CcAddresses.AddRange(message.CcAddresses.Select(p => new EmailGrpcAddressRequest { Address = p.Address, Name = p.Name == null ? "" : p.Name }));
            request.Subject = message.Subject;
            request.Content = message.Content;

            foreach(IEmailAttachment attachment in message.Attachments)
            {
                request.Attachments.Add(new EmailGrpcAttachmentRequest
                {
                    ContentType = attachment.ContentType,
                    FileArray = ByteString.CopyFrom(attachment.FileArray),
                    FileName = attachment.FileName
                });
            }

            request.HasOwnConfig = _emailOptions.Grpc.SendConfiguration;

            if (_emailOptions.Grpc.SendConfiguration)
            {
                request.Config = GetEmailConfiguration();
            }

            try
            {
                client.Send(request, new CallOptions(headers));
            } catch(RpcException ex)
            {
                if (ex.StatusCode == StatusCode.Unauthenticated)
                {
                    headers = new Metadata();
                    headers.Add("Authorization", string.Format("Bearer {0}", AsyncUtil.RunSync(() => _tokenService.GetToken(true))));

                    client.Send(request, new CallOptions(headers));
                } else
                {
                    throw;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                headers = new Metadata();
                headers.Add("Authorization", string.Format("Bearer {0}", AsyncUtil.RunSync(() => _tokenService.GetToken(true))));

                client.Send(request, new CallOptions(headers));
            }
        }

        private EmailGrpcConfigRequest GetEmailConfiguration()
        {
            GetSettingManager();

            var smtpServer = _managerSetting.GetSettingValueOrHostSettingValue(AppSettings.Mail.Smtp.Host);
            var smtpPort = _managerSetting.GetSettingValueOrHostSettingValue(AppSettings.Mail.Smtp.Port);
            var smtpSender = _managerSetting.GetSettingValueOrHostSettingValue(AppSettings.Mail.Smtp.SenderDefault);
            var smtpSenderName = _managerSetting.GetSettingValueOrHostSettingValue(AppSettings.Mail.Smtp.SenderDefaultDisplayName);

            var useSSL = _managerSetting.GetSettingValueOrHostSettingValue(AppSettings.Mail.EnableSSL);

            EmailGrpcConfigRequest conf = new EmailGrpcConfigRequest();

            if (smtpServer != null) conf.SmtpHost = smtpServer;
            if (smtpPort != null) conf.SmtpPort = Int32.Parse(smtpPort);
            if (smtpSender != null) conf.Sender = smtpSender;
            if (smtpSenderName != null) conf.SenderDisplay = smtpSenderName;

            var smtpUseDefaultCredentials = _managerSetting.GetSettingValueOrHostSettingValue(AppSettings.Mail.UseDefaultCredentials);
            conf.UseDefaultCredential = !smtpUseDefaultCredentials.IsNullOrWhiteSpace() && smtpUseDefaultCredentials.ToLower() == "true";

            if (!conf.UseDefaultCredential)
            {
                var smtpUserName = _managerSetting.GetSettingValueOrHostSettingValue(AppSettings.Mail.UserName);
                var smtpPassword = _managerSetting.GetSettingValueOrHostSettingValue(AppSettings.Mail.UserPassword);

                if (smtpUserName != null) conf.MailUser = smtpUserName;
                if (smtpPassword != null) conf.MailPassword = smtpPassword;
            }

            if (useSSL != null) conf.IsSsl = useSSL.ToLower() == "true";

            return conf;
        }

        private void GetSettingManager()
        {
            if (_managerSetting == null)
            {
                _managerSetting = _lifeTimeScope.Resolve<SettingManager>();
            }
        }
    }
}
