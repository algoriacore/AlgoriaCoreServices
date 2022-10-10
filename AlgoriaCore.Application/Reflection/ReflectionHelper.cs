using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AlgoriaCore.Application.Reflection
{
    /// <summary>
    /// Define métodos utilitarios para la reflexión.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Verifica si <paramref name="givenType"/> implememnta/hereda de <paramref name="genericType"/>.
        /// </summary>
        /// <param name="givenType">Tipo a verificar</param>
        /// <param name="genericType">Tipo genérico</param>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var givenTypeInfo = givenType.GetTypeInfo();

            if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            foreach (var interfaceType in givenType.GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenTypeInfo.BaseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
        }

        /// <summary>
        /// Genera una lista de atributos definidos para un miembro de clase y su tipo declarado incluyento atributos heredados.
        /// </summary>
        /// <param name="inherit">Atributo heredado de clases base</param>
        /// <param name="memberInfo">MemberInfo</param>
        public static List<object> GetAttributesOfMemberAndDeclaringType(MemberInfo memberInfo, bool inherit = true)
        {
            var attributeList = new List<object>();

            attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));

            if (memberInfo.DeclaringType != null)
            {
                attributeList.AddRange(memberInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(inherit));
            }

            return attributeList;
        }

        /// <summary>
        /// Genera una lista de atributos definidos para un miembro de clase y su tipo declarado incluyento atributos heredados.
        /// </summary>
        /// <typeparam name="TAttribute">Tipo de atributo</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="inherit">Atributo heredado de clases base</param>
        public static List<TAttribute> GetAttributesOfMemberAndDeclaringType<TAttribute>(MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            var attributeList = new List<TAttribute>();

            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.GetTypeInfo().IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(memberInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            return attributeList;
        }

        /// <summary>
        /// Obtiene una lista de atributos definidos para un miembro de clase y tipo, incluidos los atributos heredados.
        /// </summary>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="type">Typo</param>
        /// <param name="inherit">Atributo heredado de clases base</param>
        public static List<object> GetAttributesOfMemberAndType(MemberInfo memberInfo, Type type, bool inherit = true)
        {
            var attributeList = new List<object>();
            attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));
            attributeList.AddRange(type.GetTypeInfo().GetCustomAttributes(inherit));
            return attributeList;
        }

        /// <summary>
        /// Obtiene una lista de atributos definidos para un miembro de clase y tipo, incluidos los atributos heredados.
        /// </summary>
        /// <typeparam name="TAttribute">Tipo de atributo</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="type">Tipo</param>
        /// <param name="inherit">Atributo heredado de clases base</param>
        public static List<TAttribute> GetAttributesOfMemberAndType<TAttribute>(MemberInfo memberInfo, Type type, bool inherit = true)
            where TAttribute : Attribute
        {
            var attributeList = new List<TAttribute>();

            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            if (type.GetTypeInfo().IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(type.GetTypeInfo().GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            return attributeList;
        }

        /// <summary>
        /// Intenta obtener un atributo definido para un miembro de la clase y su tipo declarado, incluidos los atributos heredados.
        /// Regresa el valor predeterminado si no está declarado en lo absoluto.
        /// </summary>
        /// <typeparam name="TAttribute">Tipo de atributo</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="defaultValue">Valor predeterminado (null como predeterminado)</param>
        /// <param name="inherit">Atributo heredado de clases base</param>
        public static TAttribute GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true)
            where TAttribute : class
        {
            return memberInfo.GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault()
                   ?? memberInfo.DeclaringType?.GetTypeInfo().GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault()
                   ?? defaultValue;
        }

        /// <summary>
        /// Intenta obtener un atributo definido para un miembro de la clase y su tipo declarado, incluidos los atributos heredados.
        /// Regresa el valor predeterminado si no está declarado en lo absoluto.
        /// </summary>
        /// <typeparam name="TAttribute">Tipo de atributo</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="defaultValue">Valor predeterminado (null como predeterminado)</param>
        /// <param name="inherit">Atributo heredado de clases base</param>
        public static TAttribute GetSingleAttributeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true)
            where TAttribute : Attribute
        {
            //Obtener un atributo del miembro
            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().First();
            }

            return defaultValue;
        }

        /// <summary>
        /// Obtiene el valor de una propiedad por su ruta completa del objeto dado
        /// </summary>
        /// <param name="obj">Objeto delc ual obtener el valor</param>
        /// <param name="objectType">Tipo de objeto dado</param>
        /// <param name="propertyPath">Ruta completa de la propiedad</param>
        /// <returns></returns>
        internal static object GetValueByPath(object obj, Type objectType, string propertyPath)
        {
            var value = obj;
            var currentType = objectType;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (absolutePropertyPath.StartsWith(objectPath))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            foreach (var propertyName in absolutePropertyPath.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }

            return value;
        }

        /// <summary>
        /// Establece el valor de una propiedad por su ruta completa del objeto dado
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objectType"></param>
        /// <param name="propertyPath"></param>
        /// <param name="value"></param>
        internal static void SetValueByPath(object obj, Type objectType, string propertyPath, object value)
        {
            var currentType = objectType;
            PropertyInfo property;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (absolutePropertyPath.StartsWith(objectPath))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            var properties = absolutePropertyPath.Split('.');

            if (properties.Length == 1)
            {
                property = objectType.GetProperty(properties.First());
                property.SetValue(obj, value);
                return;
            }

            for (int i = 0; i < properties.Length - 1; i++)
            {
                property = currentType.GetProperty(properties[i]);
                obj = property.GetValue(obj, null);
                currentType = property.PropertyType;
            }

            property = currentType.GetProperty(properties.Last());
            property.SetValue(obj, value);
        }
    }
}
