﻿using System;
using System.Collections;
using VMS.Domain.Interfaces;

namespace VMS.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        public IEnumerable GetKeys()
        {
            throw new NotImplementedException();
        }

        public void Remove(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public T Set<T>(string cacheKey, T value)
        {
            throw new NotImplementedException();
        }

        public bool TryGet<T>(string cacheKey, out T value)
        {
            throw new NotImplementedException();
        }
    }
}