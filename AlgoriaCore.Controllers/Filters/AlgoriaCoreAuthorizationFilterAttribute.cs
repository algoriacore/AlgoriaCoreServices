using AlgoriaCore.Application.Extensions;
using AlgoriaCore.Application.QueriesAndCommands.Authorization.Permissions;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.MultiTenancy;
using AlgoriaCore.Extensions;
using AlgoriaCore.WebUI.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Namotion.Reflection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AlgoriaCoreAuthorizationFilterAttribute : ActionFilterAttribute, IAlgoriaCoreAuthorizeAttribute
    {
        /// <summary>
        /// Una lista de permisos para autorizar.
        /// </summary>
        public string[] Permissions { get; }

        /// <summary>
        /// Si sta propiedad se establece en verdadero, todos los <see cref="Permissions"/> deben ser concedidos.
        /// Si es falso, al menos uno de los <see cref="Permissions"/> dene ser concedido.
        /// Predeterminado: false.
        /// </summary>
        public bool RequireAllPermissions { get; set; }
        public byte MultiTenancySide { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase <see cref="AlgoriaCoreAuthorizeAttribute"/>.
        /// </summary>
        /// <param name="permissions">Una lista de permisos para autorizar</param>
        public AlgoriaCoreAuthorizationFilterAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        public override async Task OnActionExecutionAsync(
                ActionExecutingContext context,
                ActionExecutionDelegate next)
        {
            if (!AuthorizationHelper.AllowAnonymous(context.ActionDescriptor))
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    throw new NoValidUserException("");
                }

                if (MultiTenancySide != 0) {
                    var multiTenancySideEnum = (MultiTenancySides)MultiTenancySide;
                    string tenantId = null;

                    if (context.HttpContext.User.Claims.Any(c => c.Type == "TenantId"))
                    {
                        tenantId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;
                    }

                    if ((multiTenancySideEnum == MultiTenancySides.Host && !tenantId.IsNullOrEmpty())
                       || (multiTenancySideEnum == MultiTenancySides.Tenant && tenantId.IsNullOrEmpty()))
                    {
                        throw new UserUnauthorizedException();
                    }
                }

                if (Permissions != null && Permissions.Length > 0)
                {
                    PermissionGetIsGrantedQuery query = new PermissionGetIsGrantedQuery
                    {
                        RequiresAll = RequireAllPermissions,
                        PermissionNames = Permissions,
                        IsTemplateProcess = AppPermissions.HasPermissionNameForProcess(Permissions),
                        IsCatalogCustomImpl = AppPermissions.HasPermissionNameForCatalogCustom(Permissions)
                    };

                    if (query.IsTemplateProcess)
                    {
                        var dto = context.ActionArguments.FirstOrDefault().Value;
                        query.Template = (int?) dto.TryGetPropertyValue<Int64?>("Template");
                    }
                    
                    if (query.IsCatalogCustomImpl)
                    {
                        var dto = context.ActionArguments.FirstOrDefault().Value;
                        query.Catalog = dto.TryGetPropertyValue<string>("Catalog");
                    }

                    IMediator mediator = context.HttpContext.RequestServices.GetService<IMediator>();
                    bool isGranted = await mediator.Send(query);

                    if (!isGranted)
                    {
                        throw new UserUnauthorizedException();
                    }
                }
            }

            // hacer algo antes de que la acción se ejecute
            await next();
            // hacer algo después de que la acción se ejecute; resultContext.Result será establecido
        }

    }
}
