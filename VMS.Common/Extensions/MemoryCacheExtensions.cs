using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VMS.Domain.Interfaces;

namespace VMS.Common.Extensions
{
    public static class MemoryCacheExtensions
    {
        private static readonly Func<MemoryCache, object> GetEntriesCollection = Delegate.CreateDelegate(
            typeof(Func<MemoryCache, object>),
            typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true),
            throwOnBindFailure: true) as Func<MemoryCache, object>;

        public static IEnumerable GetKeys(this ICacheService memoryCache)
            => ((IDictionary)GetEntriesCollection((MemoryCache)memoryCache)).Keys;

        public static IEnumerable<T> GetKeys<T>(this ICacheService memoryCache)
            => GetKeys(memoryCache).OfType<T>();
    }
}