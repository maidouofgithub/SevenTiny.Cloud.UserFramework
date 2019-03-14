using SevenTiny.Bantina.Caching;
using System;

namespace SevenTiny.Cloud.UserFramework.Infrastructure.Caching
{
    public static class LocalCacheHelper
    {
        public static void AddOrUpdate<T>(string key, T value, double expiredSecond)
        {
            MemoryCacheHelper.Put(key, value, DateTime.Now.AddSeconds(expiredSecond));
        }

        public static void Remove(string key)
        {
            MemoryCacheHelper.Delete(key);
        }

        public static T Get<T>(string key)
        {
            return MemoryCacheHelper.Get<string, T>(key);
        }
    }
}
