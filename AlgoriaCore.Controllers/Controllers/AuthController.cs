using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._1Model;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._3Commands;
using AlgoriaCore.Application.QueriesAndCommands.Users._3Commands;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.Session;
using AlgoriaCore.WebUI.Controllers.Model;
using AlgoriaCore.WebUI.Filters;
using AlgoriaCore.WebUI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly AppSettings _appSettings;
        private readonly IAppSession _sessionContext;

        public AuthController(IOptions<AppSettings> appSettings, IAppSession sessionContext)
        {
            _appSettings = appSettings.Value;
            _sessionContext = sessionContext;
        }

        // GET api/values
        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<SessionLoginResponseController> Login([FromBody]UserLoginQuery query)
        {
            var LoginResult = await Mediator.Send(query);
            var tokenString = createToken(LoginResult);

            var LoginResultController = new SessionLoginResponseController
            {
                TenantId = LoginResult.TenantId,
                TenancyName = LoginResult.TenancyName,
                UserId = LoginResult.UserId,
                UserName = LoginResult.UserName,
                FirstName = LoginResult.FirstName,
                LastName = LoginResult.LastName,
                SecondLastName = LoginResult.SecondLastName,
                EMail = LoginResult.EMail,
                IsImpersonalized = LoginResult.IsImpersonalized,
                ImpersonalizerUserId = LoginResult.ImpersonalizerUserId,
                token = tokenString
            };

            return LoginResultController;
        }

        [AlgoriaCoreAuthorizationFilter]
        [HttpPost, Route("loginbytoken")]
        public async Task<SessionLoginResponseController> LoginByToken()
        {
            var LoginResult = await Mediator.Send(new UserLoginTokenQuery { TenantId = _sessionContext.TenantId, UserId = _sessionContext.UserId.Value });
            var tokenString = Request.Headers.Authorization[0].Replace("Bearer ", "");

            var LoginResultController = new SessionLoginResponseController
            {
                TenantId = LoginResult.TenantId,
                TenancyName = LoginResult.TenancyName,
                UserId = LoginResult.UserId,
                UserName = LoginResult.UserName,
                FirstName = LoginResult.FirstName,
                LastName = LoginResult.LastName,
                SecondLastName = LoginResult.SecondLastName,
                EMail = LoginResult.EMail,
                IsImpersonalized = LoginResult.IsImpersonalized,
                ImpersonalizerUserId = LoginResult.ImpersonalizerUserId,
                token = tokenString
            };

            return LoginResultController;
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Tenants_Impersonation)]
        [HttpPost, Route("impersonalizetenant")]
        public async Task<SessionLoginResponseController> ImpersonalizeTenant([FromBody]UserImpersonalizeQuery query)
        {
            var impersonalizeResult = await Mediator.Send(query);
            var tokenString = createToken(impersonalizeResult);

            var impersonalizeResultController = new SessionLoginResponseController
            {
                TenantId = impersonalizeResult.TenantId,
                TenancyName = impersonalizeResult.TenancyName,
                UserId = impersonalizeResult.UserId,
                UserName = impersonalizeResult.UserName,
                FirstName = impersonalizeResult.FirstName,
                LastName = impersonalizeResult.LastName,
                SecondLastName = impersonalizeResult.SecondLastName,
                EMail = impersonalizeResult.EMail,
                IsImpersonalized = impersonalizeResult.IsImpersonalized,
                ImpersonalizerUserId = impersonalizeResult.ImpersonalizerUserId,
                token = tokenString
            };

            return impersonalizeResultController;
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Users_Impersonation)]
        [HttpPost, Route("impersonalizeuser")]
        public async Task<SessionLoginResponseController> ImpersonalizeUser(long user)
        {
            var impersonalizeResult = await Mediator.Send(new UserImpersonalizeQuery() { User = user, Tenant = _sessionContext.TenantId });
            var tokenString = createToken(impersonalizeResult);

            var impersonalizeResultController = new SessionLoginResponseController
            {
                TenantId = impersonalizeResult.TenantId,
                TenancyName = impersonalizeResult.TenancyName,
                UserId = impersonalizeResult.UserId,
                UserName = impersonalizeResult.UserName,
                FirstName = impersonalizeResult.FirstName,
                LastName = impersonalizeResult.LastName,
                SecondLastName = impersonalizeResult.SecondLastName,
                EMail = impersonalizeResult.EMail,
                IsImpersonalized = impersonalizeResult.IsImpersonalized,
                ImpersonalizerUserId = impersonalizeResult.ImpersonalizerUserId,
                token = tokenString
            };

            return impersonalizeResultController;
        }

        [AllowAnonymous]
        [HttpPost, Route("changep")]
        public async Task<long> ChangePassword(UserChangePasswordCommand dto)
        {
            var r = await Mediator.Send(dto);
            return r;
        }

        [AllowAnonymous]
        [HttpPost, Route("resetp")]
        public async Task<string> ResetPassword(UserResetPasswordCommand dto)
        {
            var r = await Mediator.Send(dto);
            return r;
        }

        [AllowAnonymous]
        [HttpPost, Route("confirmresetp")]
        public async Task<long> ConfirmResetPassword(ConfirmPasswordCommandReset dto)
        {
            var r = await Mediator.Send(dto);
            return r;
        }

        private string createToken(SessionLoginResponse LoginResult)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.IssuerSigningKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                {
                    new Claim("TenantId", LoginResult.TenantId == null ? "" : LoginResult.TenantId.ToString()),
                    new Claim("TenancyName", LoginResult.TenancyName == null ? "" : LoginResult.TenancyName),
                    new Claim("UserId", LoginResult.UserId.ToString()),
                    new Claim(ClaimTypes.Name, LoginResult.UserName),
                    new Claim("IsImpersonalized", LoginResult.IsImpersonalized.ToString()),
                    new Claim("ImpersonalizerUserId", LoginResult.ImpersonalizerUserId.ToString())
                };

            var tokeOptions = new JwtSecurityToken(
                issuer: _appSettings.ValidIssuer,
                audience: _appSettings.ValidAudience,
                claims: claims, 
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_appSettings.TokenExpires),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }
    }
}
