using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    class CachedComponents : ICachedCollection<Type, Component>
    {
        private IDictionary<Type, IList<Component>> _cached = new Dictionary<Type, IList<Component>>();
        public IList<Component> this[Type type]
        {
            get
            {
                _cached.TryGetValue(type, out IList<Component> result);
                if (result == null)
                {
                    return Enumerable.Empty<Component>() as IList<Component>;
                }
                return result;
            }
        }

        public void Cache(Type key, Component component)
        {
            if (_cached.ContainsKey(key))
            {
                var components = _cached[key];
                if (!components.Contains(component))
                {
                    components.Add(component);
                }
            }
        }

        public void Clear()
        {
            _cached.Clear();
        }

        public bool Remove(Type key)
        {
            if (_cached.ContainsKey(key))
            {
                return _cached.Remove(key);
            }
            return false;
        }

        public bool Remove(Component component)
        {
            var key = component.GetType();
            if (_cached.ContainsKey(key))
            {
                return _cached[key].Remove(component);
            }
            return false;
        }
    }
}
