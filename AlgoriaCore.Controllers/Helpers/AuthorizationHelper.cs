using AlgoriaCore.Application.Reflection;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AlgoriaCore.WebUI.Helpers
{
    public static class AuthorizationHelper
	{
		public static bool AllowAnonymous(ControllerActionDescriptor actionDescriptor)
		{
			return actionDescriptor.ControllerTypeInfo.IsDefined(typeof(AllowAnonymousAttribute), true);
		}
	
        public static bool AllowAnonymous(ActionDescriptor actionDescriptor)
        {
            ControllerActionDescriptor controllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
            MethodInfo methodInfo = controllerActionDescriptor.MethodInfo;

            return AllowAnonymous(methodInfo, methodInfo.DeclaringType);
        }

        public static bool AllowAnonymous(MemberInfo methodInfo, Type type)
        {
            List<CustomAttributeData> own = methodInfo.CustomAttributes.ToList();

            if (own.OfType<AllowAnonymousAttribute>().Any())
            {
                return true;
            }

            if (own.OfType<AlgoriaCoreAuthorizationFilterAttribute>().Any() || own.OfType<AuthorizeAttribute>().Any())
            {
                return false;
            } else 
            {
                return ReflectionHelper.GetAttributesOfMemberAndType(methodInfo, type).OfType<AllowAnonymousAttribute>().Any();
            } 
        }
    }
}
