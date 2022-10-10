using AlgoriaCore.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading;

namespace AlgoriaCore.Application.Localization
{
    public class CacheLanguageService : ICacheLanguageService
    {
        private IMemoryCache Cache { get; set; }

        public CacheLanguageService(IConfiguration configuration)
        {
            IConfigurationSection cacheSection = configuration.GetSection("Cache").GetSection("LanguageTexts");

            Cache = new MemoryCache(new MemoryCacheOptions 
            { 
                SizeLimit = cacheSection.GetValue<long?>("SizeLimit"), 
                CompactionPercentage = cacheSection.GetValue<double>("CompactionPercentage"), 
                ExpirationScanFrequency = TimeSpan.FromMinutes(cacheSection.GetValue<long>("ExpirationScanFrequencyInMinutes")) 
            });
        }

        public string SetEntry(int? tenantId, int? language, string keyLabel, string value)
        {
            CancellationTokenSource cts = Cache.GetOrCreate(GenerateCacheKeyForParentLanguage(tenantId, language), entry =>
            {
                CancellationTokenSource ctsNew = new CancellationTokenSource();

                entry.SetSlidingExpiration(TimeSpan.FromDays(1));
                entry.SetSize(1);
                entry.SetValue(ctsNew);

                return ctsNew; 
            });

            string cacheKey = GenerateCacheKey(tenantId, language, keyLabel);

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5), Size = 1 }
            .AddExpirationToken(new CancellationChangeToken(cts.Token));

            Cache.Set(cacheKey, value, options);

            return cacheKey;
        }

        public string GetEntry(int? tenantId, int? language, string keyLabel)
        {
            return Cache.Get<string>(GenerateCacheKey(tenantId, language, keyLabel));
        }

        public void RemoveEntry(int? tenantId, int? language, string keyLabel)
        {
            Cache.Remove(GenerateCacheKey(tenantId, language, keyLabel));
        }

        public void CancelEntryParentLanguage(int? tenantId, int? language)
        {
            CancellationTokenSource cts = Cache.Get<CancellationTokenSource>(GenerateCacheKeyForParentLanguage(tenantId, language));

            if (cts != null)
            {
                cts.Cancel();
            }
        }

        public static string GenerateCacheKey(int? tenantId, int? language, string keyLabel)
        {
            return (tenantId == null ? "" : tenantId + "-") + language + "-" + keyLabel;
        }

        public static string GenerateCacheKeyForParentLanguage(int? tenantId, int? language)
        {
            return (tenantId == null ? "" : tenantId + "-") + language;
        }
    }
}
