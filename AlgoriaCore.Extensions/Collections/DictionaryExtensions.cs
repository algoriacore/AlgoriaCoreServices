using System;
using System.Collections.Generic;

namespace AlgoriaCore.Extensions.Collections
{
    /// <summary>
    /// Métodos de extensión para diccionarios.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Este método es usado para intentar obtener un valor en un diccionario si es que existe
        /// </summary>
        /// <typeparam name="T">Tipo del valor</typeparam>
        /// <param name="dictionary">La collección de objetos</param>
        /// <param name="key">Clave</param>
        /// <param name="value">Valor de la clave (o valor predeterminado si la clave no existe)</param>
        /// <returns>Verdadero si la clave existe en el diccionario</returns>
        internal static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            object valueObj;
            if (dictionary.TryGetValue(key, out valueObj) && valueObj is T)
            {
                value = (T)valueObj;
                return true;
            }

            value = default(T);
            return false;
        }

        /// <summary>
        /// Obtiene un valor de un diccionario con la clave dada. Regresa el valor predeterminado sino la puede encontrar.
        /// </summary>
        /// <param name="dictionary">Diccionario para verificar y obtener</param>
        /// <param name="key">Clave para encontrar el valor</param>
        /// <typeparam name="TKey">Tipo de la clave</typeparam>
        /// <typeparam name="TValue">Tipo del valor</typeparam>
        /// <returns>Valor si es encontrado, predeterminado sino pudo ser encontrado.</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue obj;
            return dictionary.TryGetValue(key, out obj) ? obj : default(TValue);
        }

        /// <summary>
        /// Obtiene un valor de un diccionario con la clave dada. Regresa el valor predeterminado sino la puede encontrar.
        /// </summary>
        /// <param name="dictionary">Diccionario para verificar y obtener</param>
        /// <param name="key">Clave para encontrar el valor</param>
        /// <param name="factory">Un método tipo fábrica utilizado para crear el valor si no se encuentra en el diccionario</param>
        /// <typeparam name="TKey">Tipo de la clave</typeparam>
        /// <typeparam name="TValue">Tipo del valor</typeparam>
        /// <returns>Valor si es encontrado, predeterminado sino pudo ser encontrado.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            TValue obj;
            if (dictionary.TryGetValue(key, out obj))
            {
                return obj;
            }

            return dictionary[key] = factory(key);
        }

        /// <summary>
        /// Obtiene un valor de un diccionario con la clave dada. Regresa el valor predeterminado sino la puede encontrar.
        /// </summary>
        /// <param name="dictionary">Diccionario para verificar y obtener</param>
        /// <param name="key">Clave para encontrar el valor</param>
        /// <param name="factory">Un método tipo fábrica utilizado para crear el valor si no se encuentra en el diccionario</param>
        /// <typeparam name="TKey">Tipo de la clave</typeparam>
        /// <typeparam name="TValue">Tipo del valor</typeparam>
        /// <returns>Valor si es encontrado, predeterminado sino pudo ser encontrado.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
        {
            return dictionary.GetOrAdd(key, k => factory());
        }
    }
}
