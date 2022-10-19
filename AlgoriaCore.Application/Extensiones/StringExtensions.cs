using System.Text;
using System.Text.RegularExpressions;

namespace AlgoriaCore.Application.Extensions
{
    public static class StringExtensions
    {
        public static string ToParam(this string value, string beginString = "[", string endString = "]")
        {
            return beginString + value + endString;
        }

        public static string ToVariableName(this string value)
        {
            string result = value.Trim().ToLower().Replace(" ", "_").Normalize(NormalizationForm.FormD);

            result = int.TryParse(result[0].ToString(), out int num) ? "a" + result : result;
            result = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(result)).Replace("?", "");
            result = Regex.Replace(result, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);

            return result;
        }

        public static string GetInvalidCharacters(this string value)
        {
            return value.GetInvalidCharacters(@"^[a-zA-Z0-9 Ññ!\""%&'´\-:;>=<@_,.{}`~áéíóúÁÉÍÓÚüÜ]+$");
        }

        public static string GetInvalidCharacters(this string value, string expression)
        {
            StringBuilder result = new StringBuilder();

            if (!Regex.IsMatch(value, expression))
            {
                foreach (var c in value)
                {
                    var str = c.ToString();

                    if (!Regex.IsMatch(str, expression))
                    {
                        result.Append(!result.ToString().Contains(str) ? str : string.Empty);
                    }
                }
            }

            return result.ToString().Trim();
        }

        public static bool IsMatch(this string value, string pattern)
        {
            Regex rg = new Regex(pattern);
            return rg.IsMatch(value);
        }
    }
}
