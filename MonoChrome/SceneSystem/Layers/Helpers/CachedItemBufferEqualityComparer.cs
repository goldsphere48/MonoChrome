using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    internal class CachedItemBufferEqualityComparer<TKey> : IEqualityComparer<CacheItem<TKey>>
    {
        public bool Equals(CacheItem<TKey> x, CacheItem<TKey> y)
        {
            return ReferenceEquals(x.Component, y.Component) && ReferenceEquals(x.Key, y.Key);
        }

        public int GetHashCode(CacheItem<TKey> obj)
        {
            return obj.Key.GetHashCode() ^ obj.Component.GetHashCode();
        }
    }
}