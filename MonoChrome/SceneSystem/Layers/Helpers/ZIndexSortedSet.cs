using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    internal class ZIndexSortedSet<TValue> : ICollection<TValue>
            where TValue : class, ILayerItem
    {
        public int Count => _list.Count;
        public bool IsReadOnly => false;
        private SortedList<int, HashSet<TValue>> _list = new SortedList<int, HashSet<TValue>>(new DescendingComparer());

        public void Add(TValue value)
        {
            _list.TryGetValue(value.ZIndex, out var container);
            if (container == null)
            {
                container = new HashSet<TValue>();
                _list.Add(value.ZIndex, container);
            }
            container.Add(value);
            value.ZIndexChanged += OnZIndexChanged;
        }

        public void Clear()
        {
            foreach (var dictionary in _list)
            {
                foreach (var value in dictionary.Value)
                {
                    value.ZIndexChanged -= OnZIndexChanged;
                }
                dictionary.Value.Clear();
            }
            _list.Clear();
        }

        public bool Contains(TValue value)
        {
            _list.TryGetValue(value.ZIndex, out var container);
            if (container != null)
            {
                return container.Contains(value);
            }
            return false;
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (var dictionary in _list.Values)
            {
                foreach (var value in dictionary)
                {
                    yield return value;
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
                var item = sender as TValue;
                container.Remove(item);
                if (container.Count == 0)
                {
                    _list.Remove(args.OldZIndex);
                }
                Add(item);
            }
        }

        public bool Remove(TValue value)
        {
            _list.TryGetValue(value.ZIndex, out var container);
            if (container == null)
            {
                return false;
            }
            value.ZIndexChanged -= OnZIndexChanged;
            container.Remove(value);
            if (container.Count == 0)
            {
                _list.Remove(value.ZIndex);
            }
            return true;
        }
    }
}