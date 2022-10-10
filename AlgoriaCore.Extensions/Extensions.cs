using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AlgoriaCore.Extensions
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
			return value == null || value.Trim() == string.Empty;
        }

        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentOutOfRangeException("str");
            }

            return str.Substring(0, len);
        }

        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        public static string StrLeft(this string value, string searchedValue)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }

            int index = value.IndexOf(searchedValue);

            if (index >= 0)
            {
                return value.Substring(0, index);
            } else
            {
                return string.Empty;
            }
        }

        public static string StrRight(this string value, string searchedValue)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }

            int index = value.IndexOf(searchedValue);

            if (index >= 0)
            {
                return value.Substring(index + searchedValue.Length);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string BulkReplace(this string source, IDictionary<string, string> replacementMap)
        {
            try
            {
                if (source == null || source.Length == 0 || replacementMap.Count == 0)
                {
                    return source;
                }

                string pattern = @"\[[A-Za-z\d\|\.\,_]*\]";
                string mins = Regex.Replace(source.ToString(), pattern, m => m.ToString().ToLower());

                StringBuilder sbR = new StringBuilder(mins);
                foreach (var d in replacementMap)
                {
                    sbR = sbR.Replace(d.Key.ToLower(), d.Value);
                }

                // Eliminar todas las entradas que tengan la forma "[config.PALABRA]"
                pattern = @"\[config.[\w]*\]";
                string uRep = Regex.Replace(sbR.ToString(), pattern, m => string.Empty);

                return uRep;
            }
            catch (Exception)
            {
                return source;
            }
        }

        /// <summary>
        /// Indica si esta cadena es nula, vacía, o contiene solamente espacios en blanco
		/// </summary>
		public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
