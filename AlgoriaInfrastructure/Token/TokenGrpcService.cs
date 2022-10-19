using AlgoriaCore.Application.Configuration;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Application.Managers.Settings;
using AlgoriaCore.Application.Managers.Settings.Dto;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Interfaces.Token;
using AlgoriaCore.Domain.Settings;
using AlgoriaCore.Extensions;
using AlgoriaCore.Grpc;
using AlgoriaCore.Grpc.Helpers;
using AlgoriaPersistence.Interfaces.Interfaces;
using Autofac;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AlgoriaInfrastructure.Email
{
    public class TokenGrpcService : ITokenService
    {
        private readonly ILifetimeScope _lifeTimeScope;
        private SettingManager _managerSetting;
        public SecurityGrpc.SecurityGrpcClient client { get; set; }
        private readonly EmailOptions _emailOptions;
        private IUnitOfWork _currentUnitOfWork { get; set; }
        private IAppLocalizationProvider _appLocalizationProvider { get; set; }

        public TokenGrpcService(
            ILifetimeScope lifeTimeScope,
            IOptions<EmailOptions> emailOptions,
            IUnitOfWork currentUnitOfWork,
            IAppLocalizationProvider appLocalizationProvider)
        {
            _lifeTimeScope = lifeTimeScope;
            _emailOptions = emailOptions.Value;
            _currentUnitOfWork = currentUnitOfWork;
            _appLocalizationProvider = appLocalizationProvider;

            GrpcChannel channel = GrpcHelper.GetGrpcChannel(
                new Uri(emailOptions.Value.Grpc.Url), 
                new Version(emailOptions.Value.Grpc.HttpVersion.Major, _emailOptions.Grpc.HttpVersion.Minor)
            );
            client = new SecurityGrpc.SecurityGrpcClient(channel);
        }

        public async Task<string> GetToken(bool force = false)
        {
            string token = null;

            if (force)
            {
                token = await GetTokenAux();
            } else
            {
                token = await GetSettingManager().GetSettingValueAsync(AppSettings.Mail.GrpcMail.Token);

                if (token.IsNullOrWhiteSpace())
                {
                    token = await GetTokenAux();
                }
            }
            
            return token;
        }

        private async Task<string> GetTokenAux()
        {
            GrpcEmailDto grpcEmailDto = GetGrpcEmailSettings();

            SessionTokenGrpcRequest requestSecurity = new SessionTokenGrpcRequest
            {
                TenancyName = grpcEmailDto.TenancyName,
                UserName = grpcEmailDto.UserName,
                Password = grpcEmailDto.Password
            };

            string token = (await client.GetSessionTokenAsync(requestSecurity)).Token;
            GetSettingManager().ChangeSetting(AppSettings.Mail.GrpcMail.Token, token);

            return token;
        }

        private SettingManager GetSettingManager()
        {
            if (_managerSetting == null)
            {
                _managerSetting = _lifeTimeScope.Resolve<SettingManager>();
            }

            return _managerSetting;
        }

        public GrpcEmailDto GetGrpcEmailSettings()
        {
            GrpcEmailDto dto = new GrpcEmailDto
            {
                SendConfiguration = _emailOptions.Grpc.SendConfiguration,
                TenancyName = GetSettingManager().GetSettingValueOrHostSettingValue(AppSettings.Mail.GrpcMail.TenancyName),
                UserName = GetSettingManager().GetSettingValueOrHostSettingValue(AppSettings.Mail.GrpcMail.GrpcUserName),
                Password = GetSettingManager().GetSettingValueOrHostSettingValue(AppSettings.Mail.GrpcMail.Password),
                Token = GetSettingManager().GetSettingValueOrHostSettingValue(AppSettings.Mail.GrpcMail.Token)
            };

            if (dto.TenancyName.IsNullOrWhiteSpace() || dto.UserName.IsNullOrWhiteSpace() || dto.Password.IsNullOrWhiteSpace())
            {
                throw new AlgoriaCoreGeneralException(L("Settings.Mail.NotConfiguredMessage"));
            }

            return dto;
        }

        public string L(string key)
        {
            return _appLocalizationProvider.L(key);
        }
    }
}
