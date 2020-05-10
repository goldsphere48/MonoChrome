using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    class ZIndexContainer<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
            where TKey : ILayerItem
    {
        public TValue this[TKey key] => _values[key];
        public int ZIndex { get; }
        public int Count => _values.Count;

        private Dictionary<TKey, TValue> _values;

        public ZIndexContainer(int zIndex)
        {
            ZIndex = zIndex;
            _values = new Dictionary<TKey, TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            _values.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return _values.Remove(key);
        }

        public bool ContainsKey(TKey key)
        {
            return _values.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
