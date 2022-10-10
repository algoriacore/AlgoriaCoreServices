using System;
using System.Collections.Generic;

namespace AlgoriaCore.Extensions
{
    /// <summary>
    /// Métodos de extensión para colecciones.
	/// </summary>
	public static class CollectionExtensions
    {
        /// <summary>
        /// Verifica si un objeto dado de tipo colección es nulo o no tiene elementos
        /// </summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        /// <summary>
        /// Agregar un elemento a la colección si este no está ya en la colección,
        /// </summary>
        /// <param name="source">Colección</param>
        /// <param name="item">Elemento a verificar y agregar</param>
        /// <typeparam name="T">Tipo de los elementos de la colección</typeparam>
        /// <returns>Regresa verdadero si es agregado, regresa falso si no.</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (source.Contains(item))
            {
                return false;
            }
            source.Add(item);
            return true;
        }
    }
}
