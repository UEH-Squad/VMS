using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using VMS.Common.Extensions;
using VMS.Domain.Configurations;
using VMS.Domain.Interfaces;

namespace VMS.Infrastructure.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheConfiguration _cacheConfig;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheConfiguration> cacheConfig)
        {
            _memoryCache = memoryCache;
            _cacheConfig = cacheConfig.Value;
            if (_cacheConfig != null)
            {
                _cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(_cacheConfig.AbsoluteExpirationInHours),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes)
                };
            }
        }

        public bool TryGet<T>(string cacheKey, out T value)
        {
            _memoryCache.TryGetValue(cacheKey, out value);
            if (value == null) return false;
            else return true;
        }

        public T Set<T>(string cacheKey, T value)
        {
            if (_cacheConfig.IsCacheEnabled)
            {
                return _memoryCache.Set(cacheKey, value, _cacheOptions);
            }

            return default;
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public IEnumerable GetKeys()
        {
            MemoryCache cache = _memoryCache as MemoryCache;
            return cache.GetKeys();
        }
    }
}