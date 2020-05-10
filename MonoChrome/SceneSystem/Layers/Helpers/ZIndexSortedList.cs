using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    class ZIndexSortedList<TKey, TValue> : IEnumerable<TValue>
        where TKey : class, ILayerItem
    {
        private SortedList<int, Dictionary<TKey, TValue>> _list = new SortedList<int, Dictionary<TKey, TValue>>();

        public void Add(TKey key, TValue value)
        {
            _list.TryGetValue(key.ZIndex, out var container);
            if (container == null)
            {
                container = new Dictionary<TKey, TValue>();
                _list.Add(key.ZIndex, container);
            }
            container.Add(key, value);
            key.ZIndexChanged += OnZIndexChanged;
        }

        public bool Remove(TKey key)
        {
            _list.TryGetValue(key.ZIndex, out var container);
            if (container == null)
            {
                return false;
            }
            key.ZIndexChanged -= OnZIndexChanged;
            container.Remove(key);
            if (container.Count == 0)
            {
                _list.Remove(key.ZIndex);
            }
            return true;
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

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (var dictionary in _list.Values)
            {
                foreach (var value in dictionary)
                {
                    yield return value.Value;
                }
            }
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
                    _list.Remove(item.ZIndex);
                }
                Add(item, value);
            }
        }

        public void Clear()
        {
            foreach (var dictionary in _list)
            {
                foreach (var value in dictionary.Value)
                {
                    value.Key.ZIndexChanged -= OnZIndexChanged;
                }
                dictionary.Value.Clear();
            }
            _list.Clear();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
