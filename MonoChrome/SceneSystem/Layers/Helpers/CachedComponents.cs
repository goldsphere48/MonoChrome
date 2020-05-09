using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    class CachedComponents : CachedCollection<Type, Component>
    {
        private static ZIndexComparator componentZIndexComporator = new ZIndexComparator();
        private IDictionary<Type, ICollection<Component>> _cached = new Dictionary<Type, ICollection<Component>>();
        private IDictionary<Type, CacheRule> _rules = new Dictionary<Type, CacheRule>();

        public override ICollection<Component> this[Type type]
        {
            get
            {
                _cached.TryGetValue(type, out ICollection<Component> result);
                if (result == null)
                {
                    return Enumerable.Empty<Component>() as IList<Component>;
                }
                return result;
            }
        }

        public override void AddCacheRule<T>(CacheRule rule)
        {
            if (!_cached.ContainsKey(typeof(T)))
            {
                _rules.Add(typeof(T), rule);
                _cached.Add(typeof(T), new SortedSet<Component>(componentZIndexComporator));
            }
        }

        public override void Clear()
        {
            _cached.Clear();
        }

        protected override void Cache(Component component, CacheRule rule)
        {
            var type = GetBaseType(component);
            if (type != null)
            {
                var cacheRule = _rules[type] & rule;
                if (cacheRule == rule)
                {
                    Add(type, component);
                }
            }
        }

        protected override void Uncache(Component component, CacheRule rule)
        {
            var type = GetBaseType(component);
            if (type != null)
            {
                var cacheRule = _rules[type] & rule;
                if (cacheRule == rule)
                {
                    Remove(type, component);
                }
            }
        }

        protected override void Add(Type key, Component component)
        {
            if (_cached.ContainsKey(key))
            {
                var components = _cached[key];
                if (!components.Contains(component))
                {
                    components.Add(component);
                    component.GameObject.ZIndexChanged += OnZIndexChanged;
                }
            }
        }

        protected override void OnZIndexChanged(object sender, EventArgs e)
        {
            var gameObject = sender as GameObject;
            var components = gameObject.GetComponents();
            foreach (var key in _cached.Keys)
            {
                foreach (var component in components)
                {
                    if (key.IsAssignableFrom(component.GetType()))
                    {
                        _cached[key].Remove(component);
                        _cached[key].Add(component);
                    }
                }
            }
        }

        protected override bool Remove(Type key, Component component)
        {
            if (_cached.ContainsKey(key))
            {
                component.GameObject.ZIndexChanged -= OnZIndexChanged;
                return _cached[key].Remove(component);
            }
            return false;
        }

        private Type GetBaseType(Component component)
        {
            var componentType = component.GetType();
            foreach (var key in _cached.Keys)
            {
                if (componentType.IsSubclassOf(key) || componentType == key)
                {
                    return key;
                }
            }
            return null;
        }
    }
}
