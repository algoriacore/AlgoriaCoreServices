using AlgoriaCore.Extensions;
using System.Linq;
using System.Text;

namespace AlgoriaCore.Application.Managers.Auditing
{
    public class NamespaceStripper
    {
        public string StripNameSpace(string serviceName)
        {
            if (serviceName.IsNullOrEmpty() || !serviceName.Contains("."))
            {
                return serviceName;
            }

            if (serviceName.Contains("["))
            {
                return StripGenericNamespace(serviceName);
            }

            return GetTextAfterLastDot(serviceName);
        }

        private static string GetTextAfterLastDot(string text)
        {
            var lastDotIndex = text.LastIndexOf('.');
            return text.Substring(lastDotIndex + 1);
        }

        private static string StripGenericNamespace(string serviceName)
        {
            var serviceNameParts = serviceName.Split('[').Where(s => !s.IsNullOrEmpty()).ToList();
            StringBuilder genericServiceName = new StringBuilder();
            var openBracketCount = 0;

            for (var i = 0; i < serviceNameParts.Count; i++)
            {
                var serviceNamePart = serviceNameParts[i];
                if (serviceNamePart.Contains("`"))
                {
					genericServiceName.Append(GetTextAfterLastDot(serviceNamePart.Substring(0, serviceNamePart.IndexOf('`'))) + "<");
                    openBracketCount++;
                }

                if (serviceNamePart.Contains(","))
                {
					genericServiceName.Append(GetTextAfterLastDot(serviceNamePart.Substring(0, serviceNamePart.IndexOf(','))));
                    if (i + 1 < serviceNameParts.Count && serviceNameParts[i + 1].Contains(","))
                    {
						genericServiceName.Append(", ");
                    }
                    else
                    {
						genericServiceName.Append(">");
                        openBracketCount--;
                    }
                }
            }

            for (int i = 0; i < openBracketCount; i++)
            {
				genericServiceName.Append(">");
            }

			return genericServiceName.ToString();
        }
    }
}
