using MonoChrome.Core;
using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    interface ICachedCollection<TKey, TCached>
    {
        IEnumerable<TCached> this[TKey type] { get; }
        void Register(GameObject gameObject);
        void Erase(GameObject gameObject);
        void AddCacheRule(CacheRule rule);
        void Clear();
    }
}
