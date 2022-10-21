using AlgoriaCore.Application.Exceptions;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.Exceptions.Abstracts;
using AlgoriaCore.WebUI.Middleware;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.WebAPI.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppLocalizationProvider _L;

        public ExceptionHandlerMiddleware(RequestDelegate next, IAppLocalizationProvider L)
        {
            _next = next;
            _L = L;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext httpContext, Exception ex) {
            ex = ex.GetBaseException();

            var cE = ex as ValidationException;

            if (cE != null)
            {
                var failures = cE.Failures;

                StringBuilder builder = new StringBuilder();
                foreach (KeyValuePair<string, string[]> validations in failures)
                {
                    builder.Append(string.Join("\r\n", validations.Value)).Append("\r\n");
                }

                // Regresamos error 'UnprocessableEntity' solo para el caso de que existan errores de validación
                // https://www.keycdn.com/support/422-unprocessable-entity

                await WriteHttpContext(httpContext, ex,
                    _L.L("ExceptionFilter.ValidationException.Message") + "\r\n" + "\r\n" + builder.ToString(),
                    _L.L("ExceptionFilter.ValidationException.Title"),
                    (int)HttpStatusCode.UnprocessableEntity
                );

                return;
            }

            var cE1 = ex as NoValidUserException;

            if (cE1 != null) // Esta excepción ocurre únicamente durante el login de usuarios o cuando el usuario no tiene sesión
            {
                await WriteHttpContext(httpContext, ex,
                   ex.Message,
                   "",
                   (int)HttpStatusCode.Unauthorized
                );

                return;
            }

            var cE2 = ex as WrongUserNameOrPasswordException;
            var cE3 = ex as UserLockedException;

            if (cE2 != null || cE3 != null)
            {
                await WriteHttpContext(httpContext, ex,
                   ex.Message,
                   "",
                   (int)HttpStatusCode.Conflict
                );

                return;
            }

            var cE4 = ex as UserUnauthorizedException;

            if (cE4 != null) // Esta excepción ocurre cuando el usuario no tiene acceso a algún recurso
            {
                await WriteHttpContext(httpContext, ex,
                    string.Format(_L.L("Users.UnauthorizedExceptionMessage"), httpContext.User.Identity.Name),
                    _L.L("ExceptionFilter.UserUnauthorizedException.Title"),
                    (int)HttpStatusCode.Forbidden
                );

                return;
            }

            var cE5 = ex as UserMustChangePasswordException;

            if (cE5 != null)
            {
                await WriteHttpContext(httpContext, ex,
                     ex.Message,
                    _L.L("ExceptionFilter.UserMustChangePasswordException.Title"),
                    (int)HttpStatusCode.NotAcceptable
                );

                return;
            }

            var cE6 = ex as AlgoriaCoreException;

            if (cE6 != null) // Otro tipo de excepción generada y controlada por el sistema
            {
                await WriteHttpContext(httpContext, ex,
                    _L.L("ExceptionFilter.AlgoriaCoreException.Message") + "\r\n" + "\r\n" + ((AlgoriaCoreException)ex).Message,
                    _L.L("ExceptionFilter.AlgoriaCoreException.Title"),
                    (int)HttpStatusCode.BadRequest
                );

                return;
            }

            // Error no controlado
            await WriteHttpContext(httpContext, ex,
                    _L.L("ExceptionFilter.InternalServerError.Message") + "\r\n" + "\r\n" + ex.Message,
                    _L.L("ExceptionFilter.InternalServerError.Title"),
                    (int)HttpStatusCode.InternalServerError
                );
        }

        private async Task WriteHttpContext(HttpContext httpContext, Exception ex,
            string Message,
            string Title,
            int StatusCode)
        {
            var errorCode = "-1";

            var o = ex as AlgoriaCoreException;

            if (o != null)
            {
                errorCode = o.ErrorCode;
            }

            var customExeption = new CustomExceptionDto
            {
                ErrorCode = errorCode,
                Message = Message,
                Title = Title,
                StatusCode = StatusCode
            };

            var jsonString = JsonConvert.SerializeObject(customExeption);
            byte[] data = Encoding.UTF8.GetBytes(jsonString);
            var base64 = Convert.ToBase64String(data);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCode;
            httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "errormessage");
            httpContext.Response.Headers.Add("errormessage", base64);
            await httpContext.Response.Body.WriteAsync(data);
        }
    }
}
