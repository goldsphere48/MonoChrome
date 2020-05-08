using MonoChrome.Core;
using MonoChrome.Core.EntityManager;
using MonoChrome.SceneSystem.Layers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoChrome.SceneSystem.Layers.Helpers
{
    class CachedComponents
    {
        private static ZIndexComparator componentZIndexComporator = new ZIndexComparator();
        private IDictionary<Type, ICollection<Component>> _cached = new Dictionary<Type, ICollection<Component>>();
        private IDictionary<Type, CacheRule> _rules = new Dictionary<Type, CacheRule>();
        private EntityStore _store;

        public ICollection<Component> this[Type type]
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

        public CachedComponents(EntityStore store)
        {
            _store = store;
            _store.ComponentAdded += OnComponentAdded;
            _store.ComponentRemoved += OnComponentRemoved;
            _store.ComponentEnabled += OnComponentEnabled;
            _store.ComponentDisabled += OnComponentDisabled;
        }

        public void AddCacheRule<T>(CacheRule rule)
        {
            if (!_cached.ContainsKey(typeof(T)))
            {
                _rules.Add(typeof(T), rule);
                _cached.Add(typeof(T), new SortedSet<Component>(componentZIndexComporator));
            }
        }

        public void Clear()
        {
            _cached.Clear();
        }

        private void OnComponentAdded(object sender, ComponentEventArgs componentArgs)
        {
            Cache(componentArgs.Component, CacheRule.CacheOnAdd);
        }
        private void OnComponentRemoved(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheRule.UnchacheOnRemove);
        }
        private void OnComponentEnabled(object sender, ComponentEventArgs componentArgs)
        {
            Cache(componentArgs.Component, CacheRule.CacheOnEnable);
        }
        private void OnComponentDisabled(object sender, ComponentEventArgs componentArgs)
        {
            Uncache(componentArgs.Component, CacheRule.UncacheOnDisable);
        }

        private void Cache(Component component, CacheRule rule)
        {
            var type = GetBaseType(component);
            if (type != null)
            {
                var cacheRule = _rules[type] & rule;
                if (cacheRule == rule)
                {
                    Console.WriteLine("In Cache " + type);
                    Add(type, component);
                }
            }
        }

        private void Uncache(Component component, CacheRule rule)
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

        private void Add(Type key, Component component)
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

        private void OnZIndexChanged(object sender, EventArgs e)
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

        private bool Remove(Type key, Component component)
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
