using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace AlgoriaCore.Application.Extensions
{
    /// <summary>
    /// Métodos de extensión para todos los objetos
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Usado para simplificar y embellecer la conversión de un objeto a otro tipo
        /// </summary>
        /// <typeparam name="T">Tipo destino</typeparam>
        /// <param name="obj">Objeto a convertir</param>
        /// <returns>Objeto transformado</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// Convierte un objeto dado a un tipo valor usando el método <see cref="Convert.ChangeType(object,System.TypeCode)"/>.
        /// </summary>
        /// <param name="obj">Objecto a ser convertido</param>
        /// <typeparam name="T">Tipo del objeto destino</typeparam>
        /// <returns>Objeto convertido</returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
            }

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///  Verifica si un elemento está en una lista
        /// </summary>
        /// <param name="item">Elemento a verificar</param>
        /// <param name="list">Lista de elementos</param>
        /// <typeparam name="T">Tipo de los elementos</typeparam>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
