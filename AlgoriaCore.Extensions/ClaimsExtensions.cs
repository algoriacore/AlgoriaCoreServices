using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoriaCore.Extensions
{
    public static class ClaimsExtensions
    {
        public static int? GetIntValue(this IEnumerable<System.Security.Claims.Claim> claims, string type)
        {
            int? value = null;

            var item = claims.FirstOrDefault(m => m.Type == type);

			try
			{
				value = item != null ? (int?)int.Parse(item.Value) : null;
			}
			catch (Exception)		{
				// No se lanzará la excepción porque no es necesario. Sólo se continuará con el proceso.
			}

            return value;
        }

        public static long? GetLongValue(this IEnumerable<System.Security.Claims.Claim> claims, string type)
        {
            long? value = null;

            var item = claims.FirstOrDefault(m => m.Type == type);

			try
			{
				value = item != null ? (long?)long.Parse(item.Value) : null;
			}
			catch (Exception)
			{
				// Si ocurre un error dentro del bloque try, no es necesario regresar la excepción.
				// Sólo se regresará el valor de la variable "value", que en ese caso sería "null"
			}

            return value;
        }

        public static string GetStringValue(this IEnumerable<System.Security.Claims.Claim> claims, string type)
        {
            var item = claims.FirstOrDefault(m => m.Type == type);

            return item != null ? item.Value : null;
        }
    }
}
