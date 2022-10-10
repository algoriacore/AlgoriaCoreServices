using Autofac;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace AlgoriaCore5.WebAPI.Lambda
{
    public static class StartupExtensions
    {
        public static void AddManagers(this ContainerBuilder services, Assembly assembly)
        {
            var validators = assembly.GetTypes(); 

            foreach (var v in validators)
            {
                bool isBaseManager = v.GetInterface("ibasemanager", true) != null;

                if (isBaseManager && v.Name.ToLower() != "basemanager")
                {
                    Type tImplementation = v.UnderlyingSystemType;

                    services.RegisterType(tImplementation).PropertiesAutowired();
                }
            }
        }

        public static void AddValidatorsAsTransient(this IServiceCollection services, Assembly assembly)
        {
            var validators = assembly.GetTypes(); 

            foreach (var v in validators)
            {
                bool isIValidator = v.GetInterface("ivalidator", true) != null;
                if (isIValidator)
                {
                    Type tService = typeof(IValidator<>).MakeGenericType(v.BaseType.UnderlyingSystemType.GenericTypeArguments[0]);
                    Type tImplementation = v.UnderlyingSystemType;

                    services.AddTransient(tService, tImplementation);
                }
            }
        }
    }
}
