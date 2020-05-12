using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    internal class ZIndexSortedList<TKey, TValue> : IEnumerable<TValue>
        where TKey : class, ILayerItem
        where TValue : class
    {
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var dictionary in _list)
                {
                    for (int i = 0; i < dictionary.Value.Count; ++i) 
                    {
                        yield return dictionary.Value.ElementAt(i).Value;
                    }
                }
            }
        }
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
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
                    _list.Remove(args.OldZIndex);
                }
                Add(item, value);
            }
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
        public bool TryGetValue(TKey key, out TValue outValue)
        {
            foreach (var dictionary in _list.Values)
            {
                foreach (var value in dictionary.Values)
                {
                    outValue = value;
                    return true;
                }
            }
            outValue = null;
            return false;
        }
        private SortedList<int, Dictionary<TKey, TValue>> _list = new SortedList<int, Dictionary<TKey, TValue>>(new DescendingComparer());
    }
}