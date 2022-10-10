using AlgoriaCore.Application.Interfaces;
using AlgoriaCore.Application.Managers.Languages.Dto;
using AlgoriaCore.Extensions;
using Autofac;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace AlgoriaCore.Application.Localization
{
    public class CacheLanguageXmlService : ICacheLanguageXmlService
    {
        private IMemoryCache Cache { get; set; }
		private readonly List<string> languageCodes = new List<string>() { "en-US" };

        public CacheLanguageXmlService(ILifetimeScope lifetimeScope)
        {
            Cache = new MemoryCache(new MemoryCacheOptions() { ExpirationScanFrequency = TimeSpan.FromDays(365 * 100) });
            
            SetEntriesDefault();
        }

        public string GenerateCacheKey(string languageCode, string keyLabel)
        {
            return (languageCode.IsNullOrWhiteSpace() ? "" : languageCode + "-") + keyLabel;
        }

        public string GetEntry(string languageCode, string keyLabel)
        {
            return Cache.Get<string>(GenerateCacheKey(languageCode, keyLabel));
        }

        public string GetEntry(string keyLabel)
        {
            return Cache.Get<string>(GenerateCacheKey(null, keyLabel));
        }

        public List<LanguageTextDto> GetLanguageTextFromXML(string languageName = "", bool takeDefaultIsNotExists = true)
        {
            List<LanguageTextDto> list = new List<LanguageTextDto>();
            XmlDocument xmlDocument = new XmlDocument();
            XmlAttributeCollection xmlNodeAttributes = null;
            XmlAttribute xmlNodeValueAttribute = null;

            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            basePath = basePath.Replace("file:\\", "").Replace("file:", "");

            string fileName = "algoriacore" + (languageName.IsNullOrEmpty() ? "" : "-" + languageName) + ".xml";
            string filePath = Path.Combine(basePath, @"Localization\algoriacore", fileName);
            bool exists = true;

            Console.WriteLine("This is my base Path" + basePath);

            if (!File.Exists(filePath))
            {
                exists = false;
                filePath = Path.Combine(basePath, @"Localization\algoriacore\algoriacore.xml");
                filePath = filePath.Replace('\\', '/');

                Console.WriteLine("This is my base path in linux: " + filePath);
            }

            if (exists || takeDefaultIsNotExists)
            {
                string xmlTemp = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(filePath));

				// Se elimina la marca BOM del documento
				StringBuilder preamble = new StringBuilder(string.Empty);
                for (int i = 0; i < xmlTemp.Length; i++)
                {
                    if (xmlTemp[i] == '<')
                    {
                        break;
                    }

					preamble.Append(xmlTemp[i]);
                }

                xmlTemp = xmlTemp.Remove(0, preamble.ToString().Length);

                xmlDocument.LoadXml(xmlTemp);
                xmlDocument.Normalize();

                XmlNodeList nodeList = xmlDocument.DocumentElement.SelectNodes("/localizationDictionary/texts/text");

                foreach (XmlNode node in nodeList)
                {
                    xmlNodeAttributes = node.Attributes;
                    xmlNodeValueAttribute = xmlNodeAttributes["value"];

                    list.Add(new LanguageTextDto()
                    {
                        Key = xmlNodeAttributes["name"].Value,
                        Value = (xmlNodeValueAttribute == null || xmlNodeValueAttribute.Value.IsNullOrEmpty()) ? node.InnerText : xmlNodeValueAttribute.Value
                    });
                }
            }

            return list;
        }

        private void SetEntriesDefault()
        {
            SetEntriesByLanguage();

            foreach (string languageCode in languageCodes)
            {
                SetEntriesByLanguage(languageCode);
            }
        }

        private void SetEntriesByLanguage(string languageCode = "")
        {
            List<LanguageTextDto> list = GetLanguageTextFromXML(languageCode, false);

            foreach (LanguageTextDto dto in list)
            {
                Cache.Set(GenerateCacheKey(languageCode, dto.Key), dto.Value);
            }
        }
    }
}
