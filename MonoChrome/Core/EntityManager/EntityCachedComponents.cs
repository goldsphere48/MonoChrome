using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.Core.EntityManager
{
    class EntityCachedComponents : ICachedCollection<Component, Type>
    {
        private IDictionary<Type, IList<Component>> _cached = new Dictionary<Type, IList<Component>>();
        public IList<Component> this[Type type]
        {
            get
            {
                _cached.TryGetValue(type, out IList<Component> result);
                return result;
            }
        }

        public void Cache(Component component)
        {
            var type = component.GetType();
            if (_cached.ContainsKey(type))
            {
                var components = _cached[type];
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

        public bool Remove(Component component)
        {
            var type = component.GetType();
            if (_cached.ContainsKey(type))
            {
                var components = _cached[type];
                if (!components.Contains(component))
                {
                    return components.Remove(component);
                }
            }
            return false;
        }
    }
}
