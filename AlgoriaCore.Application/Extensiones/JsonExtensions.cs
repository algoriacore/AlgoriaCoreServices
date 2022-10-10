using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AlgoriaCore.Application.Extensions
{
    public static class JsonExtensions
    {
        /// <summary>
        /// Convierte un objeto dado a cadena JSON
        /// </summary>
        /// <returns></returns>
        public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
        {
            var options = new JsonSerializerSettings();

            if (camelCase)
            {
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            if (indented)
            {
                options.Formatting = Formatting.Indented;
            }

            return JsonConvert.SerializeObject(obj, options);
        }
    }
}
