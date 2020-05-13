using MonoChrome.Core;
using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    internal interface ICachedCollection<TKey, TCached>
    {
        IEnumerable<TCached> this[TKey type] { get; }
        void AddCacheRule(CacheRule rule);
        void Clear();
        void Erase(GameObject gameObject);
        void Register(GameObject gameObject);
        void OnFrameEnd();
        void OnFrameStart();
    }
}