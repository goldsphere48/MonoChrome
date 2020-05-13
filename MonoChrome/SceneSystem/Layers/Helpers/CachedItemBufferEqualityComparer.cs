using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    class CachedItemBufferEqualityComparer<TKey> : IEqualityComparer<CacheItem<TKey>>
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
