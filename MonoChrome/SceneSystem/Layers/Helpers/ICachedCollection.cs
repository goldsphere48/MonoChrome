using MonoChrome.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    interface ICachedCollection<TKey, TCached>
    {
        ICollection<TCached> this[TKey type] { get; }
        void Register(GameObject gameObject);
        void Erase(GameObject gameObject);
        void AddCacheRule(CacheRule rule);
        void Clear();
    }
}
