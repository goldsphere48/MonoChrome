using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    class ZIndexSortedList<TKey, TValue>
        where TKey : class, ILayerItem
    {
        private SortedList<int, ZIndexContainer<TKey, TValue>> _list = new SortedList<int, ZIndexContainer<TKey, TValue>>();

        public void Add(TKey key, TValue value)
        {
            _list.TryGetValue(key.ZIndex, out var container);
            if (container == null)
            {
                container = new ZIndexContainer<TKey, TValue>(key.ZIndex);
                _list.Add(key.ZIndex, container);
            }
            container.Add(key, value);
            key.ZIndexChanged += OnZIndexChanged;
        }

        public bool Contains(TKey key)
        {
            _list.TryGetValue(key.ZIndex, out var container);
            if (container != null)
            {
                return container.ContainsKey(key);
            }
            return false;
        }

        public void OnZIndexChanged(object sender, ZIndexEventArgs args)
        {
            _list.TryGetValue(args.OldZIndex, out var container);
            if (container != null)
            {
                var item = sender as TKey;
                var value = container[item];
                container.Remove(item);
                if (container.Count == 0)
                {
                    _list.Remove(container.ZIndex);
                }
                Add(item, value);
            }
        }
    }
}
